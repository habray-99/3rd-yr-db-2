using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Progresses
{
    public List<Progresses> ProgressesList = [];
    public int ProgressId { get; init; }
    public int StudentId { get; init; }
    public int LessonId { get; init; }
    public LessonStatus LessonStatus { get; init; }
    public DateTime LastAccessedDate { get; init; }

    public Students? Student { get; set; }
    public Lessons? Lesson { get; set; }
    
    // public void AddProgress(Progresses progress)
    // {
    //     try
    //     {
    //         using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
    //         {
    //             string queryString = "INSERT INTO Progresses (ProgressID, StudentID, LessonID, LessonStatus, LastAccessedDate) VALUES (:ProgressID, :StudentID, :LessonID, :LessonStatus, :LastAccessedDate)";
    //             OracleCommand cmd = new OracleCommand(queryString, con);
    //             cmd.Parameters.Add("ProgressID", OracleDbType.Int32).Value = progress.ProgressId;
    //             cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = progress.StudentId;
    //             cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = progress.LessonId;
    //             cmd.Parameters.Add("LessonStatus", OracleDbType.Varchar2).Value = progress.LessonStatus;
    //             cmd.Parameters.Add("LastAccessedDate", OracleDbType.Date).Value = progress.LastAccessedDate;
    //
    //             con.Open();
    //             cmd.ExecuteNonQuery();
    //             con.Close();
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //     }
    // }
    public async Task AddProgressAsync(Progresses progress)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "INSERT INTO Progress (ProgressID, StudentID, LessonID, LessonStatus, LastAccessedDate) VALUES (:ProgressID, :StudentID, :LessonID, :LessonStatus, :LastAccessedDate)";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("ProgressID", OracleDbType.Int32).Value = progress.ProgressId;
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = progress.StudentId;
                cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = progress.LessonId;
                cmd.Parameters.Add("LessonStatus", OracleDbType.Varchar2).Value = progress.LessonStatus.ToStatusString();
                cmd.Parameters.Add("LastAccessedDate", OracleDbType.Date).Value = progress.LastAccessedDate;

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

// Similar updates should be made to EditProgress, DeleteProgress, FetchProgresses, and FetchProgressById methods.

    // public void EditProgress(Progresses progress, int oldProgressId)
    // {
    //     try
    //     {
    //         using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
    //         {
    //             string queryString = "UPDATE Progresses SET StudentID = :StudentID, LessonID = :LessonID, LessonStatus = :LessonStatus, LastAccessedDate = :LastAccessedDate WHERE ProgressID = :OldProgressID";
    //             OracleCommand cmd = new OracleCommand(queryString, con);
    //             cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = progress.StudentId;
    //             cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = progress.LessonId;
    //             cmd.Parameters.Add("LessonStatus", OracleDbType.Varchar2).Value = progress.LessonStatus.FromStatusString();
    //             cmd.Parameters.Add("LastAccessedDate", OracleDbType.Date).Value = progress.LastAccessedDate;
    //             cmd.Parameters.Add("OldProgressID", OracleDbType.Int32).Value = oldProgressId;
    //
    //             con.Open();
    //             cmd.ExecuteNonQuery();
    //             con.Close();
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //     }
    // }
    public async Task EditProgressAsync(Progresses progress, int oldProgressId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "UPDATE Progress SET StudentID = :StudentID, LessonID = :LessonID, LessonStatus = :LessonStatus, LastAccessedDate = :LastAccessedDate WHERE ProgressID = :OldProgressID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentID", OracleDbType.Int32).Value = progress.StudentId;
                cmd.Parameters.Add("LessonID", OracleDbType.Int32).Value = progress.LessonId;
                cmd.Parameters.Add("LessonStatus", OracleDbType.Varchar2).Value = progress.LessonStatus.ToStatusString();
                cmd.Parameters.Add("LastAccessedDate", OracleDbType.Date).Value = progress.LastAccessedDate;
                cmd.Parameters.Add("OldProgressID", OracleDbType.Int32).Value = oldProgressId;

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

    public void DeleteProgress(int progressId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "DELETE FROM Progress WHERE ProgressID = :ProgressID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("ProgressID", OracleDbType.Int32).Value = progressId;

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

    // public List<Progresses> FetchProgresses()
    // {
    //     List<Progresses> progressesList = new List<Progresses>();
    //     try
    //     {
    //         using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
    //         {
    //             string queryString = "SELECT ProgressID, StudentID, LessonID, LessonStatus, LastAccessedDate FROM Progresses";
    //             OracleCommand cmd = new OracleCommand(queryString, con);
    //             cmd.BindByName = true;
    //             cmd.CommandType = CommandType.Text;
    //
    //             con.Open();
    //             OracleDataReader reader = cmd.ExecuteReader();
    //             while (reader.Read())
    //             {
    //                 Progresses progress = new Progresses
    //                 {
    //                     ProgressId = reader.GetInt32(0),
    //                     StudentId = reader.GetInt32(1),
    //                     LessonId = reader.GetInt32(2),
    //                     LessonStatus = reader.GetString(3),
    //                     LastAccessedDate = reader.GetDateTime(4)
    //                 };
    //                 progressesList.Add(progress);
    //             }
    //             reader.Dispose();
    //             con.Close();
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //     }
    //     return progressesList;
    // }
    public async Task<List<Progresses>> FetchProgressesAsync()
    {
        List<Progresses> progressesList = new List<Progresses>();
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT ProgressID, StudentID, LessonID, LessonStatus, LastAccessedDate FROM Progress";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                OracleDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Progresses progress = new Progresses
                    {
                        ProgressId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        LessonId = reader.GetInt32(2),
                        LessonStatus = LessonStatusExtensions.FromStatusString(reader.GetString(3)),
                        LastAccessedDate = reader.GetDateTime(4)
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

    public Progresses FetchProgressById(int progressId)
    {
        Progresses progress = null;
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT ProgressID, StudentID, LessonID, LessonStatus, LastAccessedDate FROM Progress WHERE ProgressID = :ProgressID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("ProgressID", OracleDbType.Int32).Value = progressId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                 con.OpenAsync();
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    progress = new Progresses
                    {
                        ProgressId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        LessonId = reader.GetInt32(2),
                        LessonStatus = LessonStatusExtensions.FromStatusString(reader.GetString(3)),
                        LastAccessedDate = reader.GetDateTime(4)
                    };
                }
                reader.Dispose();
                 con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return progress;
    }

}