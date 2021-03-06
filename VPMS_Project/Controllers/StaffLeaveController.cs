using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPMS_Project.Models;
using VPMS_Project.Repository;

namespace VPMS_Project.Controllers
{
        public class StaffLeaveController : Controller
        {
            private readonly IEmpRepository _empRepository = null;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly LeaveRepository _leaveRepository = null;
            private readonly TaskRepo _taskRepository = null;


        public StaffLeaveController(TaskRepo taskRepository, IEmpRepository empRepository, IWebHostEnvironment webHostEnvironment, LeaveRepository leaveRepository)
            {
                _leaveRepository = leaveRepository;
                _empRepository = empRepository;
                _webHostEnvironment = webHostEnvironment;
            _taskRepository = taskRepository;
        }

        public async Task<IActionResult> Leavebalance()
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            int EmpId = Currentuser.EmpId;
                var data = await _leaveRepository.GetLeaveBalance(EmpId);
                return View(data);
                      
            }



            public async Task<IActionResult> PendingLeaveHistory(bool isDelete = false)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            int EmpId = Currentuser.EmpId;
            ViewBag.IsDelete = isDelete;
                var data = await _leaveRepository.GetAllPendingLeaveById(EmpId);
                return View(data);
            }

            public async Task<IActionResult> ApprovedHistory(bool isMore = false)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;

            int EmpId = Currentuser.EmpId;
            ViewBag.IsMore = isMore;
                var data = await _leaveRepository.GetApprovedLeaveById(EmpId);
                return View(data);
            }

            public async Task<IActionResult> RejectedLeave()
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            int EmpId = Currentuser.EmpId;
            var data = await _leaveRepository.GetRejectedLeaveById(EmpId);
                return View(data);
            }

            public async Task<IActionResult> LeaveApply(bool isSucceess = false, bool isUpdate = false, int leaveId = 0, bool isExist = false)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            int EmpId = Currentuser.EmpId;
            ViewBag.LeaveId = leaveId;
                ViewBag.IsSuccess = isSucceess;
                ViewBag.IsUpdate = isUpdate;
                ViewBag.IsExist = isExist;
                var data = await _leaveRepository.GetEmpLeaveById(EmpId);
                return View(data);
            }

            [HttpPost]
            public async Task<IActionResult> LeaveApply(LeaveApplyModel leaveApplyModel)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            leaveApplyModel.EmpId = Currentuser.EmpId;
                int Eid = leaveApplyModel.EmpId;
                DateTime date = leaveApplyModel.Startdate;
               bool existOne = _leaveRepository.CheckExist(Eid, date);
                if (existOne)
                {
                    return RedirectToAction(nameof(LeaveApply), new { isExist = true });
                }
                else
                {

                    TimeSpan differ = (TimeSpan)(leaveApplyModel.EndDate - leaveApplyModel.Startdate);
                    leaveApplyModel.NoOfDays = differ.Days;
                if (leaveApplyModel.EvidencePDF != null)
                {
                    String folder = "images/evidencePDF/";
                    leaveApplyModel.PdfURL = await UploadPDF(folder, leaveApplyModel.EvidencePDF); 
                }
                int id = await _leaveRepository.AddLeave(leaveApplyModel);

                    if (id > 0)
                    {
                        return RedirectToAction(nameof(LeaveApply), new { isSucceess = true, leaveId = id });
                    }

                }
                return View();
            }

            public async Task<IActionResult> EditLeave(int id)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            var data = await _leaveRepository.GetEmpLeaveJoinById(id);
                return View(data);
            }

            public async Task<IActionResult> ViewLeave(int id)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            var data = await _leaveRepository.GetOneLeaveById(id);
                return View(data);
            }

            [HttpPost]
            public async Task<IActionResult> EditLeave(LeaveApplyModel leaveApplyModel)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            leaveApplyModel.EmpId = Currentuser.EmpId;
                TimeSpan differ = (TimeSpan)(leaveApplyModel.EndDate - leaveApplyModel.Startdate);
                leaveApplyModel.NoOfDays = differ.Days;
                if (leaveApplyModel.EvidencePDF != null)
                {
                String folder = "images/evidencePDF/";
                leaveApplyModel.PdfURL = await UploadPDF(folder, leaveApplyModel.EvidencePDF);
                 }
                 bool success = await _leaveRepository.UpdateLeave(leaveApplyModel);

                if (success == true)
                {
                    return RedirectToAction(nameof(LeaveApply), new { isUpdate = true, leaveId = leaveApplyModel.LeaveApplyId });
                }

                return View();
            }

            public async Task<IActionResult> DeleteLeave(int id)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            bool success = await _leaveRepository.DeleteLeave(id);
                if (success == true)
                {
                    return RedirectToAction(nameof(PendingLeaveHistory), new { isDelete = true });

                }
                return View();
            }


            public async Task<IActionResult> ClearApprovedLeave(int id)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            bool success = await _leaveRepository.ClearLeave(id);
                if (success == true)
                {
                    return RedirectToAction(nameof(ApprovedHistory));

                }
                return View();
            }

            public async Task<IActionResult> ClearRejectLeave(int id)
            {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            bool success = await _leaveRepository.ClearLeave(id);
                if (success == true)
                {
                    return RedirectToAction(nameof(RejectedLeave));

                }
                return View();
            }
        [HttpGet]
        public async Task<IActionResult> SpecialLeave(DateTime start_Date, DateTime end_Date)
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            int EmpId = Currentuser.EmpId;
            if (start_Date == DateTime.MinValue && end_Date == DateTime.MinValue)
            {
                var data = await _leaveRepository.GetSpecialLeaveById(EmpId);
                return View(data);

            }
            else 
            {
                ViewBag.valueHas = true;
                ViewBag.start = start_Date.Day+"/"+ start_Date.Month + "/" + start_Date.Year;
                ViewBag.end = end_Date.Day + "/" + end_Date.Month + "/" + end_Date.Year; ;
                var data = await _leaveRepository.SpecialLeavePeriod(EmpId, start_Date, end_Date);
                return View(data);
            }
           
        }

        public async Task<IActionResult> NoPayLeave(DateTime start_Date, DateTime end_Date)
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            int EmpId = Currentuser.EmpId;
            if (start_Date == DateTime.MinValue && end_Date == DateTime.MinValue)
            {

                var data = await _leaveRepository.GetNoPayLeaveById(EmpId);
                return View(data);
            }
            else
            {
                ViewBag.valueHas = true;
                ViewBag.start = start_Date.Day + "/" + start_Date.Month + "/" + start_Date.Year;
                ViewBag.end = end_Date.Day + "/" + end_Date.Month + "/" + end_Date.Year; ;
                var data = await _leaveRepository.NoPayLeavePeriod(EmpId, start_Date, end_Date);
                return View(data);
            }
        }
        

        private async Task<string> UploadPDF(string folderPath, IFormFile file) 
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            String serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder,FileMode.Create));
            return "/" + folderPath;
        }

        
        }
    }

