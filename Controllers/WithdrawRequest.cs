using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawRequestController : ControllerBase
    {
        private readonly DataContext _context;

        public WithdrawRequestController(DataContext context)
        {
            _context = context;
        }

        // GET: api/WithdrawRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WithdrawRequestVM>>> GetWithdrawRequests()
        {
            try
            {
                var withdrawmodel = await _context.WithdrawRequest.OrderByDescending(c=>c.WithdrawRequestId).ToListAsync();
                var withdrawmodelVM = new List<WithdrawRequestVM>();

                if (withdrawmodel.Count > 0)
                {
                    foreach (var withdraw in withdrawmodel)
                    {
                        var withdrawVM = new WithdrawRequestVM
                        {
                            WithdrawRequestId = withdraw.WithdrawRequestId,
                            withdrawamount = withdraw.withdrawamount,
                            DriverId = withdraw.DriverId,
                            IsApproved=withdraw.IsApproved,
                            CreatedDate = withdraw.CreatedDate,
                            ModifiedDate = withdraw.ModifiedDate,
                        };

                        var Driverdata = await _context.Drivers.FindAsync(withdrawVM.DriverId);
                        if (Driverdata != null)
                        {
                            withdrawVM.DriverName = Driverdata.DriverName;
                            withdrawVM.PhoneNumber = Driverdata.PhoneNumber;
                        }

                        withdrawmodelVM.Add(withdrawVM);
                    }

                    return Ok(withdrawmodelVM);
                }
                else
                {
                    return NotFound("No withdraw requests found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        // GET: api/WithdrawRequest/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WithdrawRequestVM>> GetWithdrawRequest(int id)
        {
            try
            {
                // Find the WithdrawRequest by ID
                var withdraw = await _context.WithdrawRequest.FindAsync(id);

                if (withdraw == null)
                {
                    return NotFound($"Withdraw request with ID {id} not found.");
                }

                // Create a new WithdrawRequestVM and populate it
                var withdrawVM = new WithdrawRequestVM
                {
                    WithdrawRequestId = withdraw.WithdrawRequestId,
                    withdrawamount = withdraw.withdrawamount,
                    DriverId = withdraw.DriverId,
                    CreatedDate = withdraw.CreatedDate,
                    ModifiedDate = withdraw.ModifiedDate,
                    IsApproved = withdraw.IsApproved
                    
                };

                // Fetch the associated Driver data
                var Driverdata = await _context.Drivers.FindAsync(withdrawVM.DriverId);
                if (Driverdata != null)
                {
                    withdrawVM.DriverName = Driverdata.DriverName;
                    withdrawVM.PhoneNumber = Driverdata.PhoneNumber;
                }

                // Return the populated WithdrawRequestVM
                return Ok(withdrawVM);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/WithdrawRequest/{id}
        [HttpPost("{id}")]
        public async Task<IActionResult> PutWithdrawRequest(int id, WithdrawRequest withdrawRequest)
        {
            if (id != withdrawRequest.WithdrawRequestId)
            {
                return BadRequest();
            }

            _context.Entry(withdrawRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WithdrawRequestExists(id))
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

        // POST: api/WithdrawRequest
        [HttpPost]
        public async Task<IActionResult> PostWithdrawRequest(WithdrawRequest withdrawRequest)
        {
            try
            {
                // Find the driver's wallet data
                var driverwalletdata = await _context.Driverwallets
                    .Where(r => r.DriverId == withdrawRequest.DriverId)
                    .FirstOrDefaultAsync();

                if (driverwalletdata == null)
                {
                    return NotFound(new
                    {
                        Status = "Error",
                        Message = $"Driver with ID {withdrawRequest.DriverId} not found."
                    });
                }

                var walletBalance = Convert.ToInt32(driverwalletdata.WalletBalance);
                var withdrawAmount = Convert.ToInt32(withdrawRequest.withdrawamount);

                if (walletBalance - withdrawAmount > 500)
                {
                    if (_context.WithdrawRequest == null)
                    {
                        return Problem("Entity set 'DataContext.WithdrawRequest' is null.");
                    }
                    withdrawRequest.CreatedDate = DateTime.Now;
                    // Add the new WithdrawRequest to the database
                    _context.WithdrawRequest.Add(withdrawRequest);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetWithdrawRequest",
                        new { id = withdrawRequest.WithdrawRequestId },
                        new
                        {
                            Status = "Success",
                            Message = "Withdraw request created successfully.",
                            Data = withdrawRequest
                        });
                }
                else
                {
                    // Return a message indicating that the minimum amount is not met
                    return Ok(new
                    {
                        Status = "Error",
                        Message = "The wallet balance must be greater than 500 to make a withdrawal request.",
                            Data = withdrawRequest
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }


        // DELETE: api/WithdrawRequest/{id}
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteWithdrawRequest(int id)
        {
            if (_context.WithdrawRequest == null)
            {
                return NotFound();
            }
            var withdrawRequest = await _context.WithdrawRequest.FindAsync(id);
            if (withdrawRequest == null)
            {
                return NotFound();
            }

            _context.WithdrawRequest.Remove(withdrawRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WithdrawRequestExists(int id)
        {
            return (_context.WithdrawRequest?.Any(e => e.WithdrawRequestId == id)).GetValueOrDefault();
        }
    }
}
