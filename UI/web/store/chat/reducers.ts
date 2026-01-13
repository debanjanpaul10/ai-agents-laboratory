import {
	CLEAR_CONVERSATION_HISTORY,
	DIRECT_CHAT_REQUEST,
	GET_CHAT_RESPONSE,
	GET_CONVERSATION_HISTORY,
} from "./actionTypes";

const initialState = {
	chatResponse: {},
	isConversationHistoryClear: false,
	directChatResponse: "",
	conversationHistory: {},
};

export function ChatReducer(state = initialState, action: any) {
	switch (action.type) {
		case GET_CHAT_RESPONSE: {
			return {
				...state,
				chatResponse: action.payload,
			};
		}
		case CLEAR_CONVERSATION_HISTORY: {
			return {
				...state,
				isConversationHistoryClear: action.payload,
			};
		}
		case DIRECT_CHAT_REQUEST: {
			return {
				...state,
				directChatResponse: action.payload,
			};
		}
		case GET_CONVERSATION_HISTORY: {
			return {
				...state,
				conversationHistory: action.payload,
			};
		}
		default: {
			return {
				...state,
			};
		}
	}
}
