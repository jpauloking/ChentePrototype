﻿// <auto-generated />
using System;
using Chente.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Chente.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240512084528_CreateDatabase")]
    partial class CreateDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("Chente.DataAccess.Models.Borrower", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Borrowers");
                });

            modelBuilder.Entity("Chente.DataAccess.Models.Installment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasPrecision(16, 4)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("AmountPaid")
                        .HasPrecision(16, 4)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BeginningBalance")
                        .HasPrecision(16, 4)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateDue")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DatePaid")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("EndingBalance")
                        .HasPrecision(16, 4)
                        .HasColumnType("TEXT");

                    b.Property<int>("LoanId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LoanId");

                    b.ToTable("Installments");
                });

            modelBuilder.Entity("Chente.DataAccess.Models.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("AmountPerInstallment")
                        .HasPrecision(16, 4)
                        .HasColumnType("TEXT");

                    b.Property<int>("BorrowerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOpened")
                        .HasColumnType("TEXT");

                    b.Property<int>("DurationInDays")
                        .HasColumnType("INTEGER");

                    b.Property<double>("InterestRate")
                        .HasColumnType("REAL");

                    b.Property<decimal>("Principal")
                        .HasPrecision(16, 4)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BorrowerId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("Chente.DataAccess.Models.Installment", b =>
                {
                    b.HasOne("Chente.DataAccess.Models.Loan", "Loan")
                        .WithMany("Installments")
                        .HasForeignKey("LoanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Loan");
                });

            modelBuilder.Entity("Chente.DataAccess.Models.Loan", b =>
                {
                    b.HasOne("Chente.DataAccess.Models.Borrower", "Borrower")
                        .WithMany("Loans")
                        .HasForeignKey("BorrowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Borrower");
                });

            modelBuilder.Entity("Chente.DataAccess.Models.Borrower", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("Chente.DataAccess.Models.Loan", b =>
                {
                    b.Navigation("Installments");
                });
#pragma warning restore 612, 618
        }
    }
}
