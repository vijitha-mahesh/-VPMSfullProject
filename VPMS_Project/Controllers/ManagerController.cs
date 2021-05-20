using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VPMS_Project.Data;
using VPMS_Project.Models;

namespace VPMS_Project.Controllers
{
    [Authorize(Roles ="manager")]
    public class ManagerController : Controller
    {
        private readonly ILogger<ManagerController> _logger;
        private readonly Repo _repo = null;
        private readonly Repo2 _repo2 = null;
        private readonly Repo3 _repo3 = null;
        private readonly Repo4 _repo4 = null;

        public ManagerController(ILogger<ManagerController> logger, Repo repo, Repo2 repo2, Repo3 repo3, Repo4 repo4)
        {
            _logger = logger;
            _repo = repo;
            _repo2 = repo2;
            _repo3 = repo3;
            _repo4 = repo4;

        }

        public async Task<IActionResult> ProjectDetails(string type = "status")
        {
            ViewBag.Total = _repo4.GetCount();
            var data = await _repo4.getlist(type);
            return View(data);
        }

        public async Task<IActionResult> TaskDetails(int id = 0, int pageNumber = 1)
        {
            ViewBag.TodayAllocated = await _repo4.TaskToday();
            ViewBag.TodayStarted = await _repo4.TaskToday2();
            ViewBag.TodayFinalized = await _repo4.TaskToday3();
            
            ViewData["count"] = await _repo2.TaskOverview();
            ViewData["projects"] = await _repo.GetProjects();
            int pageSize = 10;
            
            
                var data1 =  _repo2.GetTasks2Async(id);
                ViewData["tasks"] =await PaginatedList<Tasks>.CreateAsync(data1, pageNumber, pageSize);

            
            return View();
        }

        public async Task<IActionResult> ProjectResources()
        {
            ViewBag.Total =await _repo4.TotalResources();
            ViewBag.Allocated =await _repo4.AllocatedResources();
            ViewBag.NonAllocated =await _repo4.NonAllocatedResources();

            ViewData["nonalloted"] =await _repo4.NonAllocatedResourcesDetailsAsync();
            ViewData["Alloted"] = await _repo2.GetEmployees();
            return View();
        }
    }
}