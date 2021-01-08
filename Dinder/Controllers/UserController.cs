using Dinder.Data;
using Dinder.Models;
using DinderDL;
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

        public IActionResult Profile()
        {
            var userModel = _uecontext.Users.ToList();

            var user = userModel.Where(u => u.Email == User.Identity.Name).ToList();

            return View(user);
        }
    }
}
