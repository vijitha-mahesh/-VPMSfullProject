using Microsoft.EntityFrameworkCore;
using VPMS_Project.Data;
using VPMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Repository
{
    public class ProjectManagerRepository
    {
        private readonly EmpStoreContext _context = null;

        public ProjectManagerRepository(EmpStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetProjectManager(String title)
        {
            var data1 = _context.Employees.Where(y => y.Designation == title).OrderBy(x => x.EmpFName);
            var data = await data1.Select(x => new Employee()
            {

                Id = x.EmpId,
                Name = x.EmpFName + " " + x.EmpLName,
                Designation = x.Designation
            }).ToListAsync();

            return data;
        }
    }

}
