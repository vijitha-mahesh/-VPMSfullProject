using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VPMS_Project.Data;
using VPMS_Project.Models;
using VPMS_Project.Repository;
using Microsoft.AspNetCore.Http;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VPMS_Project.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectRepository _projectRepository = null;
        private readonly CustomerRepository _customerRepository = null;
        private readonly ProjectManagerRepository _projectManagerRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Repo3 _repo3 = null;

        public object GetDocument { get; private set; }

        public ProjectController(ProjectRepository projectRepository,
            CustomerRepository customerRepository,
            ProjectManagerRepository projectManagerRepository,
            IWebHostEnvironment webHostEnvironment,
            Repo3 repo3

            )
        {
            _projectRepository = projectRepository;
            _customerRepository = customerRepository;
            _projectManagerRepository = projectManagerRepository;
            _webHostEnvironment = webHostEnvironment;
            _repo3 = repo3;

        }

        //public async Task<ViewResult>  GetAllProjects()
        //{
        //    var data = await _projectRepository.GetAllProjects();
        //    return View(data);
        //}
        //new part

        [Authorize(Roles = "admin")]
        public async Task<ViewResult> GetAllProjects(string title)
        {
            ViewData["Project"] = new ProjectModel() { Title = title };

            var data = await _projectRepository.GetAllProjects();

            return View(data);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllProjects(string PropSearch)
        //{
        //    ViewData["Getpropjectdetails"] = PropSearch;

        //    var propquary = from x in _db.Projects select x;
        //    if(string.IsNullOrEmpty(PropSearch))
        //    {
        //        propquary = propquary.Where(x => x.Title.Contains(PropSearch));
        //    }
        //    return View(await propquary.AsNoTracking().ToListAsync());
        //}

        //finish part

        [Authorize(Roles = "admin")]
        [Route("project-details/{id}", Name = "projectDetailsRoute")]
        public async Task<ViewResult> GetProject(int id)
        {

            ViewData["document"] = new DocumentModel() { ProjectsID = id };

            var data = await _projectRepository.GetProjectByID(id);

            return View(data);
        }

        //new part

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> SearchProject(string Name)
        {


            if (Name != null)
            {
                var project = await _projectRepository.SearchProject(Name);

                if (project?.Any() == true)
                {
                    return View(project);
                }

                return RedirectToAction(nameof(GetAllProjects), new { search = true });
            }

            return RedirectToAction(nameof(GetAllProjects));
        }
        //finish part

        [Authorize(Roles = "admin")]
        public async Task<ViewResult> AddNewProject(bool isSuccess = false, int projectId = 0, bool currentContext = false, bool invalid = false)
        {
            var model = new ProjectModel();

            ViewBag.Customers = new SelectList(await _repo3.GetCustomers(), "Id", "Name");
            ViewBag.ProjectManager = new SelectList(await _projectManagerRepository.GetProjectManager("project manager"), "Id", "Name");

            ViewBag.IsSuccess = isSuccess;
            ViewBag.ProjectId = projectId;
            ViewBag.invalid = invalid;
            ViewBag.context = currentContext;
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewProject(ProjectModel projectModel)
        {
            if ((projectModel.startDate < DateTime.Now) || (projectModel.EndDate < DateTime.Now))
            {
                return RedirectToAction(nameof(AddNewProject), new { currentContext = true });
            }

            if (projectModel.startDate >= projectModel.EndDate)
            {
                return RedirectToAction(nameof(AddNewProject), new { invalid = true });
            }

            if (ModelState.IsValid)
            {
                int id = await _projectRepository.AddNewProject(projectModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewProject), new { isSuccess = true, projectId = id });
                }
            }
            ViewBag.Customers = new SelectList(await _repo3.GetCustomers(), "Id", "Name");
            ViewBag.ProjectManager = new SelectList(await _projectManagerRepository.GetProjectManager("project manager"), "Id", "Name");
            return View();
        }

        //edit Project details 
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditProjects(bool Success = false, int Id = 0, int count = 0)
        {
            ViewBag.Success = Success;
            ViewBag.id = Id;
            var data1 = await _projectRepository.GetProjectByID(count);
            ViewData["project"] = data1;
            var data2 = await _projectRepository.GetProjectByID(Id);
            return View(data2);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProjects(ProjectModel project)
        {
            if (ModelState.IsValid)
            {
                bool Success = await _projectRepository.EditProjects(project);
                if (Success == true)
                {
                    return RedirectToAction(nameof(EditProjects), new { Success = true });
                }
            }
            var data = await _projectRepository.GetProjectByID(0);
            ViewData["project"] = data;
            return View(data);
        }

        //finish edit Project details 

        // delete part 
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            bool add = await _projectRepository.AddToDeletedProjects(id);
            if (add == true)
            {
                bool success = await _projectRepository.DeleteProject(id);
                return RedirectToAction(nameof(AddNewProject), new { delete = add });
            }
            return null;
        }

        // conver part 
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ConvertProject(int id)
        {
            bool add = await _projectRepository.AddToConvertProjects(id);
            if (add == true)
            {
                bool success = await _projectRepository.ConvertProject(id);
                return RedirectToAction(nameof(GetAllProjects), new { delete = add });
            }
            return null;
        }

    }
}
