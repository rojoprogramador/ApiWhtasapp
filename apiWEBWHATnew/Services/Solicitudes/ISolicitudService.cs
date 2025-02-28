using apiWEBWHATnew.Models.WhatsappCloud;

namespace apiWEBWHATnew.Services.Solicitudes
{
    public interface ISolicitudService
    {
        Task GuardarSolicitud(string tipo, string subtipo, string nombre, string descripcion, string fuente, string ciudad, string telefono);
        Task ActualizarSolicitud(string telefono, string campo, string valor);
        Task<SolicitudModel> ObtenerSolicitudIncompleta(string telefono);
    }
}

