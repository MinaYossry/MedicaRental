﻿using MedicaRental.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MedicaRental.DAL.Context
{
    public class MedicaRentalDbContext : IdentityDbContext<AppUser>
    {
        DbSet<Item> Items => Set<Item>();
        DbSet<Client> Clients => Set<Client>();
        DbSet<Category> Categories => Set<Category>();
        DbSet<SubCategory> SubCategories => Set<SubCategory>();
        DbSet<Review> Reviews => Set<Review>();
        DbSet<Message> Messages => Set<Message>();
        DbSet<Report> Reports => Set<Report>();

        public MedicaRentalDbContext(DbContextOptions<MedicaRentalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Item>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.HasIndex(i => i.CategoryId);

                entity.HasIndex(i => i.SubCategoryId);

                entity.HasOne(i => i.SubCategory)
                .WithMany(sc => sc.Items)
                .HasForeignKey(i => i.SubCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(i => i.Category)
                .WithMany(sc => sc.Items)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.Property(i => i.Price).HasColumnType("money");

                entity.Property(i => i.Image).HasColumnType("image").IsRequired(true);

                entity.Property(i => i.IsDeleted).HasDefaultValue(false);
                entity.HasQueryFilter(i => !i.IsDeleted);
            });

            builder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasIndex(c => c.Ssn).IsUnique();
                entity.Property(c => c.NationalIdImage).HasColumnType("image").IsRequired(true);
                entity.Property(c => c.UnionCardImage).HasColumnType("image").IsRequired(true);
            });

            builder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Icon).HasColumnType("image").IsRequired(true);
            });

            builder.Entity<SubCategory>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Icon).HasColumnType("image").IsRequired(true);
            });

            builder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Item)
                .WithMany(i => i.Reviews)
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.Property(r => r.IsDeleted).HasDefaultValue(false);
                entity.HasQueryFilter(r => !r.IsDeleted);
            });

            builder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.HasOne(m => m.Sender)
                .WithMany(c => c.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.Receiver)
                .WithMany(c => c.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.Property(m => m.IsDeleted).HasDefaultValue(false);
                entity.HasQueryFilter(m => !m.IsDeleted);
            });

            builder.Entity<Report>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(i => i.Reportee)
                .WithMany(sc => sc.Reports)
                .HasForeignKey(i => i.ReporteeId)
                .OnDelete(DeleteBehavior.NoAction);

                //entity.HasOne(i => i.Reported)
                //.WithMany(sc => sc.Reports)
                //.HasForeignKey(i => i.ReporteeId)
                //.OnDelete(DeleteBehavior.NoAction);

                entity.Property(r => r.IsDeleted).HasDefaultValue(false);
                entity.HasQueryFilter(r => !r.IsDeleted);
            });
        }
    }
}