using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class PreSalesProjectManager
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Descrption { get; set; }

        public ICollection<PreSalesProjects> PreSalesProjects { get; set; }
    }
}
