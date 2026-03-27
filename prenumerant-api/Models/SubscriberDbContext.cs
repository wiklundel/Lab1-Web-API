using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prenumerant_api.Models;

public partial class SubscriberDbContext : DbContext
{
    public SubscriberDbContext()
    {
    }

    public SubscriberDbContext(DbContextOptions<SubscriberDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblSubscriber> TblSubscribers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SubscriberDB;Username=admin;Password=elvisAFIserver123");

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
