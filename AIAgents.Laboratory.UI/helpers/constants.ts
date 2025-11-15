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
		MainLoader: "Loading AI Agents Data ...",
		SaveNewAgentLoader: "Saving New Agent ...",
		LoginRedirectLoader: "Redirecting to Login ...",
		SaveAgentDataLoader: "Saving agent data ...",
	},
};

export const CreateAgentConstants = {
	Headers: {
		SubText: "Build your AI agent from scratch",
	},
	InputFields: {
		AgentNamePlaceholder: "Enter agent name ...",
		ApplicationNamePlaceholder: "Enter application name ...",
		AgentMetaPromptPlaceholder:
			"Define your agent's behavior, personality, and capabilities ...",
	},
};

export const ManageAgentConstants = {
	Headers: {
		SubText: "Manage Existing AI Agents",
	},
	ModifyAgentConstants: {
		MainHeader: "Modify AI Agent Configuration",
		Placeholders: {
			AgentName: "Enter agent name...",
			ApplicationName: "Enter application name ...",
			AgentMetaprompt:
				"Define your agent's behavior, personality, and capabilities ...",
		},
		Info: "Describe how your agent should behave, what it should know, and how it should respond to users.",
		KBInfo: "You can upload knowledge base data to the agent. Available file formats: .pdf, .docx, .xlsx, .txt",
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
