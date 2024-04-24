namespace TutorManager.Models
{
    /// <summary>
    /// Model Tutor dziedziczący do User
    /// </summary>
    public class TutorModel : UserModel
    {
        public string? Subject {  get; set; }
        public int ExpectedGratification { get; set; }
        public int SumGratification { get; set; }
        public uint NumOfStudents { get; set; }
    }
}
