using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        [NotMapped]
        public string ProjectName { get; set; }
        [MaxLength(200)]
        public string Comment { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }        
    }
}
