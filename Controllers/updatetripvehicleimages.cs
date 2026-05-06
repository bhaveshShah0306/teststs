using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class updatetripvehicleimages : ControllerBase
    {
        private readonly DataContext _context;

        public updatetripvehicleimages(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<Trip>> postimages(int tripId, int driverId, string imageurls,Boolean istakenpics,Boolean isendimagestaken)
        {
            try
            {
                var tripdata = _context.Trips.Find(tripId);

                if (tripdata == null)
                {
                    return NoContent();
                }

                if (tripdata.DriverId == driverId)
                {
                        tripdata.Istakenpics = istakenpics;
                        tripdata.IsEndPicsTaken = isendimagestaken;
                    if (tripdata.ImageUrlsList == null)
                    {

                        tripdata.ImageUrlsList = imageurls;
                    }
                    else
                    {
                        tripdata.ImageUrlsList = tripdata.ImageUrlsList + ',' + imageurls;
                    }
                        _context.Entry(tripdata).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    return Ok(tripdata);

                }

                else
                {
                    return BadRequest("Driver id did not match");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Flag")]
        public async Task<ActionResult<Trip>> Postimagesflag(int triptypeid,int tripId , int driverId,string imageurls,int flag,Boolean istakenpics, Boolean isendimagestaken)
        {
            try
            {
                if (flag == 2)
                {
                    var tripdata = _context.MonthlyDateLists.Where(c=>c.MonthlyDateListId== triptypeid && c.MonthlyId == tripId).FirstOrDefault();

                    var monthlydata = _context.Monthlies.Find(tripId);
                    if (tripdata == null)
                    {
                        return NoContent();
                    }
                    if(monthlydata == null)
                    {
                        return NoContent();
                    }

                    if (monthlydata.DriverId == driverId)
                    {
                        if (tripdata.ImageUrlsList == null)
                        {

                            tripdata.ImageUrlsList = imageurls;
                            tripdata.Istakenpics = istakenpics;
                        }
                        else
                        {
                            tripdata.ImageUrlsList = tripdata.ImageUrlsList + ',' + imageurls;
                            tripdata.IsEndPicsTaken = isendimagestaken;
                        }

                        _context.Entry(tripdata).State = EntityState.Modified;

                        await _context.SaveChangesAsync();


                    }
                        return Ok(tripdata);
                }
                else if (flag == 1)
                {
                    var tripdata = _context.FlexiDatesLists.Where(c => c.FlexiDatesListId== triptypeid && c.FlexiId== tripId).FirstOrDefault();

                    var monthlydata = _context.Flexis.Find(tripId);
                    if (tripdata == null)
                    {
                        return NoContent();
                    }
                    if (monthlydata == null)
                    {
                        return NoContent();
                    }

                    if (monthlydata.DriverId == driverId)
                    {
                        if (tripdata.ImageUrlsList == null)
                        {

                            tripdata.ImageUrlsList = imageurls;
                            tripdata.Istakenpics = istakenpics;
                        }
                        else
                        {
                            tripdata.ImageUrlsList = tripdata.ImageUrlsList + ',' + imageurls;
                            tripdata.IsEndPicsTaken = isendimagestaken;
                        }

                        _context.Entry(tripdata).State = EntityState.Modified;

                        await _context.SaveChangesAsync();


                    }
                        return Ok(tripdata);
                }
                else
                {
                    return BadRequest("Driver id did not match");
                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
