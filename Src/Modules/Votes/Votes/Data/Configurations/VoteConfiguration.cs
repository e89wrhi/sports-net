using Vote.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Vote.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using Vote.ValueObjects;

public class VoteConfiguration : IEntityTypeConfiguration<VoteModel>
{
    public void Configure(EntityTypeBuilder<VoteModel> builder)
    {

        builder.ToTable("votes");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(itemId => itemId.Value, dbId => VoteId.Of(dbId));

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.VoterId,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(VoteModel.VoterId))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.MatchId,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(VoteModel.MatchId))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.Property(x => x.VotedAt);

        builder.Property(x => x.Type)
            .HasDefaultValue(Vote.Enums.VoteType.Home)
            .HasConversion(
                x => x.ToString(),
                x => (Vote.Enums.VoteType)Enum.Parse(typeof(Vote.Enums.VoteType), x));

    }
} 