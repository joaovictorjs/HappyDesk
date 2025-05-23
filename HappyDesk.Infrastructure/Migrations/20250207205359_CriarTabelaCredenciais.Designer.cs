﻿// <auto-generated />
using HappyDesk.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HappyDesk.Infrastructure.Migrations
{
    [DbContext(typeof(SqliteContext))]
    [Migration("20250207205359_CriarTabelaCredenciais")]
    partial class CriarTabelaCredenciais
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("HappyDesk.Domain.Entities.CredentialsEntity", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("codigo");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.HasKey("Code");

                    b.ToTable("credenciais");

                    b.HasData(
                        new
                        {
                            Code = 1,
                            Email = "",
                            Password = ""
                        });
                });

            modelBuilder.Entity("HappyDesk.Domain.Entities.PreferencesEntity", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("codigo");

                    b.Property<bool>("IsAutoLoginEnabled")
                        .HasColumnType("INTEGER")
                        .HasColumnName("auto_login_habilitado");

                    b.Property<bool>("IsNotificationEnable")
                        .HasColumnType("INTEGER")
                        .HasColumnName("notificacao_habilitada");

                    b.Property<string>("ObservedPeople")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("pessoas_observadas");

                    b.HasKey("Code");

                    b.ToTable("preferencias");

                    b.HasData(
                        new
                        {
                            Code = 1,
                            IsAutoLoginEnabled = true,
                            IsNotificationEnable = true,
                            ObservedPeople = ""
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
