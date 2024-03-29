﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WordApp.Persistence;

#nullable disable

namespace WordApp.Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230822213232_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WordApp.Persistence.Models.Complaint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ReasonId")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WordSetId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ReasonId");

                    b.HasIndex("UserId");

                    b.HasIndex("WordSetId");

                    b.ToTable("complaints", (string)null);
                });

            modelBuilder.Entity("WordApp.Persistence.Models.ComplaintReason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("complaintReasons", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "unacceptable content",
                            Name = "Unacceptable content"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Content mistakes",
                            Name = "Content mistakes"
                        });
                });

            modelBuilder.Entity("WordApp.Persistence.Models.Level", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("Language")
                        .HasColumnType("smallint");

                    b.Property<string>("LanguageId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("levels", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "A1",
                            Description = "CEFR",
                            Language = (byte)0,
                            LanguageId = "eng",
                            Name = "Beginner"
                        },
                        new
                        {
                            Id = "A2",
                            Description = "CEFR",
                            Language = (byte)0,
                            LanguageId = "eng",
                            Name = "Elementary"
                        },
                        new
                        {
                            Id = "B1",
                            Description = "CEFR",
                            Language = (byte)0,
                            LanguageId = "eng",
                            Name = "Intermediate"
                        },
                        new
                        {
                            Id = "B2",
                            Description = "CEFR",
                            Language = (byte)0,
                            LanguageId = "eng",
                            Name = "Upper-Intermediate"
                        },
                        new
                        {
                            Id = "C1",
                            Description = "CEFR",
                            Language = (byte)0,
                            LanguageId = "eng",
                            Name = "Advanced"
                        },
                        new
                        {
                            Id = "C2",
                            Description = "CEFR",
                            Language = (byte)0,
                            LanguageId = "eng",
                            Name = "Proficiency"
                        });
                });

            modelBuilder.Entity("WordApp.Persistence.Models.ProposedWord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OriginalContext")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OriginalWord")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TargetContext")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TargetWord")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WordSetId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WordSetId");

                    b.ToTable("proposedWords", (string)null);
                });

            modelBuilder.Entity("WordApp.Persistence.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("ExpiresOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("refreshTokens", (string)null);
                });

            modelBuilder.Entity("WordApp.Persistence.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("Role")
                        .HasColumnType("smallint");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("WordApp.Persistence.Models.UserWord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OriginalContext")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("OriginalLanguage")
                        .HasColumnType("smallint");

                    b.Property<string>("OriginalWord")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("RepeatOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Stage")
                        .HasColumnType("integer");

                    b.Property<string>("TargetContext")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("TargetLanguage")
                        .HasColumnType("smallint");

                    b.Property<string>("TargetWord")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("userWords", (string)null);
                });

            modelBuilder.Entity("WordApp.Persistence.Models.Word", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OriginalContext")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("OriginalLanguage")
                        .HasColumnType("smallint");

                    b.Property<string>("OriginalWord")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TargetContext")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("TargetLanguage")
                        .HasColumnType("smallint");

                    b.Property<string>("TargetWord")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("words", (string)null);
                });

            modelBuilder.Entity("WordApp.Persistence.Models.WordSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ConfirmedById")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ConfirmedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CoverImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LevelId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("OriginalLanguage")
                        .HasColumnType("smallint");

                    b.Property<byte>("TargetLanguage")
                        .HasColumnType("smallint");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ConfirmedById");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LevelId");

                    b.ToTable("wordSets", (string)null);
                });

            modelBuilder.Entity("WordWordSet", b =>
                {
                    b.Property<Guid>("WordSetsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WordsId")
                        .HasColumnType("uuid");

                    b.HasKey("WordSetsId", "WordsId");

                    b.HasIndex("WordsId");

                    b.ToTable("WordWordSet");
                });

            modelBuilder.Entity("WordApp.Persistence.Models.Complaint", b =>
                {
                    b.HasOne("WordApp.Persistence.Models.ComplaintReason", "Reason")
                        .WithMany()
                        .HasForeignKey("ReasonId");

                    b.HasOne("WordApp.Persistence.Models.User", "User")
                        .WithMany("Complaints")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WordApp.Persistence.Models.WordSet", "WordSet")
                        .WithMany("Complaints")
                        .HasForeignKey("WordSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reason");

                    b.Navigation("User");

                    b.Navigation("WordSet");
                });

            modelBuilder.Entity("WordApp.Persistence.Models.ProposedWord", b =>
                {
                    b.HasOne("WordApp.Persistence.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WordApp.Persistence.Models.WordSet", "WordSet")
                        .WithMany("ProposedWords")
                        .HasForeignKey("WordSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("WordSet");
                });

            modelBuilder.Entity("WordApp.Persistence.Models.RefreshToken", b =>
                {
                    b.HasOne("WordApp.Persistence.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WordApp.Persistence.Models.UserWord", b =>
                {
                    b.HasOne("WordApp.Persistence.Models.User", "User")
                        .WithMany("UserWords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WordApp.Persistence.Models.Word", b =>
                {
                    b.HasOne("WordApp.Persistence.Models.User", "User")
                        .WithMany("Words")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WordApp.Persistence.Models.WordSet", b =>
                {
                    b.HasOne("WordApp.Persistence.Models.User", "ConfirmedBy")
                        .WithMany("ConfirmedWordSets")
                        .HasForeignKey("ConfirmedById");

                    b.HasOne("WordApp.Persistence.Models.User", "CreatedBy")
                        .WithMany("CreatedWordSets")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WordApp.Persistence.Models.Level", "Level")
                        .WithMany("WordSets")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConfirmedBy");

                    b.Navigation("CreatedBy");

                    b.Navigation("Level");
                });

            modelBuilder.Entity("WordWordSet", b =>
                {
                    b.HasOne("WordApp.Persistence.Models.WordSet", null)
                        .WithMany()
                        .HasForeignKey("WordSetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WordApp.Persistence.Models.Word", null)
                        .WithMany()
                        .HasForeignKey("WordsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WordApp.Persistence.Models.Level", b =>
                {
                    b.Navigation("WordSets");
                });

            modelBuilder.Entity("WordApp.Persistence.Models.User", b =>
                {
                    b.Navigation("Complaints");

                    b.Navigation("ConfirmedWordSets");

                    b.Navigation("CreatedWordSets");

                    b.Navigation("RefreshTokens");

                    b.Navigation("UserWords");

                    b.Navigation("Words");
                });

            modelBuilder.Entity("WordApp.Persistence.Models.WordSet", b =>
                {
                    b.Navigation("Complaints");

                    b.Navigation("ProposedWords");
                });
#pragma warning restore 612, 618
        }
    }
}
