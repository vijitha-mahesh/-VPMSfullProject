using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class Staff_Task
    {
        [key]
       public int Id { get; set; }

        public String TaskName { get; set; }

        public String ProjectName { get; set; }

        public int ProjectId { get; set; }

        public DateTime AppliedDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double TakenHours { get; set; }

        public String Description { get; set; }

        public String Recommend { get; set; }

        public int EmpId { get; set; }

    }
}
