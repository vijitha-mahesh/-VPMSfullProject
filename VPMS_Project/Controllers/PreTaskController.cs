using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VPMS_Project.Models;
using VPMS_Project.Repository;

namespace VPMS_Project.Controllers
{
    public class PreTaskController : Controller
    {
        private readonly TaskRepository _taskRepository = null;
        private readonly ProjectRepository _projectRepository = null;



        public PreTaskController(TaskRepository taskRepository, ProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;

        }

        [Authorize(Roles = "admin")]
        public async Task<ViewResult> GetAllTasks()
        {


            var data = await _taskRepository.GetAllTasks();

            return View(data);
        }

        [Authorize(Roles = "admin")]
        public async Task<ViewResult> GetTask(int projectId, string Title)
        {
            ViewData["Project"] = new ProjectModel() { ID = projectId, Title = Title };

            var data = await _taskRepository.GetTaskByProjectId(projectId, Title);

            return View(data);
        }


        //don't want
        //public async Task<ViewResult> GetTask(int id)
        //{
        //    var data =await _taskRepository.GetTaskById(id);

        //    return View(data);
        //}


        [Authorize(Roles = "admin")]
        public async Task<ViewResult> AddNewTask(int projectId, bool isSuccess = false, int taskId = 0, bool currentContext = false, bool invalid = false)
        {


            ViewBag.projects = new SelectList(await _projectRepository.GetProjects(), "ID", "Title");

            ViewBag.project = await _projectRepository.GetProjectByID(projectId);

            var model = new PreTaskModel()
            {
                ProjectsID = projectId
            };


            ViewBag.IsSuccess = isSuccess;
            ViewBag.TaskId = taskId;
            ViewBag.invalid = invalid;
            ViewBag.context = currentContext;
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewTask(PreTaskModel taskModel)
        {
            if ((taskModel.StartDate < DateTime.Now) || (taskModel.EndDate < DateTime.Now))
            {
                return RedirectToAction(nameof(AddNewTask), new { currentContext = true });
            }

            if (taskModel.StartDate >= taskModel.EndDate)
            {
                return RedirectToAction(nameof(AddNewTask), new { invalid = true });
            }



            if (ModelState.IsValid)
            {
                int id = await _taskRepository.AddNewTask(taskModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewTask), new { isSuccess = true, taskId = id });
                }
            }
            ViewBag.project = new SelectList(await _projectRepository.GetProjects(), "ID", "Title");

            return View();
        }

        //edit Task details 

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditTasks(bool Success = false, int Id = 0, int count = 0)
        {
            ViewBag.Success = Success;
            ViewBag.id = Id;
            var data1 = await _taskRepository.GetTaskById(count);
            ViewData["task"] = data1;
            var data2 = await _taskRepository.GetTaskById(Id);
            return View(data2);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTasks(PreTaskModel task)
        {
            if (ModelState.IsValid)
            {
                bool Success = await _taskRepository.EditTasks(task);
                if (Success == true)
                {
                    return RedirectToAction(nameof(GetAllTasks), new { Success = true });
                }
            }
            var data = await _taskRepository.GetTaskById(0);
            ViewData["task"] = data;
            return View(data);
        }

        // delete part 
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            bool add = await _taskRepository.AddToDeletedTasks(id);
            if (add == true)
            {
                bool success = await _taskRepository.DeleteTask(id);
                return RedirectToAction(nameof(GetAllTasks), new { delete = add });
            }

            return null;

        }
        //finish delete part

    }
}
