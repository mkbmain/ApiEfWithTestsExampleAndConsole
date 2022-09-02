using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataLayer
{
    public partial class ExampleDbContext : DbContext
    {
        public ExampleDbContext()
        {
        }

        public ExampleDbContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString)
                .Options;
        }

        public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(
                    "Data Source=localhost;Initial Catalog=ExampleDb;user id=sa;password=12345678");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(150);
                entity.HasIndex(u => u.Email)
                    .IsUnique();
                
                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasIndex(e => e.CustomerId, "IX_CustomerAddresses");

                entity.Property(e => e.BuildingName).IsRequired(false).HasMaxLength(150);

                entity.Property(e => e.BuildingNumber)
                    .HasMaxLength(12)
                    .IsRequired(false)
                    .IsUnicode(false);

                entity.Property(e => e.City).HasMaxLength(150);

                entity.Property(e => e.PostCode)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Street).HasMaxLength(250);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerAddresses)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CustomerAddresses_Customer");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.CustomerId, "IX_Orders");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Orders_Customer");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}