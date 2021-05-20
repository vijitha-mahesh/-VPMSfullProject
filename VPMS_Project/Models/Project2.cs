using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class Project2
    {
        public int Id { get; set; }      
        public String Name { get; set; }  
        public String Type { get; set; }   
        public string ProjectManagerName { get; set; }
        public Double Budget { get; set; }
        public Double? ContractValue { get; set; }
        public Double Cost { get; set; }
        public string CustomerName { get; set; }
        public Double CurrentProfit { get; set; }
        public Double TargetProfit { get; set; }
        public Double Completion { get; set; }
        public String Status { get; set; }
    }
}
