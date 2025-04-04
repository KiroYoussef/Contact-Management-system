using EntityModels.DTO;
using EntityModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Contact_Management_system.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected readonly AppDbContext _context;
        private readonly IContactService _ContactService;

        public ContactController(ILogger<HomeController> logger, AppDbContext context, IContactService contactService)
        {
            _logger = logger;
            _context = context;
            _ContactService = contactService;


        }

        [HttpPost]
        public IActionResult AddContact(ContactDTO contact)
        {
            if (ModelState.IsValid)
            {

                Contact CurrentContact = new Contact();
                CurrentContact.Address = contact.Address;
                CurrentContact.Name = contact.Name;
                CurrentContact.Phone = contact.Phone;
                CurrentContact.Email = contact.Email;
                CurrentContact.CountryCode = contact.CountryCode;
                CurrentContact.Notes = contact.Notes;
                CurrentContact.InsertedUser = Guid.Parse(HttpContext.Session.GetString("UserId"));


                _context.Contacts.Add(CurrentContact);
                _context.SaveChanges();

     
            }
            var Result = _ContactService.GetContacts();


            return Json(Result);
        }

        [HttpPost]
        public IActionResult EditContact(ContactDTO model)
        {
             if (ModelState.IsValid)
            {
                var contact = _context.Contacts.Find(model.Id);
                if (contact != null)
                {
                    contact.Name = model.Name;
                    contact.Email = model.Email;
                    contact.CountryCode = model.CountryCode;
                    contact.Phone = model.Phone;
                    contact.Address = model.Address;
                    contact.Notes = model.Notes;

                    _context.Contacts.Update(contact);
                    _context.SaveChanges();
                }
            }
            var Result = _ContactService.GetContacts();


            return Json(Result);
        }

        [HttpGet]
        public IActionResult GetContact(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Json(contact);
        }

        [HttpDelete]
        public IActionResult DeleteContact(int ContactID)
        {
            var contact = _context.Contacts.Find(ContactID);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
            }
            var Result = _ContactService.GetContacts();


            return Json(Result);
        }
        [HttpGet]
        public IActionResult SearchContacts(string name, string phone, string address)
        {
           var Result = _context.Contacts
                .Include(c => c.User)
                .Where(c =>
                    (string.IsNullOrEmpty(name) || c.Name.Contains(name)) &&
                    (string.IsNullOrEmpty(phone) || c.Phone.Contains(phone)) &&
                    (string.IsNullOrEmpty(address) || c.Address.Contains(address)))
                .Include(c => c.User)
                    .Select(c => new {
                        c.Id,
                        c.Name,
                        c.Email,
                        c.Phone,
                        c.CountryCode,
                        c.Address,
                        c.Notes,
                        User = new
                        {
                            c.User.Id,
                            c.User.Username
                        }
                    })
                    .ToList();
                    

            return Json(Result);

        }

    }
}

