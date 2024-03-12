using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Enrollments
{
    public int EnrollmentId { get; init; }
    public int StudentId { get; init; }
    public string? CourseId { get; init; }
    public DateTime EnrollDate { get; set; }


    public List<Enrollments> EnrollmentsList = [];
    public Students? Student { get; set; }
    public Courses? Course { get; set; }

    public List<Enrollments> GetEnrollments()
    {
        try
        {
            using var con = new OracleConnection(ValuesConstants.DbString);
            const string queryString =
                "SELECT EnrollmentId, StudentId, CourseId, EnrollDate FROM Enrollment";
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
                    CourseId = reader.IsDBNull(2) ? null : reader.GetString(2),
                    EnrollDate = reader.GetDateTime(3)
                };
                EnrollmentsList.Add(enrollment);
            }

            reader.Dispose();
            con.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return EnrollmentsList;
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
                cmd.Parameters.Add("EnrollDate", OracleDbType.Date).Value = enrollment.EnrollDate; //enrollment.EnrollDate;

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
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "UPDATE Enrollment SET StudentId = :StudentId, CourseId = :CourseId, EnrollDate = :EnrollDate WHERE EnrollmentId = :OldEnrollmentID";
                OracleCommand cmd = new OracleCommand(queryString, con);
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
    public List<Enrollments> FetchEnrollmentsByStudentId(int studentId)
    {
        List<Enrollments> enrollmentsList = [];
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT EnrollmentId, StudentId, CourseId, EnrollDate FROM Enrollment WHERE StudentId = :StudentId";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentId", OracleDbType.Int32).Value = studentId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Enrollments enrollment = new Enrollments
                    {
                        EnrollmentId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        CourseId = reader.IsDBNull(2) ? null : reader.GetString(2),
                        EnrollDate = reader.GetDateTime(3)
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
}
