using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class PreSalesDeletedProjects
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ProjectType { get; set; }

        public Double value { get; set; }

        public Double ProjectBudget { get; set; }

        public int CustomersId { get; set; }

        public int projectManagerId { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }
}
