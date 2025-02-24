using IkariDoTrainingBackend.Data.Configurations;
using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Models.Exercises;
using Microsoft.EntityFrameworkCore;
using Timer = IkariDoTrainingBackend.Models.Timer;

namespace IkariDoTrainingBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TrainingPlan> TrainingPlans { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<ExerciseBase> Exercises { get; set; }
        public DbSet<SessionExercise> SessionExercises { get; set; }
        public DbSet<FingerboardExercise> FingerboardExercises { get; set; }

        public DbSet<Execution> Executions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply Table-Per-Type (TPT) Mapping
            modelBuilder.ApplyConfiguration(new ExerciseBaseConfiguration());

            // Konfiguration der abgeleiteten Klasse FingerboardExercise
            modelBuilder.Entity<FingerboardExercise>()
                .ToTable("fingerboard_exercises");

            // Many-to-Many-Konfiguration für Sessions & Exercises
            modelBuilder.Entity<SessionExercise>()
                .HasKey(se => new { se.SessionId, se.ExerciseId });

            modelBuilder.Entity<SessionExercise>()
                .HasOne(se => se.Session)
                .WithMany(s => s.SessionExercises)
                .HasForeignKey(se => se.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SessionExercise>()
                .HasOne(se => se.Exercise)
                .WithMany(e => e.SessionExercises)
                .HasForeignKey(se => se.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguration für Exercises und Executions
            modelBuilder.Entity<Execution>()
                .HasOne(e => e.Exercise)
                .WithMany(ex => ex.Executions)
                .HasForeignKey(e => e.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);


            // **Many-to-Many für Sessions & TrainingPlans**
            modelBuilder.Entity<Session>()
                .HasMany(s => s.TrainingPlans)
                .WithMany(tp => tp.Sessions)
                .UsingEntity<Dictionary<string, object>>(
                    "training_plan_sessions", // Tabellenname in der DB
                    j => j
                        .HasOne<TrainingPlan>()
                        .WithMany()
                        .HasForeignKey("training_plan_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Session>()
                        .WithMany()
                        .HasForeignKey("session_id")
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }

    }
}
