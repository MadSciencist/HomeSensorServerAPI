using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSensorServerAPI.Migrations
{
    public partial class RoleToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "users",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "users",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
