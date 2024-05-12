using System.Data.SqlClient;

public class Program
{
    public static void Main()
    {
        const string connectionString = "Server=db-mssql16.pjwstk.edu.pl;Database=S26094;User ID=s26094;Password=kkkkkkkkkk";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connection to the database is established.");

                var controller = new AnimalsController(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
