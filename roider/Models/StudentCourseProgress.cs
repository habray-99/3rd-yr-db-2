using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class StudentCourseProgress
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public string CourseTitle { get; set; }
    public string LessonTitle { get; set; }
    public string LessonStatus { get; set; }
    public DateTime LastAccessedDate { get; set; }

    public async Task<List<StudentCourseProgress>> FetchStudentCourseProgressAsync(int studentId)
    {
        var courseProgressList = new List<StudentCourseProgress>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    s.STUDENTID,
                    s.STUDENTNAME,
                    c.COURSETITLE,
                    l.LESSONTITLE,
                    p.LESSONSTATUS,
                    p.LASTACCESSEDDATE
                FROM 
                    STUDENTS s
                JOIN 
                    PROGRESS p ON s.STUDENTID = p.STUDENTID
                JOIN 
                    LESSONS l ON p.LESSONID = l.LESSONID
                JOIN 
                    COURSES c ON l.COURSEID = c.COURSEID
                WHERE 
                    s.STUDENTID = :StudentID
                ORDER BY 
                    s.STUDENTID, c.COURSETITLE, l.LESSONTITLE";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = studentId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    courseProgressList.Add(new StudentCourseProgress
                    {
                        StudentId = reader.GetInt32(0),
                        StudentName = reader.GetString(1),
                        CourseTitle = reader.GetString(2),
                        LessonTitle = reader.GetString(3),
                        LessonStatus = reader.GetString(4),
                        LastAccessedDate = reader.GetDateTime(5)
                    });
                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return courseProgressList;
    }
}