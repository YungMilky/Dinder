using Dinder.Data;
using Dinder.Models;
using DinderDL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
