

using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager; // Autenticacao usuario
        private readonly UserManager<IdentityUser> _userManager; // cria usuario

        public AuthController(INotificador notificador,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager) : base(notificador)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("novaConta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            // Criando usuario
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            // Gera usuario
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                // ja faz o login do usuario caso o result for sucesso
                await _signInManager.SignInAsync(user, false); // SignInAsync(usuario, Se é persistente)
                return CustomResponse(registerUser);
            }

            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }


            return CustomResponse(registerUser);
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(loginUser);
            }

            // se tiver bloqueado
            if (result.IsLockedOut)
            {
                NotificarErro("usuario Bloqueado");
                return CustomResponse(loginUser);
            }

            // Caso errar os campos
            NotificarErro("usuario e senha incorreto");
            return CustomResponse(loginUser);
        }
    }
}
