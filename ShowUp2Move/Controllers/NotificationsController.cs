using Microsoft.AspNetCore.Mvc;
using ShowUp2Move.BLL;

namespace ShowUp2Move.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsNotification.MarkAllRead(userID);

            return View(clsNotification.GetByUser(userID));
        }
    }
}