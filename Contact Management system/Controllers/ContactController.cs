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
        public IActionResult AddContact(Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contact.InsertedUser = Guid.NewGuid(); // Simulating user insertion
                    _context.Contacts.Add(contact);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("AddContact", ex);
                return RedirectToAction("Index");

            }
        }
    }
}
