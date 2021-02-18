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
            //show 24 latest profiles
            int latestID = _uecontext.Users.Max(u => u.UserID);
            int latest24 = latestID - 10;
            var user = _uecontext.Users.Where(u => u.UserID >= latest24).ToList();
            return View(user);
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
