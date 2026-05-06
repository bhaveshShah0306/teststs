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
    public class InsurenceTaxandPricesController : ControllerBase
    {
        private readonly DataContext _context;

        public InsurenceTaxandPricesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/InsurenceTaxandPrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsurencetaxandpricesVM>>> GetInsurenceTaxandPrice()
        {
            try
            {
                var insurencelist = _context.InsurenceTaxandPrice.ToList();
                var insurenceVMList = new List<InsurencetaxandpricesVM>();
                if(insurencelist.Count > 0)
                {
                    foreach(var insurence in insurencelist)
                    {
                        var insurenceVM = new InsurencetaxandpricesVM();

                        insurenceVM.InsurenceTaxandPriceId = insurence.InsurenceTaxandPriceId;
                        insurenceVM.TaxPercentage= insurence.TaxPercentage;
                        insurenceVM.Price = insurence.Price;

                        Decimal taxprice = insurenceVM.Price * insurenceVM.TaxPercentage / 100;
                        insurenceVM.Taxprice = taxprice;

                        insurenceVMList.Add(insurenceVM);
                    }
                    return Ok(insurenceVMList);
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

        // GET: api/InsurenceTaxandPrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InsurencetaxandpricesVM>> GetInsurenceTaxandPrice(int id)
        {
            try
            {
                var insurence = _context.InsurenceTaxandPrice.Find(id);
                var insurenceVM = new InsurencetaxandpricesVM();
                if(insurence != null)
                {
                    insurenceVM.InsurenceTaxandPriceId = insurence.InsurenceTaxandPriceId;
                    insurenceVM.TaxPercentage = insurence.TaxPercentage;
                    insurenceVM.Price = insurence.Price;

                    Decimal taxprice = insurenceVM.Price * insurenceVM.TaxPercentage / 100;
                    insurenceVM.Taxprice = taxprice;

                    return Ok(insurenceVM);
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

        // PUT: api/InsurenceTaxandPrices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutInsurenceTaxandPrice(int id, InsurenceTaxandPrice insurenceTaxandPrice)
        {
            if (id != insurenceTaxandPrice.InsurenceTaxandPriceId)
            {
                return BadRequest();
            }

            _context.Entry(insurenceTaxandPrice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsurenceTaxandPriceExists(id))
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

        // POST: api/InsurenceTaxandPrices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InsurenceTaxandPrice>> PostInsurenceTaxandPrice(InsurenceTaxandPrice insurenceTaxandPrice)
        {
          if (_context.InsurenceTaxandPrice == null)
          {
              return Problem("Entity set 'DataContext.InsurenceTaxandPrice'  is null.");
          }
            _context.InsurenceTaxandPrice.Add(insurenceTaxandPrice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInsurenceTaxandPrice", new { id = insurenceTaxandPrice.InsurenceTaxandPriceId }, insurenceTaxandPrice);
        }

        // DELETE: api/InsurenceTaxandPrices/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteInsurenceTaxandPrice(int id)
        {
            if (_context.InsurenceTaxandPrice == null)
            {
                return NotFound();
            }
            var insurenceTaxandPrice = await _context.InsurenceTaxandPrice.FindAsync(id);
            if (insurenceTaxandPrice == null)
            {
                return NotFound();
            }

            _context.InsurenceTaxandPrice.Remove(insurenceTaxandPrice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InsurenceTaxandPriceExists(int id)
        {
            return (_context.InsurenceTaxandPrice?.Any(e => e.InsurenceTaxandPriceId == id)).GetValueOrDefault();
        }
    }
}
