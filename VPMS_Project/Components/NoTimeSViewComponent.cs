using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Repository;

namespace VPMS_Project.Component
{
    public class NoTimeSViewComponent : ViewComponent
    {

        private readonly TaskRepo _taskRepo;

        public NoTimeSViewComponent(TaskRepo taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var emps = await _taskRepo.GetNoTimeskAsync();
            return View(emps);
        }


    }
}
