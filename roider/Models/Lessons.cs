using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Lessons
{
    public List<Lessons> LessonsList = [];
    public int LessonId { get; init; }
    public string LessonTitle { get; init; }
    public string LessonContentType { get; init; }

    public string CourseId { get; init; }

    public Courses? Course { get; set; }
    
    public void AddLesson(Lessons lesson)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "INSERT INTO Lessons (LessonID, LessonTitle, LessonContentType, CourseID) VALUES (:LessonID, :LessonTitle, :LessonContentType, :CourseID)";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = lesson.LessonId;
                cmd.Parameters.Add("LessonTitle", OracleDbType.Varchar2).Value = lesson.LessonTitle;
                cmd.Parameters.Add("LessonContentType", OracleDbType.Varchar2).Value = lesson.LessonContentType;
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = lesson.CourseId;

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
    public void EditLesson(Lessons lesson, int oldLessonId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "UPDATE Lessons SET LessonTitle = :LessonTitle, LessonContentType = :LessonContentType, CourseID = :CourseID WHERE LessonID = :OldLessonID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("LessonTitle", OracleDbType.Varchar2).Value = lesson.LessonTitle;
                cmd.Parameters.Add("LessonContentType", OracleDbType.Varchar2).Value = lesson.LessonContentType;
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = lesson.CourseId;
                cmd.Parameters.Add("OldLessonID", OracleDbType.Int32).Value = oldLessonId;
                System.Diagnostics.Debug.WriteLine("lesson: " + lesson.ToString());
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
    public void DeleteLesson(int lessonId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "DELETE FROM Lessons WHERE LessonID = :LessonID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = lessonId;

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
    public List<Lessons> FetchLessons()
    {
        List<Lessons> lessonsList = new List<Lessons>();
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT LessonID, LessonTitle, LessonContentType, CourseID FROM Lessons";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Lessons lesson = new Lessons
                    {
                        LessonId = reader.GetInt32(0),
                        LessonTitle = reader.GetString(1),
                        LessonContentType = reader.GetString(2),
                        CourseId = reader.GetString(3)
                    };
                    lessonsList.Add(lesson);
                }
                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return lessonsList;
    }
    public Lessons FetchLessonById(int lessonId)
    {
        Lessons lesson = null;
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT LessonID, LessonTitle, LessonContentType, CourseID FROM Lessons WHERE LessonID = :LessonID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = lessonId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lesson = new Lessons
                    {
                        LessonId = reader.GetInt32(0),
                        LessonTitle = reader.GetString(1),
                        LessonContentType = reader.GetString(2),
                        CourseId = reader.GetString(3)
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
        return lesson;
    }

}