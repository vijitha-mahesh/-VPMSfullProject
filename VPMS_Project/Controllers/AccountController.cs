using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VPMS_Project.Models;
using VPMS_Project.Repository;
using VPMS_System.Models;

namespace VPMS_Project.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TimeTrackRepo _timeTrackRepo = null;
        private readonly TaskRepo _taskRepository = null;
        private readonly Repo4 _repo4 = null;

        public AccountController(UserManager<ApplicationUser> userManager, TimeTrackRepo timeTrackRepo, TaskRepo taskRepo,
                              SignInManager<ApplicationUser> signInManager, Repo4 repo4)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _timeTrackRepo = timeTrackRepo;
            _taskRepository = taskRepo;
            _repo4 = repo4;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Designation = model.Designation,
                    UserName = model.UserName,
                    Email = model.UserName,
                };

               
                
                   var result = await _userManager.CreateAsync(user, model.Password);
                 

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //var addedUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;


                    if (model.admin != false)
                    {
                        await _userManager.AddToRoleAsync(user, "admin");

                    }
                    if (model.user != false)
                    {
                        await _userManager.AddToRoleAsync(user, "user");

                    }
                    if (model.manager!= false)
                    {
                        await _userManager.AddToRoleAsync(user, "manager");

                    }
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            //var branch = new Branch
            //{
            //    branchName = "Regie",
            //    address = "Naval"

            //};
            //branchContext.Branch.Add(branch);
            //branchContext.SaveChanges();

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Portal", "EmployeeHome");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
