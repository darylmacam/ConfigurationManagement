using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationManagement.External
{
    public class FakeConfigStore : ISettingsStore<string>
    {
        private readonly string _environment;

        public FakeConfigStore(string environment)
        {
            _environment = environment;
        }

        public async Task<Dictionary<Type, object>> FindAll()
            => await Task.FromResult(new Dictionary<Type, object>
            {
                { typeof(PaymentsConfig),
                    new PaymentsConfig {
                    Credentials  = new PaymentsCredentials {
                        Username = "fakeyouknowme",
                        Password = "fake"
                    },
                    Service = new PaymentService
                    {
                        Url = _environment.ToLower() == "production" ? "http://prod.paypal.com" : "http://dev.paypal.com"
                    }
                }
            }
        });

        public async Task<string> GetVersion()
            => await Task.FromResult(string.Empty);
    }
}
