using Microsoft.VisualBasic;

namespace apiWEBWHATnew.Util
{
    public class Util : IUtil
    {
        public object TextMessage(string message, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "text",
                text = new
                {
                    preview_url = false,
                    body = message
                }
            };
        }

        public object MenuPrincipal(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "list",
                    header = new { type = "text", text = "¡Bienvenido! 🎉" },
                    body = new { text = "Selecciona una opción:" },
                    footer = new { text = "Estamos aquí para ayudarte" },
                    action = new
                    {
                        button = "Menú Principal",
                        sections = new[]
                        {
                            new
                            {
                                title = "Servicios",
                                rows = new[]
                                {
                                    new { id = "solicitar_info", title = "📋 Solicitar Información", description = "Información sobre servicios y productos" },
                                    new { id = "pqrs", title = "📝 PQRS", description = "Peticiones, Quejas, Reclamos y Sugerencias" },
                                    new { id = "contacto", title = "📞 Contacto", description = "Información de contacto" }
                                }
                            }
                        }
                    }
                }
            };
        }
        public object MenuInformacion(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "list",
                    header = new { type = "text", text = "Información" },
                    body = new { text = "Selecciona el tipo de información:" },
                    footer = new { text = "Estamos aquí para ayudarte" },
                    action = new
                    {
                        button = "Información",
                        sections = new[]
                        {
                        new
                        {
                            title = "Tipos de Información",
                            rows = new[]
                            {
                                new { id = "info_servicios", title = "🛠️ Servicios", description = "Detalles de nuestros servicios" },
                                new { id = "info_productos", title = "📦 Productos", description = "Catálogo de productos" },
                                new { id = "info_contacto", title = "📞 Contacto", description = "Información de contacto" }
                            }
                        }
                    }
                    }
                }
            };
        }
        public object FlowSolicitudInfo(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "flow",
                    header = new { type = "text", text = "Solicitud de Información" },
                    body = new { text = "Por favor, proporciona los siguientes detalles:" },
                    action = new
                    {
                        name = "flow_solicitud_informacion",
                        parameters = new object[]
                        {
                        new { type = "text", label = "Nombre Completo" },
                        new { type = "text", label = "Correo Electrónico" },
                        new { type = "text", label = "Tipo de Información Requerida" },
                        new { type = "text", label = "Descripción Detallada" }
                        }
                    }
                }
            };
        }
        public object InformacionContacto(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "list",
                    header = new { type = "text", text = "Información de Contacto" },
                    body = new { text = "Selecciona el medio de contacto:" },
                    footer = new { text = "Estamos a tu disposición" },
                    action = new
                    {
                        button = "Contacto",
                        sections = new[]
                        {
                        new
                        {
                            title = "Canales de Contacto",
                            rows = new[]
                            {
                                new { id = "contacto_telefono", title = "📱 Teléfono", description = "Números de contacto" },
                                new { id = "contacto_email", title = "✉️ Correo Electrónico", description = "Direcciones de email" },
                                new { id = "contacto_direccion", title = "📍 Dirección", description = "Ubicación física" }
                            }
                        }
                    }
                    }
                }
            };
        }


        public object ButtonsMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "button",
                    header = new
                    {
                        type = "text",
                        text = "Menú Principal"
                    },
                    body = new
                    {
                        text = "¿En qué podemos ayudarte?"
                    },
                    footer = new
                    {
                        text = "Selecciona una opción"
                    },
                    action = new
                    {
                        buttons = new[]
                        {
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "01",
                                    title = "Solicitar Info"
                                }
                            },
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "02",
                                    title = "PQRS"
                                }
                            }
                        }
                    }
                }
            };
        }

        public object MenuPQRS(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "list",
                    header = new { type = "text", text = "PQRS" },
                    body = new { text = "Selecciona el tipo de solicitud:" },
                    footer = new { text = "Ayúdanos a mejorar" },
                    action = new
                    {
                        button = "Tipos PQRS",
                        sections = new[]
                        {
                            new
                            {
                                title = "Tipos de Solicitud",
                                rows = new[]
                                {
                                    new { id = "pqrs_peticion", title = "✉️ Petición", description = "Solicitud de información o trámite" },
                                    new { id = "pqrs_queja", title = "🚨 Queja", description = "Inconformidad sobre un servicio" },
                                    new { id = "pqrs_reclamo", title = "⚠️ Reclamo", description = "Reclamación formal" }
                                }
                            }
                        }
                    }
                }
            };
        }                                   
        
        public object EnviarFlow(string number)
        {
            string flowToken = Guid.NewGuid().ToString();
           
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "flow",
                    header = new { type = "text", text = "Formulario de Petición" },
                    body = new { text = "Por favor completa el siguiente formulario" },
                    footer = new { text = "Toca el botón para comenzar" },
                    action = new
                    {
                        name = "flow",
                        parameters = new
                        {
                            flow_message_version = "3",
                            flow_token = flowToken,
                            flow_id = "2369902270029017", //  ID que te dé Meta cuando aprueban tu flow
                            flow_cta = "Comenzar",
                            flow_action = "navigate",
                            flow_action_payload = new { screen = "FormPeticion" } // Este debe coincidir con el ID en tu JSON
                        }
                    }
                }
            };
        }

        public object FlowProspecto(string number)
        {
            string flowToken = Guid.NewGuid().ToString();

            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "flow",
                    header = new { type = "text", text = "Formulario Creacion prospecto" },
                    body = new { text = "Por favor completa el siguiente formulario" },
                    footer = new { text = "Toca el botón para comenzar" },
                    action = new
                    {
                        name = "flow",
                        parameters = new
                        {
                            flow_message_version = "3",
                            flow_token = flowToken,
                            flow_id = "1558603581480321", //  ID que te dé Meta cuando aprueban tu flow
                            flow_cta = "Comenzar",
                            flow_action = "navigate",
                            flow_action_payload = new { screen = "formProspecto" } // Este debe coincidir con el ID en tu JSON
                        }
                    }
                }
            };
        }
        public object ImageMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "image",
                image = new
                {
                    link = url,
                    caption = "Imagen"
                }
            };
        }

        public object AudioMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "audio",
                audio = new
                {
                    link = url
                }
            };
        }

        public object VideoMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "video",
                video = new
                {
                    link = url,
                    caption = "Video"
                }
            };
        }

        public object DocumentMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "document",
                document = new
                {
                    link = url,
                    caption = "Documento",
                    filename = "documento.pdf"
                }
            };
        }

        public object LocationMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "location",
                location = new
                {
                    latitude = "-12.067079752918158",
                    longitude = "-77.03371847563524",
                    name = "Estadio Nacional del Perú",
                    address = "C. José Díaz s/n, Lima 15046"
                }
            };
        }
    }

}