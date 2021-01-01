using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DinderDL.Models
{
    public class Friendship
    {
        [Key]
        public int FriendshipID { get; set; }

        [ForeignKey("User1ID")]
        public UserEntity UserOne { get; set; }
        public int User1ID { get; set; }


        [ForeignKey("User2ID")]
        public UserEntity UserTwo { get; set; }
        public int User2ID { get; set; }

        public bool FriendStatus { get; set; } = false; //not friends by default
    }
}
