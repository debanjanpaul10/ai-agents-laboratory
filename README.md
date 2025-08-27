# 🤖 AI Agents Laboratory

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![API](https://img.shields.io/badge/API-REST-orange.svg)]()
[![SignalR](https://img.shields.io/badge/SignalR-Real--time-red.svg)]()

A comprehensive AI-powered laboratory platform built with .NET 9, Microsoft Semantic Kernel, and modern web technologies. This platform provides a robust foundation for developing and deploying AI agents with real-time communication capabilities.

## 🌟 Features

### 🤖 AI Agent Framework

-   **Semantic Kernel Integration**: Leverage Microsoft's Semantic Kernel for AI operations
-   **Google AI Connectors**: Built-in support for Google AI services
-   **Memory Management**: Advanced AI memory plugins for context retention
-   **Extensible Architecture**: Clean architecture pattern for easy extension and maintenance

### 🧠 Available Skills & Plugins

The platform includes several pre-built AI skills and plugins organized into different categories:

#### 💬 Chatbot Skills

-   **User Intent Detection**: Automatically determine user intentions from natural language input
-   **Greeting Handler**: Generate contextual greetings and responses for user interactions
-   **Natural Language to SQL**: Convert natural language queries into SQL statements with database schema awareness
-   **RAG Text Processing**: Retrieval-Augmented Generation for knowledge-based question answering
-   **SQL Response Formatting**: Convert SQL query results into readable markdown format
-   **Follow-up Questions**: Generate intelligent follow-up questions based on user queries and AI responses

#### 🔧 Utility Plugins

-   **Bug Severity Analysis**: Intelligent classification and severity assessment of software bugs and issues
-   **Token Usage Tracking**: Monitor and report AI model token consumption for cost optimization

#### ✍️ Content Processing

-   **Text Rewriting**: Advanced rewriting and enhancement of user stories and content
-   **Genre Tag Generation**: Automatic tag generation for stories and content categorization
-   **Content Moderation**: AI-powered content filtering and safety assessment with rating system

#### 🔍 Advanced Features

-   **Knowledge Base Integration**: Support for custom knowledge bases in RAG operations
-   **Database Schema Awareness**: Context-aware SQL generation with schema validation
-   **Multi-format Output**: JSON responses with detailed token usage metrics
-   **Error Handling**: Robust exception handling with graceful degradation

### 🔄 Real-time Communication

-   **SignalR Integration**: Live status updates and real-time agent communication
-   **Agent Status Monitoring**: Track AI agent performance and availability
-   **WebSocket Support**: Efficient bidirectional communication

## 🏗️ Architecture

This solution follows **Clean Architecture** principles with **Hexagonal Architecture** patterns:

```
AIAgents.Laboratory/
├── 🎯 Core/
│   └── AIAgents.Laboratory.Domain/           # Domain entities & business logic
├── 🔌 Adapters/
│   ├── AIAgents.Laboratory.SemanticKernel.Adapters/  # AI service integrations
│   ├── AIAgents.Laboratory.API.Adapters/             # API layer adapters
│   └── AIAgents.Laboratory.Messaging.Adapters/       # SignalR & messaging
└── 🌐 AIAgents.Laboratory.API/               # Web API & controllers
```

### Key Components

-   **Domain Layer**: Core business logic, entities, and domain services
-   **Semantic Kernel Adapters**: Microsoft Semantic Kernel integration with Google AI connectors
-   **API Adapters**: Request/response handling, validation, and API contracts
-   **Messaging Adapters**: Real-time communication via SignalR hubs
-   **Web API**: Controllers, middleware, and API endpoints
-   **Clean Separation**: Dependency inversion and fully testable architecture

### Project Structure

The solution is organized into the following projects:

-   **AIAgents.Laboratory.API**: Main web API project with controllers and startup configuration
-   **AIAgents.Laboratory.Domain**: Core domain layer with business entities and logic
-   **AIAgents.Laboratory.SemanticKernel.Adapters**: AI service integrations using Semantic Kernel
-   **AIAgents.Laboratory.API.Adapters**: API layer adapters for request/response handling
-   **AIAgents.Laboratory.Messaging.Adapters**: SignalR hubs and real-time messaging

## 🚀 Getting Started

### Prerequisites

-   [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
-   Azure subscription (for Azure App Configuration and Managed Identity)
-   Google AI API access (for Semantic Kernel Google connectors)

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
    - Configure Google AI API credentials for Semantic Kernel integration

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
-   **AI Integration**: Microsoft Semantic Kernel with Google AI Connectors
-   **Real-time Communication**: SignalR Hubs
-   **API Documentation**: Swagger/OpenAPI with Annotations
-   **Configuration**: Azure App Configuration
-   **Authentication**: JWT Bearer tokens
-   **Identity Management**: Azure Identity & Managed Identity
-   **Object Mapping**: AutoMapper
-   **Architecture**: Clean Architecture + Hexagonal Pattern

## 📦 Dependencies

### Core Packages

-   `Microsoft.SemanticKernel.Connectors.Google` (v1.61.0-alpha) - Google AI integration
-   `Microsoft.SemanticKernel.Plugins.Memory` (v1.61.0-alpha) - Memory management for AI
-   `Microsoft.Extensions.Configuration.AzureAppConfiguration` (v8.3.0) - Cloud configuration
-   `Microsoft.AspNetCore.Authentication.JwtBearer` (v9.0.8) - JWT authentication

### Development & Infrastructure

-   `Swashbuckle.AspNetCore.Annotations` (v9.0.3) - API documentation with annotations
-   `Swashbuckle.AspNetCore.SwaggerUI` (v9.0.3) - Interactive API documentation
-   `AutoMapper` (v15.0.1) - Object-to-object mapping
-   `Azure.Identity` (v1.15.0) - Azure authentication and identity management
-   `Azure.Core` (v1.47.3) - Azure SDK core functionality

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
-   Google AI API credentials (configured through Azure App Configuration or local settings)

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
