using Dinder.Data;
using Dinder.Models;
using DinderDL;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

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
        public IActionResult Profile(int? userid)
        {
            //get profile of user with ID userid
            var userModel = _uecontext.Users.Where(u => u.UserID == userid).Include(u=>u.ReceivedPosts).ToList();

            //get poster IDs of this profile
            var posterID = _uecontext.Posts.Where(p => p.ReceiverID == userid).Select(p => p.PosterID);

            //get poster IDs with any user IDs in the database, along with poster names, to later display names by poster IDs in the view
            var posterNames = _uecontext.Users.Where(u => posterID.Contains(u.UserID))
                                                .ToDictionary(u =>u.UserID, u=>u.Name);

            var friendIDs = _uecontext.Friendships.Where(f => f.FriendStatus == true && (f.Friend1ID == userid || f.Friend2ID == userid))
                                                    .ToList();

            var friends = _uecontext.Users.Where(u => friendIDs
                                            .Select(f => f.Friend1ID).Contains(u.UserID) || friendIDs.Select(f => f.Friend2ID).Contains(u.UserID))
                                            .ToList();

            if (User.Identity.IsAuthenticated)
            {
                var authenticatedUserID = _uecontext.Users.First(u => u.Email == User.Identity.Name).UserID;

                var friendRequestIDs = _uecontext.Friendships.Where(f => f.FriendStatus == false && (f.Friend1ID == authenticatedUserID || f.Friend2ID == authenticatedUserID))
                                                            .ToList();
                var friendRequests = _uecontext.Users.Where(u => friendRequestIDs
                                            .Select(f => f.Friend1ID).Contains(u.UserID))
                                            .ToList();

                ViewBag.FriendRequests = friendRequests;
            }

            return View(new ProfileViewModel
            {
                CurrentUserID = _uecontext.Users.First(id => id.Email == User.Identity.Name).UserID,
                User = userModel,
                Posters = posterNames,
                Friends = friends
            });
        }

        [Route("upsertProfilePic")]
        [HttpPost]
        public IActionResult UpsertProfilePic(User user)
        {
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    file.CopyTo(dataStream);
                    user.ProfilePic = dataStream.ToArray();
                }
            }
            var dbUser = _uecontext.Users.First(u => u.Email == User.Identity.Name);
            dbUser.ProfilePic = user.ProfilePic;

            _uecontext.SaveChanges();
            return Ok();
        }
    }
}
