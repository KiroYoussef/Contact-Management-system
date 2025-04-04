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
        private readonly IContactService _ContactService;

        public ContactController(ILogger<HomeController> logger, IContactService contactService)
        {
            _logger = logger;
            _ContactService = contactService;


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddContact(ContactDTO contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var IsAdded = _ContactService.AddContact(contact, Guid.Parse(HttpContext.Session.GetString("UserId")));
                }
                var Result = _ContactService.GetContacts();


                return Json(Result);
            }catch(Exception e)
            {
                return Json(new { error = e.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditContact(ContactDTO model)
        {
            try
            {
                 if (ModelState.IsValid)
                 {
                    var IsAdded = _ContactService.EditContact(model);
                 }
                var Result = _ContactService.GetContacts();


                return Json(Result);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }

        }

        [HttpGet]
        public IActionResult GetContact(int id)
        {
            try
            {
             var contact = _ContactService.GetContact(id);
                if (contact == null)
                {
                    return NotFound();
                }
                return Json(contact);
                }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteContact(int ContactID)
        {
            try
            {
                var IsRemoved = _ContactService.DeleteContact(ContactID);

                var Result = _ContactService.GetContacts();

                return Json(Result);

            }catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }
        [HttpGet]
        public IActionResult SearchContacts(string name = null, string phone = null, string address = null, int page = 1, int pageSize = 5)
        {
            try
            {
                var Result = _ContactService.GetContacts(name, phone, address, page, pageSize);

            return Json(Result);

            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }

    }
}

