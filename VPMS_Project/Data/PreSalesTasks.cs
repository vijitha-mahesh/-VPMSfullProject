using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class PreSalesTasks
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

        public bool IsAssigned { get; set; } //Todo have error

        public string Assignee { get; set; }

        public int ProjectsID { get; set; }

        public PreSalesProjects PreSalesProjects { get; set; }
    }
}
