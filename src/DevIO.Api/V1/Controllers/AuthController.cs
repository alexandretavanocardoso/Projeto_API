

using DevIO.Api.Controller;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/Autenticacao")]
    [ApiController]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager; // Autenticacao usuario
        private readonly UserManager<IdentityUser> _userManager; // cria usuario
        private readonly AppSettings _appSettings; // cria usuario
        private readonly ILogger _logger;

        public AuthController(INotificador notificador,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<AppSettings> appSettings,
                              IUser user,
                              ILogger<AuthController> logger) : base(notificador, user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _logger = logger;
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
                return CustomResponse(await GerarJsonWebToken(registerUser.Email));
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
                // Logger
                _logger.LogInformation("usuário" + loginUser.Email + " logado com sucesso");
                return CustomResponse(await GerarJsonWebToken(loginUser.Email));
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

        private async Task<LoginResponseViewModel> GerarJsonWebToken(string email) 
        {
            // Pega o usuario
            var user = await _userManager.FindByEmailAsync(email);
            // Pega a Claim
            var claim = await _userManager.GetClaimsAsync(user);
            // Pega os Roles - Perfis
            var roles = await _userManager.GetRolesAsync(user);

            // Pessando Claims que ja sao passadas na geração do Token
            // Porem para ter certeza exister esses trechos de códigos
            claim.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id)); // usuario
            claim.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claim.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // quando token tem id proprio
            claim.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString())); 
            claim.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            // Adicionando na coleção as roles
            foreach (var role in roles)
            {
                claim.Add(new Claim("role", role));
            }

            // Convertendo as claims para o IdentityClaims
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claim);

            
            var tokenHandler = new JwtSecurityTokenHandler();
            // Gera chave
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            // Gera Token
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims, // passando as claims para o token
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            // encoding do token
            var encodingToken = tokenHandler.WriteToken(token); // Serializa um jsonwbtoken

            // Mostrando os dados do usuario daquele token gerado
            var response = new LoginResponseViewModel() {
                AccessToken = encodingToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claim.Select(claim => new ClaimViewModel {Type = claim.Type, Value = claim.Value})
                }
            };

            return response;
        }

        // metodo para retornas uma data
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
