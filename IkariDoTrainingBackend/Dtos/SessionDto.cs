using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using IkariDoTrainingBackend.Models;

namespace IkariDoTrainingBackend.Dtos
{
    public class SessionDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? SessionDate { get; set; }
        public int Duration { get; set; }
        public bool IsPublic { get; set; }

        public List<SessionExercise> SessionExercises { get; set; }

    }
}
