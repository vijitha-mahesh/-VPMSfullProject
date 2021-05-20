using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 5)]
        [Required(ErrorMessage = "Name is required")]
        public String Name { get; set; }
        public String Designation { get; set; }
        public int projectManagerId { get; set; }

        public string ProjectManager { get; set; }
    }
}
