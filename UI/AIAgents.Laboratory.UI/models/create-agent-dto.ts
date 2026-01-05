export class CreateAgentDTO {
	agentName: string = "";
	agentMetaPrompt: string = "";
	applicationName: string = "";
	knowledgeBaseDocument: File[] | null = null;
	isPrivate: boolean = false;
	mcpServerUrl: string = "";
}
