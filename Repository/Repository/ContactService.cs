using Azure;
using EntityModels.DTO;
using EntityModels.Models;
using Microsoft.AspNetCore.Http;
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

        public int AddContact(ContactDTO contact , Guid User)
        {
            Contact CurrentContact = new Contact();
            CurrentContact.Address = contact.Address;
            CurrentContact.Name = contact.Name;
            CurrentContact.Phone = contact.Phone;
            CurrentContact.Email = contact.Email;
            CurrentContact.CountryCode = contact.CountryCode;
            CurrentContact.Notes = contact.Notes;
            CurrentContact.InsertedUser = User;


            _context.Contacts.Add(CurrentContact);
           return _context.SaveChanges();
        }

        public int EditContact(ContactDTO Contact)
        {
            var contact = _context.Contacts.Find(Contact.Id);
            if (contact != null)
            {
                contact.Name = Contact.Name;
                contact.Email = Contact.Email;
                contact.CountryCode = Contact.CountryCode;
                contact.Phone = Contact.Phone;
                contact.Address = Contact.Address;
                contact.Notes = Contact.Notes;

                _context.Contacts.Update(contact);
               return _context.SaveChanges();
            }
            return 0;
        } 
        public int DeleteContact(int ContactID)
        {
            var contact = _context.Contacts.Find(ContactID);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
               return _context.SaveChanges();
            }
            return 0;
        }
        public Contact GetContact(int ContactID)
        {
            return _context.Contacts.Find(ContactID);
        }
        public dynamic GetContacts( string name = null, string phone = null, string address = null, int page = 1, int pageSize = 10)
        {
            var contactUsers = _context.Contacts
                                .Where(c =>
                                    (string.IsNullOrEmpty(name) || c.Name.Contains(name)) &&
                                    (string.IsNullOrEmpty(phone) || c.Phone.Contains(phone)) &&
                                    (string.IsNullOrEmpty(address) || c.Address.Contains(address)))
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
                                   }).Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                       .ToList();


            var contactCount = _context.Contacts.Where(c =>
            (string.IsNullOrEmpty(name) || c.Name.Contains(name)) &&
            (string.IsNullOrEmpty(phone) || c.Phone.Contains(phone)) &&
            (string.IsNullOrEmpty(address) || c.Address.Contains(address))).Count();
                          
            return new { Contacts = contactUsers, TotalCount = contactCount } ;
        }
    }
}
