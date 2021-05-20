using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class Time
    {
        [Required]
       public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; } 
    }
}
