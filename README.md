# 🤖 AI Agents Laboratory

[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![Next.js](https://img.shields.io/badge/Next.js-16-black.svg)](https://nextjs.org/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![API](https://img.shields.io/badge/API-REST-orange.svg)]()

A full-stack AI agent platform built on **.NET 10** and **Next.js 16**. It lets you create, configure, and interact with custom AI agents backed by multiple LLM providers, manage workspaces for multi-agent collaboration, upload knowledge bases for RAG, and receive real-time notifications — all deployed on Azure.

---

## 📑 Table of Contents

- [Features](#-features)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Technology Stack](#-technology-stack)
- [API Reference](#-api-reference)
- [Configuration](#-configuration)
- [Getting Started](#-getting-started)
- [CI/CD & Deployment](#-cicd--deployment)
- [Author](#-author)

---

## ✨ Features

### 🤖 AI Agent Management

- Create, update, and delete custom AI agents
- Attach knowledge base files (`.docx`, `.doc`, `.pdf`, `.xlsx`, `.xls`, `.txt`, `.json`) for RAG
- Configure agents with custom system prompts and tool skills
- Download knowledge base files associated with an agent

### 💬 Chat & Conversation

- Invoke any configured agent via a conversational chat interface
- Direct chatbot responses without workspace context
- Persistent conversation history per user
- Clear conversation history on demand

### 🗂️ Workspaces

- Group agents into workspaces for multi-agent collaboration
- Invoke workspace-level agents through a unified chat interface
- Full CRUD management for workspaces

### 🧠 Multi-Provider AI Support

Switchable AI service providers via configuration:

- **OpenAI GPT** (Azure-hosted endpoint)
- **Google Gemini** (Flash and Pro models via `Google.GenAI`)
- **Perplexity AI** (Sonar model)

### 🔍 Vision & Document Processing

- **Azure AI Vision** for image analysis
- Multi-format document reading: PDF (`itext`), Word (`DocumentFormat.OpenXml`), Excel, plain text, JSON

### 🛠️ Tool Skills & MCP

- Register reusable tool skills that agents can invoke
- **Model Context Protocol (MCP)** integration for extensible tool calling

### 🔔 Notifications

- Real-time push notifications via **Azure SignalR**
- Email notifications via **Azure Communication Services**
- Server-Sent Events (SSE) endpoint for notification streaming
- Mark-as-read support

### 🏪 Marketplace & Registered Applications

- Browse and register external applications
- Admin panel for feature requests and bug reports

### 📊 Observability

- **Azure Application Insights** telemetry
- **OpenTelemetry** API integration
- Structured logging with correlation ID tracking across all requests

---

## 🏗️ Architecture

The solution follows **Clean Architecture** with a **Hexagonal (Ports & Adapters)** pattern.

```
┌─────────────────────────────────────────────────────────────┐
│                        Clients                              │
│              Next.js Web UI  │  External APIs               │
└──────────────────┬──────────────────────────────────────────┘
                   │ HTTPS / JWT
┌──────────────────▼──────────────────────────────────────────┐
│               AIAgents.Laboratory.API                       │
│         Controllers (v2)  │  Middleware  │  DI Container    │
└──────────────────┬──────────────────────────────────────────┘
                   │
        ┌──────────┴──────────┐
        │                     │
┌───────▼──────┐   ┌──────────▼──────────────────────────────┐
│    Domain    │   │              Adapters                    │
│  (Core)      │   │  API.Adapters  │  Messaging.Adapters     │
│  Entities    │   │  Caching       │  AgentsFramework        │
│  Use Cases   │   │  MongoDatabase │  SQLDatabase            │
│  Contracts   │   │  Storage.Blobs                          │
└──────────────┘   └─────────────────────────────────────────┘
                                   │
              ┌────────────────────┼────────────────────┐
              │                    │                    │
        ┌─────▼──────┐   ┌────────▼──────┐   ┌────────▼──────┐
        │  MongoDB   │   │  SQL Server / │   │  GCP / Azure  │
        │            │   │  PostgreSQL   │   │  Blob Storage │
        └────────────┘   └───────────────┘   └───────────────┘

Azure Functions (separate solution)
  ├── PushNotificationsFunction   ← Service Bus trigger
  └── SendEmailNotificationFunction ← Service Bus trigger
```

### Key Patterns

- **Repository Pattern** — generic data access abstraction over SQL and MongoDB
- **Handler Pattern** — each API feature area has a dedicated handler (e.g., `IAgentsHandler`, `IChatHandler`)
- **Correlation ID Middleware** — every request carries a correlation ID propagated through all logs
- **Feature Flags** — AI service, caching, knowledge base, email notifications, and vision are individually toggleable
- **JWT Bearer Authentication** — Azure AD tokens validated against tenant issuer and audience

---

## 📁 Project Structure

```
ai-agents-laboratory/
├── Backend/                                    # .NET 10 backend solution
│   ├── AIAgents.Laboratory.API/               # Web API — controllers, middleware, DI
│   ├── Core/
│   │   └── AIAgents.Laboratory.Domain/        # Entities, use cases, contracts
│   ├── Adapters/
│   │   ├── Infrastructure/
│   │   │   ├── AIAgents.Laboratory.Infrastructure.AgentsFramework/  # Semantic Kernel, Gemini, MCP, Vision
│   │   │   ├── AIAgents.Laboratory.Persistence.MongoDatabase/       # MongoDB repositories
│   │   │   ├── AIAgents.Laboratory.Persistence.SQLDatabase/         # EF Core SQL repositories
│   │   │   └── AIAgents.Laboratory.Storage.Blobs/                   # GCP & Cloudinary storage
│   │   └── Services/
│   │       ├── AIAgents.Laboratory.API.Adapters/                    # Request/response models & handlers
│   │       ├── AIAgents.Laboratory.Messaging.Adapters/              # Azure Service Bus
│   │       ├── AIAgents.Laboratory.Persistence.Caching/             # In-memory cache
│   │       └── AIAgents.Laboratory.Logging/                         # Logging helpers
│   ├── Database/
│   │   └── AIAgents.Laboratory.Database/      # SQL Server DACPAC project
│   └── Tests/
│       └── AIAgents.Laboratory.Domain.UnitTests/
│
├── Functions/                                  # Azure Functions solution
│   ├── AI.Agents.Laboratory.Functions/        # Function triggers
│   ├── AI.Agents.Laboratory.Functions.Business/
│   ├── AI.Agents.Laboratory.Functions.Data/
│   ├── AI.Agents.Laboratory.Functions.Shared/
│   └── AI.Agents.Laboratory.Functions.UnitTests/
│
└── UI/
    └── web/                                    # Next.js 16 frontend
        ├── pages/                              # App routes
        │   ├── dashboard/
        │   ├── manage-agents/
        │   ├── workspaces/
        │   ├── workspace/
        │   ├── marketplace/
        │   ├── register-applications/
        │   ├── admin/
        │   └── login/
        ├── components/                         # UI components by feature
        ├── store/                              # Redux Toolkit state
        ├── auth/                               # MSAL Azure AD auth
        ├── helpers/
        └── models/
```

---

## 🛠️ Technology Stack

### Backend

| Category         | Technology                         | Version         |
| ---------------- | ---------------------------------- | --------------- |
| Runtime          | .NET                               | 10.0            |
| AI Orchestration | Microsoft Semantic Kernel          | 1.76.0          |
| AI Memory        | Semantic Kernel Plugins.Memory     | 1.72.0-alpha    |
| Google AI        | Google.GenAI                       | 1.6.2           |
| MCP              | ModelContextProtocol               | 1.3.0           |
| Computer Vision  | Azure Cognitive Services Vision    | 7.0.1           |
| ORM              | Entity Framework Core (SQL Server) | 10.0.8          |
| NoSQL            | MongoDB.Driver                     | 3.8.1           |
| Messaging        | Azure.Messaging.ServiceBus         | 7.20.1          |
| Email            | Azure.Communication.Email          | 1.1.0           |
| Blob Storage     | Google.Cloud.Storage.V1            | 4.14.0          |
| Image Storage    | CloudinaryDotNet                   | 1.29.1          |
| Authentication   | Azure.Identity / JwtBearer         | 1.21.0 / 10.0.8 |
| Configuration    | Azure App Configuration            | 8.5.0           |
| Monitoring       | Application Insights               | 3.1.1           |
| Observability    | OpenTelemetry.Api                  | 1.15.3          |
| Object Mapping   | AutoMapper                         | 15.0.1          |
| API Docs         | Swashbuckle (Swagger)              | 10.1.7          |
| PDF Processing   | itext                              | 9.6.0           |
| Word/Excel       | DocumentFormat.OpenXml             | 3.5.1           |
| Serialization    | Newtonsoft.Json                    | 13.0.4          |

### Frontend

| Category         | Technology                  | Version  |
| ---------------- | --------------------------- | -------- |
| Framework        | Next.js                     | 16.2.3   |
| UI Library       | React                       | 19.1.0   |
| Language         | TypeScript                  | 5.x      |
| State Management | Redux Toolkit               | 2.9.0    |
| Authentication   | @azure/msal-react           | 3.0.19   |
| UI Components    | @heroui/react               | 2.8.4    |
| Styling          | Tailwind CSS                | 4.1.13   |
| HTTP Client      | Axios                       | 1.15.0   |
| Animations       | Framer Motion               | 12.23.15 |
| Markdown         | react-markdown + remark-gfm | 10.1.0   |

### Azure Services

- **Azure App Service** — API hosting
- **Azure Static Web Apps** — Frontend hosting
- **Azure SQL Database** — Relational data
- **Azure App Configuration** — Centralized configuration + Key Vault references
- **Azure Service Bus** — Async messaging (email & push notification queues)
- **Azure SignalR Service** — Real-time WebSocket communication
- **Azure Communication Services** — Email delivery
- **Azure AI Vision** — Image analysis
- **Azure Application Insights** — Telemetry and monitoring
- **Azure Functions (Flex Consumption)** — Serverless notification processing
- **Azure Managed Identity** — Passwordless service authentication

---

## 📡 API Reference

All endpoints are versioned under `/aiagentsapi/v2/`. Swagger UI is available at `/swaggerui` in development.

### Chat — `/chat`

| Method | Route                       | Description                                        |
| ------ | --------------------------- | -------------------------------------------------- |
| `POST` | `/InvokeAgent`              | Invoke a workspace AI agent                        |
| `POST` | `/GetDirectChatResponse`    | Get a direct chatbot response                      |
| `GET`  | `/GetConversationHistory`   | Retrieve conversation history for the current user |
| `POST` | `/ClearConversationHistory` | Clear conversation history for the current user    |

### Agents — `/agents`

| Method   | Route                        | Description                                                       |
| -------- | ---------------------------- | ----------------------------------------------------------------- |
| `POST`   | `/CreateNewAgent`            | Create a new AI agent (multipart/form-data, supports file upload) |
| `GET`    | `/GetAllAgents`              | List all agents for the current user                              |
| `GET`    | `/GetAgentById/{id}`         | Get a specific agent by ID                                        |
| `PUT`    | `/UpdateAgent`               | Update an existing agent                                          |
| `DELETE` | `/DeleteAgent/{id}`          | Delete an agent                                                   |
| `GET`    | `/DownloadKnowledgebaseFile` | Download a knowledge base file                                    |

### Workspaces — `/workspaces`

| Method   | Route                    | Description                              |
| -------- | ------------------------ | ---------------------------------------- |
| `GET`    | `/GetAllWorkspaces`      | List all workspaces for the current user |
| `GET`    | `/GetWorkspaceById/{id}` | Get a workspace by ID                    |
| `POST`   | `/AddNewWorkspace`       | Create a new workspace                   |
| `DELETE` | `/DeleteWorkspace/{id}`  | Delete a workspace                       |

### Notifications — `/notifications`

| Method | Route                  | Description                    |
| ------ | ---------------------- | ------------------------------ |
| `POST` | `/CreateNotification`  | Create a new notification      |
| `GET`  | `/PollNotifications`   | Poll for pending notifications |
| `GET`  | `/StreamNotifications` | Stream notifications via SSE   |
| `POST` | `/MarkAsRead`          | Mark a notification as read    |

### Tool Skills — `/toolskills`

| Method | Route                    | Description                     |
| ------ | ------------------------ | ------------------------------- |
| `GET`  | `/GetAllToolSkills`      | List all registered tool skills |
| `GET`  | `/GetToolSkillById/{id}` | Get a tool skill by ID          |
| `POST` | `/AddNewToolSkill`       | Register a new tool skill       |
| `PUT`  | `/UpdateToolSkill`       | Update a tool skill             |

### Registered Applications — `/registeredapplications`

| Method | Route                      | Description                      |
| ------ | -------------------------- | -------------------------------- |
| `GET`  | `/GetAllApplications`      | List all registered applications |
| `POST` | `/RegisterApplication`     | Register a new application       |
| `GET`  | `/GetApplicationById/{id}` | Get an application by ID         |
| `PUT`  | `/UpdateApplication`       | Update an application            |

### Application Admin — `/applicationadmin`

| Method | Route                             | Description                                           |
| ------ | --------------------------------- | ----------------------------------------------------- |
| `GET`  | `/GetAllSubmittedFeatureRequests` | List all submitted feature requests                   |
| `GET`  | `/GetAllReportedBugs`             | List all reported bugs                                |
| `GET`  | `/IsAdminAccessEnabled`           | Check if admin access is enabled for the current user |

---

## ⚙️ Configuration

Configuration is loaded from **Azure App Configuration** in production and from `appsettings.Development.json` locally. Key Vault references are resolved automatically via Managed Identity.

### Key Settings

```json
{
	"AppConfigurationEndpoint": "https://<your-appconfig>.azconfig.io",
	"ManagedIdentityClientId": "<managed-identity-client-id>",
	"AiAgentsClientId": "<azure-ad-app-client-id>",
	"TenantId": "<azure-ad-tenant-id>",

	"CurrentAiServiceProvider": "OpenAiGpt",
	"AvailableAiServiceProviders": "OpenAiGpt,PerplexityAi,GoogleGemini",

	"ChatGpt": { "ApiKey": "", "Endpoint": "", "ModelId": "gpt-5-chat" },
	"GeminiFlashModel": "gemini-2.0-flash",
	"GeminiProModel": "gemini-2.5-pro",
	"PerplexityAI": {
		"ApiKey": "",
		"Endpoint": "https://api.perplexity.ai",
		"ModelId": "sonar"
	},

	"CurrentSQLProvider": "PostgreSQL",
	"AzureSqlConnectionString": "",
	"PostgreSQL": { "Connectionstring": "" },
	"MongoDbConnectionString": "",
	"AiAgentsPrimaryDatabase": "ai-agents-primary",

	"CloudStorageService": "GCP",
	"GCP": { "ProjectId": "", "BucketName": "", "ServiceAccountJson": "" },
	"Cloudinary": { "CloudName": "", "APIKey": "", "APISecret": "" },

	"ServiceBus": {
		"ConnectionString": "",
		"EmailNotificationsQueue": "emailnotificationsqueue",
		"PushNotificationsQueue": "pushnotificationsqueue"
	},
	"AzureSignalRConnection": "",
	"EmailNotification": { "ConnectionString": "", "SenderAddress": "" },

	"IsAIServiceEnabled": true,
	"IsAiVisionServiceEnabled": true,
	"IsCacheServiceEnabled": true,
	"IsKnowledgeBaseServiceEnabled": true,
	"IsEmailNotificationEnabled": false,
	"IsFeedbackFeatureEnabled": true,
	"IsProModelEnabled": false,

	"AllowedKbFileFormats": ".docx,.doc,.pdf,.xlsx,.xls,.txt,.json",
	"AllowedVisionImageFileFormats": ".png,.jpeg,.jpg,.svg"
}
```

### MongoDB Collections

| Collection Key                     | Default Name              |
| ---------------------------------- | ------------------------- |
| `AgentsCollection`                 | `agents`                  |
| `ConversationHistoryCollection`    | `conversation-history`    |
| `NotificationsCollection`          | `notifications`           |
| `OrchestratorPromptsCollection`    | `orchestrator-prompts`    |
| `ToolSkillsCollection`             | `tool-skills`             |
| `WorkspaceCollection`              | `agents-workspace`        |
| `RegisteredApplicationsCollection` | `registered-applications` |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 22.x](https://nodejs.org/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- Azure subscription (App Configuration, SQL, MongoDB, Service Bus, SignalR)
- At least one AI provider API key (OpenAI, Gemini, or Perplexity)

### Backend Setup

1. **Clone the repository**

    ```bash
    git clone https://github.com/debanjanpaul10/ai-agents-laboratory.git
    cd ai-agents-laboratory
    ```

2. **Configure local settings**

    Copy `Backend/AIAgents.Laboratory.API/appsettings.json` and create `appsettings.Development.json`. Fill in at minimum:
    - `AppConfigurationEndpoint` — your Azure App Configuration URL
    - `LocalSqlConnectionString` — local SQL Server connection string
    - `MongoDbConnectionString` — MongoDB connection string
    - AI provider keys

3. **Restore and build**

    ```bash
    dotnet restore Backend/AIAgents.Laboratory.slnx
    dotnet build Backend/AIAgents.Laboratory.slnx
    ```

4. **Run the API**

    ```bash
    dotnet run --project Backend/AIAgents.Laboratory.API
    ```

    The API starts at `https://localhost:8190`. Swagger UI is at `https://localhost:8190/swaggerui`.

5. **Run tests**
    ```bash
    dotnet test Backend/AIAgents.Laboratory.slnx
    ```

### Azure Functions Setup

```bash
dotnet restore Functions/AI.Agents.Laboratory.Functions.slnx
dotnet build Functions/AI.Agents.Laboratory.Functions.slnx
```

Set `AzureServiceBusConnectionString` in local settings before running locally.

### Frontend Setup

```bash
cd UI/web
npm install
npm run dev        # development server (Turbopack)
npm run build      # production build
npm start          # serve production build
```

The frontend runs at `http://localhost:3000` by default.

---

## 🔄 CI/CD & Deployment

The GitHub Actions workflow (`.github/workflows/deploy-azure.yml`) triggers on push to `main` or `dev`, and supports manual dispatch with per-component toggles.

| Job                                  | Runner         | Target                             |
| ------------------------------------ | -------------- | ---------------------------------- |
| `build-api` + `deploy-api`           | ubuntu-latest  | Azure App Service                  |
| `build-and-deploy-web`               | ubuntu-latest  | Azure Static Web Apps              |
| `build-database` + `deploy-database` | windows-latest | Azure SQL (DACPAC)                 |
| `build-and-deploy-function`          | ubuntu-latest  | Azure Functions (Flex Consumption) |

### Required Secrets

| Secret                            | Purpose                       |
| --------------------------------- | ----------------------------- |
| `AZURE_WEBAPI_PUBLISH_PROFILE`    | App Service deployment        |
| `AZURE_FUNCTIONS_PUBLISH_PROFILE` | Functions deployment          |
| `AZURE_STATIC_WEB_APPS_TOKEN`     | Static Web Apps deployment    |
| `AZURE_CLIENT_ID`                 | Azure login for DB deployment |
| `AZURE_TENANT_ID`                 | Azure login for DB deployment |
| `AZURE_SUBSCRIPTION_ID`           | Azure login for DB deployment |
| `AZURE_SQL_CONNECTION_STRING`     | Database schema deployment    |

### Required Variables

| Variable               | Purpose            |
| ---------------------- | ------------------ |
| `AZURE_WEBAPI_NAME`    | App Service name   |
| `AZURE_FUNCTIONS_NAME` | Functions app name |

---

## 👨‍💻 Author

**Debanjan Paul**

- Email: debanjanpaul10@gmail.com
- GitHub: [@debanjanpaul10](https://github.com/debanjanpaul10)

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

⭐ Star this repository if you find it useful!
