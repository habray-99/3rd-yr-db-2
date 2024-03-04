﻿using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;
using roider.Models;

public class QAs
{
    public List<QAs> QAsList = [];
    public int Qaid { get; set; }
    public int StudentId { get; set; }
    public string CourseID { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public DateTime FeedbackDate { get; set; }
    public Students Student { get; set; }
    public Courses Course { get; set; }
    
    public void AddQA(QAs qa)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "INSERT INTO QAs (Qaid, StudentId, CourseID, Question, Answer, FeedbackDate) VALUES (:Qaid, :StudentId, :CourseID, :Question, :Answer, :FeedbackDate)";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("Qaid", OracleDbType.Int32).Value = qa.Qaid;
                cmd.Parameters.Add("StudentId", OracleDbType.Int32).Value = qa.StudentId;
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = qa.CourseID;
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
    public void EditQA(QAs qa, int oldQaId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "UPDATE QAs SET StudentId = :StudentId, CourseID = :CourseID, Question = :Question, Answer = :Answer, FeedbackDate = :FeedbackDate WHERE Qaid = :OldQaid";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("StudentId", OracleDbType.Int32).Value = qa.StudentId;
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = qa.CourseID;
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

    public void DeleteQA(int qaId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "DELETE FROM QAs WHERE Qaid = :Qaid";
                OracleCommand cmd = new OracleCommand(queryString, con);
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
        List<QAs> qasList = new List<QAs>();
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT Qaid, StudentId, CourseID, Question, Answer, FeedbackDate FROM QAs";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    QAs qa = new QAs
                    {
                        Qaid = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        CourseID = reader.GetString(2),
                        Question = reader.GetString(3),
                        Answer = reader.GetString(4),
                        FeedbackDate = reader.GetDateTime(5)
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
    public QAs FetchQAById(int qaId)
    {
        QAs qa = null;
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT Qaid, StudentId, CourseID, Question, Answer, FeedbackDate FROM QAs WHERE Qaid = :Qaid";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("Qaid", OracleDbType.Int32).Value = qaId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    qa = new QAs
                    {
                        Qaid = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        CourseID = reader.GetString(2),
                        Question = reader.GetString(3),
                        Answer = reader.GetString(4),
                        FeedbackDate = reader.GetDateTime(5)
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
        return qa;
    }

}