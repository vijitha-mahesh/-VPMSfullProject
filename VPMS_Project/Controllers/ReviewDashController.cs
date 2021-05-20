using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Models;
using VPMS_Project.Repository;

namespace VPMS_Project.Controllers
{
    public class ReviewDashController : Controller
    {
        private readonly IEmpRepository _empRepository = null;
        private readonly LeaveRepository _leaveRepository = null;
        private readonly TaskRepo _taskRepository = null;



        public ReviewDashController(IEmpRepository empRepository, LeaveRepository leaveRepository, TaskRepo taskRepository)
        {
            _taskRepository = taskRepository;
            _leaveRepository = leaveRepository;
            _empRepository = empRepository;

        }
        public async Task<IActionResult> ReviewDash()
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            int id = Currentuser.EmpId;
            var data = await _taskRepository.GetAllTaskList2(id);
            double quality = 0;
            foreach (var task in data)
            {
                quality += ((task.AllocatedHours - task.TakenHours) / task.AllocatedHours) * 100;
            }
            Double dc = Math.Round((Double)quality, 0);
            ViewBag.qulity = dc;                               // efficiency

            int count = _taskRepository.GetWorkQualityCount(id);
            if (count!=0)
            {
                Double WorkQ = _taskRepository.GetWorkQualitySum(id) / count;
                Double Work = Math.Round((Double)WorkQ, 0);
                ViewBag.Workqulity = Work;                        // work quality
            }
            else
            {
                ViewBag.Workqulity = 0;
            }

            int count1 = _taskRepository.GetCommunicationCount(id);
            if (count1 != 0)
            {
                Double ComQ = _taskRepository.GetComSum(id) / count1;
                Double com = Math.Round((Double)ComQ, 0);
                ViewBag.Communication = com;                        // work quality
            }
            else
            {
                ViewBag.Communication = 0;
            }

            return View();
        }
    }
}
