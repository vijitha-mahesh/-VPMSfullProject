using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class TimeSheetCheck
    {
        [key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int EmpId { get; set; }

        public Boolean? TimeSheet { get; set; }

        public Boolean? EmailSent { get; set; }

    }
}
