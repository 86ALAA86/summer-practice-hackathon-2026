using Microsoft.AspNetCore.Mvc;
using ShowUp2Move.BLL;

namespace ShowUp2Move.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Chat(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.GroupID = id;
            ViewBag.Messages = clsMessage.GetByGroup(id, 50);
            ViewBag.UserID = HttpContext.Session.GetInt32("UserID")!.Value;

            return View();
        }

        [HttpPost]
        public IActionResult Send(int groupID, string content)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            if (!string.IsNullOrWhiteSpace(content))
            {
                int userID = HttpContext.Session.GetInt32("UserID")!.Value;
                int newMsgID = -1;
                clsMessage.Send(groupID, userID, content.Trim(), ref newMsgID);
            }

            return RedirectToAction("Chat", new { id = groupID });
        }
    }
}