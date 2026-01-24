using Match.Matches.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Match.Data.Configurations;

using Matches.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;

public class MatchConfiguration : IEntityTypeConfiguration<MatchModel>
{
    public void Configure(EntityTypeBuilder<MatchModel> builder)
    {

        builder.ToTable(nameof(MatchModel));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(aircraftId => aircraftId.Value, dbId => MatchId.Of(dbId));

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
            .HasDefaultValue(Matches.Enums.MatchStatus.Upcoming)
            .HasConversion(
                x => x.ToString(),
                x => (Matches.Enums.MatchStatus)Enum.Parse(typeof(Matches.Enums.MatchStatus), x));


        builder.Property(x => x.League)
            .HasDefaultValue(Matches.Enums.MatchLeague.PremierLeague)
            .HasConversion(
                x => x.ToString(),
                x => (Matches.Enums.MatchLeague)Enum.Parse(typeof(Matches.Enums.MatchLeague), x));

    }
}