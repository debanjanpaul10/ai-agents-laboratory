import {
	DeleteAsync,
	GetAsync,
	PostAsync,
	PutAsync,
} from "@helpers/http-utility";
import { AddBugReportDTO } from "@models/request/add-bug-report-dto";
import { AgentDataDTO } from "@models/response/agent-data-dto";
import { ChatRequestDTO } from "@models/request/chat-request-dto";
import { CreateAgentDTO } from "@models/request/create-agent-dto";
import { DirectChatRequestDTO } from "@models/request/direct-chat-request-dto";
import { McpServerToolRequestDTO } from "@models/request/mcp-server-tool-request-dto";
import { NewFeatureRequestDTO } from "@models/request/new-feature-request-dto";
import { ToolSkillDTO } from "@models/response/tool-skill-dto";
import { AgentsWorkspaceDTO } from "@models/response/agents-workspace-dto";
import { WorkspaceAgentChatRequestDTO } from "@models/request/workspace-agent-chat-request.dto";
import { DownloadFileDTO } from "@models/request/download-file.dto";

// #region AGENTS

export async function GetAgentsApiAsync() {
	return await GetAsync("agents/getallagents");
}

export async function GetAgentByIdApiAsync(agentId: string) {
	return await GetAsync(`agents/getagentbyid/${agentId}`);
}

export async function CreateNewAgentApiAsync(
	newAgentData: CreateAgentDTO | FormData,
) {
	return await PostAsync("agents/createagent", newAgentData);
}

export async function UpdateExistingAgentApiAsync(
	updateAgentData: AgentDataDTO | FormData,
) {
	return await PutAsync("agents/updateagent", updateAgentData);
}

export async function DeleteExistingAgentApiAsync(agentId: string) {
	return await DeleteAsync(`agents/deleteagent/${agentId}`);
}

export async function DownloadKnowledgebaseFileApiAsync(
	downloadFileData: DownloadFileDTO,
) {
	return await PostAsync(
		"agents/downloadassociateddocuments",
		downloadFileData,
	);
}

// #endregion

// #region CHAT

export async function InvokeChatAgentApiAsync(chatRequest: ChatRequestDTO) {
	return await PostAsync("chat/invokeagent", chatRequest);
}

export async function GetDirectChatResponseApiAsync(
	chatRequest: DirectChatRequestDTO,
) {
	return await PostAsync("chat/directchat", chatRequest);
}

export async function ClearConversationHistoryForUserApiAsync() {
	return await PostAsync("chat/clearconversation", null);
}

export async function GetConversationHistoryDataForUserApiAsync() {
	return await GetAsync("chat/getconversationhistory");
}

// #endregion

// #region COMMON

export async function GetConfigurationsDataApiAsync() {
	return await GetAsync("aiagentslab/getconfigurations");
}

export async function GetConfigurationByKeyNameApiAsync(keyName: string) {
	return await GetAsync(`aiagentslab/getconfigurationbykey/${keyName}`);
}

export async function AddBugReportDataApiAsync(addBugReport: AddBugReportDTO) {
	return await PostAsync("aiagentslab/addbugreport", addBugReport);
}

export async function SubmitFeatureRequestDataApiAsync(
	newFeatureRequest: NewFeatureRequestDTO,
) {
	return await PostAsync("aiagentslab/submitfeaturerequest", newFeatureRequest);
}

export async function GetTopActiveAgentsDataApiAsync() {
	return await GetAsync("aiagentslab/topactiveagents");
}

// #endregion

// #region MARKETPLACE

export async function GetAllToolSkillsApiAsync() {
	return await GetAsync("toolskills/getalltoolskills");
}

export async function GetToolSkillBySkillIdApiAsync(skillId: string) {
	return await GetAsync(`toolskills/gettoolskill/${skillId}`);
}

export async function AddNewToolSkillApiAsync(
	toolSkillData: ToolSkillDTO | FormData,
) {
	return await PostAsync("toolskills/addtoolskill", toolSkillData);
}

export async function UpdateExistingToolSkillDataApiAsync(
	updateToolSkillData: ToolSkillDTO | FormData,
) {
	return await PutAsync("toolskills/updatetoolskill", updateToolSkillData);
}

export async function DeleteExistingToolSkillBySkillIdApiAsync(
	skillId: string,
) {
	return await DeleteAsync(`toolskills/deletetoolskill/${skillId}`);
}

export async function GetAllMcpToolsAvailableApiAsync(
	mcpServerTool: McpServerToolRequestDTO,
) {
	return await PostAsync(
		"toolskills/getallmcptoolsavailable",
		mcpServerTool,
	);
}

// #endregion

// #region WORKSPACES

export async function GetAllWorkspacesDataApiAsync() {
	return await GetAsync("workspaces/getallworkspaces");
}

export async function GetWorkspaceByWorkspaceIdApiAsync(workspaceId: string) {
	return await GetAsync(`workspaces/getworkspace/${workspaceId}`);
}

export async function CreateNewWorkspaceApiAsync(
	agentsWorkspaceData: AgentsWorkspaceDTO | FormData,
) {
	return await PostAsync("workspaces/createworkspace", agentsWorkspaceData);
}

export async function DeleteExistingWorkspaceApiAsync(workspaceGuidId: string) {
	return await DeleteAsync(`workspaces/deleteworkspace/${workspaceGuidId}`);
}

export async function UpdateExistingWorkspaceDataApiAsync(
	agentsWorkspaceData: AgentsWorkspaceDTO | FormData,
) {
	return await PutAsync("workspaces/updateworkspace", agentsWorkspaceData);
}

export async function GetWorkspaceGroupChatResponseApiAsync(
	chatRequestDto: WorkspaceAgentChatRequestDTO,
) {
	return await PostAsync(
		"workspaces/workspacegroupchatresponse",
		chatRequestDto,
	);
}

// #endregion
