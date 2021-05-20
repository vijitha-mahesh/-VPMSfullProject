using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 5)]
        [Required(ErrorMessage = "Name is required")]
        public String Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress]
        public String Email { get; set; }
        public String Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required]
        public int ContactNo { get; set; }
        
    }
}
