using Microsoft.AspNetCore.Mvc;
using ShowUp2Move_BLL;

namespace ShowUp2Move.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsUser_BLL? user = clsUser_BLL.Find(userID);

            if (user == null)
                return RedirectToAction("Login", "Account");

            return View(user);
        }

        // ?? Toggle ShowUpToday 
        [HttpPost]
        public IActionResult ToggleAvailability(bool isAvailable)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsUser_BLL.UpdateAvailability(userID, isAvailable);

            return RedirectToAction("Dashboard");
        }
    }
}