import { DirectChatRequestDTO } from "@models/direct-chat-request-dto";
import { GetDirectChatResponseApiAsync } from "@shared/api-service";
import {
	DIRECT_CHAT_REQUEST,
	TOGGLE_AGENT_TEST_DRAWER,
	TOGGLE_AGENTS_LIST_DRAWER,
	TOGGLE_DIRECT_CHAT_DRAWER,
	TOGGLE_EDIT_AGENT_DRAWER,
	TOGGLE_MAIN_SPINNER,
	TOGGLE_NEW_AGENT_DRAWER,
} from "@store/common/actionTypes";
import { Action, Dispatch } from "redux";

export function ToggleNewAgentDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_NEW_AGENT_DRAWER,
		payload: isOpen,
	};
}

export function ToggleMainLoader(isLoading: boolean) {
	return {
		type: TOGGLE_MAIN_SPINNER,
		payload: isLoading,
	};
}

export function ToggleAgentsListDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_AGENTS_LIST_DRAWER,
		payload: isOpen,
	};
}

export function ToggleEditAgentDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_EDIT_AGENT_DRAWER,
		payload: isOpen,
	};
}

export function ToggleAgentTestDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_AGENT_TEST_DRAWER,
		payload: isOpen,
	};
}

export function ToggleDirectChatDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_DIRECT_CHAT_DRAWER,
		payload: isOpen,
	};
}

export function GetDirectChatResponseAsync(
	userMessage: DirectChatRequestDTO,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetDirectChatResponseApiAsync(
				userMessage,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: DIRECT_CHAT_REQUEST,
					payload: response.responseData,
				});

				return response.responseData;
			}
		} catch (error: any) {
			console.error(error);
		}
	};
}
