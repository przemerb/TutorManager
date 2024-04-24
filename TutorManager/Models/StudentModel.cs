namespace TutorManager.Models
{
    /// <summary>
    /// Model Student dziedziczący do User
    /// </summary>
    public class StudentModel : UserModel
    {
        public int Charge {  get; set; }
    }
}
