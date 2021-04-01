using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig(this IServiceCollection services)
        {
            services.AddControllers();

            // Usado para retirar a forma padrao de erro da modelState
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Abre pra quem quiser acessar
            // Politica do Cors nao funciona localmente
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("Development",
            //                      builder => 
            //            builder
            //            .AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials());

            //    // politica Padrao
            //    options.AddDefaultPolicy(builder =>
            //            builder
            //            .AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials());

            //    options.AddPolicy("Production",
            //                      builder =>
            //            builder
            //            .WithMethods("GET")
            //            .WithOrigins("https://google.com")
            //            .SetIsOriginAllowedToAllowWildcardSubdomains()
            //            .WithHeaders(HeaderNames.ContentType, "x-custom-header")
            //            .AllowAnyHeader());
            //});

            return services;
        }

        public static IApplicationBuilder UseMvcConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection(); // Segurança Https
            app.UseRouting();
            app.UseAuthorization();

            return app;
        }

    }
}
