using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class PreSalesBudget
    {
        public int Id { get; set; }



        public int LbHou { get; set; }

        public int LbRate { get; set; }

        public int MtUnit { get; set; }

        public int MtunitPr { get; set; }

        public int cost { get; set; }

        public int BudgetCost { get; set; }

        public int Actual { get; set; }

        public int Under { get; set; }

        public int ProjectsID { get; set; }



        public PreSalesProjects PreSalesProjects { get; set; }
    }
}
