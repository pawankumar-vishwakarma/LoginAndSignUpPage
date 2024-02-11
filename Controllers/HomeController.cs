using LoginFormASPCore6.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace LoginFormASPCore6.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext context;

        public HomeController(MyDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(User u)
        {
            var myUser = context.Users.Where(x => x.Email == u.Email && x.Password == u.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession",myUser.EmpName);
                return RedirectToAction("Dashboard");
            }
            else{
                ViewBag.Message = "Bad Credential Email or Password.";
            }
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Signup(User u)
        {
            if(ModelState.IsValid)
            {
               await context.Users.AddAsync(u);
                await context.SaveChangesAsync();
                TempData["Success"] = "Registered Successfully";
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["MySession"] = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
            
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}