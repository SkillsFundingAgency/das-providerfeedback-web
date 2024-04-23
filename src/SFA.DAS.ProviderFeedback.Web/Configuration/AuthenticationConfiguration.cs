﻿namespace SFA.DAS.ProviderFeedback.Web.Configuration
{
    public class AuthenticationConfiguration
    {
        public const int SessionTimeoutMinutes = 30;
        public string WtRealm { get; set; }
        public string MetaDataAddress { get; set; }
    }    
}
