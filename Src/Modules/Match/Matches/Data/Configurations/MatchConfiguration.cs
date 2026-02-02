using Match.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Match.Data.Configurations;

using Match.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;

public class MatchConfiguration : IEntityTypeConfiguration<MatchModel>
{
    public void Configure(EntityTypeBuilder<MatchModel> builder)
    {

        builder.ToTable("matches");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(itemId => itemId.Value, dbId => MatchId.Of(dbId));

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.HomeTeam,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(MatchModel.HomeTeam))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.AwayTeam,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(MatchModel.AwayTeam))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.HomeTeamScore,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(MatchModel.HomeTeamScore))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.AwayTeamScore,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(MatchModel.AwayTeamScore))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.Property(x => x.MatchTime);
        builder.Property(x => x.EventsCount);
        builder.Property(x => x.HomeVotesCount);
        builder.Property(x => x.AwayVotesCount);
        builder.Property(x => x.DrawVotesCount);

        builder.Property(x => x.Status)
            .HasDefaultValue(Match.Enums.MatchStatus.Upcoming)
            .HasConversion(
                x => x.ToString(),
                x => (Match.Enums.MatchStatus)Enum.Parse(typeof(Match.Enums.MatchStatus), x));


        builder.Property(x => x.League)
            .HasDefaultValue(Match.Enums.MatchLeague.PremierLeague)
            .HasConversion(
                x => x.ToString(),
                x => (Match.Enums.MatchLeague)Enum.Parse(typeof(Match.Enums.MatchLeague), x));

    }
}