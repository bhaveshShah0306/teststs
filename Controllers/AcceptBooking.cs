using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Nest;
using static System.Net.WebRequestMethods;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcceptBooking : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger _log;

        public AcceptBooking(DataContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _log = loggerFactory.CreateLogger("AcceptBookingLogger"); // Custom logger for API

        }

        [HttpPost]
        public async Task<ActionResult<Trip>> AcceptTrip(int id, int DriverId, int triptypeId, int flag)
        {
            try
            {
                var tripdata = await _context.Trips.FindAsync(id);
                if (tripdata != null)
                {
                        tripdata.DriverId = DriverId;
                        if (flag == 1)
                        {
                            tripdata.IsReserved = true;

                            var userdata = await _context.Users.FindAsync(tripdata.UserId);
                            if (userdata != null)
                            {
                                string text = $"Dear Patron, Your booking is confirmed! Driver details will be shared 30 mins prior to scheduled time. Booking ID: {tripdata.TripId}. Track your booking on GoChauffeurs app!";
                                string encodedText = System.Web.HttpUtility.UrlEncode(text);  // URL-encode the message

                                var url = $"http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475632442759&mobile={userdata.PhoneNumber}&message={encodedText}";

                                using (HttpClient client = new HttpClient())
                                {
                                    HttpResponseMessage response = await client.GetAsync(url);
                                    _log.LogInformation($"SMS response for user for normal trips {DateTime.Now}: {response.StatusCode}");
                                }
                            }
                        }
                        else if (flag == 2 && tripdata.IsReserved == true)
                        {
                              
                             tripdata.IsAccepted = true;

                            var driverdata = await _context.Drivers.FindAsync(DriverId);
                            var userdata = await _context.Users.FindAsync(tripdata.UserId);
                            if (driverdata != null && userdata != null)
                            {
                                string text = $"Dear Patron, Mr. {driverdata.DriverName} is on his way to drive your vehicle today. You can reach him on {driverdata.PhoneNumber}. You can track his arrival status via the Go Chauffeurs app.";
                                string encodedText = System.Web.HttpUtility.UrlEncode(text);  // URL-encode the message

                                var url = $"http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475712594902&mobile={userdata.PhoneNumber}&message={encodedText}";

                                using (HttpClient client = new HttpClient())
                                {
                                    HttpResponseMessage response = await client.GetAsync(url);
                                    _log.LogInformation($"SMS response for user for normal trips {DateTime.Now}: {response.StatusCode}");
                                }
                            }
                        }
                        else
                        {
                            return BadRequest("Trip has to be reserved first");
                        }

                        _context.Entry(tripdata).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        return Ok(tripdata);
                    
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error in AcceptTrip");
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Immdiate/{id}")]
        public async Task<ActionResult<Trip>> ImmediaetAcceptTrip(int id, int DriverId, int triptypeId)
        {
            try
            {
                var tripdata = await _context.Trips.FindAsync(id);
                if (tripdata != null)
                {
                    if (tripdata.DriverId != null)
                    {
                        return BadRequest("driver already designed");

                    }
                    tripdata.DriverId = DriverId;
                    tripdata.IsAccepted = true;

                    var driverdata = await _context.Drivers.FindAsync(DriverId);
                    var userdata = await _context.Users.FindAsync(tripdata.UserId);

                    if (driverdata != null && userdata != null)
                    {
                        string text = $"Dear Patron, Mr. {driverdata.DriverName} is on his way to drive your vehicle today. You can reach him on {driverdata.PhoneNumber}. You can track his arrival status via the Go Chauffeurs app.";
                        string encodedText = System.Web.HttpUtility.UrlEncode(text);  // URL-encode the message

                        var url = $"http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475712594902&mobile={userdata.PhoneNumber}&message={encodedText}";

                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.GetAsync(url);
                            _log.LogInformation($"SMS response for user for immediate trips {DateTime.Now}: {response.StatusCode}");
                        }
                    }

                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return Ok(tripdata);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error in ImmediaetAcceptTrip");
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("cancelled")]
        public async Task<ActionResult<Trip>> CancelTrip(int id, int userid,int triptypeId,int flag,int reasonid,Boolean isignore)
        {
            try
            {
                
                var tripdata = _context.Trips.Find(id);
                if (tripdata!=null)
                {
                   
                    var driverfines = _context.DriverFines.FirstOrDefault();
                    if (driverfines != null)
                    {

                        var walletdata = _context.Driverwallets.Where(c => c.DriverId == flag).FirstOrDefault();
                        if (walletdata == null)
                        {
                            var driverwallet = new Driverwallet();
                            driverwallet.DriverId = tripdata.DriverId;
                            driverwallet.WalletBalance = (0 - driverfines.Amount).ToString();
                            _context.Driverwallets.Add(driverwallet);
                            await _context.SaveChangesAsync();
                            var driverTransaction = new DriverTransaction()
                            {
                                DriverId = flag,
                                Amount = -driverfines.Amount, // with gst amount
                                CreatedDate = DateTime.Now,

                            };
                            _context.DriverTransactions.Add(driverTransaction);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {

                            walletdata.WalletBalance = (Convert.ToDouble(walletdata.WalletBalance) - driverfines.Amount).ToString();
                            _context.Entry(walletdata).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            
                           var driverTransaction = new DriverTransaction()
                            {
                                DriverId = flag,
                                Amount = -driverfines.Amount, // with gst amount
                                CreatedDate = DateTime.Now,
                              
                            };
                            _context.DriverTransactions.Add(driverTransaction);
                            await _context.SaveChangesAsync();
                        }

                    }

                    var ignoretrips = new IgnoredTrips();
                    ignoretrips.FlexiId = tripdata.TripId;
                    ignoretrips.DriverId = flag;
                    ignoretrips.ReasonId= reasonid;
                    _context.IgnoredTrips.Add(ignoretrips);
                    await _context.SaveChangesAsync();
                    if (isignore != true)
                    {

                        tripdata.IsCancelled = true;
                    }
                    else
                    {

                        tripdata.DriverId = null;
                        tripdata.IsAccepted = false;
                        tripdata.IsReserved= false;
                    }
                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();


                    return Ok(tripdata);
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

        [HttpPost("cancelledmontly")]
        public async Task<ActionResult<Trip>> CancelmonthlyTrip(int id, int userid,int triptypeId,int flag,int reasonid)
        {
            try
            {
                
                var tripdata = _context.Monthlies.Find(id);
                if (tripdata!=null)
                {

                    var driverfines = _context.DriverFines.FirstOrDefault();
                    if (driverfines != null)
                    {

                        var walletdata = _context.Driverwallets.Where(c => c.DriverId == flag).FirstOrDefault();
                        if (walletdata == null)
                        {
                            var driverwallet = new Driverwallet();
                            driverwallet.DriverId = tripdata.DriverId;
                            driverwallet.WalletBalance = (0 - driverfines.Amount).ToString();
                            _context.Driverwallets.Add(driverwallet);
                            await _context.SaveChangesAsync();
                            var driverTransaction = new DriverTransaction()
                            {
                                DriverId = flag,
                                Amount = -driverfines.Amount, // with gst amount
                                CreatedDate = DateTime.Now,

                            };
                            _context.DriverTransactions.Add(driverTransaction);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {

                            walletdata.WalletBalance = (Convert.ToDouble(walletdata.WalletBalance) - driverfines.Amount).ToString();
                            _context.Entry(walletdata).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            var driverTransaction = new DriverTransaction()
                            {
                                DriverId = flag,
                                Amount = -driverfines.Amount, // with gst amount
                                CreatedDate = DateTime.Now,

                            };
                            _context.DriverTransactions.Add(driverTransaction);
                            await _context.SaveChangesAsync();
                        }

                    }

                    var IgnoreMTrips = new IgnoreMontlyTrips();
                    IgnoreMTrips.MonthlyId = tripdata.MonthlyId;
                    IgnoreMTrips.DriverId = flag;
                    IgnoreMTrips.IgnoretripresonsId = reasonid;
                    _context.IgnoreMontlyTrips.Add(IgnoreMTrips);
                    await _context.SaveChangesAsync();
                    tripdata.IsCancelled = true;
                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    var userdata= _context.Users.Find(userid);

                    if (userdata!=null)
                    {
                        string text = "Dear Patron, Your Booking ID:"+tripdata.MonthlyId +" has been cancelled. Contact us via app Chat/Call for any further assistance. Team Gochauffeurs here to assist you!";
                        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                        var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                        var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475698792818&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                        HttpClient client = new HttpClient();

                        client.BaseAddress = new Uri(url);
                        HttpResponseMessage response = client.GetAsync(url).Result;
                    }


                    return Ok(tripdata);
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
        
        [HttpPost("cancelledflexi")]
        public async Task<ActionResult<Trip>> CancelflexiTrip(int id, int userid,int triptypeId,int flag, int reasonid)
        {
            try
            {
                
                var tripdata = _context.Flexis.Find(id);
                if (tripdata!=null)
                {

                    var driverfines = _context.DriverFines.FirstOrDefault();
                    if (driverfines != null)
                    {

                        var walletdata = _context.Driverwallets.Where(c => c.DriverId == flag).FirstOrDefault();
                        if (walletdata == null)
                        {
                            var driverwallet = new Driverwallet();
                            driverwallet.DriverId = tripdata.DriverId;
                            driverwallet.WalletBalance = (0 - driverfines.Amount).ToString();
                            _context.Driverwallets.Add(driverwallet);
                            await _context.SaveChangesAsync();
                            var driverTransaction = new DriverTransaction()
                            {
                                DriverId = flag,
                                Amount = -driverfines.Amount, // with gst amount
                                CreatedDate = DateTime.Now,

                            };
                            _context.DriverTransactions.Add(driverTransaction);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {

                            walletdata.WalletBalance = (Convert.ToDouble(walletdata.WalletBalance) - driverfines.Amount).ToString();
                            _context.Entry(walletdata).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            var driverTransaction = new DriverTransaction()
                            {
                                DriverId = flag,
                                Amount = -driverfines.Amount, // with gst amount
                                CreatedDate = DateTime.Now,

                            };
                            _context.DriverTransactions.Add(driverTransaction);
                            await _context.SaveChangesAsync();
                        }

                    }

                    var IgnoreFlexiTrips = new Models.IgnoreFlexiTrips();
                    IgnoreFlexiTrips.FlexiId = tripdata.FlexiId;
                    IgnoreFlexiTrips.DriverId = flag;
                    IgnoreFlexiTrips.IgnoretripresonsId= reasonid;
                    _context.IgnoreFlexiTrips.Add(IgnoreFlexiTrips);
                    await _context.SaveChangesAsync();
                    tripdata.IsCancelled = true;
                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    var userdata= _context.Users.Find(userid);

                    if (userdata!=null)
                    {
                        string text = "Dear Patron, Your Booking ID:"+tripdata.UniqueflexiId +" has been cancelled. Contact us via app Chat/Call for any further assistance. Team Gochauffeurs here to assist you!";
                        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                        var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                        var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475698792818&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                        HttpClient client = new HttpClient();

                        client.BaseAddress = new Uri(url);
                        HttpResponseMessage response = client.GetAsync(url).Result;
                    }


                    return Ok(tripdata);
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

        [HttpGet("cancelled")]
       public async Task<ActionResult<DriverFines>> CanceloptionsTrip()
        {
            try
            {
                var driverfines = _context.DriverFines.FirstOrDefault();
                if (driverfines!=null)
                {
                    return Ok(driverfines);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("Flexibooking/{id}")]

        public async Task<ActionResult<Flexi>> FlexiAcceptTrip(int id, int DriverId)
        {
            try
            {
                var tripdata = _context.Flexis.Find(id);
                if (tripdata != null)
                {
                 
                    tripdata.DriverId = DriverId;

                    tripdata.IsAccepted = true;
                    tripdata.IsDriverAssigned = true;
                    var driverdata = _context.Drivers.Find(DriverId);
                    var userdata = _context.Users.Find(tripdata.UserId);
                    if (driverdata != null && userdata != null)
                    {

                        string text = "Dear Patron, Mr." + driverdata.DriverName + " is on his way to drive your vehicle today. You can reach him on " + driverdata.PhoneNumber + ".You can track his arrival status via the Go Chauffeurs app.";
                        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                        var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                        var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475712594902&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                        HttpClient client = new HttpClient();

                        client.BaseAddress = new Uri(url);
                        HttpResponseMessage response = client.GetAsync(url).Result;
                    }
                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();



                    return Ok(tripdata);
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

        [HttpPost("Monthliesbooking/{id}")]

        public async Task<ActionResult<Monthly>> MonthlyAcceptTrip(int id, int DriverId)
        {
            try
            {
                var tripdata = _context.Monthlies.Find(id);
                if (tripdata != null)
                {
                    
                    tripdata.DriverId = DriverId;

                    tripdata.IsAccepted = true;
                    tripdata.IsDriverAssigned = true;
                    var driverdata = _context.Drivers.Find(DriverId);
                    var userdata = _context.Users.Find(tripdata.UserId);
                    if (driverdata != null && userdata != null)
                    {

                        string text = "Dear Patron, Mr." + driverdata.DriverName + " is on his way to drive your vehicle today. You can reach him on " + driverdata.PhoneNumber + ".You can track his arrival status via the Go Chauffeurs app.";
                        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                        var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                        var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475712594902&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                        HttpClient client = new HttpClient();

                        client.BaseAddress = new Uri(url);
                        HttpResponseMessage response = client.GetAsync(url).Result;
                    }
                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();



                    return Ok(tripdata);
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




        [HttpPost("valletbooking/{id}")]

        public async Task<ActionResult<ValetParking>> valletAcceptTrip(int id, int DriverId)
        {
            try
            {
                var tripdata = _context.ValetParkings.Find(id);
                if (tripdata != null)
                {
                    tripdata.DriverId = DriverId;

                    tripdata.IsAccepted = true;
                    var driverdata = _context.Drivers.Find(DriverId);
                    var userdata = _context.Users.Find(tripdata.UserID);
                    if (driverdata != null && userdata != null)
                    {

                        string text = "Dear Patron, Mr." + driverdata.DriverName + " is on his way to drive your vehicle today. You can reach him on " + driverdata.PhoneNumber + ".You can track his arrival status via the Go Chauffeurs app.";
                        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                        var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                        var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475712594902&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                        HttpClient client = new HttpClient();

                        client.BaseAddress = new Uri(url);
                        HttpResponseMessage response = client.GetAsync(url).Result;
                    }
                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();



                    return Ok(tripdata);
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











        [HttpPost("reservemontlyTrip")]
        public async Task<ActionResult<Monthly>> reservemontlyTrip(int id, int DriverId, int triptypeId, int flag)
        {
            try
            {

                var tripdata = _context.Monthlies.Find(id);
                if (tripdata != null)
                {
                    tripdata.DriverId = DriverId;
                    if (flag == 1)
                    {
                        tripdata.IsReserved = true;

                        var userdata = _context.Users.Find(tripdata.UserId);
                        if (userdata != null)
                        {

                            string text = "Dear Patron, Your booking is confirmed! Driver details will be shared 30mins prior scheduled time. Booking ID:" + tripdata.MonthlyId + ". Track your booking on GoChauffeurs app!";
                            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                            var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                            var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475632442759&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                            HttpClient client = new HttpClient();

                            client.BaseAddress = new Uri(url);
                            HttpResponseMessage response = client.GetAsync(url).Result;

                        }
                    }
                    else if (flag == 2 && tripdata.IsReserved == true)
                    {
                        tripdata.IsAccepted = true;
                        var driverdata = _context.Drivers.Find(DriverId);
                        var userdata = _context.Users.Find(tripdata.UserId);
                        if (driverdata != null && userdata != null)
                        {

                            string text = "Dear Patron, Mr." + driverdata.DriverName + " is on his way to drive your vehicle today. You can reach him on " + driverdata.PhoneNumber + ".You can track his arrival status via the Go Chauffeurs app.";
                            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                            var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                            var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475712594902&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                            HttpClient client = new HttpClient();

                            client.BaseAddress = new Uri(url);
                            HttpResponseMessage response = client.GetAsync(url).Result;
                        }
                    }
                    else
                    {
                        return BadRequest("trip has to be reserved first");
                    }
                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();



                    return Ok(tripdata);
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



        [HttpPost("reserveflexiTrip")]
        public async Task<ActionResult<Flexi>> reserveflexiTrip(int id, int DriverId, int triptypeId, int flag)
        {
            try
            {

                var tripdata = _context.Flexis.Find(id);
                if (tripdata != null)
                {
                    tripdata.DriverId = DriverId;
                    if (flag == 1)
                    {
                        tripdata.IsReserved = true;

                        var userdata = _context.Users.Find(tripdata.UserId);
                        if (userdata != null)
                        {

                            string text = "Dear Patron, Your booking is confirmed! Driver details will be shared 30mins prior scheduled time. Booking ID:" + tripdata.FlexiId + ". Track your booking on GoChauffeurs app!";
                            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                            var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                            var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475632442759&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                            HttpClient client = new HttpClient();

                            client.BaseAddress = new Uri(url);
                            HttpResponseMessage response = client.GetAsync(url).Result;

                        }
                    }
                    else if (flag == 2 && tripdata.IsReserved == true)
                    {
                        tripdata.IsAccepted = true;
                        var driverdata = _context.Drivers.Find(DriverId);
                        var userdata = _context.Users.Find(tripdata.UserId);
                        if (driverdata != null && userdata != null)
                        {

                            string text = "Dear Patron, Mr." + driverdata.DriverName + " is on his way to drive your vehicle today. You can reach him on " + driverdata.PhoneNumber + ".You can track his arrival status via the Go Chauffeurs app.";
                            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                            var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                            var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475712594902&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                            HttpClient client = new HttpClient();

                            client.BaseAddress = new Uri(url);
                            HttpResponseMessage response = client.GetAsync(url).Result;
                        }
                    }
                    else
                    {
                        return BadRequest("trip has to be reserved first");
                    }
                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();



                    return Ok(tripdata);
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
