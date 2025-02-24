using IkariDoTrainingBackend.Models.Exercises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IkariDoTrainingBackend.Data.Configurations
{
    public class ExerciseBaseConfiguration : IEntityTypeConfiguration<ExerciseBase>
    {
        public void Configure(EntityTypeBuilder<ExerciseBase> builder)
        {
            builder.ToTable("exercise_base");

            builder.HasKey(e => e.Id);

            // Table-Per-Type (TPT) Mapping für Vererbung aktivieren
            builder.UseTptMappingStrategy();

            builder.Property(e => e.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(e => e.Description)
                   .HasColumnName("description");

            builder.Property(e => e.Duration)
                   .HasColumnName("duration");

            builder.Property(e => e.IsPublic)
                   .HasColumnName("is_public");

            builder.Property(e => e.Location)
                   .HasColumnName("location")
                   .HasMaxLength(255);

            // Beziehung zu User (Owner)
            builder.HasOne(e => e.Owner)
                   .WithMany()
                   .HasForeignKey(e => e.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Beziehung zu Timer
            builder.HasOne(e => e.Timer)
                   .WithMany()
                   .HasForeignKey(e => e.TimerId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
