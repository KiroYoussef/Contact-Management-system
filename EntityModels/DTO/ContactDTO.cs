using EntityModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels.DTO
{
    public class ContactDTO
    {
        public int? Id { get; set; }= 1;

        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Country Code is required")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be 11 characters long")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string Address { get; set; }
        [Required]
        public string Notes { get; set; }

        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]

        public string Email { get; set; }



    }
}
