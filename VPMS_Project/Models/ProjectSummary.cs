using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class ProjectSummary
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public double EstimatedCost { get; set; }
        public double Budget { get; set; }
        public DateTime StartDate {get; set;}
        public DateTime EndDate{get; set;}
        public double CurrentActualCost { get; set; }   
        public double CurrentEstimatedCost { get; set; }        
        public double Settled { get; set; }      
    }
}
