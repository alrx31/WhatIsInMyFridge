﻿    using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Persistanse
{
    public class ApplicationDbContextConfiguration
        :IEntityTypeConfiguration<Fridge>,
    IEntityTypeConfiguration<UserFridge>,
    IEntityTypeConfiguration<ProductFridgeModel>
    {
        public void Configure(EntityTypeBuilder<Fridge> builder)
        {
            builder.HasKey(f => f.id);
            builder.Property(f => f.id).ValueGeneratedOnAdd();

            builder.Property(f => f.boughtDate);
        }

        public void Configure(EntityTypeBuilder<UserFridge> builder)
        {
            builder.HasOne(uf=>uf.fridge)
                .WithMany(f=>f.userModelIds)
                .HasForeignKey(uf=>uf.fridgeId);   
        }

        public void Configure(EntityTypeBuilder<ProductFridgeModel> builder)
        {
            builder.HasKey(p => p.id);
            builder.Property(p => p.id).ValueGeneratedOnAdd();

            builder.HasOne(pf => pf.Fridge)
                .WithMany(f => f.products)
                .HasForeignKey(pf => pf.fridgeId);
        }

    }
}
