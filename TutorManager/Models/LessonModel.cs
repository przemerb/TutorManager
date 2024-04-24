using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TutorManager.Models
{
    /// <summary>
    /// Model lekcji
    /// </summary>
    public class LessonModel
    {
        /// <summary>
        /// Klucz primary
        /// </summary>
        [Key]
        public int LessonId { get; set; }

        /// <summary>
        /// Klucz foreign z tabeli Tutor
        /// </summary>
        [ForeignKey("Tutor")]
        public int TutorId { get; set; }
        public virtual TutorModel? Tutor { get; set; }

        /// <summary>
        /// Klucz foreign z tabeli Student
        /// </summary>
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
