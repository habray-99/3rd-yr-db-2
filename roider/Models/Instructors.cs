using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Instructors
{
    public List<Instructors>? InstructorsList = [];
    public string? InstructorId { get; set; }
    public string? InstructorName { get; set; }
    public string? Contact { get; set; }
    public string? EmailAddress { get; set; }
    public string? Specialization { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? Country { get; set; }

    public List<Courses>? Courses { get; set; } = [];

    public void AddInstructor(Instructors instructor)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "INSERT INTO Instructors (InstructorID, InstructorName, Contact, EmailAddress, Specialization, YearsOfExperience, Country_CODE) VALUES (:InstructorID, :InstructorName, :Contact, :EmailAddress, :Specialization, :YearsOfExperience, :Country)";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("InstructorID", OracleDbType.Varchar2).Value = instructor.InstructorId;
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

    public void EditInstructor(Instructors instructor, string oldInstructorId)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "UPDATE Instructors SET InstructorID = :InstructorID,InstructorName = :InstructorName, Contact = :Contact, EmailAddress = :EmailAddress, Specialization = :Specialization, YearsOfExperience = :YearsOfExperience, Country_CODE = :Country WHERE InstructorID = :OldInstructorID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("InstructorID", OracleDbType.Varchar2).Value = instructor.InstructorId;
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

    public void DeleteInstructor(string instructorId)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "DELETE FROM Instructors WHERE InstructorID = :InstructorID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("InstructorID", OracleDbType.Varchar2).Value = instructorId;

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
        var instructorsList = new List<Instructors>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT InstructorID, InstructorName, Contact, EmailAddress, Specialization, YearsOfExperience, Country_CODE FROM Instructors";
                var cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var instructor = new Instructors
                    {
                        InstructorId = reader.GetString(0),
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

    public Instructors FetchInstructorById(string? instructorId)
    {
        Instructors instructor = null;
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "SELECT InstructorID, InstructorName, Contact, EmailAddress, Specialization, YearsOfExperience, Country_CODE FROM Instructors WHERE InstructorID = :InstructorID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("InstructorID", OracleDbType.Varchar2).Value = instructorId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    instructor = new Instructors
                    {
                        InstructorId = reader.GetString(0),
                        InstructorName = reader.GetString(1),
                        Contact = reader.GetString(2),
                        EmailAddress = reader.GetString(3),
                        Specialization = reader.GetString(4),
                        YearsOfExperience = reader.GetInt32(5),
                        Country = reader.GetString(6)
                    };
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

    public async Task<List<Instructors>> SearchInstructorsAsync(string searchTerm)
    {
        var instructorsList = new List<Instructors>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    i.InstructorId, 
                    i.InstructorName, 
                    i.Contact, 
                    i.EmailAddress, 
                    i.Specialization, 
                    i.YearsOfExperience, 
                    i.Country
                FROM 
                    INSTRUCTORS i
                WHERE 
                    UPPER(i.InstructorName) LIKE UPPER(:SearchTerm)
                ORDER BY 
                    i.InstructorName";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("SearchTerm", OracleDbType.Varchar2).Value = "%" + searchTerm + "%";
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    instructorsList.Add(new Instructors
                    {
                        InstructorId = reader.GetString(0),
                        InstructorName = reader.GetString(1),
                        Contact = reader.GetString(2),
                        EmailAddress = reader.GetString(3),
                        Specialization = reader.GetString(4),
                        YearsOfExperience = reader.GetInt32(5),
                        Country = reader.GetString(6)
                    });
                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return instructorsList;
    }
}