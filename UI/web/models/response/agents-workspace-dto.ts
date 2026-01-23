import { WorkspaceAgentsDataDTO } from "./workspace-agents-data.dto";

export class AgentsWorkspaceDTO {
	agentWorkspaceGuid: string = "";
	agentWorkspaceName: string = "";
	activeAgentsListInWorkspace: WorkspaceAgentsDataDTO[] = [];
	workspaceUsers: string[] = [];
	isGroupChatEnabled: boolean = false;
	dateCreated: Date = new Date();
	createdBy: string = "";
	dateModified: Date = new Date();
	modifiedBy: string = "";
}
