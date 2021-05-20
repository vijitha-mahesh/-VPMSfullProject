using System.Linq;
using System.Security.Claims;
using VPMS_Project.Data;
using VPMS_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace VPMS_Project.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private EmpStoreContext _ctx;

        public RoomViewComponent(EmpStoreContext ctx)
        {
            _ctx = ctx;
        }


        public IViewComponentResult Invoke()
        {

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var chats = _ctx.ChatUsers
            .Include(x => x.Chat)
            .Where(x => x.UserId == userId && x.Chat.Type == ChatType.Room)
            .Select(x => x.Chat)
            .ToList();
            return View(chats);
        }
    }
}