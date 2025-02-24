using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using IkariDoTrainingBackend.Models.Exercises;

namespace IkariDoTrainingBackend.Models
{
    [Table("session_exercises")]
    public class SessionExercise
    {
        [Key, Column("session_id")]
        public int SessionId { get; set; }

        [Key, Column("exercise_id")]
        public int ExerciseId { get; set; }

        [Column("sets")]
        [Required]
        public int Sets { get; set; } = 1;

        [Column("pause_time")]
        public int? PauseTime { get; set; }

        [JsonIgnore]
        [ForeignKey("SessionId")]
        public virtual Session Session { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual ExerciseBase Exercise { get; set; }
    }
}
