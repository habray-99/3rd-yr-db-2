using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class QAs
{
    public List<QAs> QAsList = [];
    public int Qaid { get; set; }
    public int StudentId { get; set; }
    public string CourseId { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public DateTime FeedbackDate { get; set; }

    public Students? Student { get; set; }
    public Courses? Course { get; set; }

    public void AddQa(QAs qa)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "INSERT INTO QA (Qaid, StudentId, CourseID, Question, Answer, FeedbackDate) VALUES (:Qaid, :StudentId, :CourseID, :Question, :Answer, :FeedbackDate)";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("Qaid", OracleDbType.Int32).Value = qa.Qaid;
                cmd.Parameters.Add("StudentId", OracleDbType.Int32).Value = qa.StudentId;
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = qa.CourseId;
                cmd.Parameters.Add("Question", OracleDbType.Varchar2).Value = qa.Question;
                cmd.Parameters.Add("Answer", OracleDbType.Varchar2).Value = qa.Answer;
                cmd.Parameters.Add("FeedbackDate", OracleDbType.Date).Value = qa.FeedbackDate;

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

    public void EditQa(QAs qa, int oldQaId)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "UPDATE QA SET StudentId = :StudentId, CourseID = :CourseID, Question = :Question, Answer = :Answer, FeedbackDate = :FeedbackDate WHERE Qaid = :OldQaid";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentId", OracleDbType.Int32).Value = qa.StudentId;
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = qa.CourseId;
                cmd.Parameters.Add("Question", OracleDbType.Varchar2).Value = qa.Question;
                cmd.Parameters.Add("Answer", OracleDbType.Varchar2).Value = qa.Answer;
                cmd.Parameters.Add("FeedbackDate", OracleDbType.Date).Value = qa.FeedbackDate;
                cmd.Parameters.Add("OldQaid", OracleDbType.Int32).Value = oldQaId;

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

    public void DeleteQa(int qaId)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "DELETE FROM QA WHERE Qaid = :Qaid";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("Qaid", OracleDbType.Int32).Value = qaId;

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

    public List<QAs> FetchQAs()
    {
        var qasList = new List<QAs>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    q.Qaid, 
                    q.StudentId, 
                    s.StudentName, 
                    q.CourseID, 
                    c.CourseTitle, 
                    q.Question, 
                    q.Answer, 
                    q.FeedbackDate
                FROM 
                    QA q
                JOIN 
                    Students s ON q.StudentId = s.StudentId
                JOIN 
                    Courses c ON q.CourseID = c.CourseID";
                var cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var qa = new QAs
                    {
                        Qaid = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        Student = new Students
                        {
                            StudentId = reader.GetInt32(1),
                            StudentName = reader.GetString(2)
                        },
                        CourseId = reader.GetString(3),
                        Course = new Courses
                        {
                            CourseId = reader.GetString(3),
                            CourseTitle = reader.GetString(4)
                        },
                        Question = reader.GetString(5),
                        Answer = reader.GetString(6),
                        FeedbackDate = reader.GetDateTime(7)
                    };
                    qasList.Add(qa);
                }

                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return qasList;
    }

    public QAs FetchQaById(int qaId)
    {
        QAs qa = null;
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT Qaid, StudentId, CourseID, Question, Answer, FeedbackDate FROM QA WHERE Qaid = :Qaid";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("Qaid", OracleDbType.Int32).Value = qaId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    qa = new QAs
                    {
                        Qaid = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        CourseId = reader.GetString(2),
                        Question = reader.GetString(3),
                        Answer = reader.GetString(4),
                        FeedbackDate = reader.GetDateTime(5)
                    };
                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return qa;
    }

    public async Task<List<QAs>> SearchQAsAsync(string searchTerm)
    {
        var qasList = new List<QAs>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    q.Qaid, 
                    s.StudentName, 
                    c.CourseName, 
                    q.Question, 
                    q.Answer, 
                    q.FeedbackDate
                FROM 
                    QAS q
                JOIN 
                    STUDENTS s ON q.StudentId = s.StudentId
                JOIN 
                    COURSES c ON q.CourseId = c.CourseId
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
                    qasList.Add(new QAs
                    {
                        Qaid = reader.GetInt32(0),
                        Student = new Students { StudentName = reader.GetString(1) },
                        Course = new Courses { CourseTitle = reader.GetString(2) },
                        Question = reader.GetString(3),
                        Answer = reader.GetString(4),
                        FeedbackDate = reader.GetDateTime(5)
                    });
                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return qasList;
    }
}