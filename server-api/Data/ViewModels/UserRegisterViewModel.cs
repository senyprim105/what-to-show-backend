using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server_api.Data.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage ="Имя обязательно для заполнения")]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Email обязательный для заполнения")]
        [Display(Name="Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Пароль обязательный для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
    public class UserLoginModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
    public class UserViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

}
