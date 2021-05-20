using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VPMS_Project.Data;
using VPMS_Project.Models;
using VPMS_Project.Repository;

namespace VPMS_Project.Controllers
{
    public class PreCustomerController : Controller
    {
        private readonly CustomerRepository _customerRepository = null;

        //private readonly EmpStoreContext emp = new EmpStoreContext(); //new part

        public PreCustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [Authorize(Roles = "admin")]
        public async Task<ViewResult> GetAllCustomers()
        {
            var data = await _customerRepository.GetAllCustomers();

            return View(data);
        }


        [Authorize(Roles = "admin")]
        [Route("customer-details/{id}", Name = "customerDetailsRoute")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var data = await _customerRepository.GetCustomerById(id);

            return View(data);

        }

        [Authorize(Roles = "admin")]
        public List<CustomerModel> SearchCustomers(string name)
        {
            return null/*_customerRepository.SearchCustomers(name)*/;

        }

        //new part
        //for email
        //public JsonResult IsEmailExist(string email)
        //{
        //    return Json(data:!emp.PreSalesCustomers.Any(x => x.emailAddress == email), System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //}

        //[AllowAnonymous]
        //public async Task<JsonResult> UserAlreadyExistsAsync(string email)
        //{
        //    var result =
        //        await PreSalesCustomers.FindByNameAsync(email) ??
        //        await PreSalesCustomers.FindByEmailAsync(email);
        //    return Json(result == null, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //}



        //finish


        [Authorize(Roles = "admin")]
        public ViewResult AddNewCustomer(bool isSuccess = false, int customerid = 0)
        {
            ViewBag.IsSuccess = isSuccess;
            ViewBag.CustomerId = customerid;
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewCustomer(CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                int id = await _customerRepository.AddNewCustomer(customerModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewCustomer), new { isSuccess = true, customerid = id });
                }
            }
            return View();
        }

        //edit customer details 
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditCustomers(bool Success = false, int Id = 0)
        {
            ViewBag.Success = Success;
            ViewBag.id = Id;
            var data1 = await _customerRepository.GetCustomers();
            ViewData["customer"] = data1;
            var data2 = await _customerRepository.GetCustomerById(Id);
            return View(data2);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomers(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                bool Success = await _customerRepository.EditCustomers(customer);
                if (Success == true)
                {
                    return RedirectToAction(nameof(EditCustomers), new { Success = true });
                }
            }
            var data = await _customerRepository.GetCustomers();
            ViewData["customer"] = data;
            return View(data);
        }

        // delete part 
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            bool add = await _customerRepository.AddToDeletedCustomers(id);
            if (add == true)
            {
                bool success = await _customerRepository.DeleteCustomer(id);
                return RedirectToAction(nameof(AddNewCustomer), new { delete = add });
            }

            return null;

        }

        //finish delete part
    }
}
