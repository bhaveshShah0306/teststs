using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchuduletriptimechagemodelsController : ControllerBase
    {
        private readonly DataContext _context;

        public SchuduletriptimechagemodelsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Schuduletriptimechagemodels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchuduletriptimechageVM>>> GetSchuduletriptimechagemodel()
        {
            try
            {
               
                var schuduleModelList = await _context.Schuduletriptimechagemodel.ToListAsync();
                var schuduleVmList = new List<SchuduletriptimechageVM>();

                if (schuduleModelList.Count >0)
                {
                    // Iterating through each subscription model to create the view model
                    foreach (var subModel in schuduleModelList)
                    {
                        var subvm = new SchuduletriptimechageVM();
                        subvm.SchuduletriptimechagemodelId = subModel.SchuduletriptimechagemodelId;
                        subvm.SchuduletripDuration = subModel.SchuduletripDuration;
                        subvm.Reason = subModel.Reason;
                       
                        subvm.TripTypeId = subModel.TripTypeId;
                
                        var subtimechagedata = _context.TripTypes.Find(subModel.TripTypeId);
                        if (subtimechagedata != null)
                        {
                            subvm.TripName = subtimechagedata.TripName;
                           
                        }
                        schuduleVmList.Add(subvm);
                    }

                    return Ok(schuduleVmList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Schuduletriptimechagemodel>> GetSchuduletriptimechagemodel(int id)
        {
            try
            {
               
                var subModel = await _context.Schuduletriptimechagemodel.FindAsync(id);

                if (subModel != null)
                {
                    var subvm = new SchuduletriptimechageVM
                    {
                        SchuduletriptimechagemodelId = subModel.SchuduletriptimechagemodelId,
                        SchuduletripDuration = subModel.SchuduletripDuration,
                        Reason = subModel.Reason,
                        TripTypeId = subModel.TripTypeId
                    };

                    var subtimechagedata = await _context.TripTypes.FindAsync(subModel.TripTypeId);
                    if (subtimechagedata != null)
                    {
                        subvm.TripName = subtimechagedata.TripName;
                    }

                    return Ok(subvm);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Schuduletriptimechagemodels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutSchuduletriptimechagemodel(int id, Schuduletriptimechagemodel schuduletriptimechagemodel)
        {
            if (id != schuduletriptimechagemodel.SchuduletriptimechagemodelId)
            {
                return BadRequest();
            }

            _context.Entry(schuduletriptimechagemodel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchuduletriptimechagemodelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Schuduletriptimechagemodels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Schuduletriptimechagemodel>> PostSchuduletriptimechagemodel(Schuduletriptimechagemodel schuduletriptimechagemodel)
        {
          if (_context.Schuduletriptimechagemodel == null)
          {
              return Problem("Entity set 'DataContext.Schuduletriptimechagemodel'  is null.");
          }
            _context.Schuduletriptimechagemodel.Add(schuduletriptimechagemodel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSchuduletriptimechagemodel", new { id = schuduletriptimechagemodel.SchuduletriptimechagemodelId }, schuduletriptimechagemodel);
        }

        // DELETE: api/Schuduletriptimechagemodels/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteSchuduletriptimechagemodel(int id)
        {
            if (_context.Schuduletriptimechagemodel == null)
            {
                return NotFound();
            }
            var schuduletriptimechagemodel = await _context.Schuduletriptimechagemodel.FindAsync(id);
            if (schuduletriptimechagemodel == null)
            {
                return NotFound();
            }

            _context.Schuduletriptimechagemodel.Remove(schuduletriptimechagemodel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SchuduletriptimechagemodelExists(int id)
        {
            return (_context.Schuduletriptimechagemodel?.Any(e => e.SchuduletriptimechagemodelId == id)).GetValueOrDefault();
        }
    }
}
