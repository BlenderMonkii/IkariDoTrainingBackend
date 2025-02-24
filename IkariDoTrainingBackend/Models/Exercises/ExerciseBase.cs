using IkariDoTrainingBackend.Dtos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IkariDoTrainingBackend.Models.Exercises
{
    [Table("exercise_base")]
    public abstract class ExerciseBase
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("owner_id")]
        [Required]
        public int OwnerId { get; set; }

        [Column("name")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("duration")]
        public int? Duration { get; set; }

        [Column("is_public")]
        public bool IsPublic { get; set; }

        [Column("location")]
        [StringLength(255)]
        public string Location { get; set; }

        [Column("repetitions")]
        public int? Repetitions { get; set; } // TODO: Do we need this?

        //TODO: Adding Timer?

        [ForeignKey("OwnerId")]
        public User? Owner { get; set; }

        public virtual ICollection<Execution> Executions { get; set; } = new List<Execution>();
        public virtual ICollection<SessionExercise> SessionExercises { get; set; } = new List<SessionExercise>();
    }
}
