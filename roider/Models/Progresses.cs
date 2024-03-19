using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Progresses
{
    public List<Progresses> ProgressesList = new();
    public int ProgressId { get; set; }
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public LessonStatus LessonStatus { get; set; }
    public DateTime LastAccessedDate { get; set; }
    public string CourseId { get; set; }

    public Students? Student { get; set; }
    public Lessons? Lesson { get; set; }
    public Courses? Course { get; set; }

    public async Task<List<Progresses>> FetchProgressesByStudentIdAsync(int studentId)
    {
        var progressesList = new List<Progresses>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT ProgressID, StudentID, LessonID, LessonStatus, LastAccessedDate, courseid FROM Progress WHERE StudentID = :StudentID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = studentId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var progress = new Progresses
                    {
                        ProgressId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        LessonId = reader.GetInt32(2),
                        LessonStatus =
                            LessonStatusExtensions.FromStatusString(reader.GetString(3)), // Use extension method
                        LastAccessedDate = reader.GetDateTime(4),
                        CourseId = reader.GetString(5) // Directly get string
                    };
                    progressesList.Add(progress);
                }

                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return progressesList;
    }

    public async Task AddProgressAsync(Progresses progress)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "INSERT INTO Progress (StudentID, LessonID, LessonStatus, LastAccessedDate, courseid) VALUES (:StudentID, :LessonID, :LessonStatus, :LastAccessedDate, :courseid)";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = progress.StudentId;
                cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = progress.LessonId;
                cmd.Parameters.Add("LessonStatus", OracleDbType.Varchar2).Value =
                    progress.LessonStatus.ToStatusString(); // Use extension method
                cmd.Parameters.Add("LastAccessedDate", OracleDbType.Date).Value = progress.LastAccessedDate;
                cmd.Parameters.Add("courseid", OracleDbType.Varchar2).Value = progress.CourseId; // Directly use string

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task EditProgressAsync(Progresses progress)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "UPDATE Progress SET StudentID = :StudentID, LessonID = :LessonID, LessonStatus = :LessonStatus, LastAccessedDate = :LastAccessedDate, courseid = :courseid WHERE ProgressID = :ProgressID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = progress.StudentId;
                cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = progress.LessonId;
                cmd.Parameters.Add("LessonStatus", OracleDbType.Varchar2).Value =
                    progress.LessonStatus.ToStatusString(); // Use extension method
                cmd.Parameters.Add("LastAccessedDate", OracleDbType.Date).Value = progress.LastAccessedDate;
                cmd.Parameters.Add("courseid", OracleDbType.Varchar2).Value = progress.CourseId; // Directly use string
                cmd.Parameters.Add("ProgressID", OracleDbType.Int32).Value = progress.ProgressId;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<List<Progresses>> FetchProgressesAsync()
    {
        var progressesList = new List<Progresses>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    p.ProgressId, 
                    p.StudentId, 
                    s.StudentName, 
                    p.LessonId, 
                    l.LessonTitle, 
                    p.LessonStatus, 
                    p.LastAccessedDate,
                    p.courseid
                FROM 
                    Progress p
                JOIN 
                    Students s ON p.StudentId = s.StudentId
                JOIN 
                    Lessons l ON p.LessonId = l.LessonId";
                var cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var progress = new Progresses
                    {
                        ProgressId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        Student = new Students
                        {
                            StudentId = reader.GetInt32(1),
                            StudentName = reader.GetString(2)
                        },
                        LessonId = reader.GetInt32(3),
                        Lesson = new Lessons
                        {
                            LessonId = reader.GetInt32(3),
                            LessonTitle = reader.GetString(4)
                        },
                        LessonStatus =
                            LessonStatusExtensions.FromStatusString(reader.GetString(5)), // Use extension method
                        LastAccessedDate = reader.GetDateTime(6),
                        CourseId = reader.GetString(7) // Directly get string
                    };
                    progressesList.Add(progress);
                }

                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return progressesList;
    }

    public async Task<Progresses> FetchProgressByIdAsync(int progressId)
    {
        Progresses progress = null;
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT ProgressID, StudentID, LessonID, LessonStatus, LastAccessedDate, courseid FROM Progress WHERE ProgressID = :ProgressID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("ProgressID", OracleDbType.Int32).Value = progressId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    progress = new Progresses
                    {
                        ProgressId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        LessonId = reader.GetInt32(2),
                        LessonStatus =
                            LessonStatusExtensions.FromStatusString(reader.GetString(3)), // Use extension method
                        LastAccessedDate = reader.GetDateTime(4),
                        CourseId = reader.GetString(5) // Directly get string
                    };
                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return progress;
    }

    public async Task DeleteProgressAsync(int progressId)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "DELETE FROM Progress WHERE ProgressID = :ProgressID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("ProgressID", OracleDbType.Int32).Value = progressId;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<List<Progresses>> SearchProgressesAsync(string searchTerm)
    {
        var progressesList = new List<Progresses>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    p.ProgressId, 
                    s.StudentName, 
                    l.LessonTitle, 
                    c.CourseName,
                    p.courseid
                FROM 
                    PROGRESSES p
                JOIN 
                    STUDENTS s ON p.StudentId = s.StudentId
                JOIN 
                    LESSONS l ON p.LessonId = l.LessonId
                JOIN 
                    COURSES c ON l.CourseId = c.CourseId
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
                    progressesList.Add(new Progresses
                    {
                        ProgressId = reader.GetInt32(0),
                        Student = new Students { StudentName = reader.GetString(1) },
                        Lesson = new Lessons { LessonTitle = reader.GetString(2) },
                        Course = new Courses
                        {
                            CourseTitle = reader.GetString(3)
                        }, // Assuming CourseTitle is the property to store the course name
                        CourseId = reader.GetString(4) // Directly get string
                    });
                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return progressesList;
    }
}