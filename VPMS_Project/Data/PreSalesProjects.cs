using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class PreSalesProjects
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
        public bool preProjectState { get; set; }

        public Customers Customers { get; set; }

        public PreSalesProjectManager PreSalesprojectManager { get; set; }

        public ICollection<PreSalesDocument> PreSalesDocument { get; set; }

        public ICollection<PreSalesBudget> PreSalesBudget { get; set; }

        public ICollection<PreSalesTasks> PreSalesTasks { get; set; }

        public ICollection<PreSalescollection> PreSalescollections { get; set; }
    }
}
