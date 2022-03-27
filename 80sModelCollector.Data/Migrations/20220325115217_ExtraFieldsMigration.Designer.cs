﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using _80sModelCollector.Data;

namespace _80sModelCollector.Data.Migrations
{
    [DbContext(typeof(CollectorStockContext))]
    [Migration("20220325115217_ExtraFieldsMigration")]
    partial class ExtraFieldsMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "3.1.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("_80sModelCollector.Data.Stock", b =>
                {
                    b.Property<int>("SerialNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasMaxLength(8)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasMaxLength(8);

                    b.Property<int>("RemainingStock")
                        .HasColumnType("int")
                        .HasMaxLength(8);

                    b.HasKey("SerialNumber")
                        .HasName("SerialNumber_PK");

                    b.ToTable("Stock");
                });
#pragma warning restore 612, 618
        }
    }
}
