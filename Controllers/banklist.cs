using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class banklist : ControllerBase
    {
        private readonly DataContext _context;

        public banklist(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankList>>> Banklistdata()
        {
            try
            {
                return await _context.BankList.ToListAsync();
            }
            catch (Exception ex) 
            { return BadRequest(ex.Message); }
        }

    }
}
