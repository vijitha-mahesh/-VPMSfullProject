using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VPMS_Project.Models
{
    public class EmpModel
    {
        [Required(ErrorMessage = "First name field is required")]
        public int EmpId { get; set; }

        [StringLength(20, MinimumLength = 3)]
        [Required(ErrorMessage = "First name field is required")]
        public String EmpFName { get; set; }



        [StringLength(20, MinimumLength = 3)]
        [Required(ErrorMessage = "Last name field is required")]
        public String EmpLName { get; set; }

        public String EmpFullName { get; set; }



        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress]
        public String Email { get; set; }


        [Required(ErrorMessage = "Mobile number field is required")]
        public int? Mobile { get; set; }


        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of birth field is required")]
        public DateTime Dob { get; set; }


        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Join date field is required")]
        public DateTime WorkSince { get; set; }

        public String Address { get; set; }

        [Required(ErrorMessage = "Job title field is required")]
        public int JobTitleId { get; set; }

        public String JobType { get; set; }

        public int PMId { get; set; }

        public IFormFile ProfilePhoto { get; set; }

        public String PhotoURL { get; set; }

        [Required(ErrorMessage = "Please select one")]
        public String Gender { get; set; }

        [Required(ErrorMessage = "Please select one")]
        public String Status { get; set; }

        public DateTime LastDayWorked { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date field is required")]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date field is required")]
        public DateTime Todate { get; set; }

        [Required(ErrorMessage = "Medical Leave field is required")]
        public int MedicalAllocated { get; set; }

        [Required(ErrorMessage = "Casual Leave field is required")]
        public int CasualAllocated { get; set; }

        [Required(ErrorMessage = "Annual Leave field is required")]
        public int AnnualAllocated { get; set; }


        [Required(ErrorMessage = "Short Leave field is required")]
        public int ShortLeaveAllocated { get; set; }

        [Required(ErrorMessage = "HalfDay Leave field is required")]
        public int HalfLeaveAllocated { get; set; }

        public int TotalLeaveGiven { get; set; }

        public String Designation { get; set; }

        ///////////////////////////////////////////////////////////
        ///newly added signup
        ///

        
        [Required]
        public bool admin { get; set; }
        [Required]
        public bool user { get; set; }
        [Required]
        public bool manager { get; set; }
        [Required]

        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }






    }
}
