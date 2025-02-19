using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants;
using WebAPI.Models;
using WebAPI.Seeds;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class SeedersController : ControllerBase
    {
        private readonly HRDBContext _context;

        public SeedersController(HRDBContext context) 
        {
            _context = context;
        }

        [HttpGet("run-seeders")]
        public async Task<IActionResult> RunSeeders()
        {
            if(await GeneralSeeders.Seed(_context) == false)
            {
                throw new Exception("Database already has data");
            };
            return Ok("Seeding completed successfully.");
        }
    }
}
