

using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager; // Autenticacao usuario
        private readonly UserManager<IdentityUser> _userManager; // cria usuario
        private readonly AppSettings _appSettings; // cria usuario

        public AuthController(INotificador notificador,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<AppSettings> appSettings) : base(notificador)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
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
                return CustomResponse(GerarJsonWebToken());
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
                return CustomResponse(GerarJsonWebToken());
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

        private string GerarJsonWebToken() 
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Gera chave
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            // Gera Token
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            // encoding do token
            var encodingToken = tokenHandler.WriteToken(token); // Serializa um jsonwbtoken

            return encodingToken;
        }
    }
}
