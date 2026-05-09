using Microsoft.AspNetCore.Mvc;
using ShowUp2Move.BLL;

namespace ShowUp2Move.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsUser? user = clsUser.Find(userID);

            if (user == null)
                return RedirectToAction("Login", "Account");

            ViewBag.UserSports = clsSport.GetUserSports(userID);
            ViewBag.AllSports = clsSport.GetAll();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(string fullName, string description, string skillLevel)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            bool success = clsUser.UpdateProfile(userID, fullName, description, skillLevel);

            if (success)
            {
                HttpContext.Session.SetString("FullName", fullName);
                TempData["Success"] = "Profile updated.";
            }
            else
                TempData["Error"] = "Failed to update profile.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

           try
            {
                if (photo == null || photo.Length == 0)
                {
                    TempData["Error"] = "Please select a photo.";
                    return RedirectToAction("Index");
                }

                int userID = HttpContext.Session.GetInt32("UserID")!.Value;

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string fileName = $"user_{userID}_{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);
                string photoUrl = $"/uploads/{fileName}";

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    await photo.CopyToAsync(stream);

                clsUser.UpdateProfilePhoto(userID, photoUrl);
                TempData["Success"] = "Photo updated.";
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Upload failed: " + ex.Message;

            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddSport(int sportID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsSport.AddUserSport(userID, sportID);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteSport(int sportID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsSport.DeleteUserSport(userID, sportID);

            return RedirectToAction("Index");
        }
    }
}