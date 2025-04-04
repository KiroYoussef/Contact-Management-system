using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Contact_Management_system.Models;
using EntityModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Repository.IRepository;

namespace Contact_Management_system.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly string _secretKey;
    private readonly  IContactService _ContactService;

    public HomeController(ILogger<HomeController> logger , IConfiguration configuration, IContactService contactService)
    {
        _logger = logger;
        _secretKey = configuration["SecretKey"];
        _ContactService = contactService;
    }

    public IActionResult Index()
    {
       var Result = _ContactService.GetContacts(null, null  , null,1,5);
   
        ViewBag.Contacts = Result;

       return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
