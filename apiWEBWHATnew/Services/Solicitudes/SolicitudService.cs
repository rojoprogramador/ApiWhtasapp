using apiWEBWHATnew.Models.WhatsappCloud;
using MySqlConnector;

namespace apiWEBWHATnew.Services.Solicitudes
{
    public class SolicitudService : ISolicitudService
    {
        private readonly string _connectionString;

        public SolicitudService()
        {
            _connectionString = "Server=localhost;Database=aspwa;Uid=root;Pwd=admin;";
        }

        public async Task GuardarSolicitud(string tipo, string subtipo, string nombre, string descripcion, string fuente, string ciudad, string telefono)
        {
            using var conn = new MySqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO solicitudes 
                    (tipo, subtipo, nombre, descripcion, fuente, ciudad, telefono) 
                    VALUES 
                    (@tipo, @subtipo, @nombre, @descripcion, @fuente, @ciudad, @telefono)";

                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@subtipo", subtipo ?? "");
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@fuente", fuente);
                cmd.Parameters.AddWithValue("@ciudad", ciudad);
                cmd.Parameters.AddWithValue("@telefono", telefono);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error guardando solicitud: {ex.Message}");
                throw;
            }
        }

        public async Task<SolicitudModel> ObtenerSolicitudIncompleta(string telefono)
        {
            using var conn = new MySqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT * FROM solicitudes 
                            WHERE telefono = @telefono 
                            AND (nombre = '' OR descripcion = '' OR fuente = '' OR ciudad = '')
                            ORDER BY fecha_creacion DESC 
                            LIMIT 1";

                cmd.Parameters.AddWithValue("@telefono", telefono);

                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new SolicitudModel
                    {
                        Id = reader.GetInt32("id"),
                        Tipo = reader.GetString("tipo"),
                        Subtipo = reader.GetString("subtipo"),
                        Nombre = reader.GetString("nombre"),
                        Descripcion = reader.GetString("descripcion"),
                        Fuente = reader.GetString("fuente"),
                        Ciudad = reader.GetString("ciudad"),
                        Telefono = reader.GetString("telefono"),
                        FechaCreacion = reader.GetDateTime("fecha_creacion")
                    };
                }

                // Si no hay solicitud incompleta, retorna una nueva
                return new SolicitudModel
                {
                    Telefono = telefono,
                    FechaCreacion = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo solicitud incompleta: {ex.Message}");
                throw;
            }
        }

        public async Task ActualizarSolicitud(string telefono, string campo, string valor)
        {
            using var conn = new MySqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                // Usar parámetros dinámicos de forma segura
                cmd.CommandText = $@"UPDATE solicitudes 
                             SET {campo} = @valor 
                             WHERE telefono = @telefono 
                             AND (nombre = '' OR descripcion = '' OR fuente = '' OR ciudad = '')
                             ORDER BY fecha_creacion DESC 
                             LIMIT 1";

                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@telefono", telefono);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error actualizando solicitud: {ex.Message}");
                throw;
            }
        }
    }
}

