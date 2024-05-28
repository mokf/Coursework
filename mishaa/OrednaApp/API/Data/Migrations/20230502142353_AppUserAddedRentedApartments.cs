using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AppUserAddedRentedApartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentedByUserId",
                table: "Apartments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_RentedByUserId",
                table: "Apartments",
                column: "RentedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_Users_RentedByUserId",
                table: "Apartments",
                column: "RentedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_Users_RentedByUserId",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_RentedByUserId",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "RentedByUserId",
                table: "Apartments");
        }
    }
}
