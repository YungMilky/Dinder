using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DinderDL.Models
{
    public class UserEntity
    {
        [Key]
        public int UserID { get; set; }

        [StringLength(60)]
        public String Name { get; set; }

        [StringLength(60)]
        public String Email { get; set; }
        public int Phone { get; set; }

        public String Bio { get; set; }

        public ICollection<UserPosts> UserPosts { get; set; }
        public FilesEntity File { get; set; }


        public virtual ICollection<Friendship> Friendship1 { get; set; }

        public virtual ICollection<Friendship> Friendship2 { get; set; }
    }
}
