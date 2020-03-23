using System.Configuration;

namespace ConfigurationManagement.ConfigFile
{
    public class PaymentsCredentialsConfig : ConfigurationSection
    {
        [ConfigurationProperty("username", IsKey = true)]
        public string Username
        {
            get { return (string)this["username"]; }
            set { this["username"] = value; }
        }
        [ConfigurationProperty("password")]
        public string Password
        {
            get { return this["password"].ToString(); }
            set { this["password"] = value; }
        }
    }

    public class PaymentsConfig : ConfigurationSectionGroup
    {
        [ConfigurationProperty("credentials", IsRequired = false)]
        public PaymentsCredentialsConfig Credentials
            => (PaymentsCredentialsConfig)base.Sections["credentials"];

        [ConfigurationProperty("service", IsRequired = false)]
        public PaymentsServiceConfig Service
           => (PaymentsServiceConfig)base.Sections["service"];
    }

    public class PaymentsServiceConfig : ConfigurationSection
    {
        [ConfigurationProperty("url")]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }
    }
}
