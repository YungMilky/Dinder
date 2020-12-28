using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class Files
    {
        [Key]
        public int ProfileID { get; set; }
        public string Filename { get; set; }
        public byte Filedata { get; set; }

    }
}
