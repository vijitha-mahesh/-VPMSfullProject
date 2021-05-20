using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class Customers
    {
        [key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
        public int ContactNo { get; set; }
        public ICollection<Projects> Projects { get; set; }
        public ICollection<PreSalesProjects> PreSalesProjects { get; set; }
    }
}
