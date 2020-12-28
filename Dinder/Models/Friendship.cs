using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class Friendship
    {
        [ForeignKey("User")] //Tror ej detta är rätt
        public int User1ID { get; set; }

        [ForeignKey("User")] //Tror ej detta är rätt
        public int User2ID { get; set; }

        public bool Status { get; set; } = false; //not friends by default
    }
}
