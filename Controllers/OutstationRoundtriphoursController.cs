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
using Microsoft.CodeAnalysis;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutstationRoundtriphoursController : ControllerBase
    {
        private readonly DataContext _context;

        public OutstationRoundtriphoursController(DataContext context)
        {
            _context = context;
        }

        // GET: api/OutstationRoundtriphours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoursVM>>> GetOutstationRoundtriphours()
        {
            try
            {
                var hoursdataList = _context.OutstationRoundtriphours.ToList();
                var hoursVMList = new List<HoursVM>();
                if (hoursdataList.Count > 0)
                {
                    foreach (var hoursdata in hoursdataList)
                    {
                        var hourVM = new HoursVM();

                        hourVM.HoursId = hoursdata.HoursId;
                        hourVM.TripTypeId = hoursdata.TripTypeId;
                        hourVM.Charges = hoursdata.Charges;
                        hourVM.IsinHours = hoursdata.IsinHours;
                        int? totalHours = 0;
                        if (hourVM.IsinHours == true)
                        {
                            if (hoursdata.HoursName == 1)
                            {
                                hourVM.HoursName = hoursdata.HoursName + "Hr";

                            }
                            else
                            {
                                hourVM.HoursName = hoursdata.HoursName + "Hrs";

                            }
                            totalHours = hoursdata.HoursName;
                        }
                        else
                        {
                            if (hoursdata.HoursName == 1)
                            {
                                hourVM.HoursName = hoursdata.HoursName + "Day";
                            }
                            else
                            {
                                hourVM.HoursName = hoursdata.HoursName + "Days";
                            }
                            totalHours = hoursdata.HoursName * 24;
                        }
                        hourVM.TotalHours = totalHours;


                        hoursVMList.Add(hourVM);

                    }
                    return Ok(hoursVMList);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/OutstationRoundtriphours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OutstationRoundtriphours>> GetOutstationRoundtriphours(int id)
        {
          if (_context.OutstationRoundtriphours == null)
          {
              return NotFound();
          }
            var outstationRoundtriphours = await _context.OutstationRoundtriphours.FindAsync(id);

            if (outstationRoundtriphours == null)
            {
                return NotFound();
            }

            return outstationRoundtriphours;
        }

        // PUT: api/OutstationRoundtriphours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutOutstationRoundtriphours(int id, OutstationRoundtriphours outstationRoundtriphours)
        {
            if (id != outstationRoundtriphours.HoursId)
            {
                return BadRequest();
            }

            _context.Entry(outstationRoundtriphours).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OutstationRoundtriphoursExists(id))
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

        // POST: api/OutstationRoundtriphours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OutstationRoundtriphours>> PostOutstationRoundtriphours(OutstationRoundtriphours outstationRoundtriphours)
        {
          if (_context.OutstationRoundtriphours == null)
          {
              return Problem("Entity set 'DataContext.OutstationRoundtriphours'  is null.");
          }
            _context.OutstationRoundtriphours.Add(outstationRoundtriphours);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOutstationRoundtriphours", new { id = outstationRoundtriphours.HoursId }, outstationRoundtriphours);
        }

        // DELETE: api/OutstationRoundtriphours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOutstationRoundtriphours(int id)
        {
            if (_context.OutstationRoundtriphours == null)
            {
                return NotFound();
            }
            var outstationRoundtriphours = await _context.OutstationRoundtriphours.FindAsync(id);
            if (outstationRoundtriphours == null)
            {
                return NotFound();
            }

            _context.OutstationRoundtriphours.Remove(outstationRoundtriphours);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OutstationRoundtriphoursExists(int id)
        {
            return (_context.OutstationRoundtriphours?.Any(e => e.HoursId == id)).GetValueOrDefault();
        }
    }
}
