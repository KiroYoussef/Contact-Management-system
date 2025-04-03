using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Contact_Management_system.Models;
using EntityModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Contact_Management_system.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    protected readonly AppDbContext _context;
    private readonly string _secretKey;

    public HomeController(ILogger<HomeController> logger , AppDbContext context, IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _secretKey = configuration["SecretKey"];

    }
    public IActionResult Index()
    {
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
