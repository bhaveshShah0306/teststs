using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Nest;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Driver_Wallet_Fine : ControllerBase
    {
        private readonly DataContext _context;

        public Driver_Wallet_Fine(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<Driverwallet>> CutDriverWalletBalance(int flag, int Driverid, int balance, string? message)
        {
            try
            {
                var walletdata = await _context.Driverwallets.Where(c => c.DriverId == Driverid).FirstOrDefaultAsync();
                if (walletdata == null)
                {
                    var driverwallet = new Driverwallet();
                    driverwallet.DriverId = Driverid;
                    if (flag == 2)
                    {
                        var driverwalletTransaction = new DriverWalletTransactionHistory();

                        driverwalletTransaction.DriverId = Driverid;
                        driverwalletTransaction.WalletAmount = -balance;
                        driverwalletTransaction.CreatedDate = DateTime.Now;

                        _context.DriverWalletTransactionHistories.Add(driverwalletTransaction);
                        await _context.SaveChangesAsync();

                        var finereasons = new DriverPenalty();
                        finereasons.DriverId = Driverid;
                        finereasons.Reason = message;
                        finereasons.Balance = balance.ToString();
                        finereasons.CreatedDate = DateTime.Now;

                        _context.DriverPenalties.Add(finereasons);
                        await _context.SaveChangesAsync();
                        driverwallet.WalletBalance = (0 - balance).ToString();
                        var driverTransaction = new DriverTransaction()
                        {
                            DriverId = Driverid,
                            Amount = -balance, // with gst amount
                            CreatedDate = DateTime.Now,
                            description = message,
                        };
                        _context.DriverTransactions.Add(driverTransaction);
                        await _context.SaveChangesAsync();
                    }
                    if (flag == 1)
                    {
                        var driverwalletTransaction = new DriverWalletTransactionHistory();

                        driverwalletTransaction.DriverId = Driverid;
                        driverwalletTransaction.WalletAmount = balance;
                        driverwalletTransaction.CreatedDate = DateTime.Now;

                        _context.DriverWalletTransactionHistories.Add(driverwalletTransaction);
                        await _context.SaveChangesAsync();


                        driverwallet.WalletBalance = (0 + balance).ToString();
                        var driverTransaction = new DriverTransaction()
                        {
                            DriverId = Driverid,
                            Amount = balance, // with gst amount
                            CreatedDate = DateTime.Now,
                            description = "Incentive",
                        };
                        _context.DriverTransactions.Add(driverTransaction);
                        await _context.SaveChangesAsync();

                    }
                    _context.Driverwallets.Add(driverwallet);
                    await _context.SaveChangesAsync();
                    return Ok(driverwallet);
                }

                if (flag == 1)
                {
                    int finalbalance = Convert.ToInt32(walletdata.WalletBalance) + balance;
                    walletdata.WalletBalance = finalbalance.ToString();

                    var driverwalletTransaction = new DriverWalletTransactionHistory();

                    driverwalletTransaction.DriverId = Driverid;
                    driverwalletTransaction.WalletAmount = balance;
                    driverwalletTransaction.CreatedDate = DateTime.Now;

                    _context.DriverWalletTransactionHistories.Add(driverwalletTransaction);
                    await _context.SaveChangesAsync();
                    var driverTransaction = new DriverTransaction()
                    {
                        DriverId = Driverid,
                        Amount = balance, // with gst amount
                        CreatedDate = DateTime.Now,
                        description = "Incentive",
                    };
                    _context.DriverTransactions.Add(driverTransaction);
                    await _context.SaveChangesAsync();


                }
                if (flag == 2)
                {
                    int finalbalance = Convert.ToInt32(walletdata.WalletBalance) - balance;
                    walletdata.WalletBalance = finalbalance.ToString();
                    var driverwalletTransaction = new DriverWalletTransactionHistory();

                    driverwalletTransaction.DriverId = Driverid;
                    driverwalletTransaction.WalletAmount = -balance;
                    driverwalletTransaction.CreatedDate = DateTime.Now;

                    _context.DriverWalletTransactionHistories.Add(driverwalletTransaction);
                    await _context.SaveChangesAsync();

                    var finereasons = new DriverPenalty();
                    finereasons.DriverId = Driverid;
                    finereasons.Reason = message;
                    finereasons.Balance = balance.ToString();
                    finereasons.CreatedDate = DateTime.Now;

                    _context.DriverPenalties.Add(finereasons);
                    await _context.SaveChangesAsync();
                    var driverTransaction = new DriverTransaction()
                    {
                        DriverId = Driverid,
                        Amount = -balance, // with gst amount
                        CreatedDate = DateTime.Now,
                        description = message,
                    };
                    _context.DriverTransactions.Add(driverTransaction);
                    await _context.SaveChangesAsync();
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
