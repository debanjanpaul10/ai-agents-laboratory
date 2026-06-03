import { Action, Dispatch } from "redux";

import { ChatRequestDTO } from "@models/request/chat-request-dto";
import {
	GetDirectChatResponseApiAsync,
	InvokeChatAgentApiAsync,
} from "@shared/api-service";
import { ToggleChatResponseSpinner } from "@store/agents/actions";
import {
	DIRECT_CHAT_REQUEST,
	GET_CHAT_RESPONSE,
} from "@store/chat/actionTypes";
import { DirectChatRequestDTO } from "@models/request/direct-chat-request-dto";
import { ShowErrorToaster } from "@shared/toaster";

export function InvokeChatAgentAsync(chatRequest: ChatRequestDTO) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleChatResponseSpinner(true));
			const response = await InvokeChatAgentApiAsync(chatRequest);
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
			if (error.message || typeof error === "string")
				ShowErrorToaster(error.message ?? error);
		} finally {
			dispatch(ToggleChatResponseSpinner(false));
		}
	};
}

export function GetDirectChatResponseAsync(userMessage: DirectChatRequestDTO) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetDirectChatResponseApiAsync(userMessage);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: DIRECT_CHAT_REQUEST,
					payload: response.responseData,
				});

				return response.responseData;
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		}
	};
}
