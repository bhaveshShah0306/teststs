using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Driverupdatelocationatactive : ControllerBase
    {
        private readonly DataContext _context;

        public Driverupdatelocationatactive(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Driver>> updateloacationatactive(int id, Boolean isactive, string latitude, string longitude)
        {
            try
            {
                var driverdata = await _context.Drivers.FindAsync(id);
                if (driverdata != null)
                {
                    
                    driverdata.IsDriverActive = isactive;
                    if (latitude=="undefined" || longitude == "undefined")
                    {

                        driverdata.Latitude = driverdata.Latitude;
                        driverdata.Longitude = driverdata.Longitude                                                                                                             ;
                    }
                    else
                    {

                        driverdata.Latitude = latitude;
                        driverdata.Longitude = longitude;
                    }
                    _context.Entry(driverdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(driverdata);
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

          [HttpPost("BLOCKED/driverid")]
        public async Task<ActionResult<Driver>> updateloacationatblocked(int id, Boolean IsBlock, string latitude, string longitude)
        {
            try
            {
                var driverdata = await _context.Drivers.FindAsync(id);
                if (driverdata != null)
                {                                                                                                                               
                    driverdata.IsBlock = IsBlock;
                    if (IsBlock==true) 
                    {
                        driverdata.IsDriverActive = false;
                    }
                    else
                    {
                        driverdata.IsDriverActive = true;

                    }
                    if (latitude=="undefined" || longitude == "undefined")
                    {

                        driverdata.Latitude = driverdata.Latitude;
                        driverdata.Longitude = driverdata.Longitude                                                                                                             ;
                    }
                    else
                    {

                        driverdata.Latitude = latitude;
                        driverdata.Longitude = longitude;
                    }
                    _context.Entry(driverdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(driverdata);
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




        [HttpPost("{id}")]
        public async Task<ActionResult<Driver>> updatediverstatusactive(int id, Boolean isactive)
        {
            try
            {
                var driverdata = await _context.Drivers.FindAsync(id);
                if (driverdata != null)
                {
                    driverdata.IsDriverActive = isactive;
                    _context.Entry(driverdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(driverdata);
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
    }
}
