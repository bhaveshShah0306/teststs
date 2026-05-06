using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerWalletsController : ControllerBase
    {
        private readonly DataContext _context;

        public CustomerWalletsController(DataContext context)
        {
            _context = context;
        }



        // GET: api/CustomerWallets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerWallet>>> GetCustomerWallets()
        {
          if (_context.CustomerWallets == null)
          {
              return NotFound();
          }
            return await _context.CustomerWallets.ToListAsync();
        }



        // GET: api/CustomerWallets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerWallet>> GetCustomerWallet(int id)
        {
          if (_context.CustomerWallets == null)
          {
              return NotFound();
          }
            var customerWallet = await _context.CustomerWallets.FindAsync(id);

            if (customerWallet == null)
            {
                return NotFound();
            }

            return customerWallet;
        }




        // PUT: api/CustomerWallets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerWallet(int id, CustomerWallet customerWallet)
        {
            if (id != customerWallet.CustomerWalletId)
            {
                return BadRequest();
            }

            _context.Entry(customerWallet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerWalletExists(id))
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


        // POST: api/CustomerWallets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerWallet>> PostCustomerWallet(CustomerWallet customerWallet)
        {
          if (_context.CustomerWallets == null)
          {
              return Problem("Entity set 'DataContext.CustomerWallets'  is null.");
          }
            _context.CustomerWallets.Add(customerWallet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerWallet", new { id = customerWallet.CustomerWalletId }, customerWallet);
        }


        // DELETE: api/CustomerWallets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerWallet(int id)
        {
            if (_context.CustomerWallets == null)
            {
                return NotFound();
            }
            var customerWallet = await _context.CustomerWallets.FindAsync(id);
            if (customerWallet == null)
            {
                return NotFound();
            }

            _context.CustomerWallets.Remove(customerWallet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerWalletExists(int id)
        {
            return (_context.CustomerWallets?.Any(e => e.CustomerWalletId == id)).GetValueOrDefault();
        }
    }
}
