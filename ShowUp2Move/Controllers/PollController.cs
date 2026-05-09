using Microsoft.AspNetCore.Mvc;
using ShowUp2Move.BLL;

namespace ShowUp2Move.Controllers
{
    public class PollController : Controller
    {
        public IActionResult Create(int groupID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.GroupID = groupID;
            return View();
        }

        [HttpPost]
        public IActionResult Create(int groupID, string question,
                                    string option1, string option2,
                                    string? option3, string? option4)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            int pollID = -1;

            bool success = clsPoll.Create(groupID, userID, question, null, ref pollID);

            if (success)
            {
                int optID = -1;
                clsPoll.AddOption(pollID, option1, ref optID);
                clsPoll.AddOption(pollID, option2, ref optID);
                if (!string.IsNullOrWhiteSpace(option3)) clsPoll.AddOption(pollID, option3!, ref optID);
                if (!string.IsNullOrWhiteSpace(option4)) clsPoll.AddOption(pollID, option4!, ref optID);

                TempData["Success"] = "Poll created!";
            }
            else
                TempData["Error"] = "Failed to create poll.";

            return RedirectToAction("Detail", "Group", new { id = groupID });
        }

        public IActionResult Results(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.PollID = id;
            ViewBag.Results = clsPoll.GetResults(id);

            return View();
        }

        [HttpPost]
        public IActionResult Vote(int pollID, int optionID, int groupID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsPoll.Vote(pollID, optionID, userID);

            return RedirectToAction("Results", new { id = pollID });
        }
    }
}