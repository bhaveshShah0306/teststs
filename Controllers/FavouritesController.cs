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
    public class FavouritesController : ControllerBase
    {
        private readonly DataContext _context;

        public FavouritesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Favourites
        [HttpGet("{userId}/users")]
        public async Task<ActionResult<IEnumerable<FavouriteVM>>> GetFavourites(int userId)
        {
            try
            {
                var favouriteList = _context.Favourites.Where(c=>c.UserId == userId).ToList();
                var favouriteVMList = new List<FavouriteVM>();
                if(favouriteList.Count > 0)
                {
                    foreach(var fav in favouriteList)
                    {
                        var favVM = new FavouriteVM();
                        favVM.FavouriteId = fav.FavouriteId;
                        favVM.FavouriteName=fav.FavouriteName;
                        favVM.UserId = fav.UserId;
                        favVM.Address = fav.Address;
                        favVM.place_id = fav.place_id;
                        favVM.Coordinates= fav.Coordinates;

                        var userdata = _context.Users.Find(favVM.UserId);
                        if(userdata!=null)
                        {
                            favVM.UserName = userdata.Name;
                            favVM.ContactNumber = userdata.PhoneNumber;
                        }

                        favVM.FavouriteTypesId = fav.FavouriteTypesId;
                        var favtypedata = _context.FavouriteTypes.Find(favVM.FavouriteTypesId);
                        if (favtypedata != null)
                        {
                            favVM.FavouriteTypeName = favtypedata.FavouriteTypesName;
                        }

                        favouriteVMList.Add(favVM);
                    }
                    return Ok(favouriteVMList);
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
        [HttpGet("{userId}/Formatted")]
        public async Task<ActionResult<IEnumerable<favouritesVMListfordashbord>>> GetFavouritebyuserId(int userId)
        {
            try
            {
                var favouriteList = _context.Favourites.Where(c=>c.UserId == userId).ToList();
                var favouriteVMList = new List<favouritesVMListfordashbord>();
                if(favouriteList.Count > 0)
                {
                    foreach(var fav in favouriteList)
                    {
                        var favVM = new favouritesVMListfordashbord();
                        favVM.FavouriteId = fav.FavouriteId;
                        favVM.FavouriteName = fav.FavouriteName;
                        favVM.formatted_address = fav.Address;
                        favVM.place_id = fav.place_id;
                        if(fav.Coordinates!=null && fav.Coordinates != "")
                        {
                            var coordinates= fav.Coordinates.Split(',').ToList();
                            if (coordinates.Count > 0)
                            {
                                var details = new Details();
                                details.lat = coordinates[0];
                                details.lng= coordinates[1];

                                favVM.details = details;
                            }
                        }
                        favVM.mapUrl = fav.mapUrl;
                        favVM.description = fav.Address;

                        favouriteVMList.Add(favVM);
                    }
                    return Ok(favouriteVMList);
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

        // GET: api/Favourites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Favourite>> GetFavourite(int id)
        {
            try
            {
                var fav = _context.Favourites.Find(id);
                var favVM = new FavouriteVM();
                if (fav != null)
                {
                    favVM.FavouriteId = fav.FavouriteId;
                    favVM.FavouriteName = fav.FavouriteName;
                    favVM.UserId = fav.UserId;
                    favVM.Address = fav.Address;
                    favVM.place_id = fav.place_id;
                    favVM.Coordinates = fav.Coordinates;

                    var userdata = _context.Users.Find(favVM.UserId);
                    if (userdata != null)
                    {
                        favVM.UserName = userdata.Name;
                        favVM.ContactNumber = userdata.PhoneNumber;
                    }

                    favVM.FavouriteTypesId = fav.FavouriteTypesId;
                    var favtypedata = _context.FavouriteTypes.Find(favVM.FavouriteTypesId);
                    if (favtypedata != null)
                    {
                        favVM.FavouriteTypeName = favtypedata.FavouriteTypesName;
                    }
                    return Ok(favVM);
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

        // PUT: api/Favourites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutFavourite(int id, Favourite favourite)
        {
            if (id != favourite.FavouriteId)
            {
                return BadRequest();
            }

            _context.Entry(favourite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavouriteExists(id))
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

        // POST: api/Favourites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Favourite>> PostFavourite(Favourite favourite)
        {
          if (_context.Favourites == null)
          {
              return Problem("Entity set 'DataContext.Favourites'  is null.");
          }
          var favdata = _context.Favourites.Where(fa => fa.UserId == favourite.UserId).ToList();
            if (favdata.Count == 3)
            {
                return BadRequest("Cannnot be created");
            }
            _context.Favourites.Add(favourite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavourite", new { id = favourite.FavouriteId }, favourite);
        }

        // DELETE: api/Favourites/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteFavourite(int id)
        {
            if (_context.Favourites == null)
            {
                return NotFound();
            }
            var favourite = await _context.Favourites.FindAsync(id);
            if (favourite == null)
            {
                return NotFound();
            }

            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FavouriteExists(int id)
        {
            return (_context.Favourites?.Any(e => e.FavouriteId == id)).GetValueOrDefault();
        }
    }
}
