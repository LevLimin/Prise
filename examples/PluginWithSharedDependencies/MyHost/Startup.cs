using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Contract;
using Prise.Infrastructure.NetCore;
using Prise.Infrastructure.NetCore.Contracts;

namespace MyHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor(); // Required to read out HTTP Headers from request

            services.AddPrise<IHelloPlugin>(options => options
                .WithLocalDiskAssemblyLoader("Plugins\\LanguageBased.Plugin")
                .WithPluginAssemblyName("LanguageBased.Plugin.dll")
                .ConfigureSharedServices(services =>
                {
                    services.AddHttpContextAccessor(); // Required to read out HTTP Headers from request
                    services.AddScoped<ISharedLanguageService, AcceptHeaderlanguageService>();
                })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
