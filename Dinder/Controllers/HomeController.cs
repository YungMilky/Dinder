using Dinder.Models;
using DinderDL;
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
            int latestID = _uecontext.Users.Max(u => u.UserID);

            int latestOne = latestID - 3;
            int latestTwo = latestID - 6;
            int latestThree = latestID - 9;

            var usersOne = _uecontext.Users.Where(u => u.UserID > latestOne).ToList();
            var usersTwo = _uecontext.Users.Where(u => u.UserID >= latestTwo && u.UserID < latestOne).ToList();
            var usersThree = _uecontext.Users.Where(u => u.UserID >= latestThree && u.UserID < latestTwo).ToList();

            return View(new HomeModel { 
                UsersOne = usersOne,
                UsersTwo = usersTwo,
                UsersThree = usersThree
            });
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
