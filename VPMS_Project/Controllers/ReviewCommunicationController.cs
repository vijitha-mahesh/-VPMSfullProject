using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Repository;

namespace VPMS_Project.Controllers
{
    public class ReviewCommunicationController : Controller
    {
        private readonly TaskRepo _taskRepository = null;

        public ReviewCommunicationController(TaskRepo taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IActionResult> Communication(bool isSucceess = false,int id = 0,int Val = 0)
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            int EmpId = Currentuser.EmpId;
            ViewBag.Emps = new SelectList(await _taskRepository.GetEmps(EmpId), "EmpId", "EmpFullName");
            if (id != 0)
            {
                int ComId = await _taskRepository.AddValForCommunication(id, Val, EmpId);
                if (ComId > 0)
                {
                    return RedirectToAction(nameof(Communication), new { isSucceess = true });
                }
            }
            ViewBag.IsSuccess = isSucceess;
            return View();
        }

    }
}

