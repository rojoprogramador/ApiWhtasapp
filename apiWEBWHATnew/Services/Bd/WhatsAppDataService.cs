using MySqlConnector;

namespace apiWEBWHATnew.Services.Bd
{
    public class WhatsAppDataService : IWhatsAppDataService
    {
        private readonly string _connectionString;

        public WhatsAppDataService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task InsertarDatos(string mensaje_recibido, string id_wa, string cel_wa)
        {
            using var conn = new MySqlConnection(_connectionString);
            try
            {
                var command = conn.CreateCommand();
                command.CommandText = "INSERT INTO registro (fecha_hora, mensaje_recibido, id_wa, telefono_wa) VALUES (NOW(), @mensaje_recibido, @id_wa, @telefono_wa)";
                command.Parameters.AddWithValue("@mensaje_recibido", mensaje_recibido);
                command.Parameters.AddWithValue("@id_wa", id_wa);
                command.Parameters.AddWithValue("@telefono_wa", cel_wa);

                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();

                Console.WriteLine($"Datos guardados: Mensaje={mensaje_recibido}, ID={id_wa}, Teléfono={cel_wa}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error guardando datos: {ex.Message}");
                throw;
            }
        }
    }
}
