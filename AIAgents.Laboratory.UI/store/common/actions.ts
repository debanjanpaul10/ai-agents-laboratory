import { Action, Dispatch } from "redux";

import { GetConfigurationsDataApiAsync } from "@shared/api-service";
import { addToast } from "@heroui/toast";
import {
	GET_ALL_CONFIGURATIONS,
	TOGGLE_AGENT_TEST_DRAWER,
	TOGGLE_AGENTS_LIST_DRAWER,
	TOGGLE_DIRECT_CHAT_DRAWER,
	TOGGLE_DIRECT_CHAT_LOADER,
	TOGGLE_EDIT_AGENT_DRAWER,
	TOGGLE_MAIN_SPINNER,
	TOGGLE_NEW_AGENT_DRAWER,
} from "@store/common/actionTypes";
import { ShowErrorToaster } from "@shared/toaster";

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

export function ToggleDirectChatLoader(isLoading: boolean) {
	return {
		type: TOGGLE_DIRECT_CHAT_LOADER,
		payload: isLoading,
	};
}

export function GetAllConfigurations(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetConfigurationsDataApiAsync(accessToken);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_ALL_CONFIGURATIONS,
					payload: response.responseData,
				});

				return response.responseData as {};
			}
		} catch (error: any) {
			console.error(error);
			ShowErrorToaster(error);
		}
	};
}
