using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi.Models;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using System.Numerics;
using System.Xml.Linq;
using System;

namespace GoChauffeurWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
         
        public DbSet<User> Users { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverTracking> DriverTrackings { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripType> TripTypes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionHistory> SubscriptionHistories { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<AerialDistancePrice> AerialDistancePrices { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TripStatus> TripStatuses { get; set; }

        public DbSet<Cupons> Cupons { get; set; }
        public DbSet<CuponsHistory> CuponsHistories{ get; set; }

        public DbSet<Ratings> Ratings { get; set; }
        public DbSet<TransmissionType> TransmissionTypes { get; set; }
        public DbSet<Refer> Referals { get; set; }

        public DbSet<Hours> Hours { get; set; }
        public DbSet<Driversubscription> Driversubscriptions { get; set; }
        public DbSet<Driverwallet> Driverwallets{ get; set; }
        public DbSet<VerificationStatus> VerificationStatuses{ get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<TripTax> TripTaxes { get; set; }
        public DbSet<InsurenceTaxandPrice> InsurenceTaxandPrice { get; set; }
        public DbSet<DriverWalletTransactionHistory> DriverWalletTransactionHistories { get; set; }
        public DbSet<TripCancellationReasons> TripCancellationReasons { get; set; }
        public DbSet<TripVariant> TripVariants { get; set; }
        public DbSet<NightCharges> NightCharges { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<FavouriteTypes> FavouriteTypes { get; set; }
        public DbSet<ServicePlaces> ServicePlaces { get; set; }
        public DbSet<FlexiDatesList> FlexiDatesLists { get; set; }
        public DbSet<Flexi> Flexis { get; set; }
        public DbSet<OutstationRoundtriphours> OutstationRoundtriphours { get; set; }
        public DbSet<ValetParking> ValetParkings { get; set; }
        public DbSet<ValetParkingPricing> ValetParkingPricing { get; set; }
        public DbSet<ValetparkingStaff> ValetparkingStaff { get; set; }
        public DbSet<Monthly> Monthlies { get; set; }
        public DbSet<MonthlyDateList> MonthlyDateLists { get; set; }
        public DbSet<AdvanceAmount> AdvanceAmounts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ValetParkingHourlyPrices> ValetParkingHourlyPrices { get; set; }
        public DbSet<RequiredValetstaffs> RequiredValetstaffs { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Driver_Joining_Fee> Driver_Joining_Fees { get; set; }
        public DbSet<Driversurroundingtrips> Driversurroundingtrips { get; set; }
        public DbSet<CustomerWallet> CustomerWallets { get; set; }
        public DbSet<DriverTransaction> DriverTransactions { get; set; }
        public DbSet<DriverFines> DriverFines { get; set; }
        public DbSet<CustomerWalletFine> CustomerWalletFines { get; set; }

        public DbSet<Anonymoustripcharges> Anonymoustripcharges { get; set; }

        public DbSet<DriverTickets> DriverTickets { get; set; }

        public DbSet<DriverTicketsReason> DriverTicketsReasons { get; set; }

        public DbSet<CustomerTicketReasons> CustomerTicketReasons { get; set; }

        public DbSet<CustomerTickets> CustomerTickets { get; set; }

        public DbSet<IgnoredTrips> IgnoredTrips { get; set; }

        public   DbSet <Ignoretripresons> ignoretripresons { get; set; }
        
       public DbSet<IgnoreFlexiTrips> IgnoreFlexiTrips { get; set; }
 
        public DbSet<IgnoreMontlyTrips> IgnoreMontlyTrips { get; set; }

         public DbSet<IgnoreValletTrips> IgnoreValletTrips { get; set; }

        public DbSet<UsersTripsCancelResons> UsersTripsCancelResons { get; set; }

        public DbSet<DriverWalletDeductionResons> DriverWalletDeductionResons { get; set; }


        public DbSet<SubscripationGst> SubscripationGst { get; set; }

        public DbSet<Schuduletriptimechagemodel> Schuduletriptimechagemodel { get; set; }
        public DbSet<DriverPenalty> DriverPenalties { get; set; }
        public DbSet<ReferEarning> ReferEarnings { get; set; }
        public DbSet<CustomerwalletTransactionHistory> customerwalletTransactionHistories { get; set; }
        public DbSet<GoChauffeurWebApi.Models.InsurenceTaxandPriceoutoffcity> InsurenceTaxandPriceoutoffcity { get; set; } = default!;

        public DbSet<BankList> BankList { get; set; }
        public DbSet<Outstationinsurence> Outstationinsurence { get; set; }

        public DbSet<WithdrawRequest> WithdrawRequest { get; set; }
        

             public DbSet<Withdrawamountvalue> Withdrawamountvalue { get; set; }
        

    }
}
