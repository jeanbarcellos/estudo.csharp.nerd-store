﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NerdStore.Pagamentos.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NerdStore.Pagamentos.Data.Migrations
{
    [DbContext(typeof(PagamentoContext))]
    [Migration("20210615002812_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("NerdStore.Pagamentos.Business.Pagamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CVVCartao")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ExpiracaoCartao")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NomeCartao")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NumeroCartao")
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Pagamentos");
                });

            modelBuilder.Entity("NerdStore.Pagamentos.Business.Transacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PagamentoId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uuid");

                    b.Property<int>("StatusTransacao")
                        .HasColumnType("integer");

                    b.Property<decimal>("Total")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("PagamentoId")
                        .IsUnique();

                    b.ToTable("Transacoes");
                });

            modelBuilder.Entity("NerdStore.Pagamentos.Business.Transacao", b =>
                {
                    b.HasOne("NerdStore.Pagamentos.Business.Pagamento", "Pagamento")
                        .WithOne("Transacao")
                        .HasForeignKey("NerdStore.Pagamentos.Business.Transacao", "PagamentoId")
                        .IsRequired();

                    b.Navigation("Pagamento");
                });

            modelBuilder.Entity("NerdStore.Pagamentos.Business.Pagamento", b =>
                {
                    b.Navigation("Transacao");
                });
#pragma warning restore 612, 618
        }
    }
}
