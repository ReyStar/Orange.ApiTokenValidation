using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Orange.ApiTokenValidation.API.Configuration
{
    /// <summary>
    /// Media formatter config
    /// </summary>
    public static class FormatterConfig
    {
        /// <summary>
        /// Registration newtonjson media formatter
        /// </summary>
        public static void ConfigureJsonFormat(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddJsonOptions(opt =>
            {
                //opt.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //opt .SerializerSettings.Formatting = Formatting.Indented;
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }
    }
}
