using Microsoft.EntityFrameworkCore.Migrations;

namespace _80sModelCollector.Data.Migrations
{
    public partial class ExtraFieldsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Stock",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                schema: "dbo",
                table: "Stock",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                schema: "dbo",
                table: "Stock",
                maxLength: 8,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RemainingStock",
                schema: "dbo",
                table: "Stock",
                maxLength: 8,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "Picture",
                schema: "dbo",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "dbo",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "RemainingStock",
                schema: "dbo",
                table: "Stock");
        }
    }
}
