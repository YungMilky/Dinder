using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class ProfileViewModel
    {
        public int CurrentUserID { get; set; }
        public IEnumerable<DinderDL.Models.UserEntity> User { get; set; }
        public Dictionary<int, string> Posters { get; set; }
        public IEnumerable<DinderDL.Models.UserEntity> Friends { get; set; }
        public bool FriendRequested { get; set; }
    }
}
