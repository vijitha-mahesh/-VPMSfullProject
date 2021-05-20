using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class PreSalescollection
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public double value { get; set; }

        public int ProjecstID { get; set; }

        public PreSalesProjects PreSalesProjects { get; set; }
    }
}
