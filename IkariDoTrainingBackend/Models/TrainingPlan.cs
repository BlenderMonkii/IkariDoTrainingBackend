using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IkariDoTrainingBackend.Models
{
    [Table("training_plans")]
    public class TrainingPlan
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

        [Column("goal")]
        [StringLength(255)]
        public string Goal { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("is_public")]
        public bool IsPublic { get; set; }

        public virtual ICollection<TrainingPlanSession> TrainingPlanSessions { get; set; } = new List<TrainingPlanSession>();

        [NotMapped]
        public IEnumerable<Session> Sessions => TrainingPlanSessions.Select(tps => tps.Session);
    }
}
