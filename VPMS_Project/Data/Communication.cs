using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Data
{
    public class Communication
    {
        [key]
        public int Id { get; set; }

        public int EmployeesId { get; set; }

        public int CommunicationVal { get; set; }

        public int GivenEmpId { get; set; }

        public Employees Employees { get; set; }

    }
}
