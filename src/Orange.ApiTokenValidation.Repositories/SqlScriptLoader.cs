using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using EnsureThat;

namespace Orange.ApiTokenValidation.Repositories
{
    internal static class SqlScriptLoader
    {
        public static async Task<string> LoadAsync(string scriptName)
        {
            EnsureArg.IsNotNullOrWhiteSpace(scriptName, nameof(scriptName));

            var dataBasesAssembly = Assembly.GetAssembly(typeof(SqlScriptLoader));
            var resourceName = $"Orange.ApiTokenValidation.Repositories.SQL.{scriptName}.sql";
            using (var resourceStream = dataBasesAssembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(resourceStream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public static string Load(string scriptName)
        {
            return LoadAsync(scriptName).GetAwaiter().GetResult();
        }
    }
}
