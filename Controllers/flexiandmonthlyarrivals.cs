﻿using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Globalization;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class flexiandmonthlyarrivals : ControllerBase
    {
        private readonly DataContext _context;

        public flexiandmonthlyarrivals(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<dynamic>> DriverArrived(int tripid, int tripdateid, int driverId, int flag)
        {
            try
            {
                var tripdata = new List<dynamic>();
                string fromlocation = string.Empty;
                if (flag == 1)
                {
                    var trip = await _context.Flexis.FindAsync(tripid);
                    if (trip != null)
                    {

                        fromlocation = trip.Coordinates;
                        tripdata.Add(trip);
                        trip.IsdriverArrived = true;
                        trip.IsTripStarted = true;
                        _context.Entry(trip).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
                else if (flag == 2)
                {
                    var trip = await _context.Monthlies.FindAsync(tripid);
                    if (trip != null)
                    {
                        fromlocation = trip.PickUpLocationCoordinates;
                        tripdata.Add(trip);
                        trip.IsdriverArrived = true;
                        _context.Entry(trip).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                    }
                }

                if (tripdata == null)
                {
                    return NoContent();
                }

                if (fromlocation != "" && fromlocation != null)
                {
                    var fromloaction = fromlocation.Split(',').ToList();
                    if (fromloaction.Count == 2)
                    {

                        var locationlatitude = Convert.ToDouble(fromloaction[0]);
                        var locationlongitude = Convert.ToDouble(fromloaction[1]);
                        var driverdata = _context.Drivers.Find(driverId);
                        if (driverdata != null)
                        {

                            var distance = CalculateDistance(locationlatitude, locationlongitude, Convert.ToDouble(driverdata.Latitude), Convert.ToDouble(driverdata.Longitude));
                            if (flag == 1)
                            {
                                var flexidatelist = _context.FlexiDatesLists.Find(tripdateid);
                                if (flexidatelist != null)
                                {
                                    flexidatelist.IsDriverArrival = true;
                                    _context.Entry(flexidatelist).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();

                                    return Ok(flexidatelist);
                                }
                            }
                            if (flag == 2)
                            {
                                var monthlydatelist = _context.MonthlyDateLists.Find(tripdateid);
                                if (monthlydatelist != null)
                                {
                                    monthlydatelist.IsDriverArrival = true;
                                   
                                    _context.Entry(monthlydatelist).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();

                                    return Ok(monthlydatelist);
                                }
                            }
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
        
        
        [HttpPost("&ATDROP")]
        public async Task<ActionResult<dynamic>> DriverAtDrop(int tripid, int tripdateid, int driverId, int flag)
        {
            try
            {
                var tripdata = new List<dynamic>();
                string fromlocation = string.Empty;
                if (flag == 1)
                {
                    var trip = await _context.Flexis.FindAsync(tripid);
                    if (trip != null)
                    {

                        fromlocation = trip.Coordinates;
                    }
                }
                else if (flag == 2)
                {
                    var trip = await _context.Monthlies.FindAsync(tripid);
                    if (trip != null)
                    {
                        fromlocation = trip.PickUpLocationCoordinates;
                        tripdata.Add(trip);

                    }
                }

                if (tripdata == null)
                {
                    return NoContent();
                }

                if (fromlocation != "" && fromlocation != null)
                {
                    var fromloaction = fromlocation.Split(',').ToList();
                    if (fromloaction.Count == 2)
                    {

                        var locationlatitude = Convert.ToDouble(fromloaction[0]);
                        var locationlongitude = Convert.ToDouble(fromloaction[1]);
                        var driverdata = _context.Drivers.Find(driverId);
                        if (driverdata != null)
                        {

                            var distance = CalculateDistance(locationlatitude, locationlongitude, Convert.ToDouble(driverdata.Latitude), Convert.ToDouble(driverdata.Longitude));
                            if (flag == 1)
                            {
                                var flexidatelist = _context.FlexiDatesLists.Find(tripdateid);
                                if (flexidatelist != null)
                                {
                                    flexidatelist.IsAtDrop = true;
                                    _context.Entry(flexidatelist).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();

                                    return Ok(flexidatelist);
                                }
                            }
                            if (flag == 2)
                            {
                                var monthlydatelist = _context.MonthlyDateLists.Find(tripdateid);
                                if (monthlydatelist != null)
                                {
                                    monthlydatelist.IsAtDrop = true;
                                   
                                    _context.Entry(monthlydatelist).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();

                                    return Ok(monthlydatelist);
                                }
                            }
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

        [HttpPost("tansport")]
        public async Task<ActionResult<dynamic>> Drivertransport(int tripid, int tripdateid, int driverId, int flag, string transport)
        {
            try
            {
                var tripdata = new List<dynamic>();
                string fromlocation = string.Empty;

                if (flag == 1)
                {
                    var trip = await _context.Flexis.FindAsync(tripid);
                    if (trip != null)
                    {
                        fromlocation = trip.Coordinates;
                        trip.Isonroute = true;
                        trip.selecteddateListvalue = tripdateid;
                        _context.Entry(trip).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        var flexidata = await _context.FlexiDatesLists
                            .FirstOrDefaultAsync(f => f.FlexiId == tripid && f.FlexiDatesListId == tripdateid);

                        if (flexidata != null)
                        {
                            flexidata.selecteddateListvalue = trip.selecteddateListvalue;
                            flexidata.Isonroute = true;
                            _context.Entry(flexidata).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }

                        // Add the relevant data to tripdata
                        tripdata.Add(trip);
                    }
                    else
                    {
                        // Handle case when trip is null
                        Console.WriteLine("Trip not found.");
                    }
                }
                else if (flag == 2)
                {
                    var trip = await _context.Monthlies.FindAsync(tripid);
                    if (trip != null)
                    {
                        fromlocation = trip.PickUpLocationCoordinates;
                        trip.Isonroute = true;
                       trip.selecteddateListvalue = tripdateid;
                        _context.Entry(trip).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        var flexidata = await _context.MonthlyDateLists
                           .FirstOrDefaultAsync(f => f.MonthlyId == tripid && f.MonthlyDateListId == trip.selecteddateListvalue);

                        if (flexidata != null)
                        {
                            flexidata.selecteddateListvalue = trip.selecteddateListvalue;
                            flexidata.Isonroute = true;
                            _context.Entry(flexidata).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                        tripdata.Add(trip);
                    }
                }

                if (tripdata == null)
                {
                    return NoContent();
                }

                var driverdata = _context.Drivers.Find(driverId);
                if (driverdata != null)
                {
                    if (flag == 1)
                    {
                        var flexidatelist = _context.FlexiDatesLists.Find(tripdateid);
                        if (flexidatelist != null)
                        {
                            flexidatelist.DriverMeansOfTransport = transport;
                            _context.Entry(flexidatelist).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            return Ok(flexidatelist);
                        }
                    }
                    if (flag == 2)
                    {
                        var monthlydatelist = _context.MonthlyDateLists.Find(tripdateid);
                        if (monthlydatelist != null)
                        {
                            monthlydatelist.DriverMeansOfTransport = transport;
                            _context.Entry(monthlydatelist).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            return Ok(monthlydatelist);
                        }
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

        public async Task<ActionResult<dynamic>> startTriptype(int id, int DriverId, int tripid, int tripdateid, int flag)
        {
            try
            {
                if (flag == 1)
                {
                    var tripdata = _context.FlexiDatesLists.Where(c => c.FlexiId == tripid && c.FlexiDatesListId == tripdateid).FirstOrDefault();
                    if (tripdata != null)
                    {

                        if (tripdata.IsDriverArrival == true)
                        {
                            tripdata.IsTripStarted = true;
                            tripdata.StartDateTime = DateTime.Now;
                        }
                        tripdata.selecteddateListvalue = tripdateid;
                        _context.Entry(tripdata).State = EntityState.Modified;
                        await _context.SaveChangesAsync();



                        return Ok(tripdata);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                if (flag == 2)
                {
                    var tripdata = _context.MonthlyDateLists.Where(c => c.MonthlyId== tripid && c.MonthlyDateListId== tripdateid).FirstOrDefault();
                    if (tripdata != null)
                    {

                        if (tripdata.IsDriverArrival == true)
                        {
                            tripdata.IsTripStarted = true;
                        }
                        tripdata.selecteddateListvalue = tripdateid;
                        _context.Entry(tripdata).State = EntityState.Modified;
                        await _context.SaveChangesAsync();



                        return Ok(tripdata);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                return BadRequest("data not available") ;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //  [HttpPost("END/{id}")]

        //  public async Task<ActionResult<Trip>> ImmediaetendTrip(int id, int DriverId, int triptypeId, int flag)
        //  {
        //      try
        //      {
        //          if (flag == 1)
        //          {
        //              var tripdata = _context.FlexiDatesLists.Where(c=>c.FlexiId == id && c.FlexiDatesListId==triptypeId).FirstOrDefault();
        //              if (tripdata != null)
        //              {
        //                  var flexidata = _context.Flexis.Find(id);
        //                  if (tripdata.IsDriverArrival == true && tripdata.IsTripStarted == true && flexidata!=null)
        //                  {
        //                      var date = DateTime.Now;
        //                      tripdata.endtime = date;
        //                      tripdata.StartDateTime = tripdata.StartDateTime;
        //                      TimeSpan? timeDifference = DateTime.Now - tripdata.StartDateTime;
        //                      if (timeDifference.HasValue)
        //                      {
        //                          double hoursDifference = timeDifference.Value.TotalHours;

        //                          // Assign the calculated hours to ActualEndTime or any other property as needed
        //                          tripdata.ActualEndTime = DateTime.Now;
        //                          tripdata.ActualHours = Convert.ToInt32(hoursDifference);

        //                          var difference = (Convert.ToInt32(tripdata.ActualHours) - Convert.ToInt32(flexidata.EstimatedHours));
        //                          var hoursdata = _context.Hours
        //.Where(c => c.TripTypeId == 4&& c.HoursName <= difference)
        //.OrderByDescending(c => c.HoursName) // Order by HoursName in descending order
        //.FirstOrDefault();
        //                          if (hoursdata != null)
        //                          {
        //                              if (difference <= 0)
        //                              {
        //                                  tripdata.ActualPrice = flexidata.EstimatedPrice;

        //                                  var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
        //                                  if (driversubdata != null)
        //                                  {
        //                                      var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
        //                                      if (subscriptiondata != null)
        //                                      {
        //                                          var percentage = subscriptiondata.SubPrice;
        //                                          if (percentage > 0)
        //                                          {
        //                                              var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
        //                                              tripdata.ActualPrice = drivervalue;
        //                                          }
        //                                      }
        //                                  }
        //                              }
        //                              else
        //                              {
        //                                  int minutes = difference * 60;
        //                                  decimal totalprice = 0;
        //                                  var tripvarientsdata = _context.TripVariants.Where(c => c.TriptypeId == 4).FirstOrDefault();
        //                                  if (tripvarientsdata != null)
        //                                  {
        //                                      totalprice = minutes * Convert.ToInt32(tripvarientsdata.ChargesperMinute);
        //                                  }
        //                                  var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
        //                                  if (driversubdata != null)
        //                                  {
        //                                      var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
        //                                      if (subscriptiondata != null)
        //                                      {
        //                                          var percentage = Convert.ToInt32(subscriptiondata.SubPrice);
        //                                          if (percentage > 0)
        //                                          {
        //                                              tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.EstimatedPrice) + totalprice);
        //                                              decimal totalValue = Convert.ToDecimal(flexidata.EstimatedPrice) + Convert.ToDecimal(totalprice);

        //                                              // Calculate drivervalue using decimal arithmetic to preserve precision
        //                                              decimal drivervalue = totalValue * percentage / 100;

        //                                              // Convert drivervalue to string for assignment
        //                                              tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
        //                                          }
        //                                      }
        //                                  }
        //                                  else
        //                                  {
        //                                      var percentage = 70;
        //                                      if (percentage > 0)
        //                                      {
        //                                          tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.EstimatedPrice) + totalprice);
        //                                          // Convert EstimatedPrice and totalprice to decimal and add them
        //                                          decimal totalValue = Convert.ToDecimal(flexidata.EstimatedPrice) + Convert.ToDecimal(totalprice);

        //                                          // Calculate drivervalue using decimal arithmetic to preserve precision
        //                                          decimal drivervalue = totalValue * percentage / 100;

        //                                          // Convert drivervalue to string for assignment
        //                                          tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
        //                                      }
        //                                  }
        //                              }
        //                          }
        //                          else
        //                          {
        //                              tripdata.ActualPrice = flexidata.EstimatedPrice;

        //                              var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId&& c.Expirydate <= DateTime.Now).FirstOrDefault();
        //                              if (driversubdata != null)
        //                              {
        //                                  var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
        //                                  if (subscriptiondata != null)
        //                                  {
        //                                      var percentage = subscriptiondata.SubPrice;
        //                                      if (percentage > 0)
        //                                      {
        //                                          var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
        //                                          tripdata.DriversPrice = drivervalue.ToString();
        //                                      }
        //                                  }
        //                              }
        //                              else
        //                              {
        //                                  var percentage = 70;
        //                                  if (percentage > 0)
        //                                  {
        //                                      tripdata.ActualPrice= Math.Abs(Convert.ToDecimal(flexidata.EstimatedPrice));
        //                                      // Convert EstimatedPrice and totalprice to decimal and add them
        //                                      decimal totalValue = Convert.ToDecimal(flexidata.EstimatedPrice);

        //                                      // Calculate drivervalue using decimal arithmetic to preserve precision
        //                                      decimal drivervalue = totalValue * percentage / 100;

        //                                      // Convert drivervalue to string for assignment
        //                                      tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
        //                                  }
        //                              }
        //                          }


        //                          // Optionally, update StartDateTime if necessary
        //                          // tripdata.StartDateTime = tripdata.StartDateTime; // This line seems redundant unless you have a specific reason to reassign StartDateTime to itself


        //                      }
        //                      else
        //                      {
        //                          tripdata.ActualPrice= flexidata.EstimatedPrice;

        //                          var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
        //                          if (driversubdata != null)
        //                          {
        //                              var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
        //                              if (subscriptiondata != null)
        //                              {
        //                                  var percentage = subscriptiondata.SubPrice;
        //                                  if (percentage > 0)
        //                                  {
        //                                      var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
        //                                      tripdata.DriversPrice = drivervalue.ToString();
        //                                  }
        //                              }
        //                          }
        //                          else
        //                          {
        //                              var percentage = 70;
        //                              if (percentage > 0)
        //                              {
        //                                  tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.EstimatedPrice));
        //                                  // Convert EstimatedPrice and totalprice to decimal and add them
        //                                  decimal totalValue = Convert.ToDecimal(flexidata.EstimatedPrice);

        //                                  // Calculate drivervalue using decimal arithmetic to preserve precision
        //                                  decimal drivervalue = totalValue * percentage / 100;

        //                                  // Convert drivervalue to string for assignment
        //                                  tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
        //                              }
        //                          }
        //                      }

        //                      tripdata.ActualEndTime = DateTime.Now;
        //                      tripdata.IsTripCompByDriver = true;
        //                  }
        //                  else
        //                  {
        //                      return BadRequest("Driver arival is not clearly done");
        //                  }
        //                  tripdata.IsTripCompByDriver = true;
        //                  _context.Entry(tripdata).State = EntityState.Modified;
        //                  await _context.SaveChangesAsync();



        //                  return Ok(tripdata);
        //              }
        //              else
        //              {
        //                  return NoContent();
        //              }
        //          }
        //          if (flag == 2)
        //          {
        //              var tripdata = _context.MonthlyDateLists.Where(c=>c.MonthlyId== id && c.MonthlyDateListId==triptypeId).FirstOrDefault();
        //              if (tripdata != null)
        //              {
        //                  var flexidata = _context.Monthlies.Find(id);
        //                  if (tripdata.IsDriverArrival == true && tripdata.IsTripStarted == true && flexidata!=null)
        //                  {
        //                      tripdata.endtime = DateTime.Now;
        //                      tripdata.StartDateTime = tripdata.StartDateTime;
        //                      TimeSpan? timeDifference = DateTime.Now - tripdata.StartDateTime;
        //                      if (timeDifference.HasValue)
        //                      {
        //                          double hoursDifference = timeDifference.Value.TotalHours;

        //                          // Assign the calculated hours to ActualEndTime or any other property as needed
        //                          tripdata.ActualEndTime = DateTime.Now;
        //                          tripdata.ActualHours = Convert.ToInt32(hoursDifference);

        //                          var difference = (Convert.ToInt32(tripdata.ActualHours) - Convert.ToInt32(flexidata.EstimatedHours));
        //                          var hoursdata = _context.Hours
        //.Where(c => c.TripTypeId == 4&& c.HoursName <= difference)
        //.OrderByDescending(c => c.HoursName) // Order by HoursName in descending order
        //.FirstOrDefault();
        //                          if (hoursdata != null)
        //                          {
        //                              if (difference <= 0)
        //                              {
        //                                  tripdata.ActualPrice = flexidata.Estimatedprice;

        //                                  var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
        //                                  if (driversubdata != null)
        //                                  {
        //                                      var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
        //                                      if (subscriptiondata != null)
        //                                      {
        //                                          var percentage = subscriptiondata.SubPrice;
        //                                          if (percentage > 0)
        //                                          {
        //                                              var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
        //                                              tripdata.ActualPrice = drivervalue;
        //                                          }
        //                                      }
        //                                  }
        //                              }
        //                              else
        //                              {
        //                                  int minutes = difference * 60;
        //                                  decimal totalprice = 0;
        //                                  var tripvarientsdata = _context.TripVariants.Where(c => c.TriptypeId == 4).FirstOrDefault();
        //                                  if (tripvarientsdata != null)
        //                                  {
        //                                      totalprice = minutes * Convert.ToInt32(tripvarientsdata.ChargesperMinute);
        //                                  }
        //                                  var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
        //                                  if (driversubdata != null)
        //                                  {
        //                                      var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
        //                                      if (subscriptiondata != null)
        //                                      {
        //                                          var percentage = Convert.ToInt32(subscriptiondata.SubPrice);
        //                                          if (percentage > 0)
        //                                          {
        //                                              tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.Estimatedprice) + totalprice);
        //                                              decimal totalValue = Convert.ToDecimal(flexidata.Estimatedprice) + Convert.ToDecimal(totalprice);

        //                                              // Calculate drivervalue using decimal arithmetic to preserve precision
        //                                              decimal drivervalue = totalValue * percentage / 100;

        //                                              // Convert drivervalue to string for assignment
        //                                              tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
        //                                          }
        //                                      }
        //                                  }
        //                                  else
        //                                  {
        //                                      var percentage = 70;
        //                                      if (percentage > 0)
        //                                      {
        //                                          tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.Estimatedprice) + totalprice);
        //                                          // Convert EstimatedPrice and totalprice to decimal and add them
        //                                          decimal totalValue = Convert.ToDecimal(flexidata.Estimatedprice) + Convert.ToDecimal(totalprice);

        //                                          // Calculate drivervalue using decimal arithmetic to preserve precision
        //                                          decimal drivervalue = totalValue * percentage / 100;

        //                                          // Convert drivervalue to string for assignment
        //                                          tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
        //                                      }
        //                                  }
        //                              }
        //                          }
        //                          else
        //                          {
        //                              tripdata.ActualPrice = flexidata.Estimatedprice;

        //                              var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId&& c.Expirydate <= DateTime.Now).FirstOrDefault();
        //                              if (driversubdata != null)
        //                              {
        //                                  var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
        //                                  if (subscriptiondata != null)
        //                                  {
        //                                      var percentage = subscriptiondata.SubPrice;
        //                                      if (percentage > 0)
        //                                      {
        //                                          var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
        //                                          tripdata.DriversPrice = drivervalue.ToString();
        //                                      }
        //                                  }
        //                              }
        //                              else
        //                              {
        //                                  var percentage = 70;
        //                                  if (percentage > 0)
        //                                  {
        //                                      tripdata.ActualPrice= Math.Abs(Convert.ToDecimal(flexidata.Estimatedprice));
        //                                      // Convert EstimatedPrice and totalprice to decimal and add them
        //                                      decimal totalValue = Convert.ToDecimal(flexidata.Estimatedprice);

        //                                      // Calculate drivervalue using decimal arithmetic to preserve precision
        //                                      decimal drivervalue = totalValue * percentage / 100;

        //                                      // Convert drivervalue to string for assignment
        //                                      tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
        //                                  }
        //                              }
        //                          }


        //                          // Optionally, update StartDateTime if necessary
        //                          // tripdata.StartDateTime = tripdata.StartDateTime; // This line seems redundant unless you have a specific reason to reassign StartDateTime to itself


        //                      }
        //                      else
        //                      {
        //                          tripdata.ActualPrice= flexidata.Estimatedprice;

        //                          var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
        //                          if (driversubdata != null)
        //                          {
        //                              var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
        //                              if (subscriptiondata != null)
        //                              {
        //                                  var percentage = subscriptiondata.SubPrice;
        //                                  if (percentage > 0)
        //                                  {
        //                                      var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
        //                                      tripdata.DriversPrice = drivervalue.ToString();
        //                                  }
        //                              }
        //                          }
        //                          else
        //                          {
        //                              var percentage = 70;
        //                              if (percentage > 0)
        //                              {
        //                                  tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.Estimatedprice));
        //                                  // Convert EstimatedPrice and totalprice to decimal and add them
        //                                  decimal totalValue = Convert.ToDecimal(flexidata.Estimatedprice);

        //                                  // Calculate drivervalue using decimal arithmetic to preserve precision
        //                                  decimal drivervalue = totalValue * percentage / 100;

        //                                  // Convert drivervalue to string for assignment
        //                                  tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
        //                              }
        //                          }
        //                      }

        //                      tripdata.ActualEndTime = DateTime.Now;
        //                      tripdata.IsTripCompByDriver = true;
        //                  }
        //                  else
        //                  {
        //                      return BadRequest("Driver arival is not clearly done");
        //                  }
        //                  tripdata.IsTripCompByDriver = true;
        //                  _context.Entry(tripdata).State = EntityState.Modified;
        //                  await _context.SaveChangesAsync();



        //                  return Ok(tripdata);
        //              }
        //              else
        //              {
        //                  return NoContent();
        //              }
        //          }

        //          return BadRequest("some thing went wrong");
        //      }
        //      catch (Exception ex)
        //      {
        //          return BadRequest(ex.Message);
        //      }
        //  }


        //sreekar's code
        [HttpPost("END/{id}")]

        public async Task<ActionResult<Trip>> ImmediaetendTrip(int id, int DriverId, int triptypeId, int flag)
        {
            try
            {
                if (flag == 1)
                {
                    var tripdata = _context.FlexiDatesLists.Where(c => c.FlexiId == id && c.FlexiDatesListId == triptypeId).FirstOrDefault();
                    if (tripdata != null)
                    {
                        var flexidata = _context.Flexis.Find(id);
                        if (tripdata.IsDriverArrival == true && tripdata.IsTripStarted == true && flexidata != null)
                        {
                            var date = DateTime.Now;
                            var estimatedhours = flexidata.EstimatedHours;
                            tripdata.endtime = tripdata.StartDateTime.Value.AddHours(Convert.ToInt32(estimatedhours));
                            tripdata.StartDateTime = tripdata.StartDateTime;
                            TimeSpan? timeDifference = DateTime.Now - tripdata.StartDateTime;
                            if (timeDifference.HasValue)
                            {
                                double hoursDifference = timeDifference.Value.TotalHours;

                                // Assign the calculated hours to ActualEndTime or any other property as needed
                                tripdata.ActualEndTime = DateTime.Now;
                                tripdata.ActualHours = Convert.ToInt32(hoursDifference);

                                var difference = (Convert.ToInt32(tripdata.ActualHours) - Convert.ToInt32(flexidata.EstimatedHours));
                                var hoursdata = _context.Hours
                                  .Where(c => c.TripTypeId == 4 && c.HoursName <= flexidata.EstimatedHours)
                                  .OrderByDescending(c => c.HoursName) // Order by HoursName in descending order
                                  .FirstOrDefault();
                                if (hoursdata != null)
                                {
                                    if (difference <= 0)
                                    {
                                        var flexidatelist = _context.FlexiDatesLists.Where(c => c.FlexiId == id && c.FlexiDatesListId == triptypeId).FirstOrDefault();
                                        if (flexidatelist != null)
                                        {

                                            tripdata.ActualPrice = flexidatelist.ActualPrice;
                                        }
                                        else
                                        {
                                            tripdata.ActualPrice = flexidata.EstimatedPrice;

                                        }
                                        var taxdata = await _context.Taxes.ToListAsync();
                                        decimal? taxprice = 0;
                                        if (taxdata.Count > 0)
                                        {
                                            foreach (var tax in taxdata)
                                            {

                                                var taxpercentage = tax.Percentage;
                                                var price = tripdata.ActualPrice * (taxpercentage / 100);
                                                taxprice += price;
                                            }
                                        }
                                        var total = flexidata.EstimatedPrice + taxprice;
                                        tripdata.ActualPrice = total;
                                        var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                        if (driversubdata != null)
                                        {
                                            var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                            if (subscriptiondata != null)
                                            {
                                                var percentage = subscriptiondata.SubPrice;
                                                if (percentage > 0)
                                                {
                                                    var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
                                                    tripdata.ActualPrice = drivervalue;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int minutes = difference * 60;
                                        decimal totalprice = 0;
                                        var tripvarientsdata = _context.TripVariants.Where(c => c.TriptypeId == 4).FirstOrDefault();
                                        if (tripvarientsdata != null)
                                        {
                                            totalprice = minutes * Convert.ToInt32(tripvarientsdata.ChargesperMinute);
                                        }
                                        tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.EstimatedPrice) + totalprice);
                                        var taxdata = await _context.Taxes.ToListAsync();
                                        decimal? taxprice = 0;
                                        if (taxdata.Count > 0)
                                        {
                                            foreach (var tax in taxdata)
                                            {

                                                var taxpercentage = tax.Percentage;
                                                var price = tripdata.ActualPrice * (taxpercentage / 100);
                                                taxprice += price;
                                            }
                                        }
                                        tripdata.ActualPrice = tripdata.ActualPrice + taxprice;
                                        var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                        if (driversubdata != null)
                                        {
                                            var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                            if (subscriptiondata != null)
                                            {
                                                var percentage = Convert.ToInt32(subscriptiondata.SubPrice);
                                                if (percentage > 0)
                                                {

                                                    decimal totalValue = Convert.ToDecimal(tripdata.ActualPrice);

                                                    // Calculate drivervalue using decimal arithmetic to preserve precision
                                                    decimal drivervalue = totalValue * percentage / 100;

                                                    // Convert drivervalue to string for assignment
                                                    tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var percentage = 70;
                                            if (percentage > 0)
                                            {

                                                // Convert EstimatedPrice and totalprice to decimal and add them
                                                decimal totalValue = Convert.ToDecimal(tripdata.ActualPrice);

                                                // Calculate drivervalue using decimal arithmetic to preserve precision
                                                decimal drivervalue = totalValue * percentage / 100;

                                                // Convert drivervalue to string for assignment
                                                tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    tripdata.ActualPrice = flexidata.EstimatedPrice;

                                    var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                    if (driversubdata != null)
                                    {
                                        var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                        if (subscriptiondata != null)
                                        {
                                            var percentage = subscriptiondata.SubPrice;
                                            if (percentage > 0)
                                            {
                                                var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
                                                tripdata.DriversPrice = drivervalue.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var percentage = 70;
                                        if (percentage > 0)
                                        {
                                            tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.EstimatedPrice));
                                            // Convert EstimatedPrice and totalprice to decimal and add them
                                            decimal totalValue = Convert.ToDecimal(flexidata.EstimatedPrice);

                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            decimal drivervalue = totalValue * percentage / 100;

                                            // Convert drivervalue to string for assignment
                                            tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
                                        }
                                    }
                                }


                                // Optionally, update StartDateTime if necessary
                                // tripdata.StartDateTime = tripdata.StartDateTime; // This line seems redundant unless you have a specific reason to reassign StartDateTime to itself


                            }
                            else
                            {

                                tripdata.ActualPrice = flexidata.EstimatedPrice;

                                var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                if (driversubdata != null)
                                {
                                    var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                    if (subscriptiondata != null)
                                    {
                                        var percentage = subscriptiondata.SubPrice;
                                        if (percentage > 0)
                                        {
                                            var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
                                            tripdata.DriversPrice = drivervalue.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    var percentage = 70;
                                    if (percentage > 0)
                                    {
                                        tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.EstimatedPrice));
                                        // Convert EstimatedPrice and totalprice to decimal and add them
                                        decimal totalValue = Convert.ToDecimal(flexidata.EstimatedPrice);

                                        // Calculate drivervalue using decimal arithmetic to preserve precision
                                        decimal drivervalue = totalValue * percentage / 100;

                                        // Convert drivervalue to string for assignment
                                        tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
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
                        tripdata.IsTripCompByDriver = true;
                        _context.Entry(tripdata).State = EntityState.Modified;
                        await _context.SaveChangesAsync();



                        return Ok(tripdata);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                if (flag == 2)
                {
                    var tripdata = _context.MonthlyDateLists.Where(c => c.MonthlyId == id && c.MonthlyDateListId == triptypeId).FirstOrDefault();
                    if (tripdata != null)
                    {
                        var flexidata = _context.Monthlies.Find(id);
                        if (tripdata.IsDriverArrival == true && tripdata.IsTripStarted == true && flexidata != null)
                        {
                            tripdata.endtime = DateTime.Now;
                            tripdata.StartDateTime = tripdata.StartDateTime;
                            TimeSpan? timeDifference = DateTime.Now - tripdata.StartDateTime;
                            if (timeDifference.HasValue)
                            {
                                double hoursDifference = timeDifference.Value.TotalHours;

                                // Assign the calculated hours to ActualEndTime or any other property as needed
                                tripdata.ActualEndTime = DateTime.Now;
                                tripdata.ActualHours = Convert.ToInt32(hoursDifference);

                                var difference = (Convert.ToInt32(tripdata.ActualHours) - Convert.ToInt32(flexidata.EstimatedHours));
                                var hoursdata = _context.Hours
      .Where(c => c.TripTypeId == 4 && c.HoursName <= difference)
      .OrderByDescending(c => c.HoursName) // Order by HoursName in descending order
      .FirstOrDefault();
                                if (hoursdata != null)
                                {
                                    if (difference <= 0)
                                    {
                                        var flexidatelist = _context.MonthlyDateLists.Where(c => c.MonthlyId == id && c.MonthlyDateListId == triptypeId).FirstOrDefault();
                                        if (flexidatelist != null)
                                        {

                                            tripdata.ActualPrice = flexidatelist.ActualPrice;
                                        }
                                        else
                                        {
                                            tripdata.ActualPrice = flexidata.Estimatedprice;

                                        }
                                        var taxdata = await _context.Taxes.ToListAsync();
                                        decimal? taxprice = 0;
                                        if (taxdata.Count > 0)
                                        {
                                            foreach (var tax in taxdata)
                                            {

                                                var taxpercentage = tax.Percentage;
                                                var price = tripdata.ActualPrice * (taxpercentage / 100);
                                                taxprice += price;
                                            }
                                        }
                                        var total = flexidata.Estimatedprice + taxprice;
                                        tripdata.ActualPrice = total;
                                        var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                        if (driversubdata != null)
                                        {
                                            var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                            if (subscriptiondata != null)
                                            {
                                                var percentage = subscriptiondata.SubPrice;
                                                if (percentage > 0)
                                                {
                                                    var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
                                                    tripdata.ActualPrice = drivervalue;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var flexidatelist = _context.MonthlyDateLists.Where(c => c.MonthlyId == id && c.MonthlyDateListId == triptypeId).FirstOrDefault();
                                        if (flexidatelist != null)
                                        {

                                            tripdata.ActualPrice = flexidatelist.ActualPrice;
                                        }
                                        else
                                        {
                                            tripdata.ActualPrice = flexidata.Estimatedprice;

                                        }
                                        int minutes = difference * 60;
                                        decimal totalprice = 0;
                                        var tripvarientsdata = _context.TripVariants.Where(c => c.TriptypeId == 4).FirstOrDefault();
                                        if (tripvarientsdata != null)
                                        {
                                            totalprice = minutes * Convert.ToInt32(tripvarientsdata.ChargesperMinute);
                                        }
                                        var taxdata = await _context.Taxes.ToListAsync();
                                        decimal? taxprice = 0;
                                        if (taxdata.Count > 0)
                                        {
                                            foreach (var tax in taxdata)
                                            {

                                                var taxpercentage = tax.Percentage;
                                                var price = tripdata.ActualPrice * (taxpercentage / 100);
                                                taxprice += price;
                                            }
                                        }
                                        var total = flexidata.Estimatedprice + taxprice + totalprice;
                                        tripdata.ActualPrice = totalprice;
                                        var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                        if (driversubdata != null)
                                        {
                                            var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                            if (subscriptiondata != null)
                                            {
                                                var percentage = Convert.ToInt32(subscriptiondata.SubPrice);
                                                if (percentage > 0)
                                                {
                                                    decimal totalValue = Convert.ToDecimal(tripdata.ActualPrice);

                                                    // Calculate drivervalue using decimal arithmetic to preserve precision
                                                    decimal drivervalue = totalValue * percentage / 100;

                                                    // Convert drivervalue to string for assignment
                                                    tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var percentage = 70;
                                            if (percentage > 0)
                                            {

                                                // Convert EstimatedPrice and totalprice to decimal and add them
                                                decimal totalValue = Convert.ToDecimal(flexidata.Estimatedprice) + Convert.ToDecimal(totalprice);

                                                // Calculate drivervalue using decimal arithmetic to preserve precision
                                                decimal drivervalue = totalValue * percentage / 100;

                                                // Convert drivervalue to string for assignment
                                                tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var flexidatelist = _context.MonthlyDateLists.Where(c => c.MonthlyId == id && c.MonthlyDateListId == triptypeId).FirstOrDefault();
                                    if (flexidatelist != null)
                                    {

                                        tripdata.ActualPrice = flexidatelist.ActualPrice;
                                    }
                                    else
                                    {
                                        tripdata.ActualPrice = flexidata.Estimatedprice;

                                    }

                                    var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                    if (driversubdata != null)
                                    {
                                        var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                        if (subscriptiondata != null)
                                        {
                                            var percentage = subscriptiondata.SubPrice;
                                            if (percentage > 0)
                                            {
                                                var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
                                                tripdata.DriversPrice = drivervalue.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var percentage = 70;
                                        if (percentage > 0)
                                        {
                                            tripdata.ActualPrice = Math.Abs(Convert.ToDecimal(flexidata.Estimatedprice));
                                            // Convert EstimatedPrice and totalprice to decimal and add them
                                            decimal totalValue = Convert.ToDecimal(flexidata.Estimatedprice);

                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            decimal drivervalue = totalValue * percentage / 100;

                                            // Convert drivervalue to string for assignment
                                            tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
                                        }
                                    }
                                }


                                // Optionally, update StartDateTime if necessary
                                // tripdata.StartDateTime = tripdata.StartDateTime; // This line seems redundant unless you have a specific reason to reassign StartDateTime to itself


                            }
                            else
                            {
                                var flexidatelist = _context.MonthlyDateLists.Where(c => c.MonthlyId == id && c.MonthlyDateListId == triptypeId).FirstOrDefault();
                                if (flexidatelist != null)
                                {

                                    tripdata.ActualPrice = flexidatelist.ActualPrice;
                                }
                                else
                                {
                                    tripdata.ActualPrice = flexidata.Estimatedprice;

                                }
                                var taxdata = await _context.Taxes.ToListAsync();
                                decimal? taxprice = 0;
                                if (taxdata.Count > 0)
                                {
                                    foreach (var tax in taxdata)
                                    {

                                        var taxpercentage = tax.Percentage;
                                        var price = tripdata.ActualPrice * (taxpercentage / 100);
                                        taxprice += price;
                                    }
                                }
                                var total = flexidata.Estimatedprice + taxprice;
                                tripdata.ActualPrice = total;
                                var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                if (driversubdata != null)
                                {
                                    var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                    if (subscriptiondata != null)
                                    {
                                        var percentage = subscriptiondata.SubPrice;
                                        if (percentage > 0)
                                        {
                                            var drivervalue = Convert.ToInt32(tripdata.ActualPrice) * (percentage / 100);
                                            tripdata.DriversPrice = drivervalue.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    var percentage = 70;
                                    if (percentage > 0)
                                    {

                                        // Convert EstimatedPrice and totalprice to decimal and add them
                                        decimal totalValue = Convert.ToDecimal(flexidata.Estimatedprice);

                                        // Calculate drivervalue using decimal arithmetic to preserve precision
                                        decimal drivervalue = totalValue * percentage / 100;

                                        // Convert drivervalue to string for assignment
                                        tripdata.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);
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
                        tripdata.IsTripCompByDriver = true;
                        _context.Entry(tripdata).State = EntityState.Modified;
                        await _context.SaveChangesAsync();



                        return Ok(tripdata);
                    }
                    else
                    {
                        return NoContent();
                    }
                }

                return BadRequest("some thing went wrong");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]

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