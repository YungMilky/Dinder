using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DinderDL.Models
{
    public class FilesEntity
    {
        [Key]
        public int FileID { get; set; }
        //[StringLength(40)] kommer validering bubbla upp till BL-PL?
        public string Filename { get; set; }
        public byte Filedata { get; set; }

        public int UserID { get; set; }
        public UserEntity User { get; set; }

    }
}
