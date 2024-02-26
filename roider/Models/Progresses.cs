namespace roider.Models;

public class Progresses
{
    public int ProgressID { get; set; }
    public int StudentID { get; set; }
    public int LessonID { get; set; }
    public string LessonStatus { get; set; }
    public DateTime LastAccessedDate { get; set; }

    public Students Student { get; set; }
    public Lessons Lesson { get; set; }

    public List<Progresses> ProgressesList = [];
}