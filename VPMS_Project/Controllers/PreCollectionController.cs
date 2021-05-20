using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VPMS_Project.Data;
using VPMS_Project.Models;
using VPMS_Project.Repository;

namespace preSaleStageWeb.Controllers
{
    public class PreCollectionController : Controller
    {
        private readonly CollectionRepository _collectionRepository = null;
        private readonly ProjectRepository _projectRepository = null;

        public PreCollectionController(CollectionRepository collectionRepository, ProjectRepository projectRepository)
        {
            _collectionRepository = collectionRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ViewResult> GetAllCollection(int projectId)
        {


            var data = await _collectionRepository.GetAllCollection(projectId);

            return View(data);
        }

        public async Task<ViewResult> AddNewCollection(bool isSuccess = false, int projectId = 1, string Title = "A")
        {

            //ViewData["collection"] = new CollectionModel() { ProjectsID = projectId };

            ViewBag.project = await _projectRepository.GetProjectByID(projectId);

            var model = new CollectionModel()
            {
                ProjectsID = projectId
            };

            ViewBag.IsSuccess = isSuccess;
            //ViewBag.CollectionId = collectionId;

            ViewData["collection"] = await _collectionRepository.GetAllCollection(projectId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCollection(CollectionModel collectionModel)
        {
            VPMS_Project.Utility.EmailSender.BudgetRemainder(); //Todo email

            if (ModelState.IsValid)
            {
                int id = await _collectionRepository.AddNewCollection(collectionModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewCollection), new { isSuccess = true, projectId = collectionModel.ProjectsID });
                }
            }

            ViewBag.projects = new SelectList(await _projectRepository.GetProjects(), "ID", "Title");
            //ViewData["collection"] = new CollectionModel() { ProjectsID = collectionModel.ProjectsID };

            var data = await _collectionRepository.GetAllCollection(collectionModel.ProjectsID);
            ViewData["collection"] = data;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            CollectionModel col = await _collectionRepository.GetCollectionById(id);
            await _collectionRepository.Delete(id);
            return RedirectToAction(nameof(AddNewCollection), new { isSuccess = true, projectId = col.ProjectsID });
        }



    }
}


