using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class asdfsadfasdfasdfasdfsdfasdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RolesId = table.Column<int>(type: "int", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "AdvanceAmounts",
                columns: table => new
                {
                    AdvanceAmountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Advance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TripTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvanceAmounts", x => x.AdvanceAmountId);
                });

            migrationBuilder.CreateTable(
                name: "AerialDistancePrices",
                columns: table => new
                {
                    AerialDistancePriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AerialDistancePriceperKilometer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AerialDistancePrices", x => x.AerialDistancePriceId);
                });

            migrationBuilder.CreateTable(
                name: "Anonymoustripcharges",
                columns: table => new
                {
                    AnonymoustripchargesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TriptypeId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anonymoustripcharges", x => x.AnonymoustripchargesId);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Cupons",
                columns: table => new
                {
                    CuponsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuponCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuponName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Createdby = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Flag = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupons", x => x.CuponsId);
                });

            migrationBuilder.CreateTable(
                name: "CuponsHistories",
                columns: table => new
                {
                    CuponsHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CouponId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuponsHistories", x => x.CuponsHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTicketReasons",
                columns: table => new
                {
                    CustomerTicketReasonsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTicketReasonsName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Createddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Modifieddate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTicketReasons", x => x.CustomerTicketReasonsId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTickets",
                columns: table => new
                {
                    CustomerTicketsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TicketId = table.Column<int>(type: "int", nullable: true),
                    Reasons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTicketClosed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTickets", x => x.CustomerTicketsId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerWalletFines",
                columns: table => new
                {
                    CustomerWalletFineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerWalletFines", x => x.CustomerWalletFineId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerWallets",
                columns: table => new
                {
                    CustomerWalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    WalletBalance = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerWallets", x => x.CustomerWalletId);
                });

            migrationBuilder.CreateTable(
                name: "Driver_Joining_Fees",
                columns: table => new
                {
                    Driver_Joining_FeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fee = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver_Joining_Fees", x => x.Driver_Joining_FeeId);
                });

            migrationBuilder.CreateTable(
                name: "DriverFines",
                columns: table => new
                {
                    DriverFinesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverFinesName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverFines", x => x.DriverFinesId);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverRecID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AltPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermenentAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermenentState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermenentCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermenentCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermenentPostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharNumberFrontImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharNumberBackImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PANNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PANNumberFrontImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PANNumberBackImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Licence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenceFrontImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenceBackImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VehicleTypeIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransmissionTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Experiance = table.Column<int>(type: "int", nullable: true),
                    RatingId = table.Column<int>(type: "int", nullable: true),
                    CurrentLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationStatusIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoOfTripsCount = table.Column<int>(type: "int", nullable: true),
                    ReferedBy = table.Column<int>(type: "int", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    IsSubscribed = table.Column<bool>(type: "bit", nullable: true),
                    SubscriptionId = table.Column<int>(type: "int", nullable: true),
                    SubscribedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Licencevalidddate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IFSCCODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountHolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDriverActive = table.Column<bool>(type: "bit", nullable: true),
                    IsPaymentDone = table.Column<bool>(type: "bit", nullable: true),
                    TodaysEarning = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TodaysLogInHrs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    reasonforoffline = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverId);
                });

            migrationBuilder.CreateTable(
                name: "Driversubscriptions",
                columns: table => new
                {
                    DriversubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    SubscriptionId = table.Column<int>(type: "int", nullable: true),
                    Expirydate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driversubscriptions", x => x.DriversubscriptionId);
                });

            migrationBuilder.CreateTable(
                name: "Driversurroundingtrips",
                columns: table => new
                {
                    DriversurroundingtripsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Distance = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driversurroundingtrips", x => x.DriversurroundingtripsId);
                });

            migrationBuilder.CreateTable(
                name: "DriverTickets",
                columns: table => new
                {
                    DriverTicketsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    TicketId = table.Column<int>(type: "int", nullable: true),
                    Reasons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTicketClosed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverTickets", x => x.DriverTicketsId);
                });

            migrationBuilder.CreateTable(
                name: "DriverTicketsReasons",
                columns: table => new
                {
                    DriverTicketsReasonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverTicketsReasonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Createddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Modifieddate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverTicketsReasons", x => x.DriverTicketsReasonId);
                });

            migrationBuilder.CreateTable(
                name: "DriverTrackings",
                columns: table => new
                {
                    DriverTrackingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    TripId = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverTrackings", x => x.DriverTrackingId);
                });

            migrationBuilder.CreateTable(
                name: "DriverTransactions",
                columns: table => new
                {
                    DriverTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    transactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsJoiningFee = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverTransactions", x => x.DriverTransactionId);
                });

            migrationBuilder.CreateTable(
                name: "Driverwallets",
                columns: table => new
                {
                    DriverwalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    WalletBalance = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driverwallets", x => x.DriverwalletId);
                });

            migrationBuilder.CreateTable(
                name: "DriverWalletTransactionHistories",
                columns: table => new
                {
                    DriverWalletTransactionHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    WalletAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverWalletTransactionHistories", x => x.DriverWalletTransactionHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    FavouriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FavouriteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    place_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mapUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    FavouriteTypesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.FavouriteId);
                });

            migrationBuilder.CreateTable(
                name: "FavouriteTypes",
                columns: table => new
                {
                    FavouriteTypesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FavouriteTypesName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteTypes", x => x.FavouriteTypesId);
                });

            migrationBuilder.CreateTable(
                name: "FlexiDatesLists",
                columns: table => new
                {
                    FlexiDatesListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlexiId = table.Column<int>(type: "int", nullable: true),
                    IsTripCompByDriver = table.Column<bool>(type: "bit", nullable: true),
                    ActualHours = table.Column<int>(type: "int", nullable: true),
                    ActualPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DriversPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrlsList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualTaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    endtime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CuponId = table.Column<int>(type: "int", nullable: true),
                    ActualCuponPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsTripStarted = table.Column<bool>(type: "bit", nullable: true),
                    IsDriverArrival = table.Column<bool>(type: "bit", nullable: true),
                    DriverMeansOfTransport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlexiDatesLists", x => x.FlexiDatesListId);
                });

            migrationBuilder.CreateTable(
                name: "Flexis",
                columns: table => new
                {
                    FlexiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickupLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedHours = table.Column<int>(type: "int", nullable: true),
                    PickUpTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: true),
                    TransmissionId = table.Column<int>(type: "int", nullable: true),
                    IsDriverAssigned = table.Column<bool>(type: "bit", nullable: true),
                    EstimatedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EstimatedTaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EstimatedCuponPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    IsAdvancePaid = table.Column<bool>(type: "bit", nullable: true),
                    AdvancePaid = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NoofDays = table.Column<int>(type: "int", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    FlexiSelectedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DriverMeansOfTransport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickUPMapURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flexis", x => x.FlexiId);
                });

            migrationBuilder.CreateTable(
                name: "Hours",
                columns: table => new
                {
                    HoursId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoursName = table.Column<int>(type: "int", nullable: true),
                    TripTypeId = table.Column<int>(type: "int", nullable: false),
                    Charges = table.Column<int>(type: "int", nullable: true),
                    NightCharges = table.Column<int>(type: "int", nullable: true),
                    IsinHours = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hours", x => x.HoursId);
                });

            migrationBuilder.CreateTable(
                name: "IgnoredTrips",
                columns: table => new
                {
                    IgnoredTripsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    TripId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoredTrips", x => x.IgnoredTripsID);
                });

            migrationBuilder.CreateTable(
                name: "IgnoreFlexiTrips",
                columns: table => new
                {
                    IgnoreFlexiTripsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlexiId = table.Column<int>(type: "int", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    IgnoretripresonsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoreFlexiTrips", x => x.IgnoreFlexiTripsId);
                });

            migrationBuilder.CreateTable(
                name: "IgnoreMontlyTrips",
                columns: table => new
                {
                    IgnoreMontlyTripsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthlyId = table.Column<int>(type: "int", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    IgnoretripresonsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoreMontlyTrips", x => x.IgnoreMontlyTripsId);
                });

            migrationBuilder.CreateTable(
                name: "ignoretripresons",
                columns: table => new
                {
                    IgnoretripresonsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IgnoretripresonsName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IgnoretripresonsDescrption = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ignoretripresons", x => x.IgnoretripresonsId);
                });

            migrationBuilder.CreateTable(
                name: "IgnoreValletTrips",
                columns: table => new
                {
                    IgnoreValletTripsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValetParkingId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    IgnoretripresonsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoreValletTrips", x => x.IgnoreValletTripsId);
                });

            migrationBuilder.CreateTable(
                name: "InsurenceTaxandPrice",
                columns: table => new
                {
                    InsurenceTaxandPriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurenceTaxandPrice", x => x.InsurenceTaxandPriceId);
                });

            migrationBuilder.CreateTable(
                name: "Monthlies",
                columns: table => new
                {
                    MonthlyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripVarientId = table.Column<int>(type: "int", nullable: true),
                    PickUpLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickupMapURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickUpLocationCoordinates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoofDays = table.Column<int>(type: "int", nullable: true),
                    SelectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pickuptime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: true),
                    TransmissionId = table.Column<int>(type: "int", nullable: true),
                    EstimatedHours = table.Column<int>(type: "int", nullable: false),
                    Estimatedprice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsAdvancedPayment = table.Column<bool>(type: "bit", nullable: true),
                    IsPermanent = table.Column<bool>(type: "bit", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    DriverMeansOfTransport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monthlies", x => x.MonthlyId);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyDateLists",
                columns: table => new
                {
                    MonthlyDateListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MonthlyId = table.Column<int>(type: "int", nullable: true),
                    IsTripCompByDriver = table.Column<bool>(type: "bit", nullable: true),
                    ActualHours = table.Column<int>(type: "int", nullable: true),
                    ImageUrlsList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DriversPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualTaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    endtime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CuponId = table.Column<int>(type: "int", nullable: true),
                    ActualCuponPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsTripStarted = table.Column<bool>(type: "bit", nullable: true),
                    IsDriverArrival = table.Column<bool>(type: "bit", nullable: true),
                    DriverMeansOfTransport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyDateLists", x => x.MonthlyDateListId);
                });

            migrationBuilder.CreateTable(
                name: "NightCharges",
                columns: table => new
                {
                    NightChargesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Charges = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NightCharges", x => x.NightChargesId);
                });

            migrationBuilder.CreateTable(
                name: "OutstationRoundtriphours",
                columns: table => new
                {
                    HoursId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoursName = table.Column<int>(type: "int", nullable: true),
                    TripTypeId = table.Column<int>(type: "int", nullable: false),
                    Charges = table.Column<int>(type: "int", nullable: true),
                    IsinHours = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutstationRoundtriphours", x => x.HoursId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatus",
                columns: table => new
                {
                    PaymentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatus", x => x.PaymentStatusId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.PaymentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingsId);
                });

            migrationBuilder.CreateTable(
                name: "Referals",
                columns: table => new
                {
                    ReferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Isused = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referals", x => x.ReferId);
                });

            migrationBuilder.CreateTable(
                name: "RequiredValetstaffs",
                columns: table => new
                {
                    RequiredValetstaffsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfDrivers = table.Column<int>(type: "int", nullable: true),
                    NumberOfSupervisors = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequiredValetstaffs", x => x.RequiredValetstaffsId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolesName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolesId);
                });

            migrationBuilder.CreateTable(
                name: "ServicePlaces",
                columns: table => new
                {
                    ServicePlacesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PinCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePlaces", x => x.ServicePlacesId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SubStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubEndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    SubPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SubBenefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDriverSub = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    TaxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.TaxId);
                });

            migrationBuilder.CreateTable(
                name: "TransmissionTypes",
                columns: table => new
                {
                    TransmissionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransmissionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: true),
                    icon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransmissionTypes", x => x.TransmissionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "TripCancellationReasons",
                columns: table => new
                {
                    TripCancellationReasonsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripCancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripCancellationReasons", x => x.TripCancellationReasonsId);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequstedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FromLocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToLocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedDistance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DistanceTravelled = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AerialDistance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PickUPMapURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DropUPMapURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTimeScheduled = table.Column<bool>(type: "bit", nullable: true),
                    TripTypeId = table.Column<int>(type: "int", nullable: true),
                    TripvarientId = table.Column<int>(type: "int", nullable: true),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: true),
                    TransmissionTypeId = table.Column<int>(type: "int", nullable: true),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    IsSelfBook = table.Column<bool>(type: "bit", nullable: true),
                    BookedForName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookedForNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSecuredTrip = table.Column<bool>(type: "bit", nullable: true),
                    NoOfHoursActual = table.Column<int>(type: "int", nullable: true),
                    NoOfHoursSelected = table.Column<int>(type: "int", nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: true),
                    EstimatedPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TripStatus = table.Column<int>(type: "int", nullable: true),
                    IsTripStarted = table.Column<bool>(type: "bit", nullable: true),
                    ImageUrlsList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTripCompByDriver = table.Column<bool>(type: "bit", nullable: true),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: true),
                    CuponId = table.Column<int>(type: "int", nullable: true),
                    TaxIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalTripValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriversPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TripRating = table.Column<int>(type: "int", nullable: true),
                    DriverRating = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsdriverArrived = table.Column<bool>(type: "bit", nullable: true),
                    IsProcessing = table.Column<bool>(type: "bit", nullable: true),
                    IsReserved = table.Column<bool>(type: "bit", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: true),
                    DriverMeansOfTransport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripId);
                });

            migrationBuilder.CreateTable(
                name: "TripStatuses",
                columns: table => new
                {
                    TripStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripStatuses", x => x.TripStatusId);
                });

            migrationBuilder.CreateTable(
                name: "TripTaxes",
                columns: table => new
                {
                    TripTaxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripTaxes", x => x.TripTaxId);
                });

            migrationBuilder.CreateTable(
                name: "TripTypes",
                columns: table => new
                {
                    TripTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TripDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Endtime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripTypes", x => x.TripTypeId);
                });

            migrationBuilder.CreateTable(
                name: "TripVariants",
                columns: table => new
                {
                    TripVariantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripVariantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TriptypeId = table.Column<int>(type: "int", nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KilometerLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NightCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChargesperMinute = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PricePerKilometers = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsTripOneway = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripVariants", x => x.TripVariantId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValetParkingHourlyPrices",
                columns: table => new
                {
                    ValetParkingHourlyPricesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverPrice = table.Column<int>(type: "int", nullable: true),
                    SuperVisorPrice = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValetParkingHourlyPrices", x => x.ValetParkingHourlyPricesId);
                });

            migrationBuilder.CreateTable(
                name: "ValetParkingPricing",
                columns: table => new
                {
                    ValetParkingPricingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupervisorCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DriverCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BaseHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HourlyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValetParkingPricing", x => x.ValetParkingPricingId);
                });

            migrationBuilder.CreateTable(
                name: "ValetParkings",
                columns: table => new
                {
                    ValetParkingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    LocationCoordinates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberOfDriversRequired = table.Column<int>(type: "int", nullable: true),
                    NumberOfSupervisors = table.Column<int>(type: "int", nullable: true),
                    NumberOfHours = table.Column<int>(type: "int", nullable: true),
                    GSTPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GSTPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdvanceAMount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsAdvancePaid = table.Column<bool>(type: "bit", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    DriverMeansOfTransport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValetParkings", x => x.ValetParkingId);
                });

            migrationBuilder.CreateTable(
                name: "ValetparkingStaff",
                columns: table => new
                {
                    ValetparkingStaffId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuperVisiorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValetParkingId = table.Column<int>(type: "int", nullable: true),
                    Isregistrationclosed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValetparkingStaff", x => x.ValetparkingStaffId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.VehicleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "VerificationStatuses",
                columns: table => new
                {
                    VerificationStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VerificationStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationStatuses", x => x.VerificationStatusId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AdvanceAmounts");

            migrationBuilder.DropTable(
                name: "AerialDistancePrices");

            migrationBuilder.DropTable(
                name: "Anonymoustripcharges");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Cupons");

            migrationBuilder.DropTable(
                name: "CuponsHistories");

            migrationBuilder.DropTable(
                name: "CustomerTicketReasons");

            migrationBuilder.DropTable(
                name: "CustomerTickets");

            migrationBuilder.DropTable(
                name: "CustomerWalletFines");

            migrationBuilder.DropTable(
                name: "CustomerWallets");

            migrationBuilder.DropTable(
                name: "Driver_Joining_Fees");

            migrationBuilder.DropTable(
                name: "DriverFines");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Driversubscriptions");

            migrationBuilder.DropTable(
                name: "Driversurroundingtrips");

            migrationBuilder.DropTable(
                name: "DriverTickets");

            migrationBuilder.DropTable(
                name: "DriverTicketsReasons");

            migrationBuilder.DropTable(
                name: "DriverTrackings");

            migrationBuilder.DropTable(
                name: "DriverTransactions");

            migrationBuilder.DropTable(
                name: "Driverwallets");

            migrationBuilder.DropTable(
                name: "DriverWalletTransactionHistories");

            migrationBuilder.DropTable(
                name: "Favourites");

            migrationBuilder.DropTable(
                name: "FavouriteTypes");

            migrationBuilder.DropTable(
                name: "FlexiDatesLists");

            migrationBuilder.DropTable(
                name: "Flexis");

            migrationBuilder.DropTable(
                name: "Hours");

            migrationBuilder.DropTable(
                name: "IgnoredTrips");

            migrationBuilder.DropTable(
                name: "IgnoreFlexiTrips");

            migrationBuilder.DropTable(
                name: "IgnoreMontlyTrips");

            migrationBuilder.DropTable(
                name: "ignoretripresons");

            migrationBuilder.DropTable(
                name: "IgnoreValletTrips");

            migrationBuilder.DropTable(
                name: "InsurenceTaxandPrice");

            migrationBuilder.DropTable(
                name: "Monthlies");

            migrationBuilder.DropTable(
                name: "MonthlyDateLists");

            migrationBuilder.DropTable(
                name: "NightCharges");

            migrationBuilder.DropTable(
                name: "OutstationRoundtriphours");

            migrationBuilder.DropTable(
                name: "PaymentStatus");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Referals");

            migrationBuilder.DropTable(
                name: "RequiredValetstaffs");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ServicePlaces");

            migrationBuilder.DropTable(
                name: "SubscriptionHistories");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "TransmissionTypes");

            migrationBuilder.DropTable(
                name: "TripCancellationReasons");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "TripStatuses");

            migrationBuilder.DropTable(
                name: "TripTaxes");

            migrationBuilder.DropTable(
                name: "TripTypes");

            migrationBuilder.DropTable(
                name: "TripVariants");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ValetParkingHourlyPrices");

            migrationBuilder.DropTable(
                name: "ValetParkingPricing");

            migrationBuilder.DropTable(
                name: "ValetParkings");

            migrationBuilder.DropTable(
                name: "ValetparkingStaff");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "VerificationStatuses");
        }
    }
}
