using System.Data;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Lessons
{
    public List<Lessons> LessonsList = [];
    public int LessonId { get; set; }
    public string LessonTitle { get; set; }
    public string LessonContentType { get; set; }

    public string CourseId { get; set; }

    public Courses? Course { get; set; }

    public void AddLesson(Lessons lesson)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "INSERT INTO Lessons (LessonID, LessonTitle, LessonContentType, CourseID) VALUES (:LessonID, :LessonTitle, :LessonContentType, :CourseID)";
                var cmd = new OracleCommand(queryString, con);
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
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "UPDATE Lessons SET LessonTitle = :LessonTitle, LessonContentType = :LessonContentType, CourseID = :CourseID WHERE LessonID = :OldLessonID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("LessonTitle", OracleDbType.Varchar2).Value = lesson.LessonTitle;
                cmd.Parameters.Add("LessonContentType", OracleDbType.Varchar2).Value = lesson.LessonContentType;
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = lesson.CourseId;
                cmd.Parameters.Add("OldLessonID", OracleDbType.Int32).Value = oldLessonId;
                Debug.WriteLine("lesson: " + lesson);
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
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "DELETE FROM Lessons WHERE LessonID = :LessonID";
                var cmd = new OracleCommand(queryString, con);
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
        var lessonsList = new List<Lessons>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    l.LessonID, 
                    l.LessonTitle, 
                    l.LessonContentType, 
                    l.CourseID, 
                    c.CourseTitle
                FROM 
                    Lessons l
                JOIN 
                    Courses c ON l.CourseID = c.CourseID 
                    order by l.LessonID";
                var cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var lesson = new Lessons
                    {
                        LessonId = reader.GetInt32(0),
                        LessonTitle = reader.GetString(1),
                        LessonContentType = reader.GetString(2),
                        CourseId = reader.GetString(3),
                        Course = new Courses
                        {
                            CourseId = reader.GetString(3), // Assuming CourseId is the same in both tables
                            CourseTitle = reader.GetString(4)
                        }
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

    public List<Lessons> FetchLessonsByCourse(string id)
    {
        var lessonsList = new List<Lessons>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT LessonID, LessonTitle, LessonContentType FROM Lessons WHERE courseid = :CourseID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = id; // Corrected parameter name and type
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var lesson = new Lessons
                    {
                        LessonId = reader.GetInt32(0),
                        LessonTitle = reader.GetString(1),
                        LessonContentType = reader.GetString(2)
                        // Assuming CourseId is not directly available in the Lessons table and is inferred from the courseid parameter
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
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT LessonID, LessonTitle, LessonContentType, CourseID FROM Lessons WHERE LessonID = :LessonID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = lessonId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    lesson = new Lessons
                    {
                        LessonId = reader.GetInt32(0),
                        LessonTitle = reader.GetString(1),
                        LessonContentType = reader.GetString(2),
                        CourseId = reader.GetString(3)
                    };
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

    public async Task<List<Lessons>> SearchLessonsAsync(string searchTerm)
    {
        var lessonsList = new List<Lessons>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    l.LessonId, 
                    l.LessonTitle, 
                    l.LessonContentType, 
                    l.CourseId
                FROM 
                    LESSONS l
                WHERE 
                    UPPER(l.LessonTitle) LIKE UPPER(:SearchTerm)
                ORDER BY 
                    l.LessonTitle";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("SearchTerm", OracleDbType.Varchar2).Value = "%" + searchTerm + "%";
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    lessonsList.Add(new Lessons
                    {
                        LessonId = reader.GetInt32(0),
                        LessonTitle = reader.GetString(1),
                        LessonContentType = reader.GetString(2),
                        CourseId = reader.GetString(3)
                    });
                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return lessonsList;
    }
}