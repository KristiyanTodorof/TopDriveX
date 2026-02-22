using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Dtos
{
    public class UserSettingsDto
    {
        [Required(ErrorMessage = "Моля въведете име")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Името трябва да е между 2 и 50 символа")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля въведете фамилия")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилията трябва да е между 2 и 50 символа")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля въведете имейл")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Невалиден телефонен номер")]
        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }


        public string UserName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }


        [Display(Name = "Имейл известия за нови съобщения")]
        public bool EmailNotificationsMessages { get; set; } = true;

        [Display(Name = "Имейл известия за нови коментари")]
        public bool EmailNotificationsComments { get; set; } = true;

        [Display(Name = "Имейл известия за промени в цената на любими обяви")]
        public bool EmailNotificationsPriceChanges { get; set; } = true;

        [Display(Name = "SMS известия за важни съобщения")]
        public bool SmsNotifications { get; set; } = false;


        [Display(Name = "Покажи телефонния номер публично")]
        public bool ShowPhonePublicly { get; set; } = true;

        [Display(Name = "Покажи имейла публично")]
        public bool ShowEmailPublicly { get; set; } = false;

        [Display(Name = "Позволи на потребители да ми пишат директно")]
        public bool AllowDirectMessages { get; set; } = true;
    }
}
