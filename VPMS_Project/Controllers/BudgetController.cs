using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VPMS_Project.Models;
using VPMS_Project.Data;
using VPMS_Project.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VPMS_Project.Controllers
{
    public class BudgetController : Controller
    {
        private readonly BudgetRepository _budgetRepository = null;
        private readonly ProjectRepository _projectRepository = null;
        private readonly Repo _repo = null;

        public BudgetController(BudgetRepository budgetRepository, ProjectRepository projectRepository, Repo repo)
        {
            _budgetRepository = budgetRepository;
            _projectRepository = projectRepository;
            _repo = repo;
        }

        public async Task<ViewResult> GetAllBudgets()
        {
            var data = await _budgetRepository.GetAllBudgets();

            return View(data);
        }

        //it's working
        //public async Task<ViewResult> GetBudget(int projectId)
        //{
        //    var data = await _budgetRepository.GetBudgetByProjectId(projectId);

        //    return View(data);
        //}

        //new part
        //public ViewResult GetBudgetByProjectId()
        //{
        //    return View();
        //}

        //finish


        public async Task<ViewResult> GetBudget(int projectId, string Title)
        {          
          
            ViewData["types"] = _budgetRepository.GetChartByProjectId(projectId);
            ViewData["Project"] = new ProjectModel() { ID = projectId, Title = Title };
            var data = await _budgetRepository.GetBudgetByProjectId(projectId);
            return View(data);
        }



        public async Task<ViewResult> AddNewBudget(bool isSuccess = false, int budgetid = 0, int id = 0)
        {
            var model = new BudgetModel();

            ViewBag.projects = new SelectList(await _projectRepository.GetProjects(), "ID", "Title");

            
            ViewBag.IsSuccess = isSuccess;
            ViewBag.BudgetId = budgetid;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBudget(BudgetModel budgetModel)
        {
            if (ModelState.IsValid)
            {
                int id = await _budgetRepository.AddNewBudget(budgetModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewBudget), new { isSuccess = true, budgetid = id });
                }
            }
            ViewBag.projects = new SelectList(await _projectRepository.GetProjects(), "ID", "Title");

            return View();
        }


        public ViewResult overallBudget()
        {
            ViewBag.SixMonthRevenues = _budgetRepository.GetSixMonthRevenues();
            ViewBag.Revenues = _budgetRepository.GetRevenues();
            ViewBag.ProjectSummery = _budgetRepository.ProjectsSummery();
            return View();
        }
        //edit customer details 

        public async Task<IActionResult> EditBudgets(bool Success = false, int Id = 0)
        {
            ViewBag.Success = Success;
            ViewBag.id = Id;
            var data1 = await _budgetRepository.GetBudgets();
            ViewData["budget"] = data1;
            var data2 = await _budgetRepository.GetBudgetById(Id);
            return View(data2);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBudgets(BudgetModel budget)
        {
            if (ModelState.IsValid)
            {
                bool Success = await _budgetRepository.EditBudgets(budget);
                if (Success == true)
                {
                    return RedirectToAction(nameof(GetBudget), new { Success = true });
                }
            }
            var data = await _budgetRepository.GetBudgets();
            ViewData["budget"] = data;
            return View(data);
        }
        //finish edit part

        // delete part 
        public async Task<IActionResult> DeleteBudget(int id)
        {
            bool add = await _budgetRepository.AddToDeletedBudgets(id);
            if (add == true)
            {
                bool success = await _budgetRepository.DeleteBudget(id);
                return RedirectToAction(nameof(GetAllBudgets), new { delete = add });
            }
            return null;
        }

        //finish delete part

    }
}
