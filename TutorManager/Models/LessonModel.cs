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
        public virtual TutorModel? Tutor { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual StudentModel? Student { get; set; }
        public int Price { get; set; }
        public int TutorGratification { get; set; }
        public DateTime LessonDateTime { get; set; }
        public string? LessonStatus { get; set; }
        public string? Subject {  get; set; }
    }
}
