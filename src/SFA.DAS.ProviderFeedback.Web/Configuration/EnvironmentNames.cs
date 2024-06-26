﻿namespace SFA.DAS.ProviderFeedback.Web.Configuration
{
    public static class EnvironmentNames
    {
        public const string Development = "DEVELOPMENT";
        public const string AT = "AT";
        public const string TEST = "TEST";
        public const string TEST2 = "TEST2";
        public const string DEMO = "DEMO";
        public const string PREPROD = "PREPROD";
        public const string PROD = "PROD";

        public static string[] GetTestEnvironmentNames() => new []{ Development, AT, TEST, TEST2 };
        public static string GetTestEnvironmentNamesCommaDelimited() => string.Join(",", GetTestEnvironmentNames());
        public static string GetNonProdEnvironmentNamesCommaDelimited() => string.Join(",", Development, AT, TEST, TEST2, DEMO, PREPROD);
    }
}