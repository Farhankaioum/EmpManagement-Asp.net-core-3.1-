using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace EmpManagement
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    
                    //for logging system
                //}).ConfigureLogging((hostingContext, logging) => {

                //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //    logging.AddConsole();
                //    logging.AddDebug();
                //    logging.AddEventSourceLogger();
                    
                }).UseNLog();//register NLog external provider
                
    }
}
