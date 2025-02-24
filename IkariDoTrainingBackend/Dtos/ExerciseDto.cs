using IkariDoTrainingBackend.Models;

namespace IkariDoTrainingBackend.Dtos
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public bool IsPublic { get; set; }
        public string? Location { get; set; }
        public int? Repetitions { get; set; }
        public string ExerciseType { get; set; }
        public List<Execution> Executions { get; set; }
    }
}
