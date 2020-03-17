using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Text;

namespace TaxCalculator.Services.Artifacs.Extensions
{
    public static class SerializeExtensions
    {
        private static JsonSerializerSettings CustomJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }

        public static T DeserializeAs<T>(this string responseContent)
        {
            return JsonConvert.DeserializeObject<T>(responseContent, CustomJsonSerializerSettings());
        }
        public static StringContent Serialize<T>(this T instance)
        {
            var json = JsonConvert.SerializeObject(instance, CustomJsonSerializerSettings());

            var body = new StringContent(json, Encoding.Default, "application/json");

            return body;
        }
    }
}
