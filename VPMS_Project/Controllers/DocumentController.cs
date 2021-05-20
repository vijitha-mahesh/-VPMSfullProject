using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VPMS_Project.Models;
using VPMS_Project.Repository;

namespace VPMS_Project.Controllers
{
    public class DocumentController : Controller
    {
        private readonly DocumentRepository _documentRepository = null;
        private readonly ProjectRepository _projectRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public DocumentController(DocumentRepository documentRepository,
            ProjectRepository projectRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _documentRepository = documentRepository;
            _projectRepository = projectRepository;
            _webHostEnvironment = webHostEnvironment;

        }

        public async Task<ViewResult> GetDocument(int projectId)
        {
            ViewData["Document"] = new DocumentModel { ProjectsID = projectId };

            var data = await _documentRepository.GetDocument(projectId);

            return View(data);
        }





        public async Task<ViewResult> AddNewDocument(bool isSuccess = false, int projectId = 1)
        {
            ViewBag.projects = new SelectList(await _projectRepository.GetProjects(), "ID", "Title");

            ViewBag.project = await _projectRepository.GetProjectByID(projectId);

            var model = new PreTaskModel()
            {
                ProjectsID = projectId
            };

            ViewBag.IsSuccess = isSuccess;
            ViewData["Document"] = await _documentRepository.GetDocument(projectId);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddNewDocument(DocumentModel documentModel)
        {

            if (documentModel.ScopeDocument != null)
            {
                string folder = "projects/ScopeDocument/";
                folder += Guid.NewGuid().ToString() + " " + documentModel.ScopeDocument.FileName;

                documentModel.ScopeDocumentUrl = "/" + folder;

                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await documentModel.ScopeDocument.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }
            else if (documentModel.ActionPlan != null)
            {
                string folder = "projects/ActionPlan/";
                folder += Guid.NewGuid().ToString() + " " + documentModel.ActionPlan.FileName;

                documentModel.ActionPlanUrl = "/" + folder;

                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await documentModel.ActionPlan.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }
            else
            {
                string folder = "projects/TimePlan/";
                folder += Guid.NewGuid().ToString() + " " + documentModel.TimePlan.FileName;

                documentModel.TimePlanUrl = "/" + folder;

                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await documentModel.TimePlan.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }


            int id = await _documentRepository.AddNewDocument(documentModel);
            if (id > 0)
            {
                return RedirectToAction(nameof(AddNewDocument), new { isSuccess = true, projectId = documentModel.ProjectsID });
            }

            ViewBag.projects = new SelectList(await _projectRepository.GetProjects(), "ID", "Title");

            ViewData["Document"] = await _documentRepository.GetDocument(documentModel.ProjectsID);


            return View();
        }
    }
}
