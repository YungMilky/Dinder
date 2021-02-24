﻿using Dinder.Data;
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
            //get profile of user with ID userid
            var userModel = _uecontext.Users.Where(u => u.UserID == userid).Include(u=>u.ReceivedPosts).ToList();

            //get poster IDs of this profile
            var posterID = _uecontext.Posts.Where(p => p.ReceiverID == userid).Select(p => p.PosterID);

            //match poster IDs with any user IDs in the database, to get names
            var posterNames = _uecontext.Users.Where(u => posterID.Contains(u.UserID))
                .ToDictionary(u =>u.UserID, u=>u.Name);

                //.Select(u => new KeyValuePair<int, string>(u.PosterID, u.Name)).ToList();

            var friendIDs = _uecontext.Friendships.Where(f => f.FriendStatus == true && (f.Friend1ID == userid || f.Friend2ID == userid))
                                                    .ToList();

            var friends = _uecontext.Users.Where(u => friendIDs
                                            .Select(f => f.Friend1ID).Contains(u.UserID) || friendIDs.Select(f => f.Friend2ID).Contains(u.UserID))
                                            .ToList();

            return View(new ProfileViewModel
            {
                CurrentUserID = _uecontext.Users.First(id => id.Email == User.Identity.Name).UserID,
                User = userModel,
                Posters = posterNames,
                Friends = friends
            });
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
