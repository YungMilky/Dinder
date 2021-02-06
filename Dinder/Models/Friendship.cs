using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class Friendship
    {
        [Key]
        public int FriendshipID { get; set; }

        public bool FriendStatus { get; set; } = false; //not friends by default

        public int Friend1ID { get; set; }
        public int Friend2ID { get; set; }
    }
}
