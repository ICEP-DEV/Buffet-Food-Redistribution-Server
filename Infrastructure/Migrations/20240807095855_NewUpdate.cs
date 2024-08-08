using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequested",
                table: "FoodItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$xwwivSlf1JnRxrTln2XQRevPjj/OI6YR4QIZfFn2kH/ERHDZQmJGK");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$xwwivSlf1JnRxrTln2XQRevPjj/OI6YR4QIZfFn2kH/ERHDZQmJGK");

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "DonorId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$xwwivSlf1JnRxrTln2XQRevPjj/OI6YR4QIZfFn2kH/ERHDZQmJGK");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$xwwivSlf1JnRxrTln2XQRevPjj/OI6YR4QIZfFn2kH/ERHDZQmJGK");

            migrationBuilder.UpdateData(
                table: "Recipients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$xwwivSlf1JnRxrTln2XQRevPjj/OI6YR4QIZfFn2kH/ERHDZQmJGK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequested",
                table: "FoodItems");

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
    }
}
