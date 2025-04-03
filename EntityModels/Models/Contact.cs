using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels.Models
{
    public class Contact
    {

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string Phone { get; set; }

        [MaxLength(250 , ErrorMessage = "Address cannot exceed 250 characters")]
        public string Address { get; set; }

        public string Notes { get; set; }
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }

        public Guid InsertedUser { get; set; }

        [ForeignKey("InsertedUser")]
        public User User { get; set; }
        public ICollection<EditLock> EditLocks { get; set; } = new List<EditLock>();





    }
}
