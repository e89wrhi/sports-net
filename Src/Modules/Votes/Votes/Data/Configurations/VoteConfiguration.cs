using Votes.Votes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Votes.Data.Configurations;

using Votes.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;

public class VoteConfiguration : IEntityTypeConfiguration<VoteModel>
{
    public void Configure(EntityTypeBuilder<VoteModel> builder)
    {

        builder.ToTable(nameof(VoteModel));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(aircraftId => aircraftId.Value, dbId => VoteId.Of(dbId));

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
            .HasDefaultValue(Votes.Enums.VoteType.Home)
            .HasConversion(
                x => x.ToString(),
                x => (Votes.Enums.VoteType)Enum.Parse(typeof(Votes.Enums.VoteType), x));

    }
} 