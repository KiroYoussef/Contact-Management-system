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

        public List<Contact> GetContacts()
        {
            var contactUsers = _context.Contacts
                                .Join(_context.Users,
                                    contact => contact.InsertedUser,
                                    user => user.Id,
                                    (contact, user) => new Contact
                                    {
                                        Id = contact.Id,
                                        Name = contact.Name,
                                        Phone = contact.Phone,
                                        InsertedUser = user.Id,
                                        CountryCode = contact.CountryCode,
                                        Address = contact.Address,
                                        Notes = contact.Notes,
                                        Email = contact.Email,
                                        User = user
                                    }).ToList();
            return contactUsers;
        }
    }
}
