using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSensorServerAPI.Migrations
{
    public partial class streamDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "streaming_devices",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionString",
                table: "streaming_devices",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "streaming_devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "streaming_devices");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "streaming_devices",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionString",
                table: "streaming_devices",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
