using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class ProfileViewModel
    {
        public IEnumerable<DinderDL.Models.UserEntity> User { get; set; }
        public IEnumerable<DinderDL.Models.UserEntity> Friends { get; set; }
    }
}
