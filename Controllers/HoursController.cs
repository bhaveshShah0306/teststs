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
using System.Globalization;
using Nest;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoursController : ControllerBase
    {
        private readonly DataContext _context;

        public HoursController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Hours
        [HttpGet("{typeId}/flag")]
        public async Task<ActionResult<IEnumerable<HoursVM>>> GetHours(int typeId)
        {
            try
            {
                var hoursdataList = _context.Hours.Where(c => c.TripTypeId == typeId).ToList();
                var hoursVMList = new List<HoursVM>();
                if (hoursdataList.Count > 0)
                {
                    foreach (var hoursdata in hoursdataList)
                    {
                        var hourVM = new HoursVM();

                        hourVM.HoursId = hoursdata.HoursId;
                        hourVM.TripTypeId = hoursdata.TripTypeId;
                        hourVM.TripVarientId = hoursdata.TripVarientId;
                        var tritypedata = _context.TripTypes.Find(hourVM.TripTypeId);
                        if (tritypedata != null)
                        {
                            hourVM.Starttime = tritypedata.Starttime;
                            hourVM.Endtime = tritypedata.Endtime;
                        }
                        string startTimeString = hourVM.Starttime;
                        string endTimeString = hourVM.Endtime;

                        // Convert the strings to DateTime objects
                        DateTime startTime = DateTime.ParseExact(startTimeString, "hh:mm tt", CultureInfo.InvariantCulture);
                        DateTime endTime = DateTime.ParseExact(endTimeString, "hh:mm tt", CultureInfo.InvariantCulture);

                        // Get the current time
                        DateTime currentTime = DateTime.Now;
                        int? totalHours = 0;

                        // Check if the current time is within the specified range
                        if (currentTime.TimeOfDay >= startTime.TimeOfDay && currentTime.TimeOfDay <= endTime.TimeOfDay)
                        {
                            hourVM.Charges = hoursdata.NightCharges;
                            hourVM.IsinHours = hoursdata.IsinHours;
                            hourVM.HoursinNumber = hoursdata.HoursName;
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
                        }
                        else
                        {
                            hourVM.Charges = hoursdata.Charges;
                            hourVM.IsinHours = hoursdata.IsinHours;
                            hourVM.HoursinNumber = hoursdata.HoursName;
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



        [HttpGet("{typeId}/{variantId}/{starttime}/{hours}")]
        public async Task<ActionResult<IEnumerable<HoursVM>>> GetHours(int typeId, int variantId, string starttime, int hours)
        {
            try
            {
                var hoursdataList = _context.Hours.Where(c => c.TripTypeId == typeId && c.TripVarientId == variantId).ToList();
                var hoursVMList = new List<HoursVM>();

                if (hoursdataList.Count > 0)
                {
                    foreach (var hoursdata in hoursdataList)
                    {
                        var hourVM = new HoursVM
                        {
                            HoursId = hoursdata.HoursId,
                            TripTypeId = hoursdata.TripTypeId
                        };

                        var tritypedata = _context.TripTypes.Find(hourVM.TripTypeId);
                        if (tritypedata != null)
                        {
                            hourVM.Starttime = tritypedata.Starttime;
                            hourVM.Endtime = tritypedata.Endtime;
                        }

                        // Convert the provided starttime to DateTime object
                        DateTime startTime = DateTime.ParseExact(starttime, "hh:mm tt", CultureInfo.InvariantCulture);
                        DateTime endTime = startTime.AddHours(hours);

                        // Convert the hourVM.Starttime and hourVM.Endtime to DateTime objects
                        DateTime tripTypeStartTime = DateTime.ParseExact(hourVM.Starttime, "hh:mm tt", CultureInfo.InvariantCulture);
                        DateTime tripTypeEndTime = DateTime.ParseExact(hourVM.Endtime, "hh:mm tt", CultureInfo.InvariantCulture);

                        // Get the current time
                        DateTime currentTime = DateTime.Now;
                        int? totalHours = 0;

                        if (startTime.TimeOfDay >= tripTypeStartTime.TimeOfDay || endTime.TimeOfDay <= tripTypeEndTime.TimeOfDay)
                        {
                            hourVM.Charges = hoursdata.NightCharges;
                            hourVM.IsinHours = hoursdata.IsinHours;
                            hourVM.HoursinNumber = hoursdata.HoursName;
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
                        }
                        else
                        {
                            hourVM.Charges = hoursdata.Charges;
                            hourVM.IsinHours = hoursdata.IsinHours;
                            hourVM.HoursinNumber = hoursdata.HoursName;
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



        public class Hoursposting
        {
            public int typeId { get; set; }
            public int variantId { get; set; }
            public string starttime { get; set; }
            public int hours { get; set; }
            public int hourId { get; set; }
        }

        [HttpPost("mobile")]
        public async Task<ActionResult<HoursVM>> GetHoursmobile(Hoursposting hourspost)
        {
            try
            {
                var hoursdataList = _context.Hours.Where(c => c.TripTypeId == hourspost.typeId && c.TripVarientId == hourspost.variantId).ToList();
                var hoursVMList = new List<HoursVM>();

                if (hoursdataList.Count > 0)
                {
                    foreach (var hoursdata in hoursdataList)
                    {
                        var hourVM = new HoursVM
                        {
                            HoursId = hoursdata.HoursId,
                            TripTypeId = hoursdata.TripTypeId
                        };

                        var tritypedata = _context.TripTypes.Find(hourVM.TripTypeId);
                        if (tritypedata != null)
                        {
                            hourVM.Starttime = tritypedata.Starttime;
                            hourVM.Endtime = tritypedata.Endtime;
                        }

                        // Convert the provided starttime to DateTime object
                        DateTime startTime = DateTime.ParseExact(hourspost.starttime, "hh:mm tt", CultureInfo.InvariantCulture);
                        DateTime endTime = startTime.AddHours(hourspost.hours);
                        var tripvariantstarttime = DateTime.ParseExact("10:00 PM", "hh:mm tt", CultureInfo.InvariantCulture);
                        var tripvariantendtime = DateTime.ParseExact("06:00 AM", "hh:mm tt", CultureInfo.InvariantCulture);
                        tripvariantendtime = endTime.Date.Add(tripvariantendtime.TimeOfDay); // Combine endTime date with "06:00 PM"
                        var tripvariant = await _context.TripVariants.FindAsync(hourspost.variantId);
                        if (tripvariant != null)
                        {
                            bool startsInNight = startTime.TimeOfDay >= tripvariantstarttime.TimeOfDay || startTime.TimeOfDay < tripvariantendtime.TimeOfDay;
                            bool endsInNight = endTime.TimeOfDay >= tripvariantstarttime.TimeOfDay || endTime.TimeOfDay < tripvariantendtime.TimeOfDay;

                            if (startsInNight || endsInNight)
                            {
                                hourVM.NightCharges = Convert.ToInt32(tripvariant.NightCharges);
                            }
                            else
                            {
                                hourVM.NightCharges = 0;
                            }
                        }
                        // Convert the hourVM.Starttime and hourVM.Endtime to DateTime objects
                        DateTime tripTypeStartTime = DateTime.ParseExact(hourVM.Starttime, "hh:mm tt", CultureInfo.InvariantCulture);
                        DateTime tripTypeEndTime = DateTime.ParseExact(hourVM.Endtime, "hh:mm tt", CultureInfo.InvariantCulture);

                        // Get the current time
                        DateTime currentTime = DateTime.Now;
                        int? totalHours = 0;

                        if (startTime.TimeOfDay >= tripTypeStartTime.TimeOfDay)
                        {
                            hourVM.Charges = hoursdata.NightCharges;
                            hourVM.IsinHours = hoursdata.IsinHours;
                            hourVM.HoursinNumber = hoursdata.HoursName;
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
                        }
                        else
                        {
                            hourVM.Charges = hoursdata.Charges;
                            hourVM.IsinHours = hoursdata.IsinHours;
                            hourVM.HoursinNumber = hoursdata.HoursName;
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
                        }

                        hourVM.TotalHours = totalHours;
                        hoursVMList.Add(hourVM);
                    }
                    var hourdata = hoursVMList.Where(c => c.HoursId == hourspost.hourId).FirstOrDefault();
                    return Ok(hourdata);
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




        // GET: api/Hours
        [HttpGet("{typeId}/flag/1")]
        public async Task<ActionResult<IEnumerable<HoursVM>>> GetHourbytype(int typeId, int tripvarientid)
        {
            try
            {
                var hoursdataList = _context.Hours.Where(c => c.TripTypeId == typeId && c.TripVarientId == tripvarientid).ToList();
                var hoursVMList = new List<HoursVM>();
                if (hoursdataList.Count > 0)
                {
                    foreach (var hoursdata in hoursdataList)
                    {
                        var hourVM = new HoursVM();

                        hourVM.HoursId = hoursdata.HoursId;
                        hourVM.TripTypeId = hoursdata.TripTypeId;
                        hourVM.TripVarientId = hoursdata.TripVarientId;
                        var tritypedata = _context.TripTypes.Find(hourVM.TripTypeId);
                        if (tritypedata != null)
                        {
                            hourVM.Starttime = tritypedata.Starttime;
                            hourVM.Endtime = tritypedata.Endtime;
                        }
                        string startTimeString = hourVM.Starttime;
                        string endTimeString = hourVM.Endtime;

                        // Convert the strings to DateTime objects
                        DateTime startTime = DateTime.ParseExact(startTimeString, "hh:mm tt", CultureInfo.InvariantCulture);
                        DateTime endTime = DateTime.ParseExact(endTimeString, "hh:mm tt", CultureInfo.InvariantCulture);

                        // Get the current time
                        DateTime currentTime = DateTime.Now;
                        int? totalHours = 0;

                        // Check if the current time is within the specified range

                        hourVM.NightCharges = hoursdata.NightCharges;
                        hourVM.IsinHours = hoursdata.IsinHours;
                        hourVM.HoursinNumber = hoursdata.HoursName;
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


                        hourVM.Charges = hoursdata.Charges;
                        hourVM.IsinHours = hoursdata.IsinHours;
                        hourVM.HoursinNumber = hoursdata.HoursName;
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

        // GET: api/Hours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hours>> GetHour(int id)
        {
            if (_context.Hours == null)
            {
                return NotFound();
            }
            var hours = await _context.Hours.FindAsync(id);

            if (hours == null)
            {
                return NotFound();
            }

            return hours;
        }

        // PUT: api/Hours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutHours(int id, Hours hours)
        {
            if (id != hours.HoursId)
            {
                return BadRequest();
            }

            _context.Entry(hours).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoursExists(id))
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

        // POST: api/Hours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hours>> PostHours(Hours hours)
        {
            if (_context.Hours == null)
            {
                return Problem("Entity set 'DataContext.Hours'  is null.");
            }
            _context.Hours.Add(hours);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHour", new { id = hours.HoursId }, hours);
        }

        // DELETE: api/Hours/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteHours(int id)
        {
            if (_context.Hours == null)
            {
                return NotFound();
            }
            var hours = await _context.Hours.FindAsync(id);
            if (hours == null)
            {
                return NotFound();
            }

            _context.Hours.Remove(hours);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HoursExists(int id)
        {
            return (_context.Hours?.Any(e => e.HoursId == id)).GetValueOrDefault();
        }
    }
}
