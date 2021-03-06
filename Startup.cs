﻿using Aiursoft.Pylon;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tracer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAiurMvc();
            services.AddAiurDependencies("Tracer");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAiurUserHandler(env.IsDevelopment());
            app.UseWebSockets();
            app.UseAiursoftDefault();
        }
    }
}
