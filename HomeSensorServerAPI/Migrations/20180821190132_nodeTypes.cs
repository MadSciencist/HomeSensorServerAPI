using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSensorServerAPI.Migrations
{
    public partial class nodeTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExactType",
                table: "nodes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "nodes");

            migrationBuilder.AddColumn<int>(
                name: "ActuatorType",
                table: "nodes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NodeType",
                table: "nodes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SensorType",
                table: "nodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActuatorType",
                table: "nodes");

            migrationBuilder.DropColumn(
                name: "NodeType",
                table: "nodes");

            migrationBuilder.DropColumn(
                name: "SensorType",
                table: "nodes");

            migrationBuilder.AddColumn<string>(
                name: "ExactType",
                table: "nodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "nodes",
                nullable: true);
        }
    }
}
