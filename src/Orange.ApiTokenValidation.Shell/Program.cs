using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Orange.ApiTokenValidation.Shell
{
    public class Program
    {
        private const int SuccessExitCode = 0;
        private const int ErrorExitCode = 1;

        public static async Task<int> Main(string[] args)
        {
            try
            {
                using var host = await Bootstrapper.CreateHostAsync(args);
                
                var logger = (ILogger) host.Services.GetService(typeof(ILogger<Program>));
                logger.LogInformation(I18n.ApplicationLaunch);
                
                await host.RunAsync();
                
                logger.LogInformation(I18n.ApplicationStopped);
                return SuccessExitCode;
            }
            catch (Exception ex)
            {
                //For SystemD approach when u write exception in journal and return no zero exit code 
                Console.WriteLine(I18n.ProgramUnhandledException);
                Console.WriteLine(ex);
                return ErrorExitCode;
            }
        }
    }
}