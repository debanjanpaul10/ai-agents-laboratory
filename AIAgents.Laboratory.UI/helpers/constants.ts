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
