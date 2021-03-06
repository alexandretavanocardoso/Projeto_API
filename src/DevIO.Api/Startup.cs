using DevIO.Api.Configuration;
using DevIO.Api.Extensions;
using DevIO.Data.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api
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
            services.AddDbContext<MeuDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddIdentityConfiguration(Configuration);

            // Usando Mapper
            services.AddAutoMapper(typeof(Startup));

            services.WebApiConfig();

            #region [ Documentação Swagger ]
            services.AddSwaggerConfig();
            #endregion [ Documentação Swagger ]

            #region [ Elmah.io, Healt Checks ]
            services.AddLoggingConfig(Configuration);
            #endregion [ Elmah.io, Healt Checks ]

            // Inicializa as injeções de dependecias - Metodo Criado
            services.ResolveDependecies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                //app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            } 
            else
            {
                //app.UseCors("Production");
                app.UseHsts(); // Segurança de https
            }

            app.UseAuthentication();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMvcConfiguration();

            #region [ Swagger ]
            app.UseSwaggerConfig(provider);
            #endregion [ Swagger ]

            #region [ Elmah.io, HealthChecks ]
            app.UseLoggingConfiguration();
            #endregion [ Elmah.io, HealthChecks ]
        }
    }
}
