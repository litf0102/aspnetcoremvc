using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace aspnetcoremvc
{
    public class Program
    {
        delegate int add(int i);
        public static void Main(string[] args)
        {
            // Test Lamada
            List<string> listString = new List<string>();
            listString.Add("abc");
            listString.Add("456");
            listString.Add("bbb");
            listString.Add("iiiiiiii");

            var retList = listString.Where(u => u.Length > 4);
            foreach (string str in retList)
            {
                Console.WriteLine("str = {0}", str);
            }

            // Test Deleagte
            add myDelegate = x => x + 5;
            int ret = myDelegate(5);
            Console.WriteLine("ret = {0}", ret);

            // ASP.NET Core MVC 
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // DI
                .UseDefaultServiceProvider(options =>
                {
                    options.ValidateScopes = false;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
