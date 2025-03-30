using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<EditLock> EditLocks { get; set; } = new List<EditLock>();


        public void HashPassword(string password, string secretKey)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password + secretKey);
        }

        public bool VerifyPassword(string password, string secretKey)
        {
            return BCrypt.Net.BCrypt.Verify(password + secretKey, PasswordHash);
        } 
    }
}
