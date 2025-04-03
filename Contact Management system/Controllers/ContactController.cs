using EntityModels.DTO;
using EntityModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contact_Management_system.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected readonly AppDbContext _context;

        public ContactController(ILogger<HomeController> logger, AppDbContext context )
        {
            _logger = logger;
            _context = context;

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
                TempData["SuccessMessage"] = "Contact added successfully!";
                return RedirectToAction("Index", "Home");
            }
            TempData["ErrorMessage"] = "Failed to add contact.";
            return RedirectToAction("Index", "Home");
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
                    TempData["SuccessMessage"] = "Contact updated successfully!";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["ErrorMessage"] = "Failed to update contact.";
            return RedirectToAction("Index", "Home");
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
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Contact not found." });
        }
    }
}

