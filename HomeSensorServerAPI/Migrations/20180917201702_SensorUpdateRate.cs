using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSensorServerAPI.Migrations
{
    public partial class SensorUpdateRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SensorUpdateRateSec",
                table: "nodes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SensorUpdateRateSec",
                table: "nodes");
        }
    }
}
