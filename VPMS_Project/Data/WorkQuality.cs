using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class WorkQualityModel
    {
        [key]
        public int Id { get; set; }

        public int TaskId { get; set; }

        public String ProjectName { get; set; }

        public String TaskName { get; set; }

        public String GivenEmp { get; set; }

        public int EmployeesId { get; set; }

        public int Quality { get; set; }

        public int GivenEmpId { get; set; }

        public Employees Employees { get; set; }

        public Tasks Tasks { get; set; }
    }
}
