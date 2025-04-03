using EntityModels.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using System.Security.Claims;

namespace Contact_Management_system.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUserService _UserService;
        private readonly string _SessionTimeout;

        public AccountController(IUserService UserService, IConfiguration configuration)
        {
            _UserService = UserService;
            _SessionTimeout = configuration["SessionTimeoutHour"];

        }
        [HttpGet]
        public IActionResult login()
        {
            return View("login");

        }
        [HttpPost]
        public async Task<IActionResult> login(LoginDTO user)

        {
          var Result= _UserService.Login(user);
            if (!string.IsNullOrEmpty( Result.Result.Username))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };

                // set user Data in session 
                HttpContext.Session.SetString("UserId", Result.Result.Id.ToString()); 
                HttpContext.Session.SetString("FirstNameUser", Result.Result.FirstName); 
                HttpContext.Session.SetString("MiddleNameUser", Result.Result.MiddleName); 
                HttpContext.Session.SetString("LastNameUser", Result.Result.LastName);


                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = user.StayLogin, 
                    ExpiresUtc = DateTime.UtcNow.AddHours(int.Parse(_SessionTimeout))
                };

                 await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Msg = "Username or Password Not Correct";
                return View("login");
            }
         
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}
