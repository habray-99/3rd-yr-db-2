namespace roider.Models;

public class Enrollments
{
    public int EnrollmentID { get; set; }
    public int StudentID { get; set; }
    
    public string CourseID { get; set; }
    public DateTime EnrollDate { get; set; }

    public Students Student { get; set; }
    public Courses Course { get; set; }

    public List<Enrollments> EnrollmentsList = [];
}