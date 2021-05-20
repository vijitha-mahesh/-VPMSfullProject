using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VPMS_Project.Models
{
    public class PreTaskModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the task number here")]
        public int Number { get; set; }

        [Required]
        [DataType(DataType.Duration)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Enter the duration hours here ")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Enter the task start date & time here")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Enter the task end date & time here")]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Enter the task assigner here")]
        public int Priority { get; set; }

        public bool IsAssigned { get; set; } //Todo have error

        [Required]
        public string Assignee { get; set; }

        [Required(ErrorMessage = "Please select project")]
        [Display(Name = "Project Name")]
        public int ProjectsID { get; set; }

        public string projectName { get; set; }



    }
}
