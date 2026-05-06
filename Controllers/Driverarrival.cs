using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Driverarrival : ControllerBase
    {
        private readonly DataContext _context;

        public Driverarrival(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Trip>> DriverArrived(int tripid, int driverId, Double latitude, Double longitudes)
        {
            try
            {
                var tripdata = _context.Trips.Find(tripid);
                if (tripdata == null)
                {
                    return NoContent();
                }

                if (tripdata.FromLocation != "" && tripdata.FromLocation != null)
                {
                    var fromloaction = tripdata.FromLocation.Split(',').ToList();
                    if (fromloaction.Count == 2)
                    {

                        var locationlatitude = Convert.ToDouble(fromloaction[0]);
                        var locationlongitude = Convert.ToDouble(fromloaction[1]);
                        var driverdata = _context.Drivers.Find(driverId);
                        if (driverdata != null)
                        {

                            var distance = CalculateDistance(locationlatitude, locationlongitude, Convert.ToDouble(driverdata.Latitude), Convert.ToDouble( driverdata.Longitude));
                          
                                tripdata.IsdriverArrived = true;
                                _context.Entry(tripdata).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                                return Ok(tripdata);
                          
                        }
                    }
                    else
                    {
                        return BadRequest("cordinates pickup location are not correct");
                    }
                }
                return BadRequest("An unexpected error occurred.");


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("Droplocation/atdrop")]
        public async Task<ActionResult<Trip>> DriverATDrop(int tripid, int driverId, Double latitude, Double longitudes)
        {
            try
            {
                var tripdata = _context.Trips.Find(tripid);
                if (tripdata == null)
                {
                    return NoContent();
                }

                if (tripdata.ToLocation!= "" && tripdata.ToLocation != null)
                {
                    var fromloaction = tripdata.ToLocation.Split(',').ToList();
                    if (fromloaction.Count == 2)
                    {

                        var locationlatitude = Convert.ToDouble(fromloaction[0]);
                        var locationlongitude = Convert.ToDouble(fromloaction[1]);
                        var driverdata = _context.Drivers.Find(driverId);
                        if (driverdata != null)
                        {

                            var distance = CalculateDistance(locationlatitude, locationlongitude, Convert.ToDouble(driverdata.Latitude), Convert.ToDouble( driverdata.Longitude));
                          
                                tripdata.IsAtDrop = true;
                                _context.Entry(tripdata).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                                return Ok(tripdata);
                          
                        }
                    }
                    else
                    {
                        return BadRequest("cordinates DROP location are not correct");
                    }
                }
                return BadRequest("An unexpected error occurred.");


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{tripid}/{driverId}&{latitude}&{longitudes}")]
        public async Task<ActionResult<Trip>> DriverendArrived(int tripid, int driverId, Double latitude, Double longitudes)
        {
            try
            {
                var tripdata = _context.Trips.Find(tripid);
                if (tripdata == null)
                {
                    return NoContent();
                }

                if (tripdata.ToLocation != "" && tripdata.ToLocation != null)
                {
                    var fromloaction = tripdata.ToLocation.Split(',').ToList();
                    if (fromloaction.Count == 2)
                    {

                        var locationlatitude = Convert.ToDouble(fromloaction[0]);
                        var locationlongitude = Convert.ToDouble(fromloaction[1]);

                        var distance = CalculateDistance(locationlatitude, locationlongitude, latitude, longitudes);
                        if (distance < 10)
                        {
                            tripdata.IsdriverArrived = true;
                            _context.Entry(tripdata).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            return Ok(tripdata);
                        }
                        else
                        {

                            return BadRequest("Driver did not arrive within 10 meters.");
                        }
                    }
                    else
                    {
                        return BadRequest("cordinates Drop location are not correct");
                    }
                }
                return BadRequest("An unexpected error occurred.");


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("stattrip/{id}")]

        public async Task<ActionResult<Trip>> ImmediaetAcceptTrip(int id, int DriverId, int triptypeId)
        {
            try
            {
                var tripdata = _context.Trips.Find(id);
                if (tripdata != null)
                {
                    tripdata.DriverId = DriverId;

                    if (tripdata.IsdriverArrived == true)
                    {
                        tripdata.IsTripStarted = true;
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

        [HttpPost("END/{id}")]

        public async Task<ActionResult<Trip>> ImmediaetAcceptTrip(int id, int DriverId, int triptypeId, int flag)
        {
            try
            {
                var tripdata = _context.Trips.Find(id);
                if (tripdata != null)
                {
                    tripdata.DriverId = DriverId;

                    if (tripdata.IsdriverArrived == true && tripdata.IsTripStarted == true)
                    {
                        tripdata.ActualEndTime = DateTime.Now;
                        tripdata.StartDateTime = tripdata.StartDateTime;
                        TimeSpan? timeDifference = DateTime.Now - tripdata.StartDateTime;
                        Decimal? taxpercentage = 0;
                        var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                        if (taxdata != null) 
                        {
                            taxpercentage = taxdata.Percentage;
                        }
                        decimal? careprice = 0;
                        if (tripdata.TripTypeId != 3)
                        {

                            var insurencedata = await _context.InsurenceTaxandPrice.FirstOrDefaultAsync();
                            if (insurencedata != null)
                            {
                                var percentage = insurencedata.TaxPercentage;
                                var price = insurencedata.Price;
                                var value = price * (percentage / 100);
                                careprice = price+value;
                            }
                        }
                        else
                        {
                            var insurencedata = await _context.InsurenceTaxandPriceoutoffcity.FirstOrDefaultAsync();
                            if (insurencedata != null)
                            {
                                var percentage = insurencedata.TaxPercentage;
                                var price = insurencedata.Price;
                                var value = price * (percentage / 100);
                                careprice = price+value;
                            }
                        }

                        if (timeDifference.HasValue)
                        {
                            double hoursDifference = timeDifference.Value.TotalHours;

                            // Assign the calculated hours to ActualEndTime or any other property as needed
                            tripdata.ActualEndTime = DateTime.Now;
                            tripdata.NoOfHoursActual = Convert.ToInt32(hoursDifference);

                            var difference = (Convert.ToInt32(tripdata.NoOfHoursActual) - Convert.ToInt32(tripdata.NoOfHoursSelected));
                            var hoursdata = _context.Hours
  .Where(c => c.TripTypeId == tripdata.TripTypeId && c.HoursName <= difference)
  .OrderByDescending(c => c.HoursName) // Order by HoursName in descending order
  .FirstOrDefault();
                            
                           
                            if (hoursdata != null)
                            {
                                if (difference <= 0)
                                {
                                    tripdata.TotalTripValue = tripdata.EstimatedPrice;

                                    var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == tripdata.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                    if (driversubdata != null)
                                    {
                                        var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                        if (subscriptiondata != null)
                                        {
                                            var percentage = subscriptiondata.Percentage;
                                            if (percentage > 0)
                                            {
                                                var taxvalue = Convert.ToInt32(tripdata.TotalTripValue)*(taxpercentage/100);
                                                var drivervalue = (Convert.ToInt32(tripdata.TotalTripValue)- taxvalue - careprice) * (percentage / 100);

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int minutes = difference * 60;
                                    decimal totalprice = 0;
                                    var tripvarientsdata = _context.TripVariants.Where(c => c.TriptypeId == tripdata.TripTypeId).FirstOrDefault();
                                    if (tripvarientsdata != null)
                                    {
                                        totalprice = minutes * Convert.ToInt32(tripvarientsdata.ChargesperMinute);
                                    }
                                    var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == tripdata.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                    if (driversubdata != null)
                                    {
                                        var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                        if (subscriptiondata != null)
                                        {
                                            var percentage = Convert.ToInt32(subscriptiondata.Percentage);
                                            if (percentage > 0)
                                            {
                                                tripdata.TotalTripValue = Math.Abs(Convert.ToDecimal(tripdata.EstimatedPrice) + totalprice);
                                                decimal totalValue = Convert.ToDecimal(tripdata.EstimatedPrice) + Convert.ToDecimal(totalprice);
                                                var taxvalue = Convert.ToInt32(totalValue) * (taxpercentage / 100);
                                                
                                                // Calculate drivervalue using decimal arithmetic to preserve precision
                                                decimal drivervalue =( totalValue -Convert.ToDecimal(taxvalue)- Convert.ToDecimal(careprice ))* percentage / 100;

                                                // Convert drivervalue to string for assignment
                                                tripdata.DriversPrice = drivervalue;

                                            }
                                        }
                                    }
                                    else
                                    {
                                        var percentage = 80;
                                        if (percentage > 0)
                                        {
                                            tripdata.TotalTripValue = Math.Abs(Math.Round(Convert.ToDecimal(tripdata.EstimatedPrice), 1) + totalprice);
                                            // Convert EstimatedPrice and totalprice to decimal and add them
                                            decimal totalValue = Convert.ToDecimal(tripdata.EstimatedPrice) + Convert.ToDecimal(totalprice);

                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            var taxvalue = Convert.ToInt32(totalValue) * (taxpercentage / 100);

                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;


                                        }
                                    }
                                }
                            }
                            else
                            {
                                tripdata.TotalTripValue = tripdata.EstimatedPrice;

                                var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == tripdata.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                if (driversubdata != null)
                                {
                                    var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                    if (subscriptiondata != null)
                                    {
                                        var percentage = subscriptiondata.Percentage;
                                        if (percentage > 0)
                                        {
                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            var taxvalue = Convert.ToInt32(tripdata.TotalTripValue) * (taxpercentage / 100);

                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            var drivervalue = (Convert.ToInt32(tripdata.TotalTripValue )- Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;
                                        }
                                    }
                                }
                                else
                                {
                                    var percentage = 80;
                                    if (percentage > 0)
                                    {
                                        tripdata.TotalTripValue = Math.Abs(Convert.ToDecimal(tripdata.EstimatedPrice));
                                        // Convert EstimatedPrice and totalprice to decimal and add them
                                        decimal totalValue = Convert.ToDecimal(tripdata.EstimatedPrice);

                                        var taxvalue = Convert.ToInt32(totalValue) * (taxpercentage / 100);

                                        // Calculate drivervalue using decimal arithmetic to preserve precision
                                        decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                        // Convert drivervalue to string for assignment
                                        tripdata.DriversPrice = drivervalue;
                                    }
                                }
                            }


                            // Optionally, update StartDateTime if necessary
                            // tripdata.StartDateTime = tripdata.StartDateTime; // This line seems redundant unless you have a specific reason to reassign StartDateTime to itself


                        }
                        else
                        {
                            tripdata.TotalTripValue = tripdata.EstimatedPrice;

                            var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == tripdata.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                            if (driversubdata != null)
                            {
                                var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                if (subscriptiondata != null)
                                {
                                    var percentage = subscriptiondata.Percentage;
                                    if (percentage > 0)
                                    {

                                        var taxvalue = Convert.ToInt32(tripdata.TotalTripValue) * (taxpercentage / 100);

                                        // Calculate drivervalue using decimal arithmetic to preserve precision
                                        var drivervalue = (Convert.ToInt32(tripdata.TotalTripValue)- taxvalue - Convert.ToDecimal(careprice) )* (percentage / 100);
                                        tripdata.DriversPrice = drivervalue;
                                    }
                                }
                            }
                            else
                            {
                                var percentage = 80;
                                if (percentage > 0)
                                {
                                    tripdata.TotalTripValue = Math.Abs(Convert.ToDecimal(tripdata.EstimatedPrice) );
                                    // Convert EstimatedPrice and totalprice to decimal and add them
                                    decimal totalValue = Convert.ToDecimal(tripdata.EstimatedPrice) ;
                                    var taxvalue = Convert.ToInt32(totalValue) * (taxpercentage / 100);
                                    // Calculate drivervalue using decimal arithmetic to preserve precision
                                    decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue )- Convert.ToDecimal(careprice)) * percentage / 100;

                                    // Convert drivervalue to string for assignment
                                    tripdata.DriversPrice = drivervalue;
                                }
                            }
                        }

                        tripdata.ActualEndTime = DateTime.Now;
                        tripdata.IsTripCompByDriver = true;
                    }
                    else
                    {
                        return BadRequest("Driver arival is not clearly done");
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
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371e3; // Radius of the earth in meters
            var φ1 = lat1 * Math.PI / 180; // Convert degrees to radians
            var φ2 = lat2 * Math.PI / 180;
            var Δφ = (lat2 - lat1) * Math.PI / 180;
            var Δλ = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = R * c; // Distance in meters

            return d;
        }

    }
}
