namespace roider.Models;

public class Lessons
{
    public int LessonID { get; set; }
    public string LessonTitle { get; set; }
    public string LessonContentType { get; set; }
    
    public string CourseID { get; set; }

    public Courses Course { get; set; }

    public List<Lessons> LessonsList = [];
}