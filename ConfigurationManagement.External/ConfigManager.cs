using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationManagement.External
{
    public class ConfigManager<TVersion> : IDisposable
    {
        // An abstraction of the configuration store.
        private readonly ISettingsStore<TVersion> _settings;
        private readonly ISubject<KeyValuePair<Type, object>> changed;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private Task _monitoringTask;
        private readonly TimeSpan _interval;

        private readonly SemaphoreSlim _timerSemaphore = new SemaphoreSlim(1);
        private readonly ReaderWriterLockSlim _settingsCacheLock = new ReaderWriterLockSlim();
        private readonly SemaphoreSlim syncCacheSemaphore = new SemaphoreSlim(1);

        private Dictionary<Type, object> settingsCache;
        private TVersion currentVersion;

        public ConfigManager(ISettingsStore<TVersion> settings, TimeSpan interval)
        {
            _settings = settings;
            _interval = interval;
            CheckForConfigurationChangesAsync().Wait();
            changed = new Subject<KeyValuePair<Type, object>>();
        }

        public IObservable<KeyValuePair<Type, object>> Changed => changed.AsObservable();

        /// <summary>
        /// Check to see if the current instance is monitoring for changes
        /// </summary>
        public bool IsMonitoring => _monitoringTask != null && !_monitoringTask.IsCompleted;

        /// <summary>
        /// Start the background monitoring for configuration changes in the central store
        /// </summary>
        public void StartMonitor()
        {
            if (IsMonitoring)
                return;

            try
            {
                _timerSemaphore.Wait();

                //Check again to make sure we are not already running.
                if (IsMonitoring)
                    return;

                //Start runnin our task loop.
                _monitoringTask = ConfigChangeMonitor();
            }
            finally
            {
                _timerSemaphore.Release();
            }
        }

        /// <summary>
        /// Loop that monitors for configuration changes
        /// </summary>
        /// <returns></returns>
        public async Task ConfigChangeMonitor()
        {
            while (!cts.Token.IsCancellationRequested)
            {
                await CheckForConfigurationChangesAsync();

                await Task.Delay(_interval, cts.Token);
            }
        }

        /// <summary>
        /// Stop Monitoring for Configuration Changes
        /// </summary>
        public void StopMonitor()
        {
            try
            {
                _timerSemaphore.Wait();

                //Signal the task to stop
                cts.Cancel();

                //Wait for the loop to stop
                _monitoringTask.Wait();

                _monitoringTask = null;
            }
            finally
            {
                _timerSemaphore.Release();
            }
        }

        public void Dispose()
        {
            cts.Cancel();
        }

        /// <summary>
        /// Retrieve application setting from the local cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TSettings GetSettings<TSettings>()
        {
            // Try and get the value from the settings cache.  If there's a miss, get the setting from the settings store and refresh the settings cache.
            object value;
            try
            {
                _settingsCacheLock.EnterReadLock();

                settingsCache.TryGetValue(typeof(TSettings), out value);
            }
            finally
            {
                _settingsCacheLock.ExitReadLock();
            }

            return (TSettings)value;
        }

        /// <summary>
        /// Check the central repository for configuration changes and update the local cache
        /// </summary>
        private async Task CheckForConfigurationChangesAsync()
        {
            try
            {
                // It is assumed that updates are infrequent.
                // To avoid race conditions in refreshing the cache synchronize access to the in memory cache
                await syncCacheSemaphore.WaitAsync();

                var latestVersion = await _settings.GetVersion();

                // If the versions are the same, nothing has changed in the configuration.
                if (currentVersion != null && currentVersion.Equals(latestVersion)) return;

                // Get the latest settings from the settings store and publish changes.
                var latestSettings = await _settings.FindAll();

                // Refresh the settings cache.
                try
                {
                    _settingsCacheLock.EnterWriteLock();

                    if (settingsCache != null)
                    {
                        //Notify settings changed
                        latestSettings.Except(settingsCache).ToList().ForEach(kv => changed.OnNext(kv));
                    }
                    settingsCache = latestSettings;

                    settingsCache = latestSettings;
                }
                finally
                {
                    _settingsCacheLock.ExitWriteLock();
                }

                // Update the current version.
                currentVersion = latestVersion;
            }
            catch (Exception ex)
            {
                changed.OnError(ex);
            }
            finally
            {
                syncCacheSemaphore.Release();
            }
        }
    }
}
