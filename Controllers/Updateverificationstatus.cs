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
    public class Updateverificationstatus : ControllerBase
    {
        private readonly DataContext _context;

        public Updateverificationstatus(DataContext context)
        {
            _context = context;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(int id)
        {
            try
            {
                var driverdata = _context.Drivers.Find(id);

                if (driverdata != null)
                {
                    if (!string.IsNullOrEmpty(driverdata.VerificationStatusIds))
                    {
                        var verificationbyiddata = _context.VerificationStatuses.ToList();
                        var verificationDataList = driverdata.VerificationStatusIds.Split(',').Select(int.Parse).ToList();

                        bool allVerificationsPresent = true;
                        int lastAddedVerificationId = 0;

                        foreach (var verificationstatus in verificationbyiddata)
                        {
                            if (!verificationDataList.Contains(verificationstatus.VerificationStatusId))
                            {
                                lastAddedVerificationId = verificationstatus.VerificationStatusId;

                                driverdata.VerificationStatusIds += (string.IsNullOrEmpty(driverdata.VerificationStatusIds) ? "" : ",") + lastAddedVerificationId;
                                driverdata.VerifiedDate = DateTime.Now;
                                _context.Entry(driverdata).State = EntityState.Modified;
                                await _context.SaveChangesAsync();

                                if (lastAddedVerificationId == verificationbyiddata.Last().VerificationStatusId)
                                {
                                    driverdata.IsVerified = true; 
                                    driverdata.VerifiedDate = DateTime.Now; 
                                    _context.Entry(driverdata).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    allVerificationsPresent = false; // Set to false if the last verification is not present
                                }

                                break; // Exit the loop after adding the first verification status
                            }
                        }

                        return Ok(driverdata);




                    }
                    else
                    {
                        var verificationstatus = _context.VerificationStatuses.FirstOrDefault();

                        if (verificationstatus != null)
                        {
                            driverdata.VerificationStatusIds = Convert.ToString(verificationstatus.VerificationStatusId);
                            _context.Entry(driverdata).State = EntityState.Modified;

                            await _context.SaveChangesAsync();
                        }

                        return Ok(driverdata);
                    }

                    return Ok(driverdata); // Return Ok outside the if-else block to cover all cases
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
