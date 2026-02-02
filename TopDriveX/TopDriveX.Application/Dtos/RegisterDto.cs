using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Dtos
{
    public class RegisterDto
    {
        // ==================== ACCOUNT INFO ====================

        [Required(ErrorMessage = "Потребителското име е задължително")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Потребителското име трябва да е между 3 и 50 символа")]
        [Display(Name = "Потребителско име")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Паролата е задължителна")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да е поне 6 символа")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Потвърдете паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат")]
        [DataType(DataType.Password)]
        [Display(Name = "Потвърди парола")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // ==================== PERSONAL INFO ====================

        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(100, ErrorMessage = "Името не може да е повече от 100 символа")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилията е задължителна")]
        [StringLength(100, ErrorMessage = "Фамилията не може да е повече от 100 символа")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефонът е задължителен")]
        [Phone(ErrorMessage = "Невалиден телефонен номер")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; } = string.Empty;

        // ==================== LOCATION ====================

        [StringLength(100)]
        [Display(Name = "Град")]
        public string? City { get; set; }

        // ==================== USER TYPE ====================

        [Required(ErrorMessage = "Изберете тип потребител")]
        [Display(Name = "Тип потребител")]
        public string UserType { get; set; } = "Private"; // Private or Dealer

        // ==================== DEALER SPECIFIC (Optional) ====================

        [StringLength(200)]
        [Display(Name = "Име на автокъщата")]
        public string? DealershipName { get; set; }

        [StringLength(500)]
        [Display(Name = "Адрес на автокъщата")]
        [DataType(DataType.MultilineText)]
        public string? DealershipAddress { get; set; }

        // ==================== TERMS ====================

        [Required(ErrorMessage = "Трябва да приемете условията за ползване")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Трябва да приемете условията за ползване")]
        [Display(Name = "Приемам условията за ползване")]
        public bool AcceptTerms { get; set; }
    }
}
