﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestingOfApplicants.Models;

namespace TestingOfApplicants.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20220503130414_AddSubject")]
    partial class AddSubject
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("TestingOfApplicants.Models.Tests.CompletedTestDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CountByPersent")
                        .HasColumnType("int");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CompletedTestsDto");
                });

            modelBuilder.Entity("TestingOfApplicants.Models.Tests.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ask")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FakeAnswer1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FakeAnswer2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FakeAnswer3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HeaderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("TestingOfApplicants.Models.Tests.SubjectDto", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("subjects");
                });

            modelBuilder.Entity("TestingOfApplicants.Models.Tests.TestHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Subjectid")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Subjectid");

                    b.ToTable("TestHeaders");
                });

            modelBuilder.Entity("TestingOfApplicants.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("mName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TestingOfApplicants.Models.Tests.TestHeader", b =>
                {
                    b.HasOne("TestingOfApplicants.Models.Tests.SubjectDto", "Subject")
                        .WithMany()
                        .HasForeignKey("Subjectid");

                    b.Navigation("Subject");
                });
#pragma warning restore 612, 618
        }
    }
}
