using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IkariDoTrainingBackend.Models.Exercises
{
    [Table("fingerboard_exercises")]
    public class FingerboardExercise : ExerciseBase
    {
        [Column("board_name")]
        [StringLength(100)]
        public string BoardName { get; set; }

        [Column("edge_size")]
        [Required]
        public float EdgeSize { get; set; }

        [Column("grip_type")]
        [Required]
        [StringLength(50)]
        public string GripType { get; set; }

        [Column("fingers")]
        [Required]
        [StringLength(50)]
        public string Fingers { get; set; }
    }
}
