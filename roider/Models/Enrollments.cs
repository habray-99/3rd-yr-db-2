using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Enrollments
{
    public List<Enrollments> EnrollmentsList = [];
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public string? CourseId { get; set; }
    public DateTime EnrollDate { get; set; }
    public Students? Student { get; set; }
    public Courses? Course { get; set; }

    public List<Enrollments> GetEnrollments()
    {
        var enrollmentsList = new List<Enrollments>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    e.EnrollmentId, 
                    e.StudentId, 
                    s.StudentName, 
                    e.CourseId, 
                    c.CourseTitle,
                    e.EnrollDate
                FROM 
                    ENROLLMENT e
                JOIN 
                    STUDENTS s ON e.StudentId = s.StudentId
                JOIN 
                    COURSES c ON e.CourseId = c.CourseId
                ORDER BY 
                    e.EnrollDate";
                var cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var enrollment = new Enrollments
                    {
                        EnrollmentId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        Student = new Students { StudentName = reader.GetString(2) },
                        CourseId = reader.GetString(3),
                        Course = new Courses { CourseTitle = reader.GetString(4) },
                        EnrollDate = reader.GetDateTime(5)
                    };
                    enrollmentsList.Add(enrollment);
                }

                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return enrollmentsList;
    }


    // Method to add a new enrollment
    public void AddEnrollment(Enrollments enrollment)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                const string queryString =
                    "INSERT INTO Enrollment (StudentId, CourseId, EnrollDate) VALUES (:StudentId, :CourseId, :EnrollDate)";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentId", OracleDbType.Int32).Value = enrollment.StudentId;
                cmd.Parameters.Add("CourseId", OracleDbType.Varchar2).Value = enrollment.CourseId;
                var enrollDate = DateTime.Now; // Example date
                var formattedDate = enrollDate.ToString("dd-MMM-yy");
                cmd.Parameters.Add("EnrollDate", OracleDbType.Date).Value =
                    enrollment.EnrollDate; //enrollment.EnrollDate;

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

    // Method to edit an existing enrollment
    public void EditEnrollment(Enrollments enrollment, int oldEnrollmentId)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "UPDATE Enrollment SET StudentId = :StudentId, CourseId = :CourseId, EnrollDate = :EnrollDate WHERE EnrollmentId = :OldEnrollmentID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentId", OracleDbType.Int32).Value = enrollment.StudentId;
                cmd.Parameters.Add("CourseId", OracleDbType.Varchar2).Value = enrollment.CourseId;
                cmd.Parameters.Add("EnrollDate", OracleDbType.Date).Value = enrollment.EnrollDate;
                cmd.Parameters.Add("OldEnrollmentID", OracleDbType.Int32).Value = oldEnrollmentId;

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

    // Method to delete an enrollment
    public void DeleteEnrollment(int enrollmentId)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "DELETE FROM Enrollment WHERE EnrollmentId = :EnrollmentId";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("EnrollmentId", OracleDbType.Int32).Value = enrollmentId;

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

    // Method to fetch enrollments by student ID
    public Enrollments FetchEnrollmentsByStudentId(int enrollmentId)
    {
        Console.WriteLine(enrollmentId);
        Enrollments enrollment = null;
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT EnrollmentId, StudentId, CourseId, EnrollDate FROM Enrollment WHERE EnrollmentId = :EnrollmentId";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("EnrollmentId", OracleDbType.Int32).Value = enrollmentId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    enrollment = new Enrollments
                    {
                        EnrollmentId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        CourseId = reader.GetString(2),
                        EnrollDate = reader.GetDateTime(3)
                    };
                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return enrollment;
    }

    public async Task<List<Enrollments>> SearchEnrollmentsAsync(string searchTerm)
    {
        var enrollmentsList = new List<Enrollments>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    e.EnrollmentId, 
                    e.StudentId, 
                    s.StudentName, 
                    e.CourseId, 
                    c.CourseName,
                    e.EnrollDate
                FROM 
                    ENROLLMENTS e
                JOIN 
                    STUDENTS s ON e.StudentId = s.StudentId
                JOIN 
                    COURSES c ON e.CourseId = c.CourseId
                WHERE 
                    UPPER(s.StudentName) LIKE UPPER(:SearchTerm)
                ORDER BY 
                    e.EnrollDate";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("SearchTerm", OracleDbType.Varchar2).Value = "%" + searchTerm + "%";
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var item = new Enrollments
                    {
                        EnrollmentId = reader.GetInt32(0),
                        EnrollDate = reader.GetDateTime(5)
                    };
                    item.Course.CourseId = reader.GetString(3);
                    item.Course.CourseTitle =
                        reader.GetString(4); // Assuming CourseName is the property name in your Courses model
                    item.Student.StudentId = reader.GetInt32(1);
                    item.Student.StudentName = reader.GetString(2);
                    enrollmentsList.Add(item);
                }

                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return enrollmentsList;
    }
}