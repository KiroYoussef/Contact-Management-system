using EntityModels.DTO;
using EntityModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;
        private readonly string _secretKey;

        public ContactService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _secretKey = configuration["SecretKey"];
        }

        public dynamic GetContacts()
        {
            var contactUsers = _context.Contacts
                               .Join(_context.Users,
                                   contact => contact.InsertedUser,
                                   user => user.Id,
                                   (contact, user) => new {
                                       contact.Id,
                                       contact.Name,
                                       contact.Phone,
                                       contact.InsertedUser,
                                       contact.CountryCode,
                                       contact.Address,
                                       contact.Notes,
                                       contact.Email,
                                       User = new
                                       {
                                           user.Id,
                                           user.Username
                                       }
                                   })
                               .ToList();


            return contactUsers;
        }
    }
}
