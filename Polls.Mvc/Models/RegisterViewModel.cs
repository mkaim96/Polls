using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Polls.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasło i jego potwierdzenie są niezgodne.")]
        public string ConfirmPassword { get; set; }
    }
}
