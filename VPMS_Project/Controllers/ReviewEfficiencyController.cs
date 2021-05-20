using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Repository;

namespace VPMS_Project.Controllers
{
    public class ReviewEfficiencyController : Controller
    {
        private readonly IEmpRepository _empRepository = null;
        private readonly LeaveRepository _leaveRepository = null;
        private readonly TaskRepo _taskRepository = null;



        public ReviewEfficiencyController(IEmpRepository empRepository,LeaveRepository leaveRepository, TaskRepo taskRepository)
        {
            _taskRepository = taskRepository;
            _leaveRepository = leaveRepository;
            _empRepository = empRepository;

        }
        public async Task<IActionResult> Index()
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            int id = Currentuser.EmpId;
            var data = await _taskRepository.GetAllTaskList2(id);
            return View(data);
        }
    }
}
