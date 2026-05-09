using Microsoft.AspNetCore.Mvc;
using ShowUp2Move_BLL;

namespace ShowUp2Move.Controllers
{
    public class ProfileController : Controller
    {
        // GET Profile 
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsUser_BLL? user = clsUser_BLL.Find(userID);

            if (user == null)
                return RedirectToAction("Login", "Account");

            ViewBag.UserSports = clsSport_BLL.GetUserSports(userID);
            ViewBag.AllSports = clsSport_BLL.GetAll();

            return View(user);
        }

        // POST Edit Profile
        [HttpPost]
        public IActionResult Edit(string fullName, string description, string skillLevel)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            bool success = clsUser_BLL.UpdateProfile(userID, fullName, description, skillLevel);

            if (success)
            {
                HttpContext.Session.SetString("FullName", fullName);
                TempData["Success"] = "Profile updated successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to update profile.";
            }

            return RedirectToAction("Index");
        }

        //  POST Add Sport 
        [HttpPost]
        public IActionResult AddSport(int sportID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsSport_BLL.AddUserSport(userID, sportID);

            return RedirectToAction("Index");
        }

        // POST Delete Sport 
        [HttpPost]
        public IActionResult DeleteSport(int sportID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsSport_BLL.DeleteUserSport(userID, sportID);

            return RedirectToAction("Index");
        }
    }
}