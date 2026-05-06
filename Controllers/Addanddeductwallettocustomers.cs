using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Addanddeductwallettocustomers : ControllerBase
    {
        private readonly DataContext _context;

        public Addanddeductwallettocustomers(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<CustomerWallet>> Postdata(int flag,int userId , int balance)
        {
            try
            {
                var walletdata = await _context.CustomerWallets.Where(c => c.UserId == userId).FirstOrDefaultAsync();
                if (walletdata == null)
                {
                    return NoContent();
                }

                if (flag == 1)
                {
                    walletdata.WalletBalance = walletdata.WalletBalance + balance;
                }
                if(flag ==2)
                {
                    walletdata.WalletBalance = walletdata.WalletBalance - balance;
                }

                _context.Entry(walletdata).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(walletdata);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
