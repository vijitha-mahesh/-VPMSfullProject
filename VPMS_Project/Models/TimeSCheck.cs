using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class TimeSCheck
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int EmpId { get; set; }

        public String EmpName { get; set; }

        public Boolean? TimeSheet { get; set; }

        public Boolean? EmailSent { get; set; }

        public String PhotoURL { get; set; }
    }
}
