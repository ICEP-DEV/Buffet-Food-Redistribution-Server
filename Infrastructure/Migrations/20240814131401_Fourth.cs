using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    DonorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonorPhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonorAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.DonorId);
                });

            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateCooked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequested = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Regno = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientPhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodDonations",
                columns: table => new
                {
                    DonationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateCooked = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodDonations", x => x.DonationId);
                    table.ForeignKey(
                        name: "FK_FoodDonations_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "DonorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodDonations_FoodItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "FoodItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonationRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonationId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isCollected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_DonationRequests_FoodDonations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "FoodDonations",
                        principalColumn: "DonationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonationRequests_Recipients_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Recipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "AdminId", "Email", "Name", "Password", "Phone" },
                values: new object[] { 1, "admin@gmail.com", "admin", "$2a$11$3PyWrlP14zc9i7WYKErxiesxDnJKoLehHK/caePPp8dg5pbjHmPwi", "0126547380" });

            migrationBuilder.InsertData(
                table: "Donors",
                columns: new[] { "DonorId", "DonorAddress", "DonorEmail", "DonorName", "DonorPhoneNum", "Password" },
                values: new object[,]
                {
                    { 1, "222 Servaas street", "kamomohapi17@gmail.com", "Kamohelo", "0123456789", "$2a$11$3PyWrlP14zc9i7WYKErxiesxDnJKoLehHK/caePPp8dg5pbjHmPwi" },
                    { 2, "848 Motsi street", "tshepo@gmail.com", "Tshepo", "0712563738", "$2a$11$3PyWrlP14zc9i7WYKErxiesxDnJKoLehHK/caePPp8dg5pbjHmPwi" },
                    { 3, "101 Linden street", "thabo@gmail.com", "Thabo", "0812435627", "$2a$11$3PyWrlP14zc9i7WYKErxiesxDnJKoLehHK/caePPp8dg5pbjHmPwi" }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "OrganizationId", "OrganizationName", "Regno" },
                values: new object[,]
                {
                    { 1, "Sizwe Old Age Home", 112233 },
                    { 2, "Ubuntu Old Age Home", 223344 },
                    { 3, "Kids of tomorrow Children's Home", 334455 },
                    { 4, "Little lamb Children's Home", 445566 }
                });

            migrationBuilder.InsertData(
                table: "Recipients",
                columns: new[] { "Id", "Password", "RecipientAddress", "RecipientEmail", "RecipientName", "RecipientPhoneNum" },
                values: new object[,]
                {
                    { 1, "$2a$11$3PyWrlP14zc9i7WYKErxiesxDnJKoLehHK/caePPp8dg5pbjHmPwi", "191 Frederick street", "kamomohapi17@gmail.com", "Lesedi", "0193377233" },
                    { 2, "$2a$11$3PyWrlP14zc9i7WYKErxiesxDnJKoLehHK/caePPp8dg5pbjHmPwi", "1921 Maltzan street", "karabo@gmail.com", "Karabo", "0135537733" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_DonationId",
                table: "DonationRequests",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_RecipientId",
                table: "DonationRequests",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodDonations_DonorId",
                table: "FoodDonations",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodDonations_ItemId",
                table: "FoodDonations",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "DonationRequests");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "FoodDonations");

            migrationBuilder.DropTable(
                name: "Recipients");

            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.DropTable(
                name: "FoodItems");
        }
    }
}
