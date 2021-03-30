using DevIO.Api.Data;
using DevIO.Api.Extensions;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Conexão com o banco
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityMensagemPortugues>()
                .AddDefaultTokenProviders();
                
             // JWT - Json Web Token

            // Configurando
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Obtendo os dados
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret); // Definindo uma chave

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // gera token
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // quando gera token ver a validacao
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false; // se for trabalhar apenas com https - deixar true
                x.SaveToken = true; // Se token deve ser guardado após autenticacao com sucesso
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Valida se o emitor tem que ser o mesmo do token com base na chave
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Configurando chave
                    ValidateIssuer = true, // Valida o emitor
                    ValidateAudience = true, // Valida onde o token é valido
                    ValidAudience = appSettings.ValidoEm, // configurando Audience
                    ValidIssuer = appSettings.Emissor // Configurando emissor
                };
            });
            
            return services;
        }
    }
}
