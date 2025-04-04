# ApiWhatsapp

A robust .NET Core API for sending WhatsApp messages with webhooks and automated flows.

## Overview

ApiWhatsapp is a .NET Core middleware solution that simplifies the integration of WhatsApp messaging capabilities into your applications. It provides a clean API interface to send messages via WhatsApp, with support for webhooks to handle incoming messages and flows to automate message sequences.

## Features

- **WhatsApp Message Sending**: Send text, media, and template messages through WhatsApp
- **Webhook Integration**: Configure webhooks to receive and process incoming messages
- **Automated Flows**: Create automated conversation flows for common scenarios
- **Easy Integration**: RESTful API for integration with any platform or language
- **.NET Core Framework**: Built with modern C# and .NET Core for cross-platform compatibility
- **Dependency Injection**: Follows best practices with built-in DI container
- **Entity Framework Core**: Efficient data access and management
- **Swagger Documentation**: Auto-generated API documentation

## Technology Stack

- **Framework**: .NET Core 6.0+
- **Database**: SQL Server (configurable to use other providers)
- **ORM**: Entity Framework Core
- **Documentation**: Swagger/OpenAPI
- **Authentication**: JWT Bearer tokens
- **Logging**: Serilog
- **Testing**: xUnit, Moq

## Prerequisites

- .NET 6.0 SDK or later
- SQL Server (or alternative compatible with EF Core)
- Visual Studio 2022, VS Code, or JetBrains Rider

## Installation

```bash
# Clone the repository
git clone https://github.com/yourusername/ApiWhatsapp.git

# Navigate to the project directory
cd ApiWhatsapp

# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update

# Run the application
dotnet run
```

## Configuration

Update `appsettings.json` with your configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WhatsAppApi;Trusted_Connection=True;"
  },
  "WhatsAppSettings": {
    "ApiKey": "your_whatsapp_api_key",
    "PhoneNumberId": "your_phone_number_id"
  },
  "WebhookSettings": {
    "VerifyToken": "your_custom_verification_token"
  },
  "Jwt": {
    "Key": "your_secret_key_at_least_16_characters",
    "Issuer": "your_issuer",
    "Audience": "your_audience",
    "ExpiryInMinutes": 60
  }
}
```

## API Reference

### Send a Message

```
POST /api/messages
```

Request body:
```json
{
  "to": "5511999999999",
  "type": "text",
  "content": "Hello from ApiWhatsapp!"
}
```

Response:
```json
{
  "success": true,
  "messageId": "wamid.abcdefg123456",
  "timestamp": "2025-04-04T12:00:00Z"
}
```

### Register a Webhook

```
POST /api/webhooks
```

Request body:
```json
{
  "url": "https://your-app.com/webhook",
  "events": ["message.received", "message.read"]
}
```

### Create an Automated Flow

```
POST /api/flows
```

Request body:
```json
{
  "name": "Welcome Flow",
  "trigger": "keyword:hello",
  "steps": [
    {
      "type": "message",
      "content": "Welcome to our service!",
      "delay": 0
    },
    {
      "type": "message",
      "content": "How can we help you today?",
      "delay": 2000
    }
  ]
}
```

## Project Structure

```
ApiWhatsapp/
├── ApiWhatsapp.API/            # Web API project
│   ├── Controllers/            # API endpoints
│   ├── Program.cs              # Application entry point
│   ├── Startup.cs              # Application configuration
│   └── appsettings.json        # Configuration settings
├── ApiWhatsapp.Core/           # Core business logic
│   ├── Interfaces/             # Abstractions/contracts
│   ├── Models/                 # Domain models
│   └── Services/               # Business services
├── ApiWhatsapp.Infrastructure/ # Data access & external services
│   ├── Data/                   # EF Core context and migrations
│   ├── Repositories/           # Data access logic
│   └── WhatsApp/               # WhatsApp client implementation
├── ApiWhatsapp.Tests/          # Unit and integration tests
└── ApiWhatsapp.sln             # Solution file
```

## Example Usage (.NET Client)

```csharp
// Example using HttpClient in C#
public async Task SendMessageExample()
{
    using var client = new HttpClient();
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", "YOUR_API_KEY");
    
    var content = new StringContent(
        JsonSerializer.Serialize(new
        {
            to = "5511999999999",
            type = "text",
            content = "Hello from my .NET application!"
        }), 
        Encoding.UTF8, 
        "application/json");
    
    var response = await client.PostAsync("https://your-api.com/api/messages", content);
    var data = await response.Content.ReadAsStringAsync();
    Console.WriteLine(data);
}
```

## Webhook Implementation

To handle incoming WhatsApp messages, implement a webhook controller:

```csharp
[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly IFlowService _flowService;
    private readonly string _verifyToken;
    
    public WebhookController(IFlowService flowService, IConfiguration config)
    {
        _flowService = flowService;
        _verifyToken = config["WebhookSettings:VerifyToken"];
    }
    
    [HttpGet]
    public IActionResult VerifyWebhook([FromQuery] string mode, [FromQuery] string token, [FromQuery] string challenge)
    {
        if (mode == "subscribe" && token == _verifyToken)
        {
            return Ok(challenge);
        }
        
        return Unauthorized();
    }
    
    [HttpPost]
    public async Task<IActionResult> ReceiveMessage([FromBody] WebhookPayload payload)
    {
        // Process incoming message
        await _flowService.ProcessWebhookAsync(payload);
        return Ok();
    }
}
```

## Flow Automation

Flows allow you to create automated conversation sequences triggered by specific events or keywords:

1. **Define Flow Triggers**: Keywords, message patterns, or specific events
2. **Create Flow Steps**: Sequence of messages, delays, and conditional branches
3. **Activate Flows**: Enable flows to automatically respond to customer interactions

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support, please open an issue in the GitHub repository or contact us at support@example.com.
