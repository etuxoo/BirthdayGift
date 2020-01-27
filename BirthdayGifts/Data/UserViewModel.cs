using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayGifts.Data
{
    public class UserViewModel
    {
        [Required]
        public DateTime BirthDate { get; set; }

        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        [MaxLength(32)]
        public string Password { get; set; }

        [Required]
        [MaxLength(32)]
        public string UserName { get; set; }
    }
}
