import { GetAsync, PostAsync } from "@helpers/http-utility";
import { AddBugReportDTO } from "@models/add-bug-report-dto";
import { AgentDataDTO } from "@models/agent-data-dto";
import { ChatRequestDTO } from "@models/chat-request-dto";
import { CreateAgentDTO } from "@models/create-agent-dto";
import { DirectChatRequestDTO } from "@models/direct-chat-request-dto";
import { NewFeatureRequestDTO } from "@models/new-feature-request-dto";

export async function GetAgentsApiAsync(accessToken: string) {
	return await GetAsync("agents/getallagents", accessToken);
}

export async function GetAgentByIdApiAsync(
	agentId: string,
	accessToken: string
) {
	return await GetAsync(`agents/getagentbyid/${agentId}`, accessToken);
}

export async function CreateNewAgentApiAsync(
	newAgentData: CreateAgentDTO | FormData,
	accessToken: string
) {
	return await PostAsync("agents/createagent", newAgentData, accessToken);
}

export async function UpdateExistingAgentApiAsync(
	updateAgentData: AgentDataDTO | FormData,
	accessToken: string
) {
	return await PostAsync("agents/updateagent", updateAgentData, accessToken);
}

export async function DeleteExistingAgentApiAsync(
	agentId: string,
	accessToken: string
) {
	return await PostAsync(`agents/deleteagent/${agentId}`, null, accessToken);
}

export async function InvokeChatAgentApiAsync(
	chatRequest: ChatRequestDTO,
	accessToken: string
) {
	return await PostAsync("chat/invokeagent", chatRequest, accessToken);
}

export async function GetDirectChatResponseApiAsync(
	chatRequest: DirectChatRequestDTO,
	accessToken: string
) {
	return await PostAsync("chat/directchat", chatRequest, accessToken);
}

export async function ClearConversationHistoryForUserApiAsync(
	accessToken: string
) {
	return await PostAsync("chat/clearconversation", null, accessToken);
}

export async function GetConversationHistoryDataForUserApiAsync(
	accessToken: string
) {
	return await GetAsync("chat/getconversationhistory", accessToken);
}

export async function GetConfigurationsDataApiAsync(accessToken: string) {
	return await GetAsync("aiagentslab/getconfigurations", accessToken);
}

export async function GetConfigurationByKeyNameApiAsync(
	keyName: string,
	accessToken: string
) {
	return await GetAsync(
		`aiagentslab/getconfigurationbykey/${keyName}`,
		accessToken
	);
}

export async function AddBugReportDataApiAsync(
	addBugReport: AddBugReportDTO,
	accessToken: string
) {
	return await PostAsync(
		"aiagentslab/addbugreport",
		addBugReport,
		accessToken
	);
}

export async function SubmitFeatureRequestDataApiAsync(
	newFeatureRequest: NewFeatureRequestDTO,
	accessToken: string
) {
	return await PostAsync(
		"aiagents/submitfeaturerequest",
		newFeatureRequest,
		accessToken
	);
}
