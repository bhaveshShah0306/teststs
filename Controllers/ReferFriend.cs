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
    public class ReferFriend : ControllerBase
    {
        private readonly DataContext _context;

        public ReferFriend(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Refer>> Refer(int driverid, string ContactNumber)
        {
            try
            {
                var driverdata = await _context.Drivers.Where(c => c.PhoneNumber == ContactNumber).FirstOrDefaultAsync();
                if (driverdata == null)
                {
                    var referdriverdata = await _context.Drivers.FindAsync(driverid);
                    if (referdriverdata != null)
                    {
                        string randomWord = GenerateRandomWord();
                        string dummyNumbers = GenerateDummyNumbers();
                        if (referdriverdata.ReferCode == null)
                        {
                            referdriverdata.ReferCode = $"{randomWord}{dummyNumbers}_{referdriverdata.DriverId}";
                            _context.Entry(referdriverdata).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                        var driver = new Driver();
                        driver.PhoneNumber = ContactNumber;
                        
                        _context.Drivers.Add(driver);
                        await _context.SaveChangesAsync();
                        var referfreinds = new Refer();
                        referfreinds.Isused = false;
                        referfreinds.ReferedBy = driverid;
                        referfreinds.ReferredTo = driver.DriverId;
                       
                        _context.Referals.Add(referfreinds);
                        await _context.SaveChangesAsync();
                        referfreinds.ReferalCode = referdriverdata.ReferCode;
                        _context.Entry(referfreinds).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        driver.ReferCode = $"{randomWord}{dummyNumbers}_{referdriverdata.DriverId}";
                        _context.Entry(driver).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return Ok(referfreinds);
                    }
                    else { return BadRequest("Driver Not found"); }

                }
                else
                {
                    if (driverdata.ReferedBy==null || driverdata.ReferedBy == 0)
                    {
                        var referdriverdata = await _context.Drivers.FindAsync(driverid);
                        if (referdriverdata != null)
                        {
                            string randomWord = GenerateRandomWord();

                            // Generate three dummy random numbers
                            string dummyNumbers = GenerateDummyNumbers();
                            if (referdriverdata.ReferCode == null)
                            {
                                referdriverdata.ReferCode = $"{randomWord}{dummyNumbers}_{referdriverdata.DriverId}";
                                _context.Entry(referdriverdata).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                            var referfreinds = new Refer();
                            referfreinds.Isused = false;
                            referfreinds.ReferedBy = driverid;
                            referfreinds.ReferredTo = driverdata.DriverId;
                            _context.Referals.Add(referfreinds);
                            await _context.SaveChangesAsync();
                            referfreinds.ReferalCode = referdriverdata.ReferCode;
                            _context.Entry(referfreinds).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            return Ok(referfreinds);
                        }
                        else
                        {
                            return BadRequest("Driver Not Found");
                        }
                    }
                    else
                    {
                        return BadRequest("already Refered");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string GenerateRandomWord()
        {
            // Add your logic to generate a random word here
            // For simplicity, using a predefined list of words
            List<string> wordList = new List<string> { "Limo", "Chauffeur", "Sedan", "Luxury", "Service", "Travel", "Comfort", "Elegance", "Ride" };
            Random random = new Random();
            int randomIndex = random.Next(wordList.Count);

            return wordList[randomIndex];
        }

        private string GenerateDummyNumbers()
        {
            // Generate three dummy random numbers
            Random random = new Random();
            int randomNumber = random.Next(100, 1000); // Generates a random number between 100 and 999

            // Concatenate the dummy numbers
            return randomNumber.ToString();
        }

        [HttpPost("{driverid}/{refercode}")]
        public async Task<ActionResult<dynamic>> Earnings(int driverid, string? refercode)
        {
            try
            {
                var driverdata = await _context.Drivers.FindAsync(driverid);
                var driverwallet = await _context.Driverwallets.Where(c => c.DriverId == driverid).FirstOrDefaultAsync();
                var referreddata = await _context.Drivers.Where(c => c.ReferCode == refercode).FirstOrDefaultAsync();

                if (driverdata != null && driverdata.ReferCode != refercode)
                {
                    if (refercode == "DUMMYCODE")
                    {
                        driverdata.ReferedBy = 0;
                        _context.Entry(driverdata).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return NoContent();
                    }
                    if (driverdata.ReferedBy == null || driverdata.ReferedBy == 0)
                    {
                        if (referreddata != null)
                        {
                            var referearningdata = await _context.ReferEarnings.FirstOrDefaultAsync();
                            if (referearningdata != null)
                            {
                                if (driverwallet != null)
                                {
                                    driverwallet.WalletBalance = (Convert.ToDecimal(driverwallet.WalletBalance) + referearningdata.ReferredEarning).ToString();
                                    _context.Entry(driverwallet).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    var dwallet = new Driverwallet();

                                    dwallet.DriverId = driverid;
                                    dwallet.WalletBalance = (referearningdata.ReferredEarning).ToString();
                                    _context.Driverwallets.Add(dwallet);
                                    await _context.SaveChangesAsync();
                                }

                                var referedwallet = await _context.Driverwallets.Where(c => c.DriverId == referreddata.DriverId).FirstOrDefaultAsync();
                                if (referedwallet != null)
                                {
                                    referedwallet.WalletBalance = (Convert.ToDecimal(referedwallet.WalletBalance) + referearningdata.ReferredByEarning).ToString();
                                    _context.Entry(referedwallet).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    var rwallet = new Driverwallet();

                                    rwallet.DriverId = referreddata.DriverId;
                                    rwallet.WalletBalance = (referearningdata.ReferredEarning).ToString();
                                    _context.Driverwallets.Add(rwallet);
                                    await _context.SaveChangesAsync();
                                }

                                driverdata.ReferedBy = referreddata.DriverId;
                                _context.Entry(driverdata).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                        }
                        return Ok(driverwallet);
                    }
                    else
                    {
                        return BadRequest("Refer Earning is already done");
                    }
                }
                else
                {
                    return NotFound("Driver Not Found or refercode is of same driver");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
