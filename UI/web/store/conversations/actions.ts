import { Action, Dispatch } from "redux";

import {
	ClearConversationHistoryForUserApiAsync,
	ClearWorkspaceConversationHistoryApiAsync,
	GetConversationHistoryDataForUserApiAsync,
	GetWorkspaceConversationHistoryApiAsync,
} from "@shared/api-service";
import { ToggleChatResponseSpinner } from "@store/agents/actions";
import {
	CLEAR_CONVERSATION_HISTORY,
	CLEAR_WORKSPACE_CONVERSATION_HISTORY,
	GET_CONVERSATION_HISTORY,
	GET_WORKSPACE_CONVERSATION_HISTORY,
} from "@store/conversations/actionTypes";
import { ShowErrorToaster, ShowSuccessToaster } from "@shared/toaster";
import { ChatToasterConstants } from "@helpers/toaster-constants";
import { ToggleDirectChatLoader } from "@store/common/actions";
import { ConversationHistoryDTO } from "@models/response/conversation-history-dto";

/**
 * Clears the conversation history for the user.
 * @returns A thunk action that dispatches the cleared conversation history to the store.
 */
export function ClearConversationHistoryAsync() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleDirectChatLoader(true));
			const response = await ClearConversationHistoryForUserApiAsync();
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: CLEAR_CONVERSATION_HISTORY,
					payload: response.responseData,
				});

				ShowSuccessToaster(ChatToasterConstants.CLEAR_CONVERSATION);
				dispatch(GetConversationHistoryDataForUserAsync() as any);
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

/**
 * Gets the conversation history data for the user.
 * @returns A thunk action that dispatches the conversation history data to the store.
 */
export function GetConversationHistoryDataForUserAsync() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleDirectChatLoader(true));
			const response = await GetConversationHistoryDataForUserApiAsync();
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

/**
 * Clears the conversation history for the workspace.
 * @param workspaceGuid The GUID of the workspace.
 * @param conversationId The ID of the conversation.
 * @returns A thunk action that dispatches the cleared conversation history to the store.
 */
export function ClearWorkspaceConversationHistory(
	workspaceGuid: string,
	conversationId: string,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleChatResponseSpinner(true));
			const response = await ClearWorkspaceConversationHistoryApiAsync(
				workspaceGuid,
				conversationId,
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: CLEAR_WORKSPACE_CONVERSATION_HISTORY,
					payload: response.responseData,
				});

				ShowSuccessToaster(ChatToasterConstants.CLEAR_CONVERSATION);
				dispatch(
					GetWorkspaceConversationHistoryAsync(workspaceGuid) as any,
				);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleChatResponseSpinner(false));
		}
	};
}

/**
 * Gets the conversation history for the workspace.
 * @param workspaceGuid The GUID of the workspace.
 * @returns A thunk action that dispatches the conversation history for the workspace to the store.
 */
export function GetWorkspaceConversationHistoryAsync(workspaceGuid: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleChatResponseSpinner(true));
			const response =
				await GetWorkspaceConversationHistoryApiAsync(workspaceGuid);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_WORKSPACE_CONVERSATION_HISTORY,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleChatResponseSpinner(false));
		}
	};
}
