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
}
