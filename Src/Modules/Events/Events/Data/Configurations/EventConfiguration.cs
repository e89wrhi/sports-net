using Event.Events.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event.Data.Configurations;

using Events.ValueObjects;
using global::Event.Events.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

public class EventConfiguration : IEntityTypeConfiguration<EventModel>
{
    public void Configure(EntityTypeBuilder<EventModel> builder)
    {

        builder.ToTable(nameof(EventModel));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(aircraftId => aircraftId.Value, dbId => EventId.Of(dbId));

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.Title,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(EventModel.Title))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.Time,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(EventModel.Time))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.Property(x => x.Type)
            .HasDefaultValue(EventType.FreeKick)
            .HasConversion(
                x => x.ToString(),
                x => (Events.Enums.EventType)Enum.Parse(typeof(Events.Enums.EventType), x));

    }
}