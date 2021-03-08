using Dinder.Models;
using DinderDL;
using DinderDL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly UserEntityContext _uecontext;

        public HomeController(ILogger<HomeController> logger, UserEntityContext uecontext)
        {
            _uecontext = uecontext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            /*  This function + HomeModel become overhead to do this, but this is my solution
            *   for three rows of bootstrap user cards in the front page
            */
            List<UserEntity> one = new List<UserEntity>();
            List<UserEntity> two = one;
            List<UserEntity> three = one;

            HomeModel model = new HomeModel();

            try
            {
                /*  this logic is supposed to get the nine latest registered users,
                *   but SQL Server has a feature for its identity column cache where
                *   it increments the identity column by 1000 for security reasons, 
                *   and idk how to work around that right now
                */
                int latestID = _uecontext.Users.Max(u => u.UserID);

                int latestOne = latestID - 3;
                int latestTwo = latestID - 6;
                int latestThree = latestID - 9;

                var usersOne = _uecontext.Users.Where(u => u.UserID > latestOne).ToList();
                one = usersOne;
                
                var usersTwo = _uecontext.Users.Where(u => u.UserID >= latestTwo && u.UserID < latestOne).ToList();
                two = usersTwo;
                
                var usersThree = _uecontext.Users.Where(u => u.UserID >= latestThree && u.UserID < latestTwo).ToList();
                three = usersThree;
            }
            catch (Exception ex)
            {
                Console.WriteLine("HomeModel error: " + ex.Message);
            }

            if (User.Identity.IsAuthenticated)
            {
                var authenticatedUserID = _uecontext.Users.First(u => u.Email == User.Identity.Name).UserID;

                var friendRequestIDs = _uecontext.Friendships.Where(f => f.FriendStatus == false && (f.Friend2ID == authenticatedUserID))
                                                            .Select(f => f.Friend1ID)
                                                            .ToList();
                var friendRequests = _uecontext.Users.Where(u => friendRequestIDs.Contains(u.UserID)).ToList();

                ViewBag.FriendRequests = friendRequests;
            }

            model.UsersOne = one;
            model.UsersTwo = two;
            model.UsersThree = three;

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
