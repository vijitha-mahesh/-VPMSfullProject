using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Models;
using VPMS_Project.Repository;

namespace VPMS_Project.Controllers
{
    public class PaymentController : Controller
    {

        private readonly PaymentRepository _paymentRepository = null;

        public PaymentController(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<ViewResult> ProjectPayments(int projectId, string Title, bool isSuccess = false)
        {
            //ViewData["collection"] = new CollectionModel() { ProjectsID = projectId };

            ViewBag.projects = await _paymentRepository.GetByProjectID(projectId);

            var model = new Payment()
            {
                ProjectId = projectId,
                ProjectName = Title
            };

            ViewBag.IsSuccess = isSuccess;
            //ViewBag.CollectionId = collectionId;
            // ViewData["collection"] = await _collectionRepository.GetAllCollection(projectId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProjectPayments(Payment payment)
        {
            if (ModelState.IsValid)
            {
                int id = await _paymentRepository.AddNew(payment);
                if (id > 0)
                {
                    return RedirectToAction(nameof(ProjectPayments), new { isSuccess = true, projectId = payment.ProjectId, Title = payment.ProjectName });
                }
            }
            var data = await _paymentRepository.GetByProjectID(payment.ProjectId);
            ViewBag.projects = data;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Payment payment = await _paymentRepository.GetByID(id);
            await _paymentRepository.Delete(id);
            return RedirectToAction(nameof(ProjectPayments), new { isSuccess = true, projectId = payment.ProjectId, Title = payment.ProjectName });
        }
    }
}
