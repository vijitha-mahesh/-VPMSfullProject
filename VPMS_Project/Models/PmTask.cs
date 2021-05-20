using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class PmTask
    {
        public String Name { get; set; }
        public IList<Taskz> Tasks { get; set; }
    }
}
