using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalyProject.Migrations
{
    public partial class FixedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BodyTypes_Categories_CategoryId",
                table: "BodyTypes");

            migrationBuilder.DropIndex(
                name: "IX_BodyTypes_CategoryId",
                table: "BodyTypes");

            migrationBuilder.DropColumn(
                name: "FeautureId",
                table: "CarFeatures");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BodyTypes");

            migrationBuilder.AlterColumn<int>(
                name: "Luggage",
                table: "Cars",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Luggage",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FeautureId",
                table: "CarFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "BodyTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BodyTypes_CategoryId",
                table: "BodyTypes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BodyTypes_Categories_CategoryId",
                table: "BodyTypes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
