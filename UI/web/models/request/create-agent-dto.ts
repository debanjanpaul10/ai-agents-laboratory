export class CreateAgentDTO {
	agentName: string = "";
	agentDescription: string = "";
	agentMetaPrompt: string = "";
	applicationName: string = "";
	knowledgeBaseDocument: File[] | null = null;
	isPrivate: boolean = false;
	visionImages: File[] | null = null;
	associatedSkillGuids: string[] = [];
}
