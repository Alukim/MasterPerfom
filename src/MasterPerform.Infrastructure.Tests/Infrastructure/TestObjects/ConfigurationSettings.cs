using System.Collections.Generic;

namespace MasterPerform.Tests.Infrastructure.TestObjects
{
    public static class ConfigurationSettings
    {
        public static string Prefix = "test";
        public static string NodeUrl = "http://localhost";
        public static int ShardsNumber = 1;

        public static Dictionary<string, string> CustomSettings
            => new Dictionary<string, string>
            {
                {"EnvironmentSettings:Prefix", Prefix},
                {"ElasticsearchSettings:NodeUrl", NodeUrl},
                {"ElasticsearchSettings:ShardsNumber", ShardsNumber.ToString()},
            };
    }
}
