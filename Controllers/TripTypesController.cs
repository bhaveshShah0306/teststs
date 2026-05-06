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
using Nest;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public TripTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/TripTypes
        [HttpGet]
        public async Task<IActionResult> GetTripTypes()
        {
            try
            {
                var triptypes = await _context.TripTypes.AsNoTracking().ToListAsync();
                var triptypeVMs = new List<TripTypeVM>();

                if (triptypes.Count > 0)
                {
                    foreach (var tripType in triptypes)
                    {
                        var tripTypeVM = new TripTypeVM
                        {
                            TripName = tripType.TripName,
                            TripDescription = tripType.TripDescription,
                            Icon = tripType.Icon,
                            Starttime = tripType.Starttime,
                            Endtime = tripType.Endtime,
                            TripTypeId = tripType.TripTypeId,
                        };

                        var tripvariantData = await _context.TripVariants
                            .AsNoTracking()
                            .Where(c => c.TriptypeId == tripType.TripTypeId)
                            .ToListAsync();

                        var tripVariantsVM = new List<TripVarientListVM>();

                        if (tripvariantData.Count > 0)
                        {
                            foreach (var tripVariant in tripvariantData)
                            {
                                var tvvm = new TripVarientListVM
                                {
                                    TripVariantId = tripVariant.TripVariantId,
                                    TripVariantName = tripVariant.TripVariantName,
                                    TriptypeId = tripVariant.TriptypeId,
                                    TripTypeName = tripTypeVM.TripName,
                                    BasePrice = tripVariant.BasePrice,
                                    KilometerLimit = tripVariant.KilometerLimit,
                                    NightCharges = tripVariant.NightCharges,
                                    ChargesperMinute = tripVariant.ChargesperMinute,
                                    PricePerKilometers = tripVariant.PricePerKilometers,
                                    IsTripOneway = tripVariant.IsTripOneway,
                                };
                                tripVariantsVM.Add(tvvm);
                            }
                        }

                        tripTypeVM.Tripvarients = tripVariantsVM;
                        triptypeVMs.Add(tripTypeVM);
                    }

                    return Ok(triptypeVMs);
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


        // GET: api/TripTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripTypeVM>> GetTripType(int id)
        {
            try
            {
                var triptype = _context.TripTypes.Find(id);
                var triptypeVM = new TripTypeVM();
                if (triptype!=null)
                {
                    triptypeVM.TripTypeId = triptype.TripTypeId;


                    triptypeVM.TripName = triptype.TripName;
                    triptypeVM.TripDescription = triptype.TripDescription;
                    triptypeVM.Icon = triptype.Icon;

                    var tripvarientdata = await _context.TripVariants.Where(c => c.TriptypeId == triptypeVM.TripTypeId).ToListAsync();
                    var tripvarents = new List<TripVarientListVM>();
                    if(tripvarientdata.Count>0)
                    {

                        foreach (var tripvariant in tripvarientdata)
                    {
                        var tvvm = new TripVarientListVM();

                        tvvm.TripVariantId = tripvariant.TripVariantId;
                        
                        tvvm.TripVariantName = tripvariant.TripVariantName;
                        
                        tvvm.TriptypeId = tripvariant.TriptypeId;
                        
                        tvvm.TripTypeName = triptypeVM.TripName;
                        
                        tvvm.BasePrice = tripvariant.BasePrice;
                        
                        tvvm.KilometerLimit = tripvariant.KilometerLimit;
                        
                        tvvm.NightCharges = tripvariant.NightCharges;
                        
                        tvvm.ChargesperMinute = tripvariant.ChargesperMinute;
                        
                        tvvm.PricePerKilometers = tripvariant.PricePerKilometers;
                        
                        tvvm.IsTripOneway = tripvariant.IsTripOneway;
                        tripvarents.Add(tvvm);

                    }
                    }

                    triptypeVM.Tripvarients = tripvarents;

                    return Ok(triptypeVM);
                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT: api/TripTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutTripType(int id, TripType tripType)
        {
            if (id != tripType.TripTypeId)
            {
                return BadRequest();
            }

            _context.Entry(tripType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripTypeExists(id))
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

        // POST: api/TripTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TripType>> PostTripType(TripType tripType)
        {
          if (_context.TripTypes == null)
          {
              return Problem("Entity set 'DataContext.TripTypes'  is null.");
          }
            _context.TripTypes.Add(tripType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripType", new { id = tripType.TripTypeId }, tripType);
        }

        // DELETE: api/TripTypes/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteTripType(int id)
        {
            if (_context.TripTypes == null)
            {
                return NotFound();
            }
            var tripType = await _context.TripTypes.FindAsync(id);
            if (tripType == null)
            {
                return NotFound();
            }

            _context.TripTypes.Remove(tripType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripTypeExists(int id)
        {
            return (_context.TripTypes?.Any(e => e.TripTypeId == id)).GetValueOrDefault();
        }
    }
}
