using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VPMS_Project.Data;

namespace WebApplication3.Controllers
{
    [Route("api/postapi")]
    [ApiController]
    public class PostApiController : ControllerBase
    {

        private readonly EmpStoreContext _context = null;

        public PostApiController(EmpStoreContext context)
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
                var names = _context.Projects.Where(p => p.Name.Contains(term)).Select(p => p.Name).ToList();
                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }


        [Produces("application/json")]
        [HttpGet("searchTask")]
        public IActionResult SearchTask()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = _context.Tasks.Where(p => p.Name.Contains(term)).Select(p => p.Name).ToList();
                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
