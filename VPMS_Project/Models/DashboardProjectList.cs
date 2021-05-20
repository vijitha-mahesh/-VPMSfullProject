using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class DashboardProjectList
    {
        public string Name { get; set; }
        public List<Project> Projects { get; set; }
    }
}
