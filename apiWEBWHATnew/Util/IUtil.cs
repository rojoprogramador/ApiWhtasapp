namespace apiWEBWHATnew.Util
{
    public interface IUtil
    {
        object TextMessage(string message, string number);
        object ImageMessage(string url, string number);
        object AudioMessage(string url, string number);
        object VideoMessage(string url, string number);
        object DocumentMessage(string url, string number);
        object LocationMessage(string number);
        object ButtonsMessage(string number);
        object FlowProspecto(string number);
        object MenuPrincipal(string number);
        object MenuPQRS(string number);
        object MenuInformacion(string number);
        object InformacionContacto(string number);          
        object FlowSolicitudInfo(string number);               
        object EnviarFlow(string number);
    }
}
