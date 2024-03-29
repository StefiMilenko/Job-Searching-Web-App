﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

#nullable disable

namespace Projekat.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20230710012658_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IdentityRole");

                    b.HasData(
                        new
                        {
                            Id = "6697473b-7416-4b66-99b9-13460e5ee4e9",
                            Name = "Posetilac",
                            NormalizedName = "POSETILAC"
                        },
                        new
                        {
                            Id = "c615dc48-a3e4-44fa-a282-4b58ae448398",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        },
                        new
                        {
                            Id = "827d5d60-2132-4260-a67e-9b6fcf38ba3b",
                            Name = "Trazilac Posla",
                            NormalizedName = "TRAZILAC"
                        },
                        new
                        {
                            Id = "7e60e6c1-4728-4b7e-a47c-d6938378ea6c",
                            Name = "Poslodavac",
                            NormalizedName = "POSLODAVAC"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Models.AuthenticationToken", b =>
                {
                    b.Property<string>("TokenString")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ExpireTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("KorisnikId")
                        .HasColumnType("int");

                    b.HasKey("TokenString");

                    b.HasIndex("KorisnikId");

                    b.ToTable("AuthenticationTokens");
                });

            modelBuilder.Entity("Models.Komentar", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("AutorId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DatumEditovanja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatumSlanja")
                        .HasColumnType("datetime2");

                    b.Property<int?>("KomentarId")
                        .HasColumnType("int");

                    b.Property<int?>("PosaoId")
                        .HasColumnType("int");

                    b.Property<string>("Sadrzaj")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.HasIndex("KomentarId");

                    b.HasIndex("PosaoId");

                    b.ToTable("Komentari");
                });

            modelBuilder.Entity("Models.Korisnik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Biografija")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Drzava")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Korisnici", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Models.Notifikacija", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("KorisnikId")
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sadrzaj")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("KorisnikId");

                    b.ToTable("Notifikacije");
                });

            modelBuilder.Entity("Models.Ocena", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<float>("AvgVal")
                        .HasColumnType("real");

                    b.Property<int?>("KorisnikId")
                        .HasColumnType("int");

                    b.Property<int>("Napredovanje")
                        .HasColumnType("int");

                    b.Property<int>("Okruzenje")
                        .HasColumnType("int");

                    b.Property<int>("Plata")
                        .HasColumnType("int");

                    b.Property<int?>("PosaoId")
                        .HasColumnType("int");

                    b.Property<int>("PosoKuca")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("KorisnikId");

                    b.HasIndex("PosaoId");

                    b.ToTable("Ocene");
                });

            modelBuilder.Entity("Models.Posao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Industrija")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lokacija")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OpisPosla")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlataDo")
                        .HasColumnType("int");

                    b.Property<string>("PlataInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlataOd")
                        .HasColumnType("int");

                    b.Property<string>("PlataValuta")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PoslodavacId")
                        .HasColumnType("int");

                    b.Property<float>("ProsecnaOcena")
                        .HasColumnType("real");

                    b.Property<string>("SektorRada")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipPosla")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VrstaPosla")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("latitude")
                        .HasColumnType("float");

                    b.Property<double?>("longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("PoslodavacId");

                    b.ToTable("Poslovi");
                });

            modelBuilder.Entity("Models.Poslodavac", b =>
                {
                    b.HasBaseType("Models.Korisnik");

                    b.Property<string>("NazivFirme")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Opis")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Poslodavci", (string)null);
                });

            modelBuilder.Entity("Models.Trazilac", b =>
                {
                    b.HasBaseType("Models.Korisnik");

                    b.Property<string>("CV")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StrucnaSprema")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Trazioci", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Models.Korisnik", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Models.Korisnik", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Korisnik", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Models.Korisnik", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.AuthenticationToken", b =>
                {
                    b.HasOne("Models.Korisnik", "Korisnik")
                        .WithMany("Tokens")
                        .HasForeignKey("KorisnikId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Korisnik");
                });

            modelBuilder.Entity("Models.Komentar", b =>
                {
                    b.HasOne("Models.Trazilac", "Autor")
                        .WithMany("Komentari")
                        .HasForeignKey("AutorId");

                    b.HasOne("Models.Komentar", null)
                        .WithMany("Odgovori")
                        .HasForeignKey("KomentarId");

                    b.HasOne("Models.Posao", "Posao")
                        .WithMany("Komentari")
                        .HasForeignKey("PosaoId");

                    b.Navigation("Autor");

                    b.Navigation("Posao");
                });

            modelBuilder.Entity("Models.Notifikacija", b =>
                {
                    b.HasOne("Models.Korisnik", "Korisnik")
                        .WithMany()
                        .HasForeignKey("KorisnikId");

                    b.Navigation("Korisnik");
                });

            modelBuilder.Entity("Models.Ocena", b =>
                {
                    b.HasOne("Models.Korisnik", "Korisnik")
                        .WithMany()
                        .HasForeignKey("KorisnikId");

                    b.HasOne("Models.Posao", "Posao")
                        .WithMany("Ocene")
                        .HasForeignKey("PosaoId");

                    b.Navigation("Korisnik");

                    b.Navigation("Posao");
                });

            modelBuilder.Entity("Models.Posao", b =>
                {
                    b.HasOne("Models.Poslodavac", "Poslodavac")
                        .WithMany("Poslovi")
                        .HasForeignKey("PoslodavacId");

                    b.Navigation("Poslodavac");
                });

            modelBuilder.Entity("Models.Poslodavac", b =>
                {
                    b.HasOne("Models.Korisnik", null)
                        .WithOne()
                        .HasForeignKey("Models.Poslodavac", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Trazilac", b =>
                {
                    b.HasOne("Models.Korisnik", null)
                        .WithOne()
                        .HasForeignKey("Models.Trazilac", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Komentar", b =>
                {
                    b.Navigation("Odgovori");
                });

            modelBuilder.Entity("Models.Korisnik", b =>
                {
                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("Models.Posao", b =>
                {
                    b.Navigation("Komentari");

                    b.Navigation("Ocene");
                });

            modelBuilder.Entity("Models.Poslodavac", b =>
                {
                    b.Navigation("Poslovi");
                });

            modelBuilder.Entity("Models.Trazilac", b =>
                {
                    b.Navigation("Komentari");
                });
#pragma warning restore 612, 618
        }
    }
}
