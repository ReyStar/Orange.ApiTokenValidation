using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using EnsureThat;

namespace Orange.ApiTokenValidation.Common
{
    public class ResourceLoader
    {
        private readonly Assembly _assembly;

        public ResourceLoader(Assembly assembly)
        {
            _assembly = assembly;
            EnsureArg.IsNotNull(assembly, nameof(assembly));
        }

        public async Task<string> LoadStringAsync(string resourceName)
        {
            EnsureArg.IsNotNullOrWhiteSpace(resourceName, nameof(resourceName));

            await using var resourceStream = _assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(resourceStream ?? throw new InvalidOperationException($"The {nameof(resourceStream)} can't be loaded"));
            
            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        public string LoadString(string scriptName)
        {
            return LoadStringAsync(scriptName).GetAwaiter().GetResult();
        }
    }
}
