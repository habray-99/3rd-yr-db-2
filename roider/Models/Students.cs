namespace roider.Models;

public class Students
{
    public int StudentID { get; set; }
    public string StudentName { get; set; }
    public string Contact { get; set; }
    public DateTime DOB { get; set; }
    public string EmailAddress { get; set; }
    public string Country { get; set; }

    public List<Students> StudentsList = [];
}