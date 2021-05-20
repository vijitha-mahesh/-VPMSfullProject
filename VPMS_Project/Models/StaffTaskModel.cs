using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class StaffTaskModel
    {
        public int Id { get; set; }

        public String TaskName { get; set; }

        public String ProjectName { get; set; }

        public int ProjectId { get; set; }

        public DateTime AppliedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Start Time field is required")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "End Time field is required")]
        public DateTime EndDate { get; set; }

        public double TakenHours { get; set; }

        public String Description { get; set; }

        public String Recommend { get; set; }

        public int EmpId { get; set; }

    }
}
