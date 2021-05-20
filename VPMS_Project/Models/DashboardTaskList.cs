using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class DashboardTaskList
    {
        public string Name { get; set; }
        public List<Taskz> Tasks{get; set;}
    }
}
