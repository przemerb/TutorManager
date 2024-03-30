using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TutorManager.Models
{
    public class LessonModel
    {
        [Key]
        public int LessonId { get; set; }

        [ForeignKey("Tutor")]
        public int TutorId { get; set; }
        public TutorModel Tutor { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public StudentModel Student { get; set; }
    }
}
