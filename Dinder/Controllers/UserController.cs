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
        private readonly UserEntityContext _uecontext;
        private ApplicationDbContext _appDbContext;

        public UserController(UserEntityContext uecontext)
        {
            _uecontext = uecontext;
            var user = _uecontext.Users.SingleOrDefault(u => u.Email == User.Identity.Name);
            if (user == null)
            {
                uecontext.Users.Add(new DinderDL.Models.UserEntity
                {
                    Email = User.Identity.Name
                });
                _uecontext.SaveChanges();
            }
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult ShowUsers()
        {
            var userEntities = _uecontext.Users.ToList();
            //lala
            return View(new UserViewModel { 
            
            });
        }
    }
}
