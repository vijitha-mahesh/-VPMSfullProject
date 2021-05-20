using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Repository;

namespace VPMS_Project.Components
{
    public class WorkQualityViewComponent : ViewComponent
    {
        private readonly TaskRepo _taskRepo;

        public WorkQualityViewComponent(TaskRepo taskRepo)
        {
            _taskRepo = taskRepo;
        }


        public async Task<IViewComponentResult> InvokeAsync(string user)
        {
            var Currentuser = await _taskRepo.GetCurrentUser(user);
            int EmpId = Currentuser.EmpId;
            var data = await _taskRepo.GetTasksForWorkQuality(EmpId);
            return View(data);
        }
    }
}