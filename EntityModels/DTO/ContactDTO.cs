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


        [Required, MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string Address { get; set; }

        public string Notes { get; set; }

        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }



    }
}
