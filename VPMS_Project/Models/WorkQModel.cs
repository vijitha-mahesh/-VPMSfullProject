using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class WorkQModel
    {
        public int WorkQId { get; set; }

        public int TaskId { get; set; }

        public String Task { get; set; }

        public String GivenBy { get; set; }

        public String Project { get; set; }

        public int EmployeesId { get; set; }

        public int Quality { get; set; }

        public int GivenEmpId { get; set; }

    }
}
