import {
	DIRECT_CHAT_REQUEST,
	GET_CHAT_RESPONSE,
} from "@store/chat/actionTypes";

const initialState = {
	chatResponse: {},
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
		case DIRECT_CHAT_REQUEST: {
			return {
				...state,
				directChatResponse: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
