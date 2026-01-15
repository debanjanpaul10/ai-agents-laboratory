export class ToolSkillDTO {
	toolSkillGuid: string = "";
	toolSkillDisplayName: string = "";
	toolSkillTechnicalName: string = "";
	toolSkillMcpServerUrl: string = "";
	associatedAgents: Record<string, string> = {};
	dateCreated: Date = new Date();
	createdBy: string = "";
	dateModified: Date = new Date();
	modifiedBy: string = "";
}
