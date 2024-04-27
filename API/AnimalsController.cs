using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly SqlConnection _connection;

    public AnimalsController(SqlConnection connection)
    {
        _connection = connection;
    }

    // GET
    [HttpGet]
    public ActionResult<IEnumerable<Animal>> GetAnimals()
    {
        const string queryString = $"SELECT * FROM Animals ORDER BY NAME";

        using var command = new SqlCommand(queryString, _connection);
        _connection.Open();
        using var reader = command.ExecuteReader();
        var animals = new List<Animal>();
        while (reader.Read())
        {
            var animal = new Animal(
                Convert.ToInt32(reader["Id"]),
                reader["Name"].ToString(),
                reader["Description"].ToString(),
                reader["Category"].ToString(),
                reader["Area"].ToString());

            animals.Add(animal);
        }
        _connection.Close();
        return animals;
    }
    
    // POST
    [HttpPost]
    public IActionResult AddAnimal([FromBody] Animal animal)
    {
        try
        {
            const string queryString = "INSERT INTO Animals (Name, Description, Category, Area) " +
                                 "VALUES (@Name, @Description, @Category, @Area)";

            using var command = new SqlCommand(queryString, _connection);
            command.Parameters.AddWithValue("@Name", animal.Name);
            command.Parameters.AddWithValue("@Description", animal.Description);
            command.Parameters.AddWithValue("@Category", animal.Category);
            command.Parameters.AddWithValue("@Area", animal.Area);

            _connection.Open();
            var rowsAffected = command.ExecuteNonQuery();
            _connection.Close();
            
            return rowsAffected > 0 ? StatusCode(201, animal) : BadRequest("Failed to add animal");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Error: {e.Message}");
        }
    }
    
    // PUT
    [HttpPut("{id}")]
    public IActionResult UpdateAnimal(int id, [FromBody] Animal animal)
    {
        try
        {
            var queryString =
                "UPDATE Animals SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE Id = @Id";

            using var command = new SqlCommand(queryString, _connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", animal.Name);
            command.Parameters.AddWithValue("@Description", animal.Description);
            command.Parameters.AddWithValue("@Category", animal.Category);
            command.Parameters.AddWithValue("@Area", animal.Area);

            _connection.Open();
            var rowsAffected = command.ExecuteNonQuery();
            _connection.Close();
            
            if (rowsAffected > 0)
            {
                return Ok(animal);
            }
            return NotFound("Animal not found");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Error: {e.Message}");
        }
    }

    //DELETE
    [HttpDelete]
    public IActionResult DeleteAnimal(int id)
    {
        try
        {
            const string queryString = "DELETE FROM Animals WHERE Id = @Id";

            using var command = new SqlCommand(queryString, _connection);
            command.Parameters.AddWithValue("@Id", id);
            
            _connection.Open();
            var rowsAffected = command.ExecuteNonQuery();
            _connection.Close();

            if (rowsAffected > 0)
            {
                return NoContent();
            }
            return NotFound("Animal not found");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Error: {e.Message}");
        }
    }
    
}