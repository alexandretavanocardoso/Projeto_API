using DevIO.Api.Controller;
using DevIO.Business.Intefaces;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/versionamentoApi")]
    [ApiExplorerSettings(GroupName = "Teste", IgnoreApi = true)]
    [ApiController]
    public class VersionamentoApiController : MainController
    {
        private readonly ILogger _logger;

        public VersionamentoApiController(INotificador notificador, IUser appUser, ILogger<VersionamentoApiController> logger)
            : base(notificador, appUser)
        {
            _logger = logger;
        }

        [HttpGet("valor")]
        public string Valor()
        {//throw new Exception("Error");

            //try
            //{
            //    var i = 0;
            //    var result = 42 / i;
            //}
            //catch (DivideByZeroException e)
            //{
            //    Envia o erro para o elmah
            //    e.Ship(HttpContext);
            //}

            #region[ Tipos de Logging ]
            _logger.LogTrace("Log de Trace");  // log de desenvolvimento
            _logger.LogDebug("Log de Debug");
            _logger.LogInformation("Log de Informação");
            _logger.LogWarning("Log de Aviso");
            _logger.LogError("Log de Erro");
            _logger.LogCritical("Log de Problema Critico");
            #endregion[ Tipos de Logging ]

            return "V2";
        }
    }
}
