using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Campo Obrigatorio")]
        [EmailAddress(ErrorMessage = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "senhas não conferem")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Campo Obrigatorio")]
        [EmailAddress(ErrorMessage = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string Password { get; set; }
    }

    public class UserTokenViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public IEnumerable<ClaimViewModel> Claims { get; set; }
    }

    public class LoginResponseViewModel
    {
        public string AccessToken { get; set; }

        public double ExpiresIn { get; set; }
        public UserTokenViewModel UserToken { get; set; }
    }

    public class ClaimViewModel
    {
        public string Value { get; set; }

        public string Type { get; set; }
    }

}
