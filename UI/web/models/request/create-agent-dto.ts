export class CreateAgentDTO {
    agentName: string = "";
    agentDescription: string = "";
    agentMetaPrompt: string = "";
    applicationId: number = 0;
    knowledgeBaseDocument: File[] | null = null;
    isPrivate: boolean = false;
    visionImages: File[] | null = null;
    associatedSkillGuids: string[] = [];
}
