using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$givJdsRJ7Jy2SyqWwpj.xub7wkAB6soeGsSMz1d9tYe0mFI0x6Pvu");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$givJdsRJ7Jy2SyqWwpj.xub7wkAB6soeGsSMz1d9tYe0mFI0x6Pvu");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$givJdsRJ7Jy2SyqWwpj.xub7wkAB6soeGsSMz1d9tYe0mFI0x6Pvu");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$givJdsRJ7Jy2SyqWwpj.xub7wkAB6soeGsSMz1d9tYe0mFI0x6Pvu");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$givJdsRJ7Jy2SyqWwpj.xub7wkAB6soeGsSMz1d9tYe0mFI0x6Pvu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HjK/mbsu19ZlgRDPoBjXr.NhmC5tcHtCaM0YJiZEDYtQDQtnKaksG");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$HjK/mbsu19ZlgRDPoBjXr.NhmC5tcHtCaM0YJiZEDYtQDQtnKaksG");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$HjK/mbsu19ZlgRDPoBjXr.NhmC5tcHtCaM0YJiZEDYtQDQtnKaksG");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HjK/mbsu19ZlgRDPoBjXr.NhmC5tcHtCaM0YJiZEDYtQDQtnKaksG");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$HjK/mbsu19ZlgRDPoBjXr.NhmC5tcHtCaM0YJiZEDYtQDQtnKaksG");
        }
    }
}
