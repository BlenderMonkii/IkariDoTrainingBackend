using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using IkariDoTrainingBackend.Models.Exercises;
using System.Text.Json.Serialization;

namespace IkariDoTrainingBackend.Models
{
    public class Execution
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("exercise_id")]
        [Required]
        public int ExerciseId { get; set; }

        [Column("weight")]
        public double? Weight { get; set; }

        [Column("repetitions")]
        public int? Repetitions { get; set; }

        [Column("duration")]
        public string? Duration { get; set; }

        [Column("rating")]
        [Range(1, 10)]
        public int? Rating { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [ForeignKey("ExerciseId")]
        [JsonIgnore]
        public ExerciseBase? Exercise { get; set; }
    }
}
