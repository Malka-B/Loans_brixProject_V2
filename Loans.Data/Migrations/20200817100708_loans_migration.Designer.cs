﻿// <auto-generated />
using System;
using Loans.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Loans.Data.Migrations
{
    [DbContext(typeof(LoansContext))]
    [Migration("20200817100708_loans_migration")]
    partial class loans_migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Loans.Data.Entities.LoanEntity", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("BorrowerID")
                        .HasColumnType("int");

                    b.Property<bool>("FixedSalary")
                        .HasColumnType("bit");

                    b.Property<int>("LoanAmount")
                        .HasColumnType("int");

                    b.Property<DateTime>("LoanDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LoanProviderID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LoanPurpose")
                        .HasColumnType("int");

                    b.Property<int>("LoanStatus")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfRepayments")
                        .HasColumnType("int");

                    b.Property<int>("Salary")
                        .HasColumnType("int");

                    b.Property<int>("SeniorityYearsInBank")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("Loans.Data.Entities.LoanFailureRulesEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("LoanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ManagerComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManagerSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RuleDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RuleId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("LoanId");

                    b.ToTable("FailureRules");
                });

            modelBuilder.Entity("Loans.Data.Entities.LoanFailureRulesEntity", b =>
                {
                    b.HasOne("Loans.Data.Entities.LoanEntity", "Loan")
                        .WithMany("FailureRules")
                        .HasForeignKey("LoanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
