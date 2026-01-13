export class AgentsWorkspaceDTO {
	agentWorkspaceGuid: string = "";
	agentWorkspaceName: string = "";
	activeAgentsListInWorkspace: any = {};
	workspaceUsers: string[] = [];
	dateCreated: Date = new Date();
	createdBy: string = "";
	dateModified: Date = new Date();
	modifiedBy: string = "";
}
