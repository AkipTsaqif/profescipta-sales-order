using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Profescipta_Sales_Order.Models;

public partial class SOContext : DbContext
{
    public SOContext()
    {
    }

    public SOContext(DbContextOptions<SOContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ComCustomer> ComCustomers { get; set; }

    public virtual DbSet<SoItem> SoItems { get; set; }

    public virtual DbSet<SoOrder> SoOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DbConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComCustomer>(entity =>
        {
            entity.ToTable("COM_CUSTOMER");

            entity.Property(e => e.ComCustomerId).HasColumnName("COM_CUSTOMER_ID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_NAME");
        });

        modelBuilder.Entity<SoItem>(entity =>
        {
            entity.ToTable("SO_ITEM");

            entity.Property(e => e.SoItemId).HasColumnName("SO_ITEM_ID");
            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValueSql("('')")
                .HasColumnName("ITEM_NAME");
            entity.Property(e => e.Price).HasColumnName("PRICE");
            entity.Property(e => e.Quantity)
                .HasDefaultValueSql("((-99))")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.SoOrderId)
                .HasDefaultValueSql("((-99))")
                .HasColumnName("SO_ORDER_ID");
        });

        modelBuilder.Entity<SoOrder>(entity =>
        {
            entity.ToTable("SO_ORDER");

            entity.Property(e => e.SoOrderId).HasColumnName("SO_ORDER_ID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValueSql("('')")
                .HasColumnName("ADDRESS");
            entity.Property(e => e.ComCustomerId)
                .HasDefaultValueSql("('-99')")
                .HasColumnName("COM_CUSTOMER_ID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("('1900-01-01')")
                .HasColumnType("datetime")
                .HasColumnName("ORDER_DATE");
            entity.Property(e => e.OrderNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('')")
                .HasColumnName("ORDER_NO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
