using Dinder.Data;
using Dinder.Models;
using DinderDL;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Dinder.Controllers
{
    public class UserController : Controller
    {
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

        [Authorize]
        [HttpGet]
        [Route("getProfile/{userid}")]
        public IActionResult Profile(int userid)
        {
            var userModel = _uecontext.Users.Where(u => u.UserID == userid).ToList();

            var friendIDs = _uecontext.Friendships.Where(f => f.FriendStatus == true && (f.Friend1ID == userid || f.Friend2ID == userid))
                                                    .ToList();

            var friends = _uecontext.Users.Where(u => friendIDs
                                            .Select(f => f.Friend1ID).Contains(u.UserID) || friendIDs.Select(f => f.Friend2ID).Contains(u.UserID))
                                            .ToList();

            var profileModel = new ProfileViewModel
            {
                CurrentUserID = _uecontext.Users.First(id => id.Email == User.Identity.Name).UserID,
                User = userModel,
                Friends = friends
            };

            return View(profileModel);
        }

        //public IActionResult Friends(int userid)
        //{
        //    var userModel = _uecontext.Users.Where(u => u.UserID == userid).ToList();
        //    var friendshipModel = _uecontext.Friendships.Where(f => f.Friend1ID == userid).ToList();

        //    var profileModel = new ProfileViewModel
        //    {
        //        User = userModel,
        //        Friends = friendshipModel
        //    };

        //    return View(profileModel);
        //}
    }
}
