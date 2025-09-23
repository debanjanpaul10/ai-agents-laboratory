import { Action, Dispatch } from "redux";
import {
	CREATE_NEW_AGENT,
	DELETE_AGENT_DATA,
	GET_AGENT_BY_ID,
	GET_ALL_AGENTS_DATA,
	GET_CHAT_RESPONSE,
	TOGGLE_AGENT_CREATE_SPINNER,
	TOGGLE_CHAT_RESPONSE_SPINNER,
	TOGGLE_EDIT_AGENT_SPINNER,
	UPDATE_AGENT_DATA,
} from "@store/agents/actionTypes";
import {
	CreateNewAgentApiAsync,
	DeleteExistingAgentApiAsync,
	GetAgentByIdApiAsync,
	GetAgentsApiAsync,
	InvokeChatAgentApiAsync,
	UpdateExistingAgentApiAsync,
} from "@shared/api-service";
import { CreateAgentDTO } from "@models/create-agent-dto";
import { AgentDataDTO } from "@models/agent-data-dto";
import { ToggleMainLoader } from "@store/common/actions";
import { ChatRequestDTO } from "@models/chat-request-dto";

export function ToggleCreateAgentSpinner(isLoading: boolean) {
	return {
		type: TOGGLE_AGENT_CREATE_SPINNER,
		payload: isLoading,
	};
}

export function ToggleChatResponseSpinner(isLoading: boolean) {
	return {
		type: TOGGLE_CHAT_RESPONSE_SPINNER,
		payload: isLoading,
	};
}

export function ToggleEditAgentSpinner(isLoading: boolean) {
	return {
		type: TOGGLE_EDIT_AGENT_SPINNER,
		payload: isLoading,
	};
}

export function GetAllAgentsDataAsync(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleMainLoader(true));
			const response = await GetAgentsApiAsync(accessToken);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_ALL_AGENTS_DATA,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			throw error;
		} finally {
			dispatch(ToggleMainLoader(false));
		}
	};
}

export function GetAgentDataByIdAsync(agentId: string, accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleMainLoader(true));
			const response = await GetAgentByIdApiAsync(agentId, accessToken);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_AGENT_BY_ID,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			throw error;
		} finally {
			dispatch(ToggleMainLoader(false));
		}
	};
}

export function CreateNewAgentAsync(
	newAgentData: CreateAgentDTO,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleCreateAgentSpinner(true));
			const response = await CreateNewAgentApiAsync(
				newAgentData,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: CREATE_NEW_AGENT,
					payload: response.responseData,
				});

				dispatch(GetAllAgentsDataAsync(accessToken) as any);
			}
		} catch (error: any) {
			console.error(error);
			throw error;
		} finally {
			dispatch(ToggleCreateAgentSpinner(false));
		}
	};
}

export function UpdateExistingAgentDataAsync(
	existingAgentData: AgentDataDTO,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleEditAgentSpinner(true));
			const response = await UpdateExistingAgentApiAsync(
				existingAgentData,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: UPDATE_AGENT_DATA,
					payload: response.responseData,
				});
				dispatch(GetAllAgentsDataAsync(accessToken) as any);
			}
		} catch (error: any) {
			console.error(error);
			throw error;
		} finally {
			dispatch(ToggleEditAgentSpinner(false));
		}
	};
}

export function DeleteExistingAgentDataAsync(
	agentId: string,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleMainLoader(true));
			const response = await DeleteExistingAgentApiAsync(
				agentId,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: DELETE_AGENT_DATA,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			throw error;
		} finally {
			dispatch(ToggleMainLoader(false));
		}
	};
}

export function InvokeChatAgentAsync(
	chatRequest: ChatRequestDTO,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleChatResponseSpinner(true));
			const response = await InvokeChatAgentApiAsync(
				chatRequest,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_CHAT_RESPONSE,
					payload: response.responseData,
				});

				return response.responseData as string;
			}

			return null;
		} catch (error: any) {
			console.error(error);
			throw error;
		} finally {
			dispatch(ToggleChatResponseSpinner(false));
		}
	};
}
