using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class FileViewModel
    {
        [Key]
        public int ProfileID { get; set; }
        public IFormFile Image { get; set; }

    }
}
