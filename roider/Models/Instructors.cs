using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Instructors
{
    public List<Instructors> InstructorsList = [];
    public int InstructorID { get; set; }
    public string InstructorName { get; set; }
    public string Contact { get; set; }
    public string EmailAddress { get; set; }
    public string Specialization { get; set; }
    public int YearsOfExperience { get; set; }
    public string Country { get; set; }
    
    public void AddInstructor(Instructors instructor)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "INSERT INTO Instructors (InstructorID, InstructorName, Contact, EmailAddress, Specialization, YearsOfExperience, Country) VALUES (:InstructorID, :InstructorName, :Contact, :EmailAddress, :Specialization, :YearsOfExperience, :Country)";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("InstructorID", OracleDbType.Int32).Value = instructor.InstructorID;
                cmd.Parameters.Add("InstructorName", OracleDbType.Varchar2).Value = instructor.InstructorName;
                cmd.Parameters.Add("Contact", OracleDbType.Varchar2).Value = instructor.Contact;
                cmd.Parameters.Add("EmailAddress", OracleDbType.Varchar2).Value = instructor.EmailAddress;
                cmd.Parameters.Add("Specialization", OracleDbType.Varchar2).Value = instructor.Specialization;
                cmd.Parameters.Add("YearsOfExperience", OracleDbType.Int32).Value = instructor.YearsOfExperience;
                cmd.Parameters.Add("Country", OracleDbType.Varchar2).Value = instructor.Country;

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
    public void EditInstructor(Instructors instructor, int oldInstructorId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "UPDATE Instructors SET InstructorName = :InstructorName, Contact = :Contact, EmailAddress = :EmailAddress, Specialization = :Specialization, YearsOfExperience = :YearsOfExperience, Country = :Country WHERE InstructorID = :OldInstructorID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("InstructorName", OracleDbType.Varchar2).Value = instructor.InstructorName;
                cmd.Parameters.Add("Contact", OracleDbType.Varchar2).Value = instructor.Contact;
                cmd.Parameters.Add("EmailAddress", OracleDbType.Varchar2).Value = instructor.EmailAddress;
                cmd.Parameters.Add("Specialization", OracleDbType.Varchar2).Value = instructor.Specialization;
                cmd.Parameters.Add("YearsOfExperience", OracleDbType.Int32).Value = instructor.YearsOfExperience;
                cmd.Parameters.Add("Country", OracleDbType.Varchar2).Value = instructor.Country;
                cmd.Parameters.Add("OldInstructorID", OracleDbType.Int32).Value = oldInstructorId;

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
    public void DeleteInstructor(int instructorId)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "DELETE FROM Instructors WHERE InstructorID = :InstructorID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("InstructorID", OracleDbType.Int32).Value = instructorId;

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
    public List<Instructors> FetchInstructors()
    {
        List<Instructors> instructorsList = new List<Instructors>();
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT InstructorID, InstructorName, Contact, EmailAddress, Specialization, YearsOfExperience, Country FROM Instructors";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Instructors instructor = new Instructors
                    {
                        InstructorID = reader.GetInt32(0),
                        InstructorName = reader.GetString(1),
                        Contact = reader.GetString(2),
                        EmailAddress = reader.GetString(3),
                        Specialization = reader.GetString(4),
                        YearsOfExperience = reader.GetInt32(5),
                        Country = reader.GetString(6)
                    };
                    instructorsList.Add(instructor);
                }
                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return instructorsList;
    }
    public Instructors FetchInstructorById(int instructorId)
    {
        Instructors instructor = null;
        try
        {
            using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
            {
                string queryString = "SELECT InstructorID, InstructorName, Contact, EmailAddress, Specialization, YearsOfExperience, Country FROM Instructors WHERE InstructorID = :InstructorID";
                OracleCommand cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("InstructorID", OracleDbType.Int32).Value = instructorId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    instructor = new Instructors
                    {
                        InstructorID = reader.GetInt32(0),
                        InstructorName = reader.GetString(1),
                        Contact = reader.GetString(2),
                        EmailAddress = reader.GetString(3),
                        Specialization = reader.GetString(4),
                        YearsOfExperience = reader.GetInt32(5),
                        Country = reader.GetString(6)
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
        return instructor;
    }

}