import { AssociatedAgentsSkillDataDTO } from "./associated-agents-skill-data.dto";

export class ToolSkillDTO {
	toolSkillGuid: string = "";
	toolSkillDisplayName: string = "";
	toolSkillTechnicalName: string = "";
	toolSkillMcpServerUrl: string = "";
	associatedAgents: AssociatedAgentsSkillDataDTO[] = [];
	dateCreated: Date = new Date();
	createdBy: string = "";
	dateModified: Date = new Date();
	modifiedBy: string = "";
}
