namespace roider.Models;

public class Instructors
{
    public int InstructorID { get; set; }
    public string InstructorName { get; set; }
    public string Contact { get; set; }
    public string EmailAddress { get; set; }
    public string Specialization { get; set; }
    public int YearsOfExperience { get; set; }
    public string Country { get; set; }

    public List<Instructors> InstructorsList = [];
}