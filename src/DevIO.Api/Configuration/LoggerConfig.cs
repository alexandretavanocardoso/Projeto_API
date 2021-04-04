using System;
using DevIO.Api.Extensions;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services)
        {
            // Tem que criar sua conta para gera seu log
            services.AddElmahIo(o =>
            {
                o.ApiKey = "388dd3a277cb44c4aa128b5c899a3106";
                o.LogId = new Guid("c468b2b8-b35d-4f1a-849d-f47b60eef096");
            });

            #region [ Conectando o elmah com o Log do asp core, transformando em um provider ]
            // Tem que instalar o pacote Elmah.Io.Extensions.Log
            //services.AddLogging(builder =>
            //{
            //    builder.AddElmahIo(object => {
            //        o.ApiKey = "388dd3a277cb44c4aa128b5c899a3106";
            //        o.LogId = new Guid("c468b2b8-b35d-4f1a-849d-f47b60eef096");
            //    });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, logLevel.Warning);
            //});
            #endregion[ Conectando o elmah com o Log do asp core, transformando em um provider ]

            //services.AddHealthChecks()
            //    .AddElmahIoPublisher(options =>
            //    {
            //        options.ApiKey = "388dd3a277cb44c4aa128b5c899a3106";
            //        options.LogId = new Guid("c468b2b8-b35d-4f1a-849d-f47b60eef096");
            //        options.HeartbeatId = "API Fornecedores";

            //    })
            //    .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
            //    .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}