using apiWEBWHATnew.Models.WhatsappCloud;
using apiWEBWHATnew.Services.Bd;
using apiWEBWHATnew.Services.Solicitudes;
using apiWEBWHATnew.Services.WhatsappCloud.SendMessage;
using apiWEBWHATnew.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace apiWEBWHATnew.Controllers
{
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsappController : ControllerBase
    {
        private readonly IWhatsappCloudSendMessage _whatsappCloudSendMessage;
        private readonly IUtil _util;
        private readonly string _localApiUrl;
        private readonly IWhatsAppDataService _dataService;
        private readonly ISolicitudService _solicitudService;
        private const string TOKEN = "token";
        public WhatsappController(IWhatsappCloudSendMessage whatsappCloudSendMessage, IUtil util, IWhatsAppDataService dataService, ISolicitudService solicitudService, IConfiguration configuration)
        {
            _whatsappCloudSendMessage = whatsappCloudSendMessage;
            _util = util;
            _dataService = dataService;
            _solicitudService = solicitudService;

            // Obtén la URL de ngrok desde la configuración
            _localApiUrl = configuration["LocalApiUrl"];

        }

        [HttpGet("test")]
        public async Task<IActionResult> Sample()
        {
            try
            {
                var data = new
                {
                    messaging_product = "whatsapp",
                    to = "573007750833",
                    type = "text",
                    text = new
                    {
                        body = "este es un mensaje de prueba"
                    }
                };
                var result = await _whatsappCloudSendMessage.Execute(data);
                if (!result)
                {
                    return BadRequest(new { message = "Failed to send message" });
                }
                return Ok(new { message = "Message sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet]
        public IActionResult VerifyToken([FromQuery(Name = "hub.mode")] string hubMode,
                                         [FromQuery(Name = "hub.challenge")] string hubChallenge,
                                         [FromQuery(Name = "hub.verify_token")] string hubVerifyToken)
        {
            try
            {
                // Log all received parameters
                Console.WriteLine($"Modo recibido: {hubMode}");
                Console.WriteLine($"Challenge recibido: {hubChallenge}");
                Console.WriteLine($"Token recibido: {hubVerifyToken}");

                // Verificación más robusta
                if (string.IsNullOrEmpty(hubMode) ||
                    string.IsNullOrEmpty(hubChallenge) ||
                    string.IsNullOrEmpty(hubVerifyToken))
                {
                    Console.WriteLine("Parámetros incompletos");
                    return BadRequest(new
                    {
                        message = "Faltan parámetros requeridos",
                        receivedMode = hubMode,
                        receivedChallenge = hubChallenge,
                        receivedToken = hubVerifyToken
                    });
                }

                // Verifica que el modo sea 'subscribe'
                if (hubMode != "subscribe")
                {
                    Console.WriteLine("Modo inválido");
                    return BadRequest(new { message = "Modo inválido" });
                }

                // Compara el token (considera usar un token más seguro o almacenarlo en variables de entorno)
                if (hubVerifyToken != TOKEN)
                {
                    Console.WriteLine("Token de verificación incorrecto");
                    return Unauthorized(new { message = "Token de verificación incorrecto" });
                }

                // Devuelve el challenge con Content-Type text/plain
                Console.WriteLine("Webhook verificado exitosamente");
                Response.ContentType = "text/plain";
                return Content(hubChallenge);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en verificación: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new
                {
                    message = "Error interno del servidor",
                    detalle = ex.Message
                });
            }
        }

        // Clase auxiliar para asegurar el Content-Type correcto
        public class PlainTextResult : IActionResult
        {
            private readonly string _text;

            public PlainTextResult(string text)
            {
                _text = text;
            }

            public async Task ExecuteResultAsync(ActionContext context)
            {
                var response = context.HttpContext.Response;
                response.Headers["Content-Type"] = "text/plain";
                await response.WriteAsync(_text);
            }
        }
        //detecta el tipo de mensaje que se recibe 
        private string GetUserText(Message message)
        {
            if (message == null) return string.Empty;

            // Log detallado
            Console.WriteLine($"\n=== PROCESANDO MENSAJE ===");
            Console.WriteLine($"Tipo: {message.Type}");
            Console.WriteLine($"Contenido JSON: {JsonConvert.SerializeObject(message)}");

            // Método más robusto con switch expression y null-coalescing
            return (message.Type?.ToLower(), message) switch
            {
                ("text", _) => message.Text?.Body?.Trim(),
                ("interactive", { Interactive.Type: "button_reply" })
                    => message.Interactive.Button_Reply?.Title?.Trim(),
                ("interactive", { Interactive.Type: "list_reply" })
                    => message.Interactive.List_Reply?.Title?.Trim(),
                _ => string.Empty
            } ?? string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivedMessage([FromBody] WhatsAppCloudModel body)
        {
            try
            {
                Console.WriteLine($"\n=== NUEVO MENSAJE RECIBIDO ===");
                Console.WriteLine($"Body completo: {JsonConvert.SerializeObject(body)}");

                var message = body.entry[0]?.changes[0]?.Value?.messages[0];
                if (message == null)
                {
                    Console.WriteLine("No se encontró mensaje en el body");
                    return Ok(new { status = "no_message", message = "EVENT_RECEIVED" });
                }

                var userNumber = message.From;
                var userText = GetUserText(message);
                object objectMessage;

                Console.WriteLine($"Número: {userNumber}");
                Console.WriteLine($"Tipo de mensaje: {message.Type}");
                Console.WriteLine($"Texto extraído: {userText}");

                // Manejo de mensajes interactivos
                if (message.Type?.ToLower() == "interactive")
                {
                    objectMessage = HandleInteractiveMessage(message, userNumber);
                }
                // Manejo de mensajes de texto
                else if (message.Type?.ToLower() == "text")
                {
                    objectMessage = HandleTextMessage(userText, userNumber);
                }
                // Otros tipos de mensajes
                else
                {
                    objectMessage = _util.MenuPrincipal(userNumber);
                }

                // Enviar mensaje de respuesta
                Console.WriteLine($"Enviando respuesta: {JsonConvert.SerializeObject(objectMessage)}");
                var result = await _whatsappCloudSendMessage.Execute(objectMessage);
                Console.WriteLine($"Resultado del envío: {result}");

                return Ok(new { status = "success", message = "EVENT_RECEIVED" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return Ok(new { status = "error", message = "EVENT_RECEIVED" });
            }
        }

        // Método para manejar mensajes interactivos
        private object HandleInteractiveMessage(Message message, string userNumber)
        {
            Console.WriteLine($"Mensaje interactivo recibido: {JsonConvert.SerializeObject(message.Interactive)}");

            // Manejar respuestas de lista
            if (message.Interactive?.Type == "list_reply")
            {
                return HandleListReply(message.Interactive.List_Reply, userNumber);
            }
            // Manejar respuestas de flujo
            else if (message.Interactive?.Type == "flow_response")
            {
                var flowResponse = JsonConvert.DeserializeObject<FlowResponse>(
                    JsonConvert.SerializeObject(message.Interactive.Flow_Response)
                );
                return HandleFlowResponse(flowResponse, userNumber);
            }
            // Manejar otros tipos de mensajes interactivos
            else
            {
                return _util.MenuPrincipal(userNumber);
            }
        }

        // Método para manejar respuestas de lista
        private object HandleListReply(ListReply listReply, string userNumber)
        {
            Console.WriteLine($"Opción de lista seleccionada: {listReply?.Id}");
            return listReply?.Id switch
            {
                // Menú Principal
                "solicitar_info" => _util.MenuInformacion(userNumber),
                "pqrs" => _util.MenuPQRS(userNumber),
                "contacto" => _util.InformacionContacto(userNumber),

                // Menú PQRS - Ahora usando EnviarFlow en lugar de FlowFormulario
                "pqrs_peticion" => _util.EnviarFlow(userNumber),
                "pqrs_queja" => _util.FlowProspecto(userNumber),
                "pqrs_reclamo" => _util.EnviarFlow(userNumber),

                // Menú Información
                "info_servicios" => _util.TextMessage(
                    "🌟 Nuestros servicios incluyen:\n\n" +
                    "1. Servicio Técnico 🔧\n" +
                    "   - Mantenimiento preventivo\n" +
                    "   - Reparaciones\n" +
                    "   - Instalaciones\n\n" +
                    "2. Asesoría Especializada 📋\n" +
                    "   - Consultoría técnica\n" +
                    "   - Evaluación de proyectos\n" +
                    "   - Capacitaciones\n\n" +
                    "3. Soporte 24/7 🚨\n" +
                    "   - Atención de emergencias\n" +
                    "   - Soporte remoto\n" +
                    "   - Monitoreo continuo",
                    userNumber
                ),
                "info_productos" => _util.TextMessage(
                    "🎯 Nuestros productos destacados:\n\n" +
                    "1. Línea Premium ⭐\n" +
                    "   - Producto A Plus\n" +
                    "   - Producto B Pro\n" +
                    "   - Producto C Elite\n\n" +
                    "2. Línea Estándar 📦\n" +
                    "   - Producto X\n" +
                    "   - Producto Y\n" +
                    "   - Producto Z\n\n" +
                    "3. Accesorios 🛠️\n" +
                    "   - Complemento 1\n" +
                    "   - Complemento 2\n" +
                    "   - Complemento 3\n\n" +
                    "Para más detalles sobre precios y disponibilidad, seleccione 'Contacto' en el menú principal.",
                    userNumber
                ),

                // Si no coincide con ninguna opción, volver al menú principal
                _ => _util.MenuPrincipal(userNumber)
            };
        }
        private async Task<object> HandleFlowResponse(FlowResponse flowResponse, string userNumber)
        {
            try
            {
                Console.WriteLine($"=== HandleFlowResponse ===");
                Console.WriteLine($"Respuesta completa: {JsonConvert.SerializeObject(flowResponse)}");

                if (flowResponse?.Data == null)
                {
                    return new
                    {
                        messaging_product = "whatsapp",  
                        to = userNumber,
                        type = "text",
                        text = new
                        {
                            body = "Hubo un error al procesar su respuesta. Por favor, intente nuevamente."
                        }
                    };
                }

                // Procesa los datos del flow
                var formData = flowResponse.Data;
                var formulario = new FormularioRequest
                {
                    NumeroTelefono = userNumber,
                    Nombre = formData.ContainsKey("Nombre") ? formData["Nombre"] : "",
                    Descripcion = formData.ContainsKey("Peticion") ? formData["Peticion"] : "",
                    Fuente = formData.ContainsKey("Fuente") ? formData["Fuente"] : "",
                    Ciudad = formData.ContainsKey("Ciudad") ? formData["Ciudad"] : "",
                    TipoFormulario = "peticion"
                };

                // Guardado de datos
                await GuardarRespuestaFormulario(formulario);                

                var nombre = formData.ContainsKey("Nombre") ? formData["Nombre"] : "Usuario";

                return new
                {
                    messaging_product = "whatsapp",
                    to = userNumber,
                    type = "text",
                    text = new
                    {
                        body = $"¡Gracias {nombre}! Hemos recibido tu petición. Nos pondremos en contacto contigo pronto."
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando flow response: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                return new
                {
                    messaging_product = "whatsapp", 
                    to = userNumber,
                    type = "text",
                    text = new
                    {
                        body = "Lo sentimos, ocurrió un error. Por favor, intente nuevamente."
                    }
                };
            }
        }
        // Cambia la firma del método para que sea asíncrono
        private async Task GuardarRespuestaFormulario(FormularioRequest formulario)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //specify to use TLS 1.2 as default connection
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    client.Timeout = TimeSpan.FromSeconds(30);

                    // Serializa el formulario
                    var content = new StringContent(
                        JsonConvert.SerializeObject(formulario),
                        Encoding.UTF8,
                        "application/json"
                    );

                    // Usa await para la llamada asíncrona
                    var response = await client.PostAsync($"{_localApiUrl}/api/datos/guardar-formulario", content);

                    // Verifica la respuesta
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Respuesta del servidor: {responseContent}");
                    }
                    else
                    {
                        Console.WriteLine($"Error enviando formulario. Código de estado: {response.StatusCode}");
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Contenido del error: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al enviar formulario: {ex.Message}");
                Console.WriteLine($"Traza de la pila: {ex.StackTrace}");
            }
        }
        // Método para manejar mensajes de texto
        private object HandleTextMessage(string userText, string userNumber)
        {
            // Si es un saludo o solicitud de menú
            if (EsSaludo(userText) || userText.ToLower() == "menu")
            {
                return _util.MenuPrincipal(userNumber);
            }

            // Mensaje predeterminado
            return _util.TextMessage(
                "Por favor, selecciona una opción del menú principal.",
                userNumber
            );
        }

        // Método auxiliar para detectar saludos
        private bool EsSaludo(string texto)
        {
            string[] saludos = { "hola", "hi", "hey", "hello", "buenas", "buenos dias", "buenas tardes", "buenas noches" };
            return saludos.Any(s => texto.ToLower().Contains(s));
        }


       
    }
}
