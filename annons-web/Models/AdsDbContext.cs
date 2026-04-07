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

    public virtual DbSet<TblAnnonsorer> TblAnnonsorer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAd>(entity =>
        {
            entity.HasKey(e => e.AdId).HasName("tbl_ads_pkey");

            entity.ToTable("tbl_ads");

            entity.Property(e => e.AdId).HasColumnName("ad_id");
            entity.Property(e => e.AdAdvertiserId).HasColumnName("ad_advertiser_id");
            entity.Property(e => e.AdContent).HasColumnName("ad_content");
            entity.Property(e => e.AdFee)
                .HasPrecision(10, 2)
                .HasColumnName("ad_fee");
            entity.Property(e => e.AdPrice)
                .HasPrecision(10, 2)
                .HasColumnName("ad_price");
            entity.Property(e => e.AdTitle)
                .HasMaxLength(100)
                .HasColumnName("ad_title");

            entity.HasOne(d => d.AdAdvertiser).WithMany(p => p.TblAds)
                .HasForeignKey(d => d.AdAdvertiserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_ads_annonsorer_id_fkey");
        });

        modelBuilder.Entity<TblAnnonsorer>(entity =>
        {
            entity.HasKey(e => e.AdvertiserId).HasName("tbl_annonsorer_pkey");

            entity.ToTable("tbl_annonsorer");

            entity.Property(e => e.AdvertiserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("advertiser_id");
            entity.Property(e => e.AdvertiserType)
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
