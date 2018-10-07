using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSensorServerAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dictionary_actuator_types",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionary_actuator_types", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "dictionary_genders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionary_genders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "dictionary_node_types",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionary_node_types", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "dictionary_roles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionary_roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "dictionary_sensor_types",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionary_sensor_types", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "nodes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    LoginName = table.Column<string>(nullable: true),
                    LoginPassword = table.Column<string>(nullable: true),
                    NodeType = table.Column<int>(nullable: true),
                    RegistredProperties = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    GatewayAddress = table.Column<string>(nullable: true),
                    IsOn = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nodes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "system_data",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RpiUrl = table.Column<string>(nullable: true),
                    RpiLogin = table.Column<string>(nullable: true),
                    RpiPassword = table.Column<string>(nullable: true),
                    AppVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_data", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    Birthdate = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    Role = table.Column<int>(nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    LastValidLogin = table.Column<DateTime>(nullable: true),
                    LastInvalidLogin = table.Column<DateTime>(nullable: true),
                    JoinDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "streaming_devices",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    ConnectionString = table.Column<string>(nullable: false),
                    CreatorID = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streaming_devices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_streaming_devices_users_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_streaming_devices_CreatorID",
                table: "streaming_devices",
                column: "CreatorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dictionary_actuator_types");

            migrationBuilder.DropTable(
                name: "dictionary_genders");

            migrationBuilder.DropTable(
                name: "dictionary_node_types");

            migrationBuilder.DropTable(
                name: "dictionary_roles");

            migrationBuilder.DropTable(
                name: "dictionary_sensor_types");

            migrationBuilder.DropTable(
                name: "nodes");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropTable(
                name: "streaming_devices");

            migrationBuilder.DropTable(
                name: "system_data");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
