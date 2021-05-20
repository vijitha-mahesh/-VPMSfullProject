using VPMS_Project.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Mvc; //newpart

namespace VPMS_Project.Models
{
    public class CustomerModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Client Name*")]
        public string name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Number*")]
        public int contactNumber { get; set; }

        [Required]
        [Display(Name = "Address*")]
        public string address { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email Address*")]
        //[System.Web.Mvc.Remote(action: "IsEmailExist", controller: "PreCustomerController", ErrorMessage = "Email Address already available")] //new part
        public string emailAddress { get; set; }

        public int projectCount { get; set; }




    }
}
