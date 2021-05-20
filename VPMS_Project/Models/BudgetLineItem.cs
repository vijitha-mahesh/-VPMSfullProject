using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class BudgetLineItem
    {
        public int Id { get; set; }
        public int TaskID { get; set; }
        public int ProjectId { get; set; }
        public string TaskName { get; set; }
        public string Employee { get; set; }
        public double Time { get; set; }
        public double EstimatedCost { get; set; }
        public double ActualCost { get; set; }
        public bool IsCompleted { get; set; }
        public double Other { get; set; }
    }
}