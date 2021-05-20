using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using VPMS_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace VPMS_Project.Controllers
{
    public class PreHomeController : Controller
    {
        [ViewData]

        public string CustomProperty { get; set; }

        [Authorize(Roles = "admin")]
        public ViewResult Index()
        {

            CustomProperty = "Application";

            return View();
        }

        public ViewResult AboutUs()
        {
            return View();
        }

        public ViewResult ContactUs()
        {
            return View();
        }

        public ViewResult Index1()
        {
            return View();
        }


    }
}
