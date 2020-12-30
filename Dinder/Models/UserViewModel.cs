using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class UserViewModel
    {
        public List<User> Users { get; set; }
    }
}