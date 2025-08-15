# 🤖 AI Agents Laboratory

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![API](https://img.shields.io/badge/API-REST-orange.svg)]()
[![SignalR](https://img.shields.io/badge/SignalR-Real--time-red.svg)]()

A comprehensive AI-powered laboratory platform that provides intelligent services for fitness applications and content management systems. Built with .NET 9, Semantic Kernel, and modern web technologies.

## 🌟 Features

### 🏋️ FitGym Tool AI

-   **Bug Severity Analysis**: Intelligent classification of software bugs and issues
-   **Automated Quality Assessment**: AI-powered evaluation of code quality and potential issues

### 📰 IBBS (Intelligent Bulletin Board System) AI

-   **Content Rewriting**: Advanced text rewriting and enhancement capabilities
-   **Smart Tagging**: Automatic tag generation for user stories and content
-   **Content Moderation**: AI-powered content filtering and safety assessment

### 🔄 Real-time Communication

-   **SignalR Integration**: Live status updates and real-time agent communication
-   **Agent Status Monitoring**: Track AI agent performance and availability
-   **WebSocket Support**: Efficient bidirectional communication

## 🏗️ Architecture

This solution follows **Clean Architecture** principles with **Hexagonal Architecture** patterns:

```
├── 🎯 Core/
│   └── AIAgents.Laboratory.Domain/     # Business logic & domain entities
├── 🔌 Adapters/
│   ├── SemanticKernel.Adapters/        # AI service integrations
│   ├── API.Adapters/                   # API layer adapters
│   └── Messaging.Adapters/             # SignalR & messaging
└── 🌐 AIAgents.Laboratory.API/         # Web API & controllers
```

### Key Components

-   **Domain Layer**: Core business logic and entities
-   **Semantic Kernel Integration**: Microsoft Semantic Kernel for AI operations
-   **API Adapters**: Request/response handling and validation
-   **Messaging Layer**: Real-time communication via SignalR
-   **Clean Separation**: Dependency inversion and testable architecture

## 🚀 Getting Started

### Prerequisites

-   [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
-   Azure subscription (for Azure App Configuration)

### Installation

1. **Clone the repository**

    ```bash
    git clone https://github.com/[username]/AIAgents.Laboratory.git
    cd AIAgents.Laboratory
    ```

2. **Restore dependencies**

    ```bash
    dotnet restore
    ```

3. **Configure settings**

    - Update `appsettings.json` with your Azure App Configuration connection
    - Set up managed identity credentials for Azure services

4. **Build the solution**

    ```bash
    dotnet build
    ```

5. **Run the application**
    ```bash
    dotnet run --project AIAgents.Laboratory.API
    ```

The API will be available at `https://localhost:8190`

## 📚 API Documentation

### Swagger UI

Access the interactive API documentation at: `https://localhost:8190/swaggerui`

## 🔄 Real-time Features

### SignalR Hub

Connect to the agent status hub for real-time updates:

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:8190/hubs/agent-status")
    .build();

connection.on("ReceiveAgentStatus", function (status) {
    console.log("Agent Status:", status);
});
```

Test the SignalR functionality using the included `signalr-test.html` file.

## 🛠️ Technology Stack

-   **Framework**: .NET 9.0
-   **AI Integration**: Microsoft Semantic Kernel
-   **Real-time Communication**: SignalR
-   **API Documentation**: Swagger/OpenAPI
-   **Configuration**: Azure App Configuration
-   **Authentication**: JWT Bearer tokens
-   **Architecture**: Clean Architecture + Hexagonal Pattern

## 📦 Dependencies

### Core Packages

-   `Microsoft.SemanticKernel.Connectors.Google` - Google AI integration
-   `Microsoft.SemanticKernel.Plugins.Memory` - Memory management for AI
-   `Microsoft.AspNetCore.SignalR` - Real-time communication
-   `Microsoft.Extensions.Configuration.AzureAppConfiguration` - Cloud configuration

### Development Tools

-   `Swashbuckle.AspNetCore` - API documentation
-   `AutoMapper` - Object mapping
-   `Newtonsoft.Json` - JSON serialization

## 🔧 Configuration

### Azure App Configuration

The application uses Azure App Configuration for centralized settings management:

```json
{
    "ConnectionStrings": {
        "AppConfig": "your-azure-app-configuration-connection-string"
    }
}
```

### Environment Variables

-   `MANAGED_IDENTITY_CLIENT_ID`: Azure Managed Identity client ID
-   `ASPNETCORE_ENVIRONMENT`: Environment setting (Development/Production)

## 🧪 Testing

### Running Tests

```bash
dotnet test
```

### SignalR Testing

Open `signalr-test.html` in your browser to test real-time functionality.

## 📈 Monitoring & Logging

The application includes comprehensive logging using `ILogger<T>`:

-   Request/response logging
-   Error tracking and exception handling
-   Performance monitoring
-   Agent status tracking

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Debanjan Paul**

-   Email: debanjanpaul10@gmail.com
-   GitHub: [@debanjanpaul10](https://github.com/debanjanpaul10)

## 🙏 Acknowledgments

-   Microsoft Semantic Kernel team for the AI framework
-   .NET community for excellent tooling and support
-   Azure team for cloud services integration

---

⭐ **Star this repository if you find it helpful!**
