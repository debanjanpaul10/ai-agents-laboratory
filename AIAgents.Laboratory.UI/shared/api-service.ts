import { GetAsync, PostAsync } from "@helpers/http-utility";
import { AgentDataDTO } from "@models/agent-data-dto";
import { ChatRequestDTO } from "@models/chat-request-dto";
import { CreateAgentDTO } from "@models/create-agent-dto";
import { DirectChatRequestDTO } from "@models/direct-chat-request-dto";

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
	newAgentData: CreateAgentDTO,
	accessToken: string
) {
	return await PostAsync("agents/createagent", newAgentData, accessToken);
}

export async function UpdateExistingAgentApiAsync(
	updateAgentData: AgentDataDTO,
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
