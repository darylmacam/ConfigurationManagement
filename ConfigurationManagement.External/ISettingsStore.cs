using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationManagement.External
{
    public interface ISettingsStore<TVersion>
    {
        Task<TVersion> GetVersion();
        Task<Dictionary<Type, object>> FindAll();
    }
}
