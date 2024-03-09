using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Students
{
    public List<Students> StudentsList = [];
    public int StudentId { get; init; }
    public string StudentName { get; init; }
    public string Contact { get; init; }
    public DateTime Dob { get; init; }
    public string EmailAddress { get; init; }
    public string Country { get; init; }

    public void GetStudents()
    {
        try
        {
            using var con = new OracleConnection(ValuesConstants.DbString);
            const string queryString =
                "SELECT StudentID, StudentName, Contact, DOB, EmailAddress, Country FROM Students";
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
    }

    public void AddStudent(Students student)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                const string queryString =
                    "INSERT INTO Students (StudentID, StudentName, Contact, DOB, EmailAddress, Country) VALUES (:StudentID, :StudentName, :Contact, :DOB, :EmailAddress, :Country)";
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
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "UPDATE Students SET StudentName = :StudentName, Contact = :Contact, DOB = :DOB, EmailAddress = :EmailAddress, Country = :Country WHERE StudentID = :OldStudentID";
                OracleCommand cmd = new OracleCommand(queryString, con);
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
        List<Students> studentsList = new List<Students>();
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT StudentID, StudentName, Contact, DOB, EmailAddress, Country FROM Students";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Students student = new Students
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
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT StudentID, StudentName, Contact, DOB, EmailAddress, Country FROM Students WHERE StudentID = :StudentID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = studentId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    student = new Students
                    {
                        StudentId = reader.GetInt32(0),
                        StudentName = reader.GetString(1),
                        Contact = reader.GetString(2),
                        Dob = reader.GetDateTime(3),
                        EmailAddress = reader.GetString(4),
                        Country = reader.GetString(5)
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
        return student;
    }

}