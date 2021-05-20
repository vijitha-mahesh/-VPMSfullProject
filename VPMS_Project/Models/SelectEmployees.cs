using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VPMS_Project.Models
{
    public class SelectEmployees
    {
        [Required(ErrorMessage = "Start date and time is required")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "End date and time is required")]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Project manager is required")]
        public int ProjectManagerId { get; set; }
    }
}
