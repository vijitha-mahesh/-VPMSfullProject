using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;

namespace VPMS_Project.Models
{
    public class DocumentModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please select project")]
        [Display(Name = "Project ID")]
        public int ProjectsID { get; set; }


        [Display(Name = "Scope document of the project")]
        [NotMapped]
        public IFormFile ScopeDocument { get; set; }

        public string ScopeDocumentUrl { get; set; }


        [Display(Name = "Action Plan of the project")]
        [NotMapped]
        public IFormFile ActionPlan { get; set; }

        public string ActionPlanUrl { get; set; }


        [Display(Name = "Time Plan of the project")]
        [NotMapped]
        public IFormFile TimePlan { get; set; }

        public string TimePlanUrl { get; set; }








    }
}
