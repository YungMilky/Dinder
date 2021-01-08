using Dinder.Areas.Identity.Pages.Account.Manage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class UserViewModel
    {
        //public IEnumerable<User> Users { get; set; }
        public User Users { get; set; }
    }
}