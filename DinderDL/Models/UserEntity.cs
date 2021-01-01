using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DinderDL.Models
{
    public class UserEntity
    {
        [Key]
        public int UserID { get; set; }

        [StringLength(60)]
        public String Firstname { get; set; }

        [StringLength(60)]
        public String Lastname { get; set; }

        [StringLength(60)]
        public String Email { get; set; }

        [StringLength(100)]
        public String Password { get; set; }

        public ICollection<UserPosts> UserPosts { get; set; }
        public FilesEntity File { get; set; }
        public virtual ICollection<Friendship> Friend1 { get; set; }
        public virtual ICollection<Friendship> Friend2 { get; set; }
    }
}
