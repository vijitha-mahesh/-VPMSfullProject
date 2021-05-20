using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VPMS_Project.Models;


namespace WebApplication3.Controllers
{
    [Authorize(Roles = "admin")]
    public class TaskController : Controller
    {
        private readonly Repo _repo = null;
        private readonly Repo2 _repo2 = null;
        private readonly Repo3 _repo3 = null;
        public TaskController(Repo repo, Repo2 repo2, Repo3 repo3)
        { 
            _repo = repo;
            _repo2 = repo2;
            _repo3 = repo3;
        }

        public async Task<IActionResult> SelectEmployee(bool currentContext = false, bool invalid = false)
        {
            ViewBag.context = currentContext;
            ViewBag.invalid = invalid;
            ViewBag.ProjectManager = new SelectList(await _repo2.GetEmployeeByTitle("project manager"), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SelectEmployee(SelectEmployees emp)
        {
            if((emp.StartDate < DateTime.Now) || (emp.EndDate < DateTime.Now)){
                return RedirectToAction(nameof(SelectEmployee), new { currentContext = true });
            }
            if(emp.StartDate >= emp.EndDate)
            {
                return RedirectToAction(nameof(SelectEmployee), new { invalid = true });
            }


              var ids = await _repo2.SelectEmployees(emp);
               
              return RedirectToAction(nameof(AddTask), new {managerId =emp.ProjectManagerId, idss = ids ,start= emp.StartDate, end= emp.EndDate});
                
        }


        public async Task<IActionResult> AddSalaryInfo(bool msg = false, bool delete =false)
        {
            ViewBag.msg = msg;
            ViewBag.delete = delete;
            var data = await _repo2.GetSalaryInfo();
            ViewData["salary"] = data;
            ViewBag.titles = new SelectList(await _repo2.GetTitles(), "JobId", "JobName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSalaryInfo(Salary salary)
        {
            bool success = await _repo2.AddSalaryInfo(salary);
            if(success == true)
            {
                return RedirectToAction(nameof(AddSalaryInfo), new { msg = success });
            }
            var data = await _repo2.GetSalaryInfo();
            ViewData["salary"] = data;
            ViewBag.titles = new SelectList(await _repo2.GetTitles(), "JobId", "JobName");
            return View();
        }

        public async Task<IActionResult> EditSalaryInfo(bool msg = false , int id =0)
        {
            ViewBag.msg = msg;
            var data = await _repo2.GetSalaryInfo();
            ViewData["salary"] = data;
            var model = await _repo2.GetRateById(id);
            ViewBag.titles = new SelectList(await _repo2.GetTitles(), "JobId", "JobName");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditSalaryInfo(Salary salary)
        {
            bool success = await _repo2.EditSalaryInfo(salary);
            if (success == true)
            {
                return RedirectToAction(nameof(EditSalaryInfo), new { msg = success });
            }
            var data = await _repo2.GetSalaryInfo();
            ViewData["salary"] = data;
            ViewBag.titles = new SelectList(await _repo2.GetTitles(), "JobId", "JobName");
            return View();
        }
        public IActionResult GetSalaryInfo()
        {
            return View();
        }

        public async Task<IActionResult> DeleteSalary(int id)
        {
            bool add = await _repo2.DeleteSalary(id);
            if (add == true)
            {
                return RedirectToAction(nameof(AddSalaryInfo), new { delete = add});
            }

            return null;

        }

        [HttpPost]
        public IActionResult PostTask(int id)
        {
            List<PreTaskModel> tasks = new List<PreTaskModel>();
            tasks = _repo2.GetTaskByProjectId(id);
            SelectList data = new SelectList(tasks, "Id", "Description");
            //var data = _repo2.GetTaskByProjectId(id);
            return Json(data);
        }

        public async Task<IActionResult> AddTask(int Id,List<int> idss , DateTime start, DateTime end ,int managerId,bool success = false, bool invalid = false)
        {
            ViewBag.invalid = invalid;
            ViewBag.Id = Id;
            ViewBag.Success = success;
            ViewBag.Employees = new SelectList(_repo2.GetEmployeesDropdown(idss), "Id", "Name");
            ViewBag.Projects = new SelectList( await _repo.GetProjectsByManager(managerId), "Id", "Name");
            ViewBag.manager = await _repo2.GetEmployeeNameById(managerId);
            var model = new Taskz()
            {
                ChooseStartDate=start,
                ChooseEndDate=end,
                StartDate=start,
                EndDate=end,
                ProjectManager =ViewBag.manager,
                ProjectManagerId =managerId
            };
              return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(Taskz task)
        {   

            if ((task.ChooseStartDate < DateTime.Now) || (task.ChooseEndDate < DateTime.Now) || (task.ChooseStartDate >= task.ChooseEndDate))
            {
                var emp1 = new SelectEmployees()
                {
                    StartDate = task.StartDate,
                    EndDate = task.EndDate
                };
                var ids1 = await _repo2.SelectEmployees(emp1);

                return RedirectToAction(nameof(AddTask), new { invalid = true, idss = ids1, start = emp1.StartDate , end = emp1.EndDate });
            }

            if (ModelState.IsValid)
            {
               
                int id = await _repo2.AddTask(task);
                if (id>0)
                {
                    var emp = new SelectEmployees()
                    {
                        StartDate = task.ChooseStartDate,
                        EndDate = task.ChooseEndDate,
                        ProjectManagerId = task.ProjectManagerId
                    };
                    var ids = await _repo2.SelectEmployees(emp);

                    return RedirectToAction(nameof(AddTask), new {Id=id, idss = ids, start = emp.StartDate, end = emp.EndDate ,success=true, managerId=task.ProjectManagerId });
                }
                
            }

            var emp2 = new SelectEmployees()
            {
                StartDate = task.ChooseStartDate,
                EndDate = task.ChooseEndDate,
                ProjectManagerId = task.ProjectManagerId
            };
            var ids2 = await _repo2.SelectEmployees(emp2);

            ViewBag.Employees = (new SelectList(_repo2.GetEmployeesDropdown(ids2), "Id", "Name"));
            ViewBag.Projects = new SelectList(await _repo.GetCurrentProjects(), "Id", "Name");
            return View();
        }

        public async Task<IActionResult> TaskOverview()
        {
            var data = await _repo2.TaskOverview();
            ViewData["tasks2"] = await _repo2.TaskOverview2();
            ViewData["tasks"] = await _repo2.GetTasks(0,0,0,"today");
            return View(data);
        }

        public IActionResult TodayTask()
        {
            return View();
        }

        public IActionResult GetTaskByProjectId(int id)
        {
            return RedirectToAction(nameof(GetTasks), new { proj = id });

        }

        public async Task<IActionResult> TodayAllocatedTasks()
        {
            var data =await _repo2.TodayAllocatedTasks();
            return View(data);
        }

        public async Task<IActionResult> GetTasks(bool success = false, bool delete = false, bool search = false, int count = 20,int emp =0,int proj=0,String state=null)
        {
            ViewBag.Success = success;
            ViewBag.Delete = delete;
            ViewBag.search = search;
            ViewBag.employees = new SelectList(await _repo2.GetDevelopers(), "Id", "Name");
            ViewBag.projects = new SelectList(await _repo.GetProjects(), "Id", "Name");
            var data = await _repo2.GetTasks(count, emp, proj, state);
            return View(data);
        }

        [HttpPost]
        public IActionResult GetTask(int counts, int emps, int projs, String states)
        {
            return RedirectToAction(nameof(GetTasks), new { count = counts, emp = emps, proj = projs, state = states });
        }



        public async Task<IActionResult> EditTask(int id, bool success = false)
        {
            ViewBag.Success = success;
            ViewBag.Id = id;
            var model = await _repo2.GetTasksById(id);
            ViewBag.Name = await _repo2.GetEmployeeNameById(model.EmployeeId);
            ViewBag.Project = (await _repo.GetProjectById(model.projectId)).Name;
            
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(Taskz task)
        {
            bool state = await _repo2.EditTask(task);
            if(state == true)
            {
               return RedirectToAction(nameof(GetTasks), new { success = state });
            }


            return View();
        }


        public async Task<IActionResult> DeleteTask(int id)
        {

            bool add = await _repo2.AddToDeletedTasks(id);
            if (add == true)
            {
                bool deduct = await _repo2.DeductCost(id);
                if(deduct == true)
                {
                bool success = await _repo2.DeleteTask(id);
                return RedirectToAction(nameof(GetTasks), new { delete = success });
                }
                
            }

            return null;

        }


        [HttpGet]
        public async Task<IActionResult> SearchTask(String Name, bool Success = false)
        {
            if(Name != null)
            {
              var task = await _repo2.SearchTask(Name);

            if (task?.Any() == true)
            {
                return View(task);
            }
                return RedirectToAction(nameof(GetTasks), new { search =true});
            }
            return RedirectToAction(nameof(GetTasks));
        }

        public IActionResult SelectResources(bool currentContext = false, bool invalid = false)
        {
            ViewBag.context = currentContext;
            ViewBag.invalid = invalid;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SelectResources(SelectEmployees emp)
        {
            if ((emp.StartDate < DateTime.Now) || (emp.EndDate < DateTime.Now))
            {
                return RedirectToAction(nameof(SelectResources), new { currentContext = true });
            }
            if (emp.StartDate >= emp.EndDate)
            {
                return RedirectToAction(nameof(SelectResources), new { invalid = true });
            }


            var ids = await _repo2.SelectResources(emp);

            return RedirectToAction(nameof(GetResources), new { idss = ids });

        }

        public async Task<IActionResult> GetResources(List<int> idss)
        {
            var data =await _repo2.GetResourcesAsync(idss);
            return View(data);
        }

        public IActionResult GetResourceAllocation()
        {
            return View();
        }

        public IActionResult DesignationCount()
        {
            return View();
        }

        public async Task<IActionResult> ResourceAllocation(bool success = false, bool success2 = false, bool success3 = true)
        {
            ViewData["employees"] = await _repo2.GetEmployees();
            ViewData["designation"] = await _repo2.DesignationCount();
            ViewBag.success = success;
            ViewBag.success2 = success2;
            ViewBag.success3 = success3;
            ViewBag.ProjectManager = new SelectList(await _repo2.GetEmployeeByTitle("project manager"), "Id", "Name");
            var data = await _repo2.AllocateResources1();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> ResourceAllocation(Resources resources)
        {
            bool add = await _repo2.AllocateResources2(resources);
           
            return RedirectToAction(nameof(ResourceAllocation), new { success = add });
        }

        public async Task<IActionResult> RemoveAllocation(int id)
        {
            bool data = await _repo2.RemoveAllocation(id);
            if(data == true)
            {
                return RedirectToAction(nameof(ResourceAllocation), new { success2 = data });
            }
            return RedirectToAction(nameof(ResourceAllocation), new { success3 = data });
        }

    }
}
