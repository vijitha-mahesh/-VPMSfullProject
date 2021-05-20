using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Models
{
    public class Resources
    {
        public int Id { get; set; }
        public IList<SelectListItem> employee { get; set; }
        
    }
}
