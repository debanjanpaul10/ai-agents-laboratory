import { Action, Dispatch } from "redux";

import { ChatRequestDTO } from "@models/request/chat-request-dto";
import {
	ClearConversationHistoryForUserApiAsync,
	GetConversationHistoryDataForUserApiAsync,
	GetDirectChatResponseApiAsync,
	InvokeChatAgentApiAsync,
} from "@shared/api-service";
import { ToggleChatResponseSpinner } from "@store/agents/actions";
import {
	CLEAR_CONVERSATION_HISTORY,
	DIRECT_CHAT_REQUEST,
	GET_CHAT_RESPONSE,
	GET_CONVERSATION_HISTORY,
} from "./actionTypes";
import { ToggleDirectChatLoader } from "@store/common/actions";
import { DirectChatRequestDTO } from "@models/request/direct-chat-request-dto";
import { ConversationHistoryDTO } from "@models/response/conversation-history-dto";
import { ShowErrorToaster, ShowSuccessToaster } from "@shared/toaster";
import { ChatToasterConstants } from "@helpers/toaster-constants";

export function InvokeChatAgentAsync(
	chatRequest: ChatRequestDTO,
	accessToken: string,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleChatResponseSpinner(true));
			const response = await InvokeChatAgentApiAsync(
				chatRequest,
				accessToken,
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
			if (error.message || typeof error === "string")
				ShowErrorToaster(error.message ?? error);
		} finally {
			dispatch(ToggleChatResponseSpinner(false));
		}
	};
}

export function ClearConversationHistoryAsync(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleDirectChatLoader(true));
			const response =
				await ClearConversationHistoryForUserApiAsync(accessToken);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: CLEAR_CONVERSATION_HISTORY,
					payload: response.responseData,
				});

				ShowSuccessToaster(ChatToasterConstants.CLEAR_CONVERSATION);
				dispatch(
					GetConversationHistoryDataForUserAsync(accessToken) as any,
				);
				return response.responseData as boolean;
			}
			return null;
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleDirectChatLoader(false));
		}
	};
}

export function GetDirectChatResponseAsync(
	userMessage: DirectChatRequestDTO,
	accessToken: string,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetDirectChatResponseApiAsync(
				userMessage,
				accessToken,
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
			if (error.message) ShowErrorToaster(error.message);
		}
	};
}

export function GetConversationHistoryDataForUserAsync(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleDirectChatLoader(true));
			const response =
				await GetConversationHistoryDataForUserApiAsync(accessToken);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_CONVERSATION_HISTORY,
					payload: response.responseData,
				});

				return response.responseData as ConversationHistoryDTO;
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleDirectChatLoader(false));
		}
	};
}
