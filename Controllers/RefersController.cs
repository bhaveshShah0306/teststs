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
    public class RefersController : ControllerBase
    {
        private readonly DataContext _context;

        public RefersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Refers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Refer>>> GetReferals()
        {
            if (_context.Referals == null)
            {
                return NotFound();
            }
            return await _context.Referals.ToListAsync();
        }

        [HttpGet("{driverid}/flag")]
        public async Task<ActionResult<IEnumerable<DriverVM>>> GetReferalbydriverid(int driverid)
        {
            try
            {
                var referdata = _context.Referals.Where(c => c.ReferedBy == driverid).ToList();
                var driverVMList = new List<DriverVM>();
                if (referdata.Count > 0)
                {
                    var referearndingdata = await _context.ReferEarnings.FirstOrDefaultAsync();
                    int? total = 0;
                    if (referearndingdata != null)
                    {

                        foreach (var refer in referdata)
                        {
                            var driver = _context.Drivers.Find(refer.ReferredTo);
                            if (driver != null)
                            {
                                var drivervm = new DriverVM();
                                drivervm.DriverId = driver.DriverId;
                                drivervm.DriverRecID = driver.DriverRecID;
                                drivervm.DriverName = driver.DriverName;
                                drivervm.DOB = driver.DOB;
                                drivervm.PhoneNumber = driver.PhoneNumber;
                                drivervm.AltPhoneNumber = driver.AltPhoneNumber;
                                drivervm.AltPhoneNumber = driver.AltPhoneNumber;
                                drivervm.Address = driver.Address;
                                drivervm.AadharNumber = driver.AadharNumber;
                                drivervm.AadharNumberFrontImage = driver.AadharNumberFrontImage;
                                drivervm.AadharNumberBackImage = driver.AadharNumberBackImage;
                                drivervm.PANNumber = driver.PANNumber;
                                drivervm.PANNumberFrontImage = driver.PANNumberFrontImage;
                                drivervm.PANNumberBackImage = driver.PANNumberBackImage;
                                drivervm.Licence = driver.Licence;
                                drivervm.LicenceFrontImage = driver.LicenceFrontImage;
                                drivervm.LicenceBackImage = driver.LicenceBackImage;
                                drivervm.Image = driver.Image;
                                drivervm.Qualification = driver.Qualification;
                                drivervm.Status = driver.Status;
                                drivervm.IsVerified = driver.IsVerified;
                                drivervm.IsDriverActive = driver.IsDriverActive;
                                drivervm.Licencevalidddate = driver.Licencevalidddate;
                                drivervm.BloodGroup = driver.BloodGroup;
                                drivervm.VehicleTypeIds = driver.VehicleTypeIds;
                                drivervm.BankName = driver.BankName;

                                drivervm.AccountNo = driver.AccountNo;
                                drivervm.AccountHolderName = driver.AccountHolderName;
                                drivervm.Branch = driver.Branch;
                                drivervm.IFSCCODE = driver.IFSCCODE;
                                drivervm.VerifiedDate = driver.VerifiedDate;
                                drivervm.VechileTypeList = new List<VehicletypeVM>();
                                if (!string.IsNullOrEmpty(driver.VehicleTypeIds))
                                {
                                    var vehiclestypeids = drivervm.VehicleTypeIds.Split(',').ToList();
                                    if (vehiclestypeids.Count > 0)
                                    {
                                        foreach (var vehiclestypeid in vehiclestypeids)
                                        {
                                            var vehicletypevm = new VehicletypeVM();
                                            var vtdata = _context.VehicleTypes.Find(Convert.ToInt32(vehiclestypeid));
                                            if (vtdata != null)
                                            {
                                                vehicletypevm.VehicleTypeName = vtdata.VehicleTypeName;
                                                drivervm.VechileTypeList.Add(vehicletypevm);
                                            }

                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(driver.TransmissionTypeId))
                                {
                                    var TransmissionTypeId = driver.TransmissionTypeId.Split(",").ToList();
                                    var transmissiontypelist = new List<TransmissionTypeVM>();
                                    if (TransmissionTypeId.Count > 0)
                                    {
                                        foreach (var transmissionTypeId in TransmissionTypeId)
                                        {
                                            var ttvm = new TransmissionTypeVM();
                                            var transmissiondata = _context.TransmissionTypes.Find(Convert.ToInt32(transmissionTypeId));
                                            if (transmissiondata != null)
                                            {
                                                ttvm.TransmissionTypeId = transmissiondata.TransmissionTypeId;
                                                ttvm.TransmissionTypeName = transmissiondata.TransmissionName;
                                            }
                                            transmissiontypelist.Add(ttvm);
                                        }
                                    }
                                    drivervm.TransmissionType = transmissiontypelist;
                                }
                                drivervm.TransmissionTypeId = driver.TransmissionTypeId;
                                drivervm.Experiance = driver.Experiance;
                                drivervm.RatingId = driver.RatingId;
                                drivervm.CurrentLocation = driver.CurrentLocation;
                                drivervm.VerificationStatusIds = driver.VerificationStatusIds;
                                drivervm.verificationStatuses = new List<VerificationStatusVMS>();
                                if (!string.IsNullOrEmpty(drivervm.VerificationStatusIds))
                                {
                                    var verficationstatuses = drivervm.VerificationStatusIds.Split(",").ToList();
                                    if (verficationstatuses.Count > 0)
                                    {
                                        var verificationlaststatus = verficationstatuses.Last();
                                        drivervm.StatusId = Convert.ToInt32(verificationlaststatus);
                                        foreach (var verf in verficationstatuses)
                                        {
                                            var verfvm = new VerificationStatusVMS();
                                            var verificationstatusdata = _context.VerificationStatuses.Find(Convert.ToInt32(verf));
                                            if (verificationstatusdata != null)
                                            {
                                                verfvm.VerificationStatusId = verificationstatusdata.VerificationStatusId;
                                                verfvm.VerificationStatusName = verificationstatusdata.VerificationStatusName;
                                                drivervm.verificationStatuses.Add(verfvm);
                                            }
                                        }
                                    }
                                }
                                drivervm.VerifiedDate = driver.VerifiedDate;
                                drivervm.ReferedBy = driver.ReferedBy;
                                drivervm.ApprovedBy = driver.ApprovedBy;
                                drivervm.IsPaymentDone = driver.IsPaymentDone;

                                driverVMList.Add(drivervm);
                            }
                        }


                        total = referearndingdata.ReferredByEarning * referdata.Count;
                    }
                    return Ok(new { driverVMList = driverVMList, totalearning = total });
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

        // GET: api/Refers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Refer>> GetRefer(int id)
        {
            if (_context.Referals == null)
            {
                return NotFound();
            }
            var refer = await _context.Referals.FindAsync(id);

            if (refer == null)
            {
                return NotFound();
            }

            return refer;
        }

        // PUT: api/Refers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRefer(int id, Refer refer)
        {
            if (id != refer.ReferId)
            {
                return BadRequest();
            }

            _context.Entry(refer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReferExists(id))
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

        // POST: api/Refers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Refer>> PostRefer(Refer refer)
        {
            if (_context.Referals == null)
            {
                return Problem("Entity set 'DataContext.Referals'  is null.");
            }
            refer.Isused = false;
            string randomWord = GenerateRandomWord();

            // Generate three dummy random numbers
            string dummyNumbers = GenerateDummyNumbers();

            // Concatenate the elements to form the referral code

            _context.Referals.Add(refer);
            await _context.SaveChangesAsync();
            refer.ReferalCode = $"{randomWord}{dummyNumbers}{refer.ReferedBy}{refer.ReferId}";

            return CreatedAtAction("GetRefer", new { id = refer.ReferId }, refer);
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
        // DELETE: api/Refers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRefer(int id)
        {
            if (_context.Referals == null)
            {
                return NotFound();
            }
            var refer = await _context.Referals.FindAsync(id);
            if (refer == null)
            {
                return NotFound();
            }

            _context.Referals.Remove(refer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReferExists(int id)
        {
            return (_context.Referals?.Any(e => e.ReferId == id)).GetValueOrDefault();
        }
    }
}
