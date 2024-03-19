using Oracle.ManagedDataAccess.Client;
using roider.Datas;

namespace roider.Models;

public class Country
{
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public List<Country> GetCountries()
    {
        List<Country> countries = new List<Country>();
        try
        {
            using (var con = new OracleConnection(ValuesConstants.DbString))
            {
                var queryString = "SELECT country_code, country_name FROM Countries";
                var cmd = new OracleCommand(queryString, con);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countries.Add(new Country
                        {
                            CountryCode = reader.GetString(0),
                            CountryName = reader.GetString(1)
                        });
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return countries;
    }

}