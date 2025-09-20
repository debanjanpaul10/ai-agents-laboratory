import { GET_CHAT_RESPONSE, TOGGLE_CHAT_RESPONSE_SPINNER } from "./actionTypes";

const initialState: any = {
	chatResponse: {},
	isChatDataLoading: false,
};

export function ChatReducer(state = initialState, action: any) {
	switch (action.type) {
		case GET_CHAT_RESPONSE: {
			return {
				...state,
				chatResponse: action.payload,
			};
		}
		case TOGGLE_CHAT_RESPONSE_SPINNER: {
			return {
				...state,
				isChatDataLoading: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
