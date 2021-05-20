using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class Taskz
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ChooseStartDate { get; set; }
        public DateTime? ChooseEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public Boolean? TimeSheet { get; set; }
        public int EmployeeId { get; set; }
        public int projectId { get; set; }
        public String Employee { get; set; }
        public String project { get; set; }
        public int ProjectManagerId { get; set; }
        public String ProjectManager { get; set; }
    }
}
