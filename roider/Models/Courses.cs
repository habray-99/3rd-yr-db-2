

namespace roider.Models;

public class Courses
{
    public string CourseID { get; set; }
    public string CourseTitle { get; set; }
    public List<Courses> CoursesList = [];
    
}