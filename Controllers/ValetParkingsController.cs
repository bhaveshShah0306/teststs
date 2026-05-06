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
    public class ValetParkingsController : ControllerBase
    {
        private readonly DataContext _context;

        public ValetParkingsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ValetParkings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValetVM>>> GetValetParkings()
        {
            try
            {
                var valetdatalist =await _context.ValetParkings.ToListAsync();
                var valetMList = new List<ValetVM>();
                if (valetdatalist.Count > 0)
                {
                    foreach(var valet in valetdatalist)
                    {
                        var valetVm = new ValetVM();
                        valetVm.ValetParkingId = valet.ValetParkingId;
                        valetVm.UniquevaletParkingId = valet.UniquevaletParkingId;
                        valetVm.Venue= valet.Venue;
                        
                        valetVm.UserID = valet.UserID;
                        
                        if(valetVm.UserID != null && valetVm.UserID != 0)
                        {

                            var userdata = await _context.Users.FindAsync(valetVm.UserID);
                            if (userdata != null)
                            {
                                valetVm.UserName = userdata.Name;
                                valetVm.ContactNumber= userdata.PhoneNumber;
                            }
                        }

                        valetVm.LocationCoordinates = valet.LocationCoordinates;
                        
                        valetVm.StartDatetime = valet.StartDatetime;
                        
                        valetVm.EndDatetime = valet.EndDatetime;
                        
                        valetVm.NumberOfDriversRequired = valet.NumberOfDriversRequired;
                        
                        valetVm.NumberOfSupervisors = valet.NumberOfSupervisors;

                        valetVm.DriverMeansOfTransport = valet.DriverMeansOfTransport;

                        valetVm.NumberOfHours = valet.NumberOfHours;
                        
                        valetVm.GSTPercentage = valet.GSTPercentage;
                        
                        valetVm.GSTPrice = valet.GSTPrice;
                        
                        valetVm.GrandTotal = valet.GrandTotal;
                        valetVm.IsAdvancePaid = valet.IsAdvancePaid;
                        valetVm.AdvanceAMount = valet.AdvanceAMount;
                        valetMList.Add(valetVm);


                    }
                    return Ok(valetMList);
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





        [HttpGet("{driverId}/ValetParkingsdata")]
        public async Task<ActionResult<IEnumerable<ValetVM>>> GetValetParkingsbydriver(int driverId)
        {
            try
            {
                var valetdatalist =  _context.ValetParkings.Where(c => c.DriverId == driverId).ToList();
                var valetMList = new List<ValetVM>();
                if (valetdatalist.Count > 0)
                {
                    foreach (var valet in valetdatalist)
                    {
                        var valetVm = new ValetVM();
                        valetVm.ValetParkingId = valet.ValetParkingId;
                        valetVm.UniquevaletParkingId = valet.UniquevaletParkingId;
                        valetVm.Venue = valet.Venue;

                        valetVm.UserID = valet.UserID;

                        if (valetVm.UserID != null && valetVm.UserID != 0)
                        {

                            var userdata = await _context.Users.FindAsync(valetVm.UserID);
                            if (userdata != null)
                            {
                                valetVm.UserName = userdata.Name;
                                valetVm.ContactNumber = userdata.PhoneNumber;
                            }
                        }

                        valetVm.LocationCoordinates = valet.LocationCoordinates;

                        valetVm.StartDatetime = valet.StartDatetime;

                        valetVm.EndDatetime = valet.EndDatetime;

                        valetVm.NumberOfDriversRequired = valet.NumberOfDriversRequired;

                        valetVm.NumberOfSupervisors = valet.NumberOfSupervisors;

                        valetVm.DriverMeansOfTransport = valet.DriverMeansOfTransport;

                        valetVm.NumberOfHours = valet.NumberOfHours;

                        valetVm.GSTPercentage = valet.GSTPercentage;

                        valetVm.GSTPrice = valet.GSTPrice;

                        valetVm.GrandTotal = valet.GrandTotal;
                        valetVm.IsAdvancePaid = valet.IsAdvancePaid;
                        valetVm.AdvanceAMount = valet.AdvanceAMount;
                        valetMList.Add(valetVm);


                    }
                    return Ok(valetMList);
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














        // GET: api/ValetParkings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ValetParking>> GetValetParking(int id)
        {
            try
            {
                var valet = await _context.ValetParkings.FindAsync(id);
                var valetVm =new ValetVM();
                if(valet!=null)
                {
                    valetVm.ValetParkingId = valet.ValetParkingId;
                    valetVm.UniquevaletParkingId = valet.UniquevaletParkingId;
                    valetVm.Venue = valet.Venue;

                    valetVm.UserID = valet.UserID;

                    if (valetVm.UserID != null && valetVm.UserID != 0)
                    {

                        var userdata = await _context.Users.FindAsync(valetVm.UserID);
                        if (userdata != null)
                        {
                            valetVm.UserName = userdata.Name;
                            valetVm.ContactNumber = userdata.PhoneNumber;
                        }
                    }

                    valetVm.LocationCoordinates = valet.LocationCoordinates;

                    valetVm.StartDatetime = valet.StartDatetime;

                    valetVm.EndDatetime = valet.EndDatetime;

                    valetVm.NumberOfDriversRequired = valet.NumberOfDriversRequired;

                    valetVm.DriverMeansOfTransport = valet.DriverMeansOfTransport;

                    valetVm.NumberOfSupervisors = valet.NumberOfSupervisors;

                    valetVm.NumberOfHours = valet.NumberOfHours;

                    valetVm.GSTPercentage = valet.GSTPercentage;

                    valetVm.GSTPrice = valet.GSTPrice;

                    valetVm.GrandTotal = valet.GrandTotal;
                    valetVm.IsAdvancePaid = valet.IsAdvancePaid;
                    valetVm.AdvanceAMount = valet.AdvanceAMount;


                    return Ok(valetVm);
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

        // PUT: api/ValetParkings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutValetParking(int id, ValetParking valetParking)
        {
            if (id != valetParking.ValetParkingId)
            {
                return BadRequest();
            }

            _context.Entry(valetParking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValetParkingExists(id))
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

        // POST: api/ValetParkings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ValetParking>> PostValetParking(ValetParking valetParking)
        {

            if (valetParking.StartDatetime.HasValue)
            {
                TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(valetParking.StartDatetime.Value, istTimeZone);

                valetParking.StartDatetime = istStartDateTime;
            }
            var numberofhours = valetParking.NumberOfHours;

            if (numberofhours != null)
            {
                var endtime = valetParking.StartDatetime?.AddHours(Convert.ToDouble(numberofhours));
                valetParking.EndDatetime= endtime;
            }
            if (_context.ValetParkings == null)
            {
              return Problem("Entity set 'DataContext.ValetParkings'  is null.");
            }
            _context.ValetParkings.Add(valetParking);
            await _context.SaveChangesAsync();

            // Get the offerId as a string and pad it with leading zeros if needed
            string valetParkingId = valetParking.ValetParkingId.ToString().PadLeft(1, '0');

            // Get current month and day as strings
            string currentMonth = DateTime.Today.Month.ToString().PadLeft(2, '0');
            string currentDay = DateTime.Today.Day.ToString().PadLeft(2, '0');

            // Construct the offer code following the pattern:
            // 3 chars from OfferName, '0', OfferId, '0', currentMonth, currentDay
            var monthlyCode = $"Go{valetParkingId}vp{currentDay}{currentMonth}";
            valetParking.UniquevaletParkingId = monthlyCode;
            _context.Entry(valetParking).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetValetParking", new { id = valetParking.ValetParkingId }, valetParking);
        }

        // DELETE: api/ValetParkings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValetParking(int id)
        {
            if (_context.ValetParkings == null)
            {
                return NotFound();
            }
            var valetParking = await _context.ValetParkings.FindAsync(id);
            if (valetParking == null)
            {
                return NotFound();
            }

            _context.ValetParkings.Remove(valetParking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ValetParkingExists(int id)
        {
            return (_context.ValetParkings?.Any(e => e.ValetParkingId == id)).GetValueOrDefault();
        }
    }
}
