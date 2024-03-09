using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Courses
{
    public List<Courses> CoursesList = [];
    public string CourseId { get; init; }
    public string CourseTitle { get; init; }
    public void AddCourse(Courses course)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "INSERT INTO Courses (CourseID, CourseTitle) VALUES (:CourseID, :CourseTitle)";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = course.CourseId;
                cmd.Parameters.Add("CourseTitle", OracleDbType.Varchar2).Value = course.CourseTitle;

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
    public void EditCourse(Courses course, string oldCourseId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "UPDATE Courses SET CourseID = :NewCourseID, CourseTitle = :CourseTitle WHERE CourseID = :OldCourseID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("NewCourseID", OracleDbType.Varchar2).Value = course.CourseId;
                cmd.Parameters.Add("CourseTitle", OracleDbType.Varchar2).Value = course.CourseTitle;
                cmd.Parameters.Add("OldCourseID", OracleDbType.Varchar2).Value = oldCourseId;

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

    public void DeleteCourse(string courseId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "DELETE FROM Courses WHERE CourseID = :CourseID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = courseId;

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
    public List<Courses> FetchCourses()
    {
        List<Courses> coursesList = new List<Courses>();
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT CourseID, CourseTitle FROM Courses";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Courses course = new Courses
                    {
                        CourseId = reader.GetString(0),
                        CourseTitle = reader.GetString(1)
                    };
                    coursesList.Add(course);
                }
                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return coursesList;
    }
    public Courses FetchCourseById(string courseId)
    {
        Courses course = null;
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT CourseID, CourseTitle FROM Courses WHERE CourseID = :CourseID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = courseId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    course = new Courses
                    {
                        CourseId = reader.GetString(0),
                        CourseTitle = reader.GetString(1)
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
        return course;
    }

}