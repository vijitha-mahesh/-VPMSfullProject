using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class PreSalesDocument
    {
        public int ID { get; set; }

        public string ScopeDocumentUrl { get; set; }

        public string ActionPlanUrl { get; set; }

        public string TimePlanUrl { get; set; }

        public int ProjectsID { get; set; }

        public PreSalesProjects? PreSalesProjects { get; set; }
    }
}
