using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DinderDL.Models
{
    public class PostsEntity
    {
        [Key]
        public int PostID { get; set; }
        public string Content { get; set; }


        [ForeignKey("Poster")]
        public int PosterID { get; set; }
        public UserEntity Poster { get; set; }


        [ForeignKey("Receiver")]
        public int ReceiverID { get; set; }
        public UserEntity Receiver { get; set; }
    }
}
