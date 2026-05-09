using Microsoft.AspNetCore.Mvc;
using ShowUp2Move.BLL;

namespace ShowUp2Move.Controllers
{
    public class VenueController : Controller
    {
        public IActionResult Suggest(int groupID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.GroupID = groupID;
            return View();
        }

        [HttpPost]
        public IActionResult Suggest(int groupID, string venueName, string? address,
                                     string? notes, decimal? priceEst)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            int venueID = -1;

            clsVenue.Add(groupID, userID, venueName, address,
                         null, null, priceEst, notes, ref venueID);

            TempData["Success"] = "Venue suggested!";
            return RedirectToAction("Detail", "Group", new { id = groupID });
        }
    }
}