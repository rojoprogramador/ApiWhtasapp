namespace apiWEBWHATnew.Models.WhatsappCloud
{
    public class WhatsAppCloudModel
    {
        public Entry[] entry { get; set; }
    }
    public class Entry
    {
        public string id { get; set; }
        public Change[] changes { get; set; }
    }
    public class Change
    {
        public Value? Value { get; set; }
        public string? Field { get; set; }
    }
    public class Value
    {
        public string messaging_product { get; set; }
        public Metadata metadata { get; set; }
        public Message[] messages { get; set; }
        public Contact[] contacts { get; set; }
    }
    public class Contact
    {
        public string wa_id { get; set; }
        public Profile profile { get; set; }
    }
    public class Metadata
    {
        public string display_phone_number { get; set; }
        public string phone_number_id { get; set; }
    }
    public class Profile
    {
        public string name { get; set; }
    }
    public class Message
    {
        public string? From { get; set; }
        public string? Id { get; set; }
        public string? Timestamp { get; set; }
        public string? Type { get; set; }
        public Text? Text { get; set; }
        public Interactive? Interactive { get; set; }
        public Flow? Flow { get; set; }
        public FlowResponse? Flow_Response { get; set; } = null;

    }
    public class Body
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public List<BodyParameter> Parameters { get; set; }
    }
    public class BodyParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
    public class FlowResponse
    {
        public string Screen_Name { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
    public class Flow
    {
        public string? Id { get; set; }
        public string? Name { get; set; }  
        public string? Mode { get; set; }
        public object? Data { get; set; }
        public FlowParameters? Parameters { get; set; }
    }
    public class FlowParameters  
    {
        public string flow_message_version { get; set; }
        public string flow_token { get; set; }
        public string flow_id { get; set; }
        public string flow_cta { get; set; }
        public string mode { get; set; }
    }
    public class Interactive
    {
        public string? Type { get; set; }
        public FlowHeader? Header { get; set; }  
        public Body? Body { get; set; }
        public FlowFooter? Footer { get; set; } 
        public FlowAction? Action { get; set; }  
        public ListReply? List_Reply { get; set; }
        public ButtonReply? Button_Reply { get; set; }
        public FlowResponse? Flow_Response { get; set; }
    }
    public class FlowHeader
    {
        public string type { get; set; }
        public string text { get; set; }
    }
    public class InteractiveHeader
    {
        public string Type { get; set; }
        public string Text { get; set; }
    }

    public class InteractiveFooter
    {
        public string Text { get; set; }
    }

    public class InteractiveAction
    {
        public string Button { get; set; }
        public List<ActionSection> Sections { get; set; }
        public FlowParameters Parameters { get; set; }
    }

    public class ActionSection
    {
        public string Title { get; set; }
        public List<ActionRow> Rows { get; set; }
    }

    public class ActionRow
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class FlowFooter
    {
        public string text { get; set; }
    }

    public class FlowAction
    {
        public FlowParameters Parameters { get; set; }
    }    
    public class ListReply
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
    public class ButtonReply
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
    }

    public class Text
    {
        public string Body { get; set; }       

    }
    public class FormularioRequest
    {
        public string NumeroTelefono { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Fuente { get; set; }
        public string Ciudad { get; set; }
        public string TipoFormulario { get; set; }
    }
    public class SolicitudModel
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Subtipo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Fuente { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public Dictionary<string, string> DatosAdicionales { get; set; }
    }
}
