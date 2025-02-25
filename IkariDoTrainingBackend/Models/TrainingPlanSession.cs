using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IkariDoTrainingBackend.Models
{
    [Table("training_plan_sessions")]
    public class TrainingPlanSession
    {
        [Column("training_plan_id")]
        public int TrainingPlanId { get; set; }

        [Column("session_id")]
        public int SessionId { get; set; }


        [JsonIgnore]
        [ForeignKey("TrainingPlanId")]
        public TrainingPlan TrainingPlan { get; set; }

        [JsonIgnore]
        [ForeignKey("SessionId")]
        public Session Session { get; set; }
    }
}
