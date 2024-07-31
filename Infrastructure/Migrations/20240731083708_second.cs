using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$EVwU9izLoLbZJi8j3VzcreFTipZrdd5DaXP.iuoWW..qtavSLKgYe");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$EVwU9izLoLbZJi8j3VzcreFTipZrdd5DaXP.iuoWW..qtavSLKgYe");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$EVwU9izLoLbZJi8j3VzcreFTipZrdd5DaXP.iuoWW..qtavSLKgYe");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$EVwU9izLoLbZJi8j3VzcreFTipZrdd5DaXP.iuoWW..qtavSLKgYe");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$EVwU9izLoLbZJi8j3VzcreFTipZrdd5DaXP.iuoWW..qtavSLKgYe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$sqnO9jFe6FlibZWOnsrfh.PbeJzGv4diES7GoWzY7HQk8H33MiGC.");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$sqnO9jFe6FlibZWOnsrfh.PbeJzGv4diES7GoWzY7HQk8H33MiGC.");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$sqnO9jFe6FlibZWOnsrfh.PbeJzGv4diES7GoWzY7HQk8H33MiGC.");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$sqnO9jFe6FlibZWOnsrfh.PbeJzGv4diES7GoWzY7HQk8H33MiGC.");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$sqnO9jFe6FlibZWOnsrfh.PbeJzGv4diES7GoWzY7HQk8H33MiGC.");
        }
    }
}
