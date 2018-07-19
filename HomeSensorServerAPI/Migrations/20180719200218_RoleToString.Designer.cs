﻿// <auto-generated />
using System;
using HomeSensorServerAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HomeSensorServerAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180719200218_RoleToString")]
    partial class RoleToString
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("HomeSensorServerAPI.Logger.LogEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOccured");

                    b.Property<int>("LogLevel");

                    b.Property<string>("LogMessage");

                    b.HasKey("Id");

                    b.ToTable("logevents");
                });

            modelBuilder.Entity("HomeSensorServerAPI.Models.Node", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ExactType");

                    b.Property<string>("GatewayAddress");

                    b.Property<string>("Identifier");

                    b.Property<string>("IpAddress");

                    b.Property<bool>("IsOn");

                    b.Property<string>("LoginName");

                    b.Property<string>("LoginPassword");

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("nodes");
                });

            modelBuilder.Entity("HomeSensorServerAPI.Models.Sensor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<string>("Identifier");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("sensors");
                });

            modelBuilder.Entity("HomeSensorServerAPI.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthdate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("Gender");

                    b.Property<DateTime>("JoinDate");

                    b.Property<DateTime>("LastInvalidLogin");

                    b.Property<DateTime>("LastValidLogin");

                    b.Property<string>("Lastname");

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("PhotoUrl");

                    b.Property<string>("Role")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("users");
                });
#pragma warning restore 612, 618
        }
    }
}
