using DinderDL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class HomeModel
    {
        public IEnumerable<UserEntity> UsersOne { get; set; }
        public IEnumerable<UserEntity> UsersTwo { get; set; }
        public IEnumerable<UserEntity> UsersThree { get; set; }
    }
}
