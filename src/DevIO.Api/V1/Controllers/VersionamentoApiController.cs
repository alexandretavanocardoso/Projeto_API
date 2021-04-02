using DevIO.Api.Controller;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [ApiVersion("1.0", Deprecated = true /* Obsoleta */)]
    [Route("api/v1/versionamentoApi")]
    [ApiController]
    public class VersionamentoApiController : MainController
    {
        public VersionamentoApiController(INotificador notificador, IUser appUser)
            : base(notificador, appUser)
        {}

        [HttpGet("valor")]
        public string Valor()
        {
            return "V1";
        }
    }
}
