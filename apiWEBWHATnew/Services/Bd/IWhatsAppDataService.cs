namespace apiWEBWHATnew.Services.Bd
{
    public interface IWhatsAppDataService
    {
        Task InsertarDatos(string mensaje_recibido, string id_wa, string cel_wa);
    }
}
