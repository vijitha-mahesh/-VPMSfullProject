using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class CollectionModel
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int value { get; set; }

        [Required(ErrorMessage = "Please select project")]
        [Display(Name = "Project Name")]
        public int ProjectsID { get; set; }

        public string Projects { get; set; }
    }
}
