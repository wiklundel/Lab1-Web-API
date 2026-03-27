using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace annons_web.Models;

public partial class AdsDbContext : DbContext
{
    public AdsDbContext()
    {
    }

    public AdsDbContext(DbContextOptions<AdsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAd> TblAds { get; set; }

    public virtual DbSet<TblAdvertiser> TblAdvertisers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=AdsDB;Username=admin;Password=elvisAFIserver123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAd>(entity =>
        {
            entity.HasKey(e => e.AdId).HasName("tbl_ads_pkey");

            entity.ToTable("tbl_ads");

            entity.Property(e => e.AdId).HasColumnName("ad_id");
            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Fee)
                .HasPrecision(10, 2)
                .HasColumnName("fee");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Advertiser).WithMany(p => p.TblAds)
                .HasForeignKey(d => d.AdvertiserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_ads_advertiser_id_fkey");
        });

        modelBuilder.Entity<TblAdvertiser>(entity =>
        {
            entity.HasKey(e => e.AdvertiserId).HasName("tbl_advertiser_pkey");

            entity.ToTable("tbl_advertiser");

            entity.Property(e => e.AdvertiserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("advertiser_id");
            entity.Property(e => e.AdvertiserType)
                .HasMaxLength(20)
                .HasColumnName("advertiser_type");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CorporateRegNr).HasColumnName("corporate_reg_nr");
            entity.Property(e => e.DeliveryAddress)
                .HasMaxLength(100)
                .HasColumnName("delivery_address");
            entity.Property(e => e.InvoiceAddress)
                .HasMaxLength(100)
                .HasColumnName("invoice_address");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNr).HasColumnName("phone_nr");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
