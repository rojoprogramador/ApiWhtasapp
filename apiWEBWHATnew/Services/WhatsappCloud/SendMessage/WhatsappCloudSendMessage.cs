using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace apiWEBWHATnew.Services.WhatsappCloud.SendMessage
{
    public class WhatsappCloudSendMessage : IWhatsappCloudSendMessage,  IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint = "https://graph.facebook.com";
        private readonly string _phoneNumberId = "516056078253513";
        private readonly string _accessToken = "EAAJNRgoCtrsBOZCxPOMcgQO9L3pCVbFhiQbLjXYJWPjzgq4NCnZBsRymqZCGNRZBK4LZCTH5IczwFqpA5TsVMg8UIeB66NUdhB3ZBIjZANWD2tCglXEbxnYVw5FH3x6HAnBOoq8IXYOaThptu9Aw2NG1tD1Q52qawlr4tR24B0hbegXVVwZB4gfWwYCGtdVayVyGF2puvBmijmrUUCY8fJGzdND5";
        private bool disposed = false;
        private bool disposedValue;

        public WhatsappCloudSendMessage()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_endpoint)
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //public async Task<bool> Execute(object data)
        //{
        //    try
        //    {
        //       var jsonRequest = JsonConvert.SerializeObject(data);
        //        Console.WriteLine($"\n=== ENVIANDO MENSAJE ===");
        //        Console.WriteLine($"URL: https://graph.facebook.com/v22.0/{_phoneNumberId}/messages");
        //        Console.WriteLine($"Token: {_accessToken.Substring(0, 20)}...");
        //        Console.WriteLine($"Request: {jsonRequest}");

        //        using var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        //        var response = await _httpClient.PostAsync($"/v22.0/{_phoneNumberId}/messages", content);

        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine($"Status: {response.StatusCode}");
        //        Console.WriteLine($"Response: {responseContent}");

        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"ERROR EN ENVÍO: {ex.Message}");
        //        return false;
        //    }
        //}
        public async Task<bool> Execute(object data)
        {
            try
            {
                // Asegurarse de que el objeto está en el formato correcto
                var request = new
                {
                    messaging_product = "whatsapp",
                    recipient_type = "individual",
                    to = (data.GetType().GetProperty("to")?.GetValue(data) ?? "").ToString(),
                    type = "interactive",
                    interactive = data.GetType().GetProperty("interactive")?.GetValue(data)
                };

                var jsonRequest = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                Console.WriteLine($"\n=== ENVIANDO MENSAJE ===");
                Console.WriteLine($"URL: {_endpoint}/v22.0/{_phoneNumberId}/messages");
                Console.WriteLine($"Request: {jsonRequest}");

                using var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"/v22.0/{_phoneNumberId}/messages", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Response: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR EN ENVÍO: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~WhatsappCloudSendMessage()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
