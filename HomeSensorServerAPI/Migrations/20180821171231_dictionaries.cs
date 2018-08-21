using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSensorServerAPI.Migrations
{
    public partial class dictionaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dictionary",
                table: "dictionary_roles");

            migrationBuilder.DropColumn(
                name: "Dictionary",
                table: "dictionary_genders");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "dictionary_roles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Key",
                table: "dictionary_roles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "dictionary_genders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Key",
                table: "dictionary_genders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "dictionary_actuator_types",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionary_actuator_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dictionary_node_types",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionary_node_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dictionary_sensor_types",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionary_sensor_types", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dictionary_actuator_types");

            migrationBuilder.DropTable(
                name: "dictionary_node_types");

            migrationBuilder.DropTable(
                name: "dictionary_sensor_types");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "dictionary_roles");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "dictionary_genders");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "dictionary_roles",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dictionary",
                table: "dictionary_roles",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "dictionary_genders",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dictionary",
                table: "dictionary_genders",
                nullable: true);
        }
    }
}
