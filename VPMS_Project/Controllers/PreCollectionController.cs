using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VPMS_Project.Data;
using VPMS_Project.Models;
using VPMS_Project.Repository;
using VPMS_Project.Utility;

namespace preSaleStageWeb.Controllers
{
    public class PreCollectionController : Controller
    {
        private readonly CollectionRepository _collectionRepository = null;
        private readonly ProjectRepository _projectRepository = null;
        private readonly EmpStoreContext _context = null;



        public PreCollectionController(CollectionRepository collectionRepository, ProjectRepository projectRepository, EmpStoreContext context)
        {
            _collectionRepository = collectionRepository;
            _projectRepository = projectRepository;
            _context = context;

        }

        public async Task<ViewResult> GetAllCollection(int projectId)
        {


            var data = await _collectionRepository.GetAllCollection(projectId);
            ViewBag.RemainBudget = null;
            return View(data);
        }

        public async Task<ViewResult> AddNewCollection(bool isSuccess = false, int projectId = 1, string Title = "A", bool isExeed = false, double remainedBudget = 0)
        {

            //ViewData["collection"] = new CollectionModel() { ProjectsID = projectId };

            ViewBag.project = await _projectRepository.GetProjectByID(projectId);
            ViewBag.isExeed = isExeed;
            ViewBag.remainedBudget = remainedBudget;

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

            List<CollectionModel> collection = await _collectionRepository.GetAllCollection(collectionModel.ProjectsID);
            var contractValue = _context.PreSalesProjects.Single(b => b.ID == collectionModel.ProjectsID).value;
            ViewBag.isExeed = false;

            double curentCollectionBudget = 0;

            if (collection != null)
            {
                foreach (CollectionModel item in collection)
                {
                    curentCollectionBudget = curentCollectionBudget + item.value;
                }
                if (curentCollectionBudget + collectionModel.value > contractValue)
                {
                    var data = await _collectionRepository.GetAllCollection(collectionModel.ProjectsID);
                    ViewData["collection"] = data;
                    ViewBag.projects = new SelectList(await _projectRepository.GetProjects(), "ID", "Title");
                    return RedirectToAction(nameof(AddNewCollection), new { isSuccess = true, projectId = collectionModel.ProjectsID, isExeed = true, remainedBudget = contractValue - curentCollectionBudget });

                }
            }

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

            var data1 = await _collectionRepository.GetAllCollection(collectionModel.ProjectsID);
            ViewData["collection"] = data1;

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


