using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class BudgetModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Labour Hours")]
        public int LbHou { get; set; }

        [Required]
        [Display(Name = "Labour Rate")]
        public int LbRate { get; set; }

        [Required]
        [Display(Name = "Matirial Unit")]
        public int MtUnit { get; set; }

        [Required]
        [Display(Name = "Matirial Unit Price")]
        public int MtunitPr { get; set; }

        [Required]
        [Display(Name = "Cost")]
        public int cost { get; set; }

        [Required]
        [Display(Name = "Budget Cost")]
        public int BudgetCost { get; set; }

        [Required]
        [Display(Name = "Actual")]
        public int Actual { get; set; }

        [Required]
        [Display(Name = "Under")]
        public int Under { get; set; }

        [Required(ErrorMessage = "Please select project")]
        [Display(Name = "Project Name")]
        public int ProjectsID { get; set; }

        public string projectName { get; set; }





    }
}
