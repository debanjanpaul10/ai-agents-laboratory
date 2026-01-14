export const metadata = {
	title: "AI Agents Laboratory",
	description: "AI Agents Laboratory Application",
};

export const DashboardConstants = {
	ManageAgentsTile: {
		SubText: "Configure, monitor, control and test your AI agents.",
		Steps: [
			"View all agents",
			"Edit configurations",
			"Monitor Agent performance",
			"Test AI agents",
		],
		ActionText: "Click to manage agents",
	},
	CreateAgentTile: {
		SubText: "Build a new AI agent from scratch using prompts.",
		Steps: [
			"Be specific about the AI agent's role",
			"Define clear boundaries and limitations",
			"Include examples of desired behavior",
			"Specify the tone and communication style",
		],
		ActionText: "Click to create an agent",
	},
	ActiveAgentsTile: {
		SubText: "The top 3 agents being actively used.",
		NoAgentsText: "No active agents to view now.",
		ReadyToDeployText: "Ready to deploy",
		Steps: [
			"Create and deploy new agents",
			"Modify and delete existing AI agents",
		],
	},
	QuickActionsTile: {
		Header: "Quick Actions",
		SubText: "More actions coming soon!",
	},
	FooterConstants: {
		FooterText: "AI Agents Laboratory - a product of Debanjan's Laboratory",
	},
	DirectChatConstants: {
		Header: {
			MainHeader: "Chat with AI Agent",
			SubHeader: "Try out a sample AI chatbot",
		},
		ConversationContent: {
			Heading: "Start a conversation",
			SubText:
				"Send a message to the AI agent to see how the functionality works",
		},
	},
	LoadingConstants: {
		MainLoader: "Loading AI Agents Data",
		SaveNewAgentLoader: "Saving New Agent",
		LoginRedirectLoader: "Redirecting to Login",
		SaveAgentDataLoader: "Saving agent data",
		CheckingAuthentication: "Checking authentication",
		CreateNewSkill: "Saving Skill details",
	},
};

export const MarketplaceConstants = {
	LoadingConstants: {
		MainLoader: "Loading AI Agents Marketplace",
		LoginRedirectLoader: "Redirecting to Login",
	},
	ComingSoonConstants: {
		Header: "Skills Marketplace Coming Soon",
		SubHeading:
			"Explore and add new capabilities to your AI agents from our curated marketplace of skills and tools.",
	},
	Headers: {
		Header: "Skills Marketplace",
		SubText:
			"Discover and integrate advanced capabilities into your AI agents.",
	},
	AddSkillConstants: {
		Header: "Add New Skill",
		SubHeader: "Create and register a new tool skill for your AI agents.",
		Placeholders: {
			DisplayName: "Enter skill display name...",
			TechnicalName: "Enter technical name (e.g. get_weather)...",
			McpUrl: "Enter MCP Server URL (if applicable)...",
		},
		Info: "The technical name should be unique. It is used by the AI agent to identify and call the tool during a conversation.",
	},
};

export const WorkspacesConstants = {
	LoadingConstants: {
		MainLoader: "Loading AI Agents Workspaces",
		LoginRedirectLoader: "Redirecting to Login",
	},

	ComingSoonConstants: {
		Header: "Agents Workspaces Coming Soon",
		SubHeading:
			"Create your own custom workspaces and add multiple agents to test and work on.",
	},
};

export const CreateAgentConstants = {
	Headers: {
		SubText: "Build your AI agent from scratch",
	},
	InputFields: {
		AgentNamePlaceholder: "Enter agent name ...",
		AgentDescriptionPlaceholder:
			"Add a summarized version of your agent's capabilities ...",
		ApplicationNamePlaceholder: "Enter application name ...",
		AgentMetaPromptPlaceholder:
			"Define your agent's behavior, personality, and capabilities ...",
		McpServerURL: "Add the URL for your MCP Server.",
	},
};

export const ManageAgentConstants = {
	Headers: {
		Header: "Agents Management",
		SubHeader: "Create and manage AI agents to cater to your needs.",
	},
	ModifyAgentConstants: {
		MainHeader: "Modify AI Agent Configuration",
		Placeholders: {
			AgentName: "Enter agent name...",
			ApplicationName: "Enter application name ...",
			AgentDescriptionPlaceholder:
				"Add a summarized version of your agent's capabilities ...",
			AgentMetaprompt:
				"Define your agent's behavior, personality, and capabilities ...",
			McpServerURL: "Add the URL for your MCP Server.",
		},
		Info: "Describe how your agent should behave, what it should know, and how it should respond to users.",
		KBInfo: "You can upload knowledge base data to the agent. Available file formats: .pdf, .docx, .xlsx, .txt",
		VisionInfo:
			"You can upload images to the agent. Available image formats: .jpg, .jpeg, .png, .svg",
		PrivateField:
			"Private agents are only accessible by you and won't appear in public listings.",
		MCPUrl: "Provide the URL for any MCP (Model Context Protocol) Servers that you have. The AI Agent will use them for tool calling.",
	},

	TestAgentConstants: {
		PlaceHolders: {
			ChatBodyHeader: "Start Testing",
			SubText:
				"Send a message to test how your agent responds based on its configuration.",
			TypeMessage: "Type your message...",
			ClearConversation: "Clear Conversation",
		},
		Loading: {
			AgentResponse: "Agent is thinking ...",
		},
	},
};

export const LoginPageConstants = {
	LoginButton: "Sign-In with SSO",
	HeaderText: "AI Agents Laboratory",
	Subtext: "Welcome to the future of AI development",
};

export const SeverityOptions = [
	{ value: 1, label: "Low", color: "text-green-400" },
	{ value: 2, label: "Medium", color: "text-yellow-400" },
	{ value: 3, label: "High", color: "text-orange-400" },
	{ value: 4, label: "Critical", color: "text-red-400" },
];

export const BugReportConstants = {
	Headers: {
		Heading: "Report a Bug",
		SubHeading: "Help us improve by reporting issues",
	},
	Placeholders: {
		BugTitle: "Brief description of the bug",
		BugDescription:
			"Describe the bug in detail. Include steps to reproduce, expected behavior, and actual behavior.",
		AgentName: "The name or ID of Agent",
		BugSeverity: "Select severity level",
	},
	PIIMessage:
		"Please do not add any PII (Personally Identifiable Information)",
};

export const NewFeatureRequestConstants = {
	Headers: {
		Heading: "Request a Feature",
		SubHeading: "Share your ideas to help us improve",
	},
	Placeholders: {
		FeatureTitle: "Brief title for your feature request",
		FeatureDescription:
			"Describe your feature request in detail. What problem does it solve? How would it work?",
	},
	PIIMessage:
		"Please do not add any PII (Personally Identifiable Information)",
};

export const ApplicationConstants = {
	ChatbotAgentConfigKeyName: "AIChatbotAgentId",
};

export const KnowledgeBaseFlyoutPropsConstants = {
	Headers: {
		Heading: "Agent Knowledge Base",
		SubHeading: "Upload multiple files for your agent",
	},
	Hints: {
		DropItems: "Drop files here or click to browse",
		SupportedTypes:
			"Supports .pdf, .doc, .docx, .txt files up to 10MB each",
		Info: "You can upload multiple knowledge base files to enhance your agent's capabilities. Supported formats include PDF, Word documents, and text files. Each file must be under 10MB.",
	},
	Buttons: {
		Clear: "Clear all files",
		Done: "Done",
	},
};

export const AiVisionImagesFlyoutPropsConstants = {
	Headers: {
		Heading: "AI Vision Images",
		SubHeading:
			"Upload multiple image files for your agent to read and process",
	},
	Hints: {
		DropItems: "Drop images here or click to browse",
		SupportedTypes:
			"Supports .png, .jpg, .jpeg, .svg files up to 10MB each",
		Info: "You can upload multiple images that will be processed by your agent to enhance its capabilites. Supported formats include most common image formats. Each file must be under 10MB.",
	},
	Buttons: {
		Clear: "Clear all images",
		Done: "Done",
	},
};
