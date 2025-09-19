import { Action, Dispatch } from "redux";
import {
	CREATE_NEW_AGENT,
	DELETE_AGENT_DATA,
	GET_AGENT_BY_ID,
	GET_ALL_AGENTS_DATA,
	TOGGLE_AGENT_CREATE_SPINNER,
	UPDATE_AGENT_DATA,
} from "./actionTypes";
import {
	CreateNewAgentApiAsync,
	DeleteExistingAgentApiAsync,
	GetAgentByIdApiAsync,
	GetAgentsApiAsync,
	UpdateExistingAgentApiAsync,
} from "@/lib/ai-agents-api-service";
import { CreateAgentDTO } from "@/models/create-agent-dto";
import { AgentDataDTO } from "@/models/agent-data-dto";
import { ToggleMainLoader } from "../common/actions";

export function ToggleCreateAgentSpinner(isLoading: boolean) {
	return {
		type: TOGGLE_AGENT_CREATE_SPINNER,
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
			dispatch(ToggleMainLoader(true));
			const response = await UpdateExistingAgentApiAsync(
				existingAgentData,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: UPDATE_AGENT_DATA,
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
