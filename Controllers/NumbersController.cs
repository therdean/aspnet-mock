using aspnet_mock.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace aspnet_mock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumbersController : ControllerBase
    {
        private readonly MySqlConnection _connection;

        public NumbersController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DefaultConnectionStringNotFound";
            _connection = new MySqlConnection(connectionString);
        }

        [HttpPost]
        public ActionResult<NumberResponse> Post([FromBody] NumberRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            var (name, description) = GetMockData(request.Number);

            var response = new NumberResponse(name, description);

            return Ok(response);
        }

        private (string, string) GetMockData(int number)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT name, description FROM mock_data WHERE id = @id";
                command.Parameters.AddWithValue("@id", number);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string description = reader.GetString(1);
                        return (name, description);
                    }
                    else
                    {
                        return ("No user with that ID is available", "");
                    }
                }
            }
        }
    }
}