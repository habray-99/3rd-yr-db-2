using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Students
{
    public List<Students> StudentsList = [];
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public string Contact { get; set; }
    public DateTime Dob { get; set; }
    public string EmailAddress { get; set; }
    public string Country { get; set; }

    public List<Courses> EnrolledCourses { get; set; } = [];
    public List<StudentCourseProgress> Progresses { get; set; } = [];

    private List<Courses> FetchEnrolledCourses(int studentId)
    {
        var enrolledCourses = new List<Courses>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "SELECT CourseID, CourseTitle FROM Courses WHERE StudentID = :StudentID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = studentId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    enrolledCourses.Add(new Courses
                    {
                        CourseId = reader.GetString(0),
                        CourseTitle = reader.GetString(1)
                    });
                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return enrolledCourses;
    }

    public List<Students> GetStudents()
    {
        try
        {
            using var con = new OracleConnection(ValuesConstants.DbString);
            const string queryString =
                "SELECT StudentID, StudentName, Contact, DOB, EmailAddress, Country_CODE FROM Students";
            var cmd = new OracleCommand(queryString, con);
            cmd.BindByName = true;
            cmd.CommandType = CommandType.Text;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var student = new Students
                {
                    StudentId = reader.GetInt32(0),
                    StudentName = reader.GetString(1),
                    Contact = reader.GetString(2),
                    Dob = reader.GetDateTime(3),
                    EmailAddress = reader.GetString(4),
                    Country = reader.GetString(5)
                };
                StudentsList.Add(student);
            }

            reader.Dispose();
            con.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return StudentsList;
    }

    public void AddStudent(Students student)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                const string queryString =
                    "INSERT INTO Students (StudentID, StudentName, Contact, DOB, EmailAddress, Country_CODE) VALUES (:StudentID, :StudentName, :Contact, :DOB, :EmailAddress, :Country)";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = student.StudentId;
                cmd.Parameters.Add("StudentName", OracleDbType.Varchar2).Value = student.StudentName;
                cmd.Parameters.Add("Contact", OracleDbType.Varchar2).Value = student.Contact;
                cmd.Parameters.Add("DOB", OracleDbType.Date).Value = student.Dob;
                cmd.Parameters.Add("EmailAddress", OracleDbType.Varchar2).Value = student.EmailAddress;
                cmd.Parameters.Add("Country", OracleDbType.Varchar2).Value = student.Country;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void EditStudentDetails(Students student, int oldStudentId)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "UPDATE Students SET StudentName = :StudentName, Contact = :Contact, DOB = :DOB, EmailAddress = :EmailAddress, Country_CODE = :Country WHERE StudentID = :OldStudentID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentName", OracleDbType.Varchar2).Value = student.StudentName;
                cmd.Parameters.Add("Contact", OracleDbType.Varchar2).Value = student.Contact;
                cmd.Parameters.Add("DOB", OracleDbType.Date).Value = student.Dob;
                cmd.Parameters.Add("EmailAddress", OracleDbType.Varchar2).Value = student.EmailAddress;
                cmd.Parameters.Add("Country", OracleDbType.Varchar2).Value = student.Country;
                cmd.Parameters.Add("OldStudentID", OracleDbType.Int32).Value = oldStudentId;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    public void DeleteStudent(int studentId)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "DELETE FROM Students WHERE StudentID = :StudentID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = studentId;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public List<Students> FetchStudents()
    {
        var studentsList = new List<Students>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT StudentID, StudentName, Contact, DOB, EmailAddress, Country_CODE FROM Students";
                var cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var student = new Students
                    {
                        StudentId = reader.GetInt32(0),
                        StudentName = reader.GetString(1),
                        Contact = reader.GetString(2),
                        Dob = reader.GetDateTime(3),
                        EmailAddress = reader.GetString(4),
                        Country = reader.GetString(5)
                    };
                    studentsList.Add(student);
                }

                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return studentsList;
    }

    public Students FetchStudentById(int studentId)
    {
        Students student = null;
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT StudentID, StudentName, Contact, DOB, EmailAddress, Country_CODE FROM Students WHERE StudentID = :StudentID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = studentId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    student = new Students
                    {
                        StudentId = reader.GetInt32(0),
                        StudentName = reader.GetString(1),
                        Contact = reader.GetString(2),
                        Dob = reader.GetDateTime(3),
                        EmailAddress = reader.GetString(4),
                        Country = reader.GetString(5)
                    };
                reader.Dispose();
                con.Close();
            }

            if (student != null) student.EnrolledCourses = FetchEnrolledCourses(studentId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return student;
    }

    public async Task<List<Progresses>> GetStudentProgressAsync(int studentId)
    {
        var progressesModel = new Progresses();
        return await progressesModel.FetchProgressesByStudentIdAsync(studentId);
    }

    public async Task<List<Students>> SearchStudentsAsync(string searchTerm)
    {
        var studentsList = new List<Students>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    s.StudentId, 
                    s.StudentName, 
                    s.Contact, 
                    s.Dob, 
                    s.EmailAddress, 
                    s.Country_code
                FROM 
                    STUDENTS s
                WHERE 
                    UPPER(s.StudentName) LIKE UPPER(:SearchTerm)
                ORDER BY 
                    s.StudentName";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("SearchTerm", OracleDbType.Varchar2).Value = "%" + searchTerm + "%";
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    studentsList.Add(new Students
                    {
                        StudentId = reader.GetInt32(0),
                        StudentName = reader.GetString(1),
                        Contact = reader.GetString(2),
                        Dob = reader.GetDateTime(3),
                        EmailAddress = reader.GetString(4),
                        Country = reader.GetString(5)
                    });
                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return studentsList;
    }
}