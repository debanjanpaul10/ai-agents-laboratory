export class ToolSkillDTO {
	toolSkillGuid: string = "";
	toolSkillDisplayName: string = "";
	toolSkillTechnicalName: string = "";
	toolSkillMcpServerUrl: string = "";
	associatedAgentGuids: string[] = [];
	dateCreated: Date = new Date();
	createdBy: string = "";
	dateModified: Date = new Date();
	modifiedBy: string = "";
}
