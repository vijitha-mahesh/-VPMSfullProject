using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VPMS_Project.Data;

namespace VPMS_Project.Controllers
{
    public class PostController : ControllerBase
    {
        private readonly EmpStoreContext _context = null;

        public PostController(EmpStoreContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public IActionResult Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = _context.PreSalesProjects.Where(p => p.Title.Contains(term)).Select(p => p.Title).ToList();
                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
