using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Въведете потребителско име или имейл")]
        [Display(Name = "Потребителско име или имейл")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Въведете парола")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомни ме")]
        public bool RememberMe { get; set; }
    }
}
