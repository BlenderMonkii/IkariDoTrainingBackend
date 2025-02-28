using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IkariDoTrainingBackend.Models
{
    [Table("sessions")]
    public class Session
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }

        [Column("name")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("session_date")]
        public DateTime? SessionDate { get; set; }

        [Column("duration")]
        public int Duration { get; set; }

        [Column("is_public")]
        public bool IsPublic { get; set; }

        [Column("type")]
        public string Type { get; set; }

        // Many-to-Many Beziehung zu TrainingPlans
        public virtual ICollection<TrainingPlanSession>? TrainingPlanSessions { get; set; } = new List<TrainingPlanSession>();
        // Many-to-Many Beziehung zu ExerciseBase über SessionExercise
        public virtual ICollection<SessionExercise>? SessionExercises { get; set; } = new List<SessionExercise>();
    }
}
