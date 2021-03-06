﻿namespace ConfigurationManagement.Json.Web.DotNetCore
{
    public class PaymentsConfig
    {
        public PaymentsCredentials Credentials { get; set; }
        public PaymentService Service { get; set; }
    }

    public class PaymentsCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class PaymentService
    {
        public string Url { get; set; }
    }

    public class ParentConfig
    {
        public PaymentsConfig  Payments { get; set; }
    }
}
