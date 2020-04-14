using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace Orange.ApiTokenValidation.ClientGenerator
{
    //https://github.com/RicoSuter/NSwag/wiki/CSharpClientGenerator
    class Program
    {
        private static readonly Dictionary<string, string> CommandLineKeyMappingDictionary;
        private const string DocumentKey = "document"; 
        private const string OutputKey = "output";

        static Program()
        {
            CommandLineKeyMappingDictionary = new Dictionary<string, string>
            {
                ["-d"] = DocumentKey,
                ["-o"] = OutputKey,
            };
        }

        static async Task<int> Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddCommandLine(args, CommandLineKeyMappingDictionary)
                .Build();

            var documentPath = config.GetValue<string>(DocumentKey);
            var outputPath = config.GetValue<string>(OutputKey);

            if (string.IsNullOrWhiteSpace(documentPath)
                || string.IsNullOrWhiteSpace(outputPath))
            {
                Console.WriteLine($"Requested command line parameters must be set: {string.Join(", ", CommandLineKeyMappingDictionary.Keys)}");

                return 1;
            }

            try
            {
                //TODO create OpenAPI doc here
                //var settingsOpenApi = new WebApiOpenApiDocumentGeneratorSettings
                //{
                //    //DefaultUrlTemplate = "api/{controller}/{action}/{id}"
                //};

                //Assembly.lo

                //var generator = new WebApiOpenApiDocumentGenerator(settingsOpenApi);
                //var document = await generator.GenerateForControllersAsync() .GenerateForControllerAsync<PersonsController>();
                //var swaggerSpecification = document.ToJson();


                var settings = new CSharpClientGeneratorSettings
                {
                    CSharpGeneratorSettings =
                    {
                        Namespace = "Orange.ApiTokenValidation.Client"
                    },
                    GenerateExceptionClasses = true,
                    GenerateClientInterfaces = true,
                    InjectHttpClient = true,
                    DisposeHttpClient = false,
                    GenerateDtoTypes = true,
                };


                var document = await OpenApiDocument.FromFileAsync(documentPath);
                
                var generator = new CSharpClientGenerator(document, settings);
                var code = generator.GenerateFile();

                await File.WriteAllTextAsync(outputPath, code);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generation client error: {Environment.NewLine}{ex}");
                return -1;
            }

            Console.WriteLine("Client was generated");

            return 0;
        }
    }
}
