using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSensorServerAPI.Migrations
{
    public partial class streaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StreamingDeviceId",
                table: "users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "streaming_devices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ConnectionString = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streaming_devices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_StreamingDeviceId",
                table: "users",
                column: "StreamingDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_streaming_devices_StreamingDeviceId",
                table: "users",
                column: "StreamingDeviceId",
                principalTable: "streaming_devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_streaming_devices_StreamingDeviceId",
                table: "users");

            migrationBuilder.DropTable(
                name: "streaming_devices");

            migrationBuilder.DropIndex(
                name: "IX_users_StreamingDeviceId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "StreamingDeviceId",
                table: "users");
        }
    }
}
