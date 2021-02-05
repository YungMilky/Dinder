using Dinder.Data;
using Dinder.Models;
using DinderDL;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Dinder.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _appDbContext;
        private readonly UserEntityContext _uecontext;

        public UserController(UserEntityContext uecontext)
        {
            _uecontext = uecontext;
        }

        public IActionResult Manage()
        {
            ViewBag.Message = "Welcome to my demo!";
            User userModel = new User();
            return View(userModel);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("getProfile/{userid}")]
        public IActionResult Profile(int userid)
        {
            var user = _uecontext.Users.Where(u => u.UserID == userid).ToList();
            return View(user);
        }

        public IActionResult Friends(int userid)
        {

            var questions = _uecontext.Users.Include("Friend1").Select(q => q);

            var friends = _uecontext.Users.Include(s => s.Friend1).Select(s => new { s }).ToList();
            //= (from u in _uecontext.Users
            //           join friend1 in _uecontext.Friendships on u.UserID equals friend1.User1ID into user1
            //           join friend2 in _uecontext.Friendships on u.UserID equals friend2.User2ID into user2
            //           from x in user1.DefaultIfEmpty()
            //           from y in user2.DefaultIfEmpty()
            //           select new
            //           {
            //               u.UserID,
            //               u.Name,
            //               x.User1ID,
            //               y.User2ID
            //           }).ToList();
            return View(questions);
        }
    }
}
