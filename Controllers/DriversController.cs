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
using Microsoft.IdentityModel.Tokens;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly DataContext _context;

        public DriversController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Drivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverVM>>> GetDrivers()
        {
            try
            {
                var driverlist = _context.Drivers.ToList();
                var driverVMList = new List<DriverVM>();
                if(driverlist.Count> 0)
                {
                    foreach(var driver in driverlist)
                    {
                        var drivervm = new DriverVM();
                        drivervm.DriverId = driver.DriverId;
                        drivervm.DriverRecID = driver.DriverRecID;
                        drivervm.DriverName = driver.DriverName;
                        drivervm.DOB = driver.DOB;
                        drivervm.PhoneNumber = driver.PhoneNumber;
                        drivervm.Email = driver.Email;
                        drivervm.Image = driver.Image;
                        drivervm.Gender = driver.Gender;
                        drivervm.PermenentAddress = driver.PermenentAddress;
                        drivervm.PermenentCountry = driver.PermenentCountry;
                        drivervm.PermenentPostalCode = driver.PermenentPostalCode;
                        drivervm.PermenentCity = driver.PermenentCity;
                        drivervm.City = driver.City;
                        drivervm.PostalCode = driver.PostalCode;
                        drivervm.PermenentState = driver.PermenentState;
                  
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
                        drivervm.VehicleTypeIds = driver.VehicleTypeIds ;
                        drivervm.BankName = driver.BankName;

                        drivervm.AccountNo = driver.AccountNo;
                        drivervm.AccountHolderName = driver.AccountHolderName;
                        drivervm.Branch = driver.Branch;
                        drivervm.IFSCCODE = driver.IFSCCODE;
                        drivervm.VerifiedDate = driver.VerifiedDate;
                        drivervm.VechileTypeList = new List<VehicletypeVM>();                                                
                        if (!string.IsNullOrEmpty(driver.VehicleTypeIds))
                        {
                            var vehiclestypeids = drivervm.VehicleTypeIds.Split(',').ToList() ;
                            if (vehiclestypeids.Count > 0)
                            {
                                foreach(var vehiclestypeid in vehiclestypeids)
                                {
                                    var vehicletypevm = new VehicletypeVM();
                                    var vtdata= _context.VehicleTypes.Find(Convert.ToInt32(vehiclestypeid));
                                    if(vtdata != null )
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
                        drivervm.verificationStatuses =new List<VerificationStatusVMS>();
                        if(!string.IsNullOrEmpty(drivervm.VerificationStatusIds))
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
                        drivervm.IsBlock = driver.IsBlock;

                        driverVMList.Add(drivervm);
                    }
                    return Ok(driverVMList);

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

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverVM>> GetDriver(int id)
        {
            try
            {
                var driver = _context.Drivers.Find(id);
                if (driver != null)
                {
                    var drivervm = new DriverVM();
                    drivervm.DriverId = driver.DriverId;
                    drivervm.DriverRecID = driver.DriverRecID;
                    drivervm.DriverName = driver.DriverName;
                    drivervm.DOB = driver.DOB;
                    drivervm.PhoneNumber = driver.PhoneNumber;
                    drivervm.AltPhoneNumber = driver.AltPhoneNumber;
                    drivervm.Address = driver.Address;
                    drivervm.Image = driver.Image;
                    drivervm.AadharNumber = driver.AadharNumber;
                    drivervm.Email = driver.Email;
                    drivervm.Gender = driver.Gender;
                    drivervm.PermenentAddress = driver.PermenentAddress;
                    drivervm.PermenentCountry = driver.PermenentCountry;
                    drivervm.PermenentPostalCode = driver.PermenentPostalCode;
                    drivervm.PermenentCity = driver.PermenentCity;
                    drivervm.City = driver.City;
                    drivervm.PostalCode = driver.PostalCode;
                    drivervm.PermenentState = driver.PermenentState;
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
                    drivervm.BankName = driver.BankName;
                    drivervm.IsDriverActive = driver.IsDriverActive;
                    drivervm.AccountNo = driver.AccountNo;
                    drivervm.AccountHolderName = driver.AccountHolderName;
                    drivervm.Branch = driver.Branch;
                    drivervm.IFSCCODE = driver.IFSCCODE;
                    drivervm.Latitude = driver.Latitude;
                    drivervm.Longitude = driver.Longitude;
                    drivervm.Licencevalidddate = driver.Licencevalidddate;
                    drivervm.BloodGroup = driver.BloodGroup;
                    drivervm.VerifiedDate = driver.VerifiedDate;
                    drivervm.ReferCode = driver.ReferCode;
                    drivervm.VehicleTypeIds = driver.VehicleTypeIds;
                    drivervm.VechileTypeList = new List<VehicletypeVM>();

                    if (!string.IsNullOrEmpty(driver.VehicleTypeIds))
                    {
                        var vehiclestypeids = drivervm.VehicleTypeIds.Split(',').ToList();
                        if (vehiclestypeids.Count > 0)
                        {
                            foreach (var vehiclestypeid in vehiclestypeids)
                            {
                                if (int.TryParse(vehiclestypeid, out int vehicletypeidInt))
                                {
                                    var vehicletypevm = new VehicletypeVM();
                                    var vtdata = _context.VehicleTypes.Find(vehicletypeidInt);
                                    if (vtdata != null)
                                    {
                                        vehicletypevm.VehicleTypeName = vtdata.VehicleTypeName;
                                        drivervm.VechileTypeList.Add(vehicletypevm);
                                    }
                                }
                                else
                                {
                                    // Handle the case where the conversion fails
                                    // You might want to log this information or handle it accordingly
                                }
                            }
                        }
                    }

                    drivervm.TransmissionTypeId = driver.TransmissionTypeId;

                    if (!string.IsNullOrEmpty(driver.TransmissionTypeId))
                    {
                        var transmissionTypeIds = driver.TransmissionTypeId.Split(',').ToList();
                        var transmissionTypeList = new List<TransmissionTypeVM>();

                        if (transmissionTypeIds.Count > 0)
                        {
                            foreach (var transmissionTypeId in transmissionTypeIds)
                            {
                                if (int.TryParse(transmissionTypeId, out int transmissionTypeIdInt))
                                {
                                    var ttvm = new TransmissionTypeVM();
                                    var transmissionData = _context.TransmissionTypes.Find(transmissionTypeIdInt);

                                    if (transmissionData != null)
                                    {
                                        ttvm.TransmissionTypeId = transmissionData.TransmissionTypeId;
                                        ttvm.TransmissionTypeName = transmissionData.TransmissionName;
                                        transmissionTypeList.Add(ttvm);
                                    }
                                }
                                else
                                {
                                    // Handle the case where the conversion fails
                                    // You might want to log this information or handle it accordingly
                                }
                            }
                        }

                        drivervm.TransmissionType = transmissionTypeList;
                    }

                    drivervm.TransmissionTypeId = driver.TransmissionTypeId;
                    drivervm.Experiance = driver.Experiance;
                    drivervm.RatingId = driver.RatingId;
                    drivervm.CurrentLocation = driver.CurrentLocation;
                    drivervm.IsBlock = driver.IsBlock;
                    drivervm.VerificationStatusIds = driver.VerificationStatusIds;
                    drivervm.verificationStatuses = new List<VerificationStatusVMS>();

                    if (!string.IsNullOrEmpty(drivervm.VerificationStatusIds))
                    {
                        var verificationStatuses = drivervm.VerificationStatusIds.Split(',').ToList();
                        if (verificationStatuses.Count > 0)
                        {
                            var verificationLastStatus = verificationStatuses.Last();
                            if (int.TryParse(verificationLastStatus, out int statusId))
                            {
                                drivervm.StatusId = statusId;
                            }
                            else
                            {
                                // Handle the case where the conversion fails
                                // You might want to log this information or handle it accordingly
                            }

                            foreach (var verf in verificationStatuses)
                            {
                                if (int.TryParse(verf, out int verfId))
                                {
                                    var verfvm = new VerificationStatusVMS();
                                    var verificationStatusData = _context.VerificationStatuses.Find(verfId);

                                    if (verificationStatusData != null)
                                    {
                                        verfvm.VerificationStatusId = verificationStatusData.VerificationStatusId;
                                        verfvm.VerificationStatusName = verificationStatusData.VerificationStatusName;
                                        drivervm.verificationStatuses.Add(verfvm);
                                    }
                                }
                                else
                                {
                                    // Handle the case where the conversion fails
                                    // You might want to log this information or handle it accordingly
                                }
                            }
                        }
                    }

                    drivervm.VerifiedDate = driver.VerifiedDate;
                    drivervm.ReferedBy = driver.ReferedBy;
                    drivervm.ApprovedBy = driver.ApprovedBy;
                    drivervm.IsPaymentDone = driver.IsPaymentDone;
                    return Ok(drivervm);
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

        // PUT: api/Drivers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutDriver(int id, Driver driver)
        {
            if (id != driver.DriverId)
            {
                return BadRequest();
            }

            _context.Entry(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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

        // POST: api/Drivers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
          if (_context.Drivers == null)
          {
              return Problem("Entity set 'DataContext.Drivers'  is null.");
          }
     
            _context.Drivers.Add(driver);
            //await _context.SaveChangesAsync();
            //string uniqueId = "GoC" + DateTime.UtcNow.ToString("yyyyMMdd") +driver.DriverId;
            //driver.DriverRecID = uniqueId;
            //_context.Entry(driver).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDriver", new { id = driver.DriverId }, driver);
        }

        // DELETE: api/Drivers/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            if (_context.Drivers == null)
            {
                return NotFound();
            }
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverExists(int id)
        {
            return (_context.Drivers?.Any(e => e.DriverId == id)).GetValueOrDefault();
        }
    }
}
