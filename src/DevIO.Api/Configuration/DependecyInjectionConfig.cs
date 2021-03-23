using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static IServiceCollection ResolveDependecies(this IServiceCollection services)
        {
            services.AddScoped<MeuDbContext>();

            #region [ Repositorys ]
                    services.AddScoped<IFornecedorRepository, FornecedorRepository>();
                    services.AddScoped<IEnderecoRepository, EnderecoRepository>();
                    services.AddScoped<IProdutoRepository, ProdutoRepository>();
            #endregion [ Repositorys ]

            #region [ Services ]
                    services.AddScoped<IFornecedorService, FornecedorService>();
                    services.AddScoped<IProdutoService, ProdutoService>();
                    services.AddScoped<INotificador, Notificador>();
            #endregion [ Services ]

            return services;
        }
    }
}
