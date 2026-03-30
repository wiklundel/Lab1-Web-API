using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prenumerant_api.Models;

public partial class SubscribersDbContext : DbContext
{
    public SubscribersDbContext()
    {
    }

    public SubscribersDbContext(DbContextOptions<SubscribersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblSubscriber> TblSubscribers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblSubscriber>(entity =>
        {
            entity.HasKey(e => e.SubscriberNr).HasName("tbl_subscribers_pkey");

            entity.ToTable("tbl_subscribers");

            entity.Property(e => e.SubscriberNr)
                .UseIdentityAlwaysColumn()
                .HasColumnName("subscriber_nr");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.DeliveryAddress)
                .HasMaxLength(100)
                .HasColumnName("delivery_address");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNr).HasColumnName("phone_nr");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.SocialSecurityNr).HasColumnName("social_security_nr");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
