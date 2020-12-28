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

        public UserController(UserEntityContext uecontext)
        {
            _uecontext = uecontext;
        }

        public IActionResult Index()
        {
            //var users = _uecontext.Users.;

            return View();
        }
    }
}
