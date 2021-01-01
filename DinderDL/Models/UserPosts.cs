using System;
using System.Collections.Generic;
using System.Text;

namespace DinderDL.Models
{
    public class UserPosts
    {
        public int UserID { get; set; }
        public UserEntity User { get; set; }
        public int PostID { get; set; }
        public PostsEntity Post { get; set; }
    }
}
