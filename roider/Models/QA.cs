
    using roider.Models;

    public class QAs
    {
        public int Qaid { get; set; }
        public int StudentId { get; set; }
        public string CourseID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime FeedbackDate { get; set; }
        public Students Student { get; set; }
        public Courses Course { get; set; }

        public List<QAs> QAsList = [];
    }
