using Microsoft.AspNetCore.Mvc;
using ShowUp2Move_BLL;

namespace ShowUp2Move.Controllers
{
    public class AccountController : Controller
    {
        // GET Login 
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
                return RedirectToAction("Dashboard", "Home");

            return View();
        }

        // POST Login 
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            clsUser_BLL? user = clsUser_BLL.FindByCredentials(username, password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);

            return RedirectToAction("Dashboard", "Home");
        }

        //  GET Register 
        public IActionResult Register()
        {
            return View();
        }

        //  POST Register 
        [HttpPost]
        public IActionResult Register(string username, string password, string fullName)
        {
            int newUserID = -1;
            bool success = clsUser_BLL.Add(username, password, fullName, ref newUserID);

            if (!success)
            {
                ViewBag.Error = "Username already taken or registration failed.";
                return View();
            }

            HttpContext.Session.SetInt32("UserID", newUserID);
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("FullName", fullName);

            return RedirectToAction("Dashboard", "Home");
        }


        //  Logout 
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}