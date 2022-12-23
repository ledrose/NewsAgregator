using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NewsAgregator.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public String ConfirmPassword { get; set; }
    }
}
