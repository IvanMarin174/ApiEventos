﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPIEventos.Context;

#nullable disable

namespace WebAPIEventos.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250120181913_Inicial")]
    partial class Inicial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("WebAPIEventos.Models.Evento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Lugar")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("WebAPIEventos.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Rols");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nombre = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Nombre = "user"
                        });
                });

            modelBuilder.Entity("WebAPIEventos.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RolId");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Correo = "ivan@correo.com",
                            Nombre = "ivan",
                            Password = "12345",
                            RolId = 2
                        });
                });

            modelBuilder.Entity("WebAPIEventos.Models.Evento", b =>
                {
                    b.HasOne("WebAPIEventos.Models.Usuario", "Usuario")
                        .WithMany("Eventos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("WebAPIEventos.Models.Usuario", b =>
                {
                    b.HasOne("WebAPIEventos.Models.Rol", "Rol")
                        .WithMany("Usuario")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("WebAPIEventos.Models.Rol", b =>
                {
                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("WebAPIEventos.Models.Usuario", b =>
                {
                    b.Navigation("Eventos");
                });
#pragma warning restore 612, 618
        }
    }
}
