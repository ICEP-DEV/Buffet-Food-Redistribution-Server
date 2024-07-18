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
            migrationBuilder.DropForeignKey(
                name: "FK_FoodDonations_FoodItems_FoodItemId",
                table: "FoodDonations");

            migrationBuilder.DropIndex(
                name: "IX_FoodDonations_FoodItemId",
                table: "FoodDonations");

            migrationBuilder.DropColumn(
                name: "FoodItemId",
                table: "FoodDonations");

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

            migrationBuilder.CreateIndex(
                name: "IX_FoodDonations_ItemId",
                table: "FoodDonations",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodDonations_FoodItems_ItemId",
                table: "FoodDonations",
                column: "ItemId",
                principalTable: "FoodItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodDonations_FoodItems_ItemId",
                table: "FoodDonations");

            migrationBuilder.DropIndex(
                name: "IX_FoodDonations_ItemId",
                table: "FoodDonations");

            migrationBuilder.AddColumn<int>(
                name: "FoodItemId",
                table: "FoodDonations",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$o528ss7rFaHHJ1qHyVO0Je75W/3PTzB2eunrWDTEXgJNLre.MdfNK");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$o528ss7rFaHHJ1qHyVO0Je75W/3PTzB2eunrWDTEXgJNLre.MdfNK");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$o528ss7rFaHHJ1qHyVO0Je75W/3PTzB2eunrWDTEXgJNLre.MdfNK");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$o528ss7rFaHHJ1qHyVO0Je75W/3PTzB2eunrWDTEXgJNLre.MdfNK");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$o528ss7rFaHHJ1qHyVO0Je75W/3PTzB2eunrWDTEXgJNLre.MdfNK");

            migrationBuilder.CreateIndex(
                name: "IX_FoodDonations_FoodItemId",
                table: "FoodDonations",
                column: "FoodItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodDonations_FoodItems_FoodItemId",
                table: "FoodDonations",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "Id");
        }
    }
}
