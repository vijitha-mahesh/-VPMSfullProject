using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class Projects
    {

        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public DateTime ClosedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public String Status { get; set; }
        public Double EstimetedBudget { get; set; }
        public Double? ContractValue { get; set; }
        public Double Cost { get; set; }
        public int? CustomersId { get; set; }
        public int EmployeesId { get; set; }
        public int AllocatedTasks { get; set; }
        public int FinalizedTasks { get; set; }
        public int PreProjectId { get; set; }
        public Employees Employees { get; set; }
        public Customers Customers { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
    }
}
