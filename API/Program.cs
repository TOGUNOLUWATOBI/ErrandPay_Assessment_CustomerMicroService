using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = CreateHostBuilder(args).Build();
            
            //using(var serviceScope = host.Services.CreateScope())
            //{



            //    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    if(!await roleManager.RoleExistsAsync("SuperAdmin"))
            //    {
            //        var superAdminRole = new IdentityRole("SuperAdmin");
            //        await roleManager.CreateAsync(superAdminRole);
            //    }
            //    if (!await roleManager.RoleExistsAsync("Staff"))
            //    {
            //        var staffRole = new IdentityRole("Staff");
            //        await roleManager.CreateAsync(staffRole);
            //    }
            //    if (!await roleManager.RoleExistsAsync("User"))
            //    {
            //        var userRole = new IdentityRole("User");
            //        await roleManager.CreateAsync(userRole);
            //    }
            //}

            //host.Run();

            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new AutoFacContainerModule());
            });
    }
}
