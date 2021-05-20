using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VPMS_Project.Models;

namespace VPMS_Project.Controllers
{
    [Authorize(Roles = "admin")]
    public class CustomerController : Controller
    {
        private readonly Repo3 _repo3 = null;
        public CustomerController( Repo3 repo3)
        {
            _repo3 = repo3;
        }
        public async Task<IActionResult> AddCustomer(bool isSuccess = false, bool delete = false, bool duplicate = false)
        {
            ViewBag.isSuccess = isSuccess;
            ViewBag.delete = delete;
            ViewBag.duplicate = duplicate;
            var data = await _repo3.GetCustomers();
            ViewData["customer"] = data;
            return View();
        }

        [HttpPost]
        public IActionResult CustomerEntries(int count = 0)
        {
            return RedirectToAction(nameof(AddCustomer), new { entries = count });
        }


        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var customerAvailable = await _repo3.DupCustomer(customer.Email);
                if (customerAvailable == false)
                {
                  int id = await _repo3.AddCustomer(customer);
                  if (id > 0)
                  {
                    return RedirectToAction(nameof(AddCustomer), new { isSuccess = true});
                  }
                }else
                    return RedirectToAction(nameof(AddCustomer), new { duplicate=true });

            }
            var data = await _repo3.GetCustomers();
            ViewData["customer"] = data;
            return View();
        }

        public IActionResult GetCustomers()
        {
            return View();
        }


        public async Task<IActionResult> GetCustomerById(int id)
        {
            var data = await _repo3.GetCustomerById(id);
            return View(data);
        }

        public async Task<IActionResult> EditCustomer(bool Success = false, int Id = 0)
        {
            ViewBag.Success = Success;
            ViewBag.id = Id;
            var data = await _repo3.GetCustomers();
            ViewData["customer"] = data;
            var data2 = await _repo3.GetCustomerById(Id);
            return View(data2);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                bool success = await _repo3.EditCustomer(customer);
                if (success == true)
                {
                    return RedirectToAction(nameof(EditCustomer), new { Success = true });
                }
            }
            var data = await _repo3.GetCustomers();
            ViewData["customer"] = data;
            return View();
        }


        public async Task<IActionResult> DeleteCustomer(int id)
        {
            bool add = await _repo3.AddToDeletedCustomers(id);
            if (add == true)
            {
                bool success = await _repo3.DeleteCustomer(id);
                return RedirectToAction(nameof(AddCustomer) ,new { delete = add} );
            }

            return null;

        }

       

    }
}
