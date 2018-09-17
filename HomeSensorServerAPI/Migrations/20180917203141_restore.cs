using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSensorServerAPI.Migrations
{
    public partial class restore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sensors_nodes_Identifier",
                table: "sensors");

            migrationBuilder.DropIndex(
                name: "IX_sensors_Identifier",
                table: "sensors");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_nodes_Identifier",
                table: "nodes");

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "sensors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "nodes",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "sensors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "nodes",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_nodes_Identifier",
                table: "nodes",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_sensors_Identifier",
                table: "sensors",
                column: "Identifier");

            migrationBuilder.AddForeignKey(
                name: "FK_sensors_nodes_Identifier",
                table: "sensors",
                column: "Identifier",
                principalTable: "nodes",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
