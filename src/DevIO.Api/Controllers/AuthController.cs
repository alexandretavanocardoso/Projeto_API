

using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : MainController
    {
        public AuthController(INotificador notificador) : base(notificador)
        {

        }
    
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUserViewModel)
        {
            return CustomResponse(registerUserViewModel);
        }
    }
}
