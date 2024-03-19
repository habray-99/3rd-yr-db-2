using System.Data;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Courses
{
    public List<Courses> CoursesList = [];
    public string CourseId { get; set; }
    public string CourseTitle { get; set; }
    public string CourseTeacher { get; set; }
    public int? EnrollmentCount { get; set; }
    public List<Instructors> Instructors { get; set; } = [];

    public void AddCourse(Courses course)
    {
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "INSERT INTO Courses (CourseID, CourseTitle, TEACHERID) VALUES (:CourseID, :CourseTitle, :TeacherId)";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = course.CourseId;
                cmd.Parameters.Add("CourseTitle", OracleDbType.Varchar2).Value = course.CourseTitle;
                cmd.Parameters.Add("TeacherId", OracleDbType.Varchar2).Value = course.CourseTeacher;

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
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString =
                    "UPDATE Courses SET CourseID = :NewCourseID, CourseTitle = :CourseTitle, TEACHERID = :TeacherId WHERE CourseID = :OldCourseID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("NewCourseID", OracleDbType.Varchar2).Value = course.CourseId;
                cmd.Parameters.Add("CourseTitle", OracleDbType.Varchar2).Value = course.CourseTitle;
                cmd.Parameters.Add("TeacherId", OracleDbType.Varchar2).Value = course.CourseTeacher;
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
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "DELETE FROM Courses WHERE CourseID = :CourseID";
                var cmd = new OracleCommand(queryString, con);
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
        var coursesList = new List<Courses>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "SELECT CourseID, CourseTitle FROM Courses";
                var cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var course = new Courses
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
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "SELECT CourseID, CourseTitle FROM Courses WHERE CourseID = :CourseID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = courseId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    course = new Courses
                    {
                        CourseId = reader.GetString(0),
                        CourseTitle = reader.GetString(1)
                    };
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

    public Courses FetchCourseDetailsAndInstructors(string courseId)
    {
        Courses course = null;
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "SELECT CourseID, CourseTitle FROM Courses WHERE CourseID = :CourseID";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("CourseID", OracleDbType.Varchar2).Value = courseId;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    course = new Courses
                    {
                        CourseId = reader.GetString(0),
                        CourseTitle = reader.GetString(1)
                    };
                reader.Dispose();
                con.Close();
            }

            // Fetch instructors for the course
            if (course != null) course.Instructors = FetchInstructorsForCourse(courseId);
            // Filter instructors to only include those engaged in at least two courses
            // course.Instructors = course.Instructors.Where(i => i.Courses.Count >= 2).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return course;
    }

    private List<Instructors> FetchInstructorsForCourse(string courseId)
    {
        var instructors = new List<Instructors>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT i.INSTRUCTORID, i.INSTRUCTORNAME
                FROM INSTRUCTORS i
                WHERE (
                    SELECT COUNT(*)
                    FROM COURSES c
                    WHERE c.TEACHERID = i.INSTRUCTORID
                ) >= 2";
                var cmd = new OracleCommand(queryString, con);
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    instructors.Add(new Instructors
                    {
                        InstructorId = reader.GetString(0),
                        InstructorName = reader.GetString(1)
                    });
                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return instructors;
    }


// public List<Courses> GetTop3CoursesByEnrollment(int year, int month)
// {
//     List<Courses> topCourses = new List<Courses>();
//     try
//     {
//         using (OracleConnection con = new OracleConnection(ValuesConstants.DbString))
//         {
//             string queryString = @"
//                 SELECT c.COURSEID, c.COURSETITLE, COUNT(e.ENROLLMENTID) AS ENROLLMENT_COUNT
//                 FROM COURSES c
//                 JOIN ENROLLMENT e ON c.COURSEID = e.COURSEID
//                 WHERE EXTRACT(YEAR FROM e.ENROLLDATE) = :Year
//                 AND EXTRACT(MONTH FROM e.ENROLLDATE) = :Month
//                 GROUP BY c.COURSEID, c.COURSETITLE
//                 ORDER BY ENROLLMENT_COUNT DESC
//                 FETCH FIRST 3 ROWS ONLY";
//             OracleCommand cmd = new OracleCommand(queryString, con);
//             cmd.Parameters.Add("Year", OracleDbType.Int32).Value = year;
//             cmd.Parameters.Add("Month", OracleDbType.Int32).Value = month;
//             cmd.BindByName = true;
//             cmd.CommandType = CommandType.Text;
//
//             con.Open();
//             OracleDataReader reader = cmd.ExecuteReader();
//             while (reader.Read())
//             {
//                 topCourses.Add(new Courses
//                 {
//                     CourseId = reader.GetString(0),
//                     CourseTitle = reader.GetString(1),
//                     EnrollmentCount = reader.GetInt32(2)
//                 });
//             }
//             reader.Dispose();
//             con.Close();
//         }
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine(ex.Message);
//     }
//     return topCourses;
// }
    public List<Courses> GetTop3CoursesByEnrollment(DateTime selectedDate)
    {
        var topCourses = new List<Courses>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT c.COURSEID, c.COURSETITLE, COUNT(e.ENROLLMENTID) AS ENROLLMENT_COUNT
                FROM COURSES c
                JOIN ENROLLMENT e ON c.COURSEID = e.COURSEID
                WHERE EXTRACT(YEAR FROM e.ENROLLDATE) = :Year
                AND EXTRACT(MONTH FROM e.ENROLLDATE) = :Month
                GROUP BY c.COURSEID, c.COURSETITLE
                ORDER BY ENROLLMENT_COUNT DESC
                FETCH FIRST 3 ROWS ONLY";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("Year", OracleDbType.Int32).Value = selectedDate.Year;
                cmd.Parameters.Add("Month", OracleDbType.Int32).Value = selectedDate.Month;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    topCourses.Add(new Courses
                    {
                        CourseId = reader.GetString(0),
                        CourseTitle = reader.GetString(1),
                        EnrollmentCount = reader.GetInt32(2)
                    });
                reader.Dispose();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return topCourses;
    }

    public async Task<List<Courses>> SearchCoursesAsync(string searchTerm)
    {
        var coursesList = new List<Courses>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = @"
                SELECT 
                    c.CourseID, 
                    c.CourseTitle,
                    i.InstructorName
                FROM 
                    COURSES c
                JOIN 
                    INSTRUCTORS i ON c.TEACHERID = i.INSTRUCTORID
                WHERE 
                    UPPER(c.CourseTitle) LIKE UPPER(:SearchTerm)
                ORDER BY 
                    c.CourseTitle";
                var cmd = new OracleCommand(queryString, con);
                cmd.Parameters.Add("SearchTerm", OracleDbType.Varchar2).Value = "%" + searchTerm + "%";
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    coursesList.Add(new Courses
                    {
                        CourseId = reader.GetString(0),
                        CourseTitle = reader.GetString(1),
                        CourseTeacher = reader.GetString(2)
                    });
                reader.Dispose();
                await con.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return coursesList;
    }
}