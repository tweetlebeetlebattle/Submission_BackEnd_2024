using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataFetchingLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusLog = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataFetchingLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    MediaId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TextUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_Media_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TrainingLog",
                columns: table => new
                {
                    TrainingLogId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ExerciseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetWorkingWeight = table.Column<float>(type: "real", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    TargetSetCount = table.Column<int>(type: "int", nullable: false),
                    TargetRepsCount = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingLog", x => x.TrainingLogId);
                    table.ForeignKey(
                        name: "FK_TrainingLog_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TrainingLog_TrainingUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TrainingUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UniversalReading",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Reading = table.Column<float>(type: "real", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversalReading", x => x.ApplicationUserId);
                    table.ForeignKey(
                        name: "FK_UniversalReading_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UniversalReading_TrainingUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TrainingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DailyGifReading",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyWaveMax = table.Column<float>(type: "real", nullable: true),
                    DailyWaveMin = table.Column<float>(type: "real", nullable: true),
                    DailyWaveAvg = table.Column<float>(type: "real", nullable: true),
                    WaveUnitId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyGifReading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyGifReading_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DailyGifReading_Units_WaveUnitId",
                        column: x => x.WaveUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "DailyGlassStormReading",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyWaveMax = table.Column<float>(type: "real", nullable: true),
                    DailyWaveMin = table.Column<float>(type: "real", nullable: true),
                    DailyWaveAvg = table.Column<float>(type: "real", nullable: true),
                    WaveUnitId = table.Column<int>(type: "int", nullable: true),
                    DailyTempMax = table.Column<float>(type: "real", nullable: true),
                    DailyTempMin = table.Column<float>(type: "real", nullable: true),
                    DailyTempAvg = table.Column<float>(type: "real", nullable: true),
                    TempUnitId = table.Column<int>(type: "int", nullable: true),
                    DailyWindMax = table.Column<float>(type: "real", nullable: true),
                    DailyWindMin = table.Column<float>(type: "real", nullable: true),
                    DailyWindAvg = table.Column<float>(type: "real", nullable: true),
                    WindUnitId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyGlassStormReading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyGlassStormReading_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DailyGlassStormReading_Units_TempUnitId",
                        column: x => x.TempUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_DailyGlassStormReading_Units_WaveUnitId",
                        column: x => x.WaveUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_DailyGlassStormReading_Units_WindUnitId",
                        column: x => x.WindUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "DailyHTMLReading",
                columns: table => new
                {
                    DailyHTMLReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyWaveMax = table.Column<float>(type: "real", nullable: true),
                    DailyWaveMin = table.Column<float>(type: "real", nullable: true),
                    DailyWaveAvg = table.Column<float>(type: "real", nullable: true),
                    WaveUnitId = table.Column<int>(type: "int", nullable: true),
                    DailyTempMax = table.Column<float>(type: "real", nullable: true),
                    DailyTempMin = table.Column<float>(type: "real", nullable: true),
                    DailyTempAvg = table.Column<float>(type: "real", nullable: true),
                    TempUnitId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyHTMLReading", x => x.DailyHTMLReadingId);
                    table.ForeignKey(
                        name: "FK_DailyHTMLReading_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DailyHTMLReading_Units_TempUnitId",
                        column: x => x.TempUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_DailyHTMLReading_Units_WaveUnitId",
                        column: x => x.WaveUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "GifData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WaveRead = table.Column<float>(type: "real", nullable: true),
                    WaveUnitId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GifData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GifData_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GifData_Units_WaveUnitId",
                        column: x => x.WaveUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "GlassStormIoData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WaveRead = table.Column<float>(type: "real", nullable: true),
                    WaveUnitId = table.Column<int>(type: "int", nullable: true),
                    TempRead = table.Column<float>(type: "real", nullable: true),
                    TempUnitId = table.Column<int>(type: "int", nullable: true),
                    WindSpeedIndex = table.Column<float>(type: "real", nullable: true),
                    WindSpeedUnitId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Locations = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlassStormIoData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlassStormIoData_Locations_Locations",
                        column: x => x.Locations,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GlassStormIoData_Units_TempUnitId",
                        column: x => x.TempUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_GlassStormIoData_Units_WaveUnitId",
                        column: x => x.WaveUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_GlassStormIoData_Units_WindSpeedUnitId",
                        column: x => x.WindSpeedUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "HTMLData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WaveRead = table.Column<float>(type: "real", nullable: true),
                    WaveUnitId = table.Column<int>(type: "int", nullable: true),
                    TempRead = table.Column<float>(type: "real", nullable: true),
                    TempUnitId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HTMLData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HTMLData_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HTMLData_Units_TempUnitId",
                        column: x => x.TempUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_HTMLData_Units_WaveUnitId",
                        column: x => x.WaveUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    WaveRead = table.Column<float>(type: "real", nullable: true),
                    WaveUnitId = table.Column<int>(type: "int", nullable: true),
                    TempRead = table.Column<float>(type: "real", nullable: true),
                    TempUnitId = table.Column<int>(type: "int", nullable: true),
                    WindSpeedIndex = table.Column<float>(type: "real", nullable: true),
                    WindSpeedUnitId = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MediaId = table.Column<string>(type: "nvarchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.ApplicationUserId);
                    table.ForeignKey(
                        name: "FK_Feedback_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Feedback_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Feedback_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId");
                    table.ForeignKey(
                        name: "FK_Feedback_Units_TempUnitId",
                        column: x => x.TempUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_Feedback_Units_WaveUnitId",
                        column: x => x.WaveUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_Feedback_Units_WindSpeedUnitId",
                        column: x => x.WindSpeedUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "SeaBlog",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MediaId = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    ApprovedStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBlog", x => x.BlogId);
                    table.ForeignKey(
                        name: "FK_SeaBlog_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SeaBlog_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TrainingBlog",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MediaId = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    ApprovedStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingBlog", x => x.BlogId);
                    table.ForeignKey(
                        name: "FK_TrainingBlog_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TrainingBlog_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSetsLog",
                columns: table => new
                {
                    WorkSetId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TrainingLogId = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    DoneSetCount = table.Column<int>(type: "int", nullable: false),
                    DoneRepCount = table.Column<int>(type: "int", nullable: false),
                    MediaId = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSetsLog", x => x.WorkSetId);
                    table.ForeignKey(
                        name: "FK_TrainingSetsLog_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId");
                    table.ForeignKey(
                        name: "FK_TrainingSetsLog_TrainingLog_TrainingLogId",
                        column: x => x.TrainingLogId,
                        principalTable: "TrainingLog",
                        principalColumn: "TrainingLogId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SeaComment",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentBlogId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MediaId = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    ApprovedStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaComment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_SeaComment_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SeaComment_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SeaComment_SeaBlog_ParentBlogId",
                        column: x => x.ParentBlogId,
                        principalTable: "SeaBlog",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TrainingComment",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentBlogId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MediaId = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    ApprovedStatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingComment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_TrainingComment_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TrainingComment_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TrainingComment_TrainingBlog_ParentBlogId",
                        column: x => x.ParentBlogId,
                        principalTable: "TrainingBlog",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DailyGifReading_LocationId",
                table: "DailyGifReading",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyGifReading_WaveUnitId",
                table: "DailyGifReading",
                column: "WaveUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyGlassStormReading_LocationId",
                table: "DailyGlassStormReading",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyGlassStormReading_TempUnitId",
                table: "DailyGlassStormReading",
                column: "TempUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyGlassStormReading_WaveUnitId",
                table: "DailyGlassStormReading",
                column: "WaveUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyGlassStormReading_WindUnitId",
                table: "DailyGlassStormReading",
                column: "WindUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyHTMLReading_LocationId",
                table: "DailyHTMLReading",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyHTMLReading_TempUnitId",
                table: "DailyHTMLReading",
                column: "TempUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyHTMLReading_WaveUnitId",
                table: "DailyHTMLReading",
                column: "WaveUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_LocationId",
                table: "Feedback",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_MediaId",
                table: "Feedback",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_TempUnitId",
                table: "Feedback",
                column: "TempUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_WaveUnitId",
                table: "Feedback",
                column: "WaveUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_WindSpeedUnitId",
                table: "Feedback",
                column: "WindSpeedUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_GifData_LocationId",
                table: "GifData",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GifData_WaveUnitId",
                table: "GifData",
                column: "WaveUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_GlassStormIoData_Locations",
                table: "GlassStormIoData",
                column: "Locations");

            migrationBuilder.CreateIndex(
                name: "IX_GlassStormIoData_TempUnitId",
                table: "GlassStormIoData",
                column: "TempUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_GlassStormIoData_WaveUnitId",
                table: "GlassStormIoData",
                column: "WaveUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_GlassStormIoData_WindSpeedUnitId",
                table: "GlassStormIoData",
                column: "WindSpeedUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_HTMLData_LocationId",
                table: "HTMLData",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_HTMLData_TempUnitId",
                table: "HTMLData",
                column: "TempUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_HTMLData_WaveUnitId",
                table: "HTMLData",
                column: "WaveUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_ApplicationUserId",
                table: "Media",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaBlog_ApplicationUserId",
                table: "SeaBlog",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaBlog_MediaId",
                table: "SeaBlog",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaComment_ApplicationUserId",
                table: "SeaComment",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaComment_MediaId",
                table: "SeaComment",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaComment_ParentBlogId",
                table: "SeaComment",
                column: "ParentBlogId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingBlog_ApplicationUserId",
                table: "TrainingBlog",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingBlog_MediaId",
                table: "TrainingBlog",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingComment_ApplicationUserId",
                table: "TrainingComment",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingComment_MediaId",
                table: "TrainingComment",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingComment_ParentBlogId",
                table: "TrainingComment",
                column: "ParentBlogId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLog_ApplicationUserId",
                table: "TrainingLog",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLog_UnitId",
                table: "TrainingLog",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSetsLog_MediaId",
                table: "TrainingSetsLog",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSetsLog_TrainingLogId",
                table: "TrainingSetsLog",
                column: "TrainingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversalReading_UnitId",
                table: "UniversalReading",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DailyGifReading");

            migrationBuilder.DropTable(
                name: "DailyGlassStormReading");

            migrationBuilder.DropTable(
                name: "DailyHTMLReading");

            migrationBuilder.DropTable(
                name: "DataFetchingLogs");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "GifData");

            migrationBuilder.DropTable(
                name: "GlassStormIoData");

            migrationBuilder.DropTable(
                name: "HTMLData");

            migrationBuilder.DropTable(
                name: "SeaComment");

            migrationBuilder.DropTable(
                name: "TrainingComment");

            migrationBuilder.DropTable(
                name: "TrainingSetsLog");

            migrationBuilder.DropTable(
                name: "UniversalReading");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "SeaBlog");

            migrationBuilder.DropTable(
                name: "TrainingBlog");

            migrationBuilder.DropTable(
                name: "TrainingLog");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "TrainingUnits");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
