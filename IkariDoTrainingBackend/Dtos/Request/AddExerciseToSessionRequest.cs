namespace IkariDoTrainingBackend.Dtos.Request
{
    public class AddExerciseToSessionRequest
    {
        public int Sets { get; set; } = 1;
        public int? PauseTime { get; set; }
    }

}
