using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class Project
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 5)]
        [Required(ErrorMessage = "Name is required")]
        public String Name { get; set; }
        [StringLength(100, MinimumLength = 10)]
        public String Description { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public String Type { get; set; }
        public DateTime ClosedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        [Required(ErrorMessage = "Project manager is required")]
        public int ProjectManagerId { get; set; }
        public string ProjectManagerName { get; set; }
        public String Status { get; set; }
        public Double EstimetedBudget { get; set; }
        public Double? ContractValue { get; set; }
        public Double Cost { get; set; }
        public Double ExtraCost { get; set; }
        public int AllocatedTasks { get; set; }
        public int FinalizedTasks { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }

    }
}
