using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IkariDoTrainingBackend.Models
{
    [Table("Timers")]
    public class Timer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("rest_time")]
        public int? RestTime { get; set; }

        [Column("active_time")]
        public int? ActiveTime { get; set; }

        [Column("pause_time")]
        public int? PauseTime { get; set; }

        [Column("sets")]
        public int? Sets { get; set; }

        [Column("reps")]
        public int? Reps { get; set; }
    }
}
