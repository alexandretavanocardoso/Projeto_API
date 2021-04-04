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

            #region [ Versionamento da API ]
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; // Quando não tiver versão especificada sobe a default
                options.DefaultApiVersion = new ApiVersion(1, 0); // Versão 1.0
                options.ReportApiVersions = true; // Passa no header do response para saber se a versao esta Ok
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // Agrupar a versao  da API ("'v'parametros aceitos")
                options.SubstituteApiVersionInUrl = true; // Seta o valor dentro da Api, onde pode ser substituido nas rotas

            });
            #endregion [ Versionamento da API ]

            #region [ Tirando Comportamentos Padroes ]
            // Usado para retirar a forma padrao de erro da modelState
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion [ Tirando Comportamentos Padroes ]

            #region [ Cors ]
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
            #endregion [ Cors ]

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
