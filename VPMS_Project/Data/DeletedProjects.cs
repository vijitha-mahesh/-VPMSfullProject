using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class DeletedProjects
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public String ProjectManager { get; set; }
        public String Status { get; set; }
        public String Health { get; set; }
        public Double EstimetedBudget { get; set; }
        public Double? ContractValue { get; set; }
        public Double Cost { get; set; }
        public String CustomerName { get; set; }
    }
}
