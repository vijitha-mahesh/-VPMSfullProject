using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using VPMS_Project.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace VPMS_Project.Models
{
    public class ProjectModel
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ProjectType { get; set; }

        [Required]
        [Display(Name = "Contact Value")]
        public Double value { get; set; }

        [Required]
        [Display(Name = "Budget")]
        public Double ProjectBudget { get; set; }


        [Display(Name = "Client Name")]
        [Required(ErrorMessage = "Please choose the customer name of the project")]
        public int CustomersId { get; set; }

        public string Customers { get; set; }

        [Required(ErrorMessage = "Please choose the project manager to this project")]
        [Display(Name = "Project Manager")]
        public int projectManagerId { get; set; }

        public string projectManager { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Strat Date")]
        public DateTime startDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public bool PreProjectState { get; set; }
    }
}
