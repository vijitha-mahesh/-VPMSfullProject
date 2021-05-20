using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class ProjectWithCategories
    {
        public List<Project> Projects { get; set; }
        public string type { get; set; }
    }
}
