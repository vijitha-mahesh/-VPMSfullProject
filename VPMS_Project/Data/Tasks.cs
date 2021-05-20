using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class Tasks
    {
        [key]
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public String Description { get; set; }
        public Boolean? TimeSheet { get; set; }
        public Boolean? TaskComplete { get; set; }
        public String ProjectManager { get; set; }
        public int ProjectManagerId { get; set; }
        public String ProjectName { get; set; }

        public int EmployeesId { get; set; }
        public String EmployeeName { get; set; }
        public double AllocatedHours { get; set; }
        public int ProjectsId { get; set; }

        public Employees Employees { get; set; }

        public Projects Projects { get; set; }

        public ICollection<TimeSheetTask> TimeSheetTask { get; set; }

    }
}
