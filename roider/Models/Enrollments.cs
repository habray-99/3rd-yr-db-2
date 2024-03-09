using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Enrollments
{
    public int EnrollmentId { get; init; }
    public int StudentId { get; init; }
    public string CourseId { get; init; }
    public DateTime EnrollDate { get; init; }


    public List<Enrollments> EnrollmentsList = [];
    public Students Student { get; init; }
    public Courses Course { get; init; }
    
    public void AddEnrollment(Enrollments enrollment)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "INSERT INTO Enrollments (EnrollmentID, StudentID, CourseID, EnrollDate) VALUES (:EnrollmentID, :StudentID, :CourseID, :EnrollDate)";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("EnrollmentID", OracleDbType.Int32).Value = enrollment.EnrollmentId;
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = enrollment.StudentId;
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = enrollment.CourseId;
                cmd.Parameters.Add("EnrollDate", OracleDbType.Date).Value = enrollment.EnrollDate;

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

    public void EditEnrollment(Enrollments enrollment, int oldEnrollmentId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "UPDATE Enrollments SET StudentID = :StudentID, CourseID = :CourseID, EnrollDate = :EnrollDate WHERE EnrollmentID = :OldEnrollmentID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = enrollment.StudentId;
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = enrollment.CourseId;
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

    public void DeleteEnrollment(int enrollmentId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "DELETE FROM Enrollments WHERE EnrollmentID = :EnrollmentID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("EnrollmentID", OracleDbType.Int32).Value = enrollmentId;

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
    public List<Enrollments> FetchEnrollments()
    {
        List<Enrollments> enrollmentsList = new List<Enrollments>();
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT EnrollmentID, StudentID, CourseID, EnrollDate FROM Enrollment";
                OracleCommand cmd = new OracleCommand(queryString, con);
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
                        CourseId = reader.GetString(2),
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
    public Enrollments FetchEnrollmentById(int enrollmentId)
    {
        Enrollments enrollment = null;
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT EnrollmentID, StudentID, CourseID, EnrollDate FROM Enrollments WHERE EnrollmentID = :EnrollmentID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("EnrollmentID", OracleDbType.Int32).Value = enrollmentId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    enrollment = new Enrollments
                    {
                        EnrollmentId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        CourseId = reader.GetString(2),
                        EnrollDate = reader.GetDateTime(3)
                    };
                }
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

}