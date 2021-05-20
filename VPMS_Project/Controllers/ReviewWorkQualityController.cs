using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Models;
using VPMS_Project.Repository;

namespace VPMS_Project.Controllers
{
    public class ReviewWorkQualityController : Controller
    {
        private readonly TaskRepo _taskRepository = null;

        public ReviewWorkQualityController(TaskRepo taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IActionResult> Index(bool isSucceess = false, bool isExist = false, int id = 0, int TaskId = 0, int Val = 0)
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            int EmpId = Currentuser.EmpId;
            ViewBag.Emps = new SelectList(await _taskRepository.GetEmps(EmpId), "EmpId", "EmpFullName");
            if (id != 0)
            {
                ViewBag.Show = true;
                ViewBag.Id = id;
                ViewBag.Name = await _taskRepository.GetName(id);
                ViewBag.Task = new SelectList(await _taskRepository.GetTask(id), "Id", "Name");
            }
            if (TaskId != 0)
            {
                bool result =  _taskRepository.CheckRate(TaskId, id, EmpId);
                if (result == true)
                {
                    return RedirectToAction(nameof(Index), new { isExist = true });
                }
                else
                {
                    int WorkId = await _taskRepository.AddVal(TaskId, id, Val, EmpId);
                    if (WorkId > 0)
                    {
                        return RedirectToAction(nameof(Index), new { isSucceess = true });
                    }
                }               
            }
            ViewBag.IsSuccess = isSucceess;
            ViewBag.IsExist = isExist;
            return View();
        }

    }
}
