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
        public String Name { get; set; } = "No value";

        [StringLength(60)]
        public String Email { get; set; } = "No value";
        public int Phone { get; set; } = 0000000000;

        public String Bio { get; set; } = "";
        public byte[] ProfilePic { get; set; }

#nullable enable
        public FilesEntity? File { get; set; } = null;
#nullable disable

        public virtual ICollection<Friendship> Friendship1 { get; set; }

        public virtual ICollection<Friendship> Friendship2 { get; set; }
        public List<PostsEntity> SentPosts { get; set; }
        public List<PostsEntity> ReceivedPosts { get; set; }
    }
}
