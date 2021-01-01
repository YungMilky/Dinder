using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DinderDL.Models
{
    public class PostsEntity
    {
        [Key]
        public int PostID { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public ICollection<UserPosts> UserPosts { get; set; }
    }
}
