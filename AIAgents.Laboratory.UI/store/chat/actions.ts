import { Action, Dispatch } from "redux";

import { ChatRequestDTO } from "@/models/chat-request-dto";
import { GET_CHAT_RESPONSE, TOGGLE_CHAT_RESPONSE_SPINNER } from "./actionTypes";
import { InvokeChatAgentApiAsync } from "@/lib/ai-agents-api-service";

export function ToggleChatResponseSpinner(isLoading: boolean) {
	return {
		type: TOGGLE_CHAT_RESPONSE_SPINNER,
		payload: isLoading,
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
			}
		} catch (error: any) {
			console.error(error);
			throw error;
		} finally {
			dispatch(ToggleChatResponseSpinner(false));
		}
	};
}
