using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class TaskCount
    {
        public int Total { get; set; }
        public int Ongoing { get; set; }
        public int TimeOut { get; set; }
        public int Finalized { get; set; }
        public int Pending { get; set; }
        public int Today { get; set; }
    }
}
