using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DinderDL.Models
{
    public class FilesEntity
    {
        [Key]
        public int FileID { get; set; }

        public string Filename { get; set; }
        public string FilePath { get; set; }

        public int UserID { get; set; }
        public UserEntity User { get; set; }

    }
}
