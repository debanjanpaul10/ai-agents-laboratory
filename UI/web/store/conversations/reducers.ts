import {
	CLEAR_CONVERSATION_HISTORY,
	CLEAR_WORKSPACE_CONVERSATION_HISTORY,
	GET_CONVERSATION_HISTORY,
	GET_WORKSPACE_CONVERSATION_HISTORY,
} from "@store/conversations/actionTypes";

const initialState: any = {
	isConversationHistoryClear: false,
	isWorkspaceConversationHistoryCleared: false,

	conversationHistory: {},
	workspaceConversationHistory: [],
};

/**
 * The `ConversationsReducer` function is a reducer that handles the state changes for the conversations feature in the application. It listens for specific action types and updates the state accordingly.
 * @param state The current state of the conversations feature. It has an initial state defined as an object with properties for conversation history and workspace conversation history.
 * @param action The action dispatched to the reducer. It is an object that has a type property and an optional payload property. The type property is used to determine how to update the state, while the payload property contains any data needed for the update.
 * @returns A new state object that reflects the changes based on the action type. If the action type is not recognized, it returns the current state without any modifications.
 */
export function ConversationsReducer(state = initialState, action: any) {
	switch (action.type) {
		case GET_CONVERSATION_HISTORY: {
			return {
				...state,
				conversationHistory: action.payload,
			};
		}
		case CLEAR_CONVERSATION_HISTORY: {
			return {
				...state,
				isConversationHistoryClear: action.payload,
			};
		}

		case GET_WORKSPACE_CONVERSATION_HISTORY: {
			return {
				...state,
				workspaceConversationHistory: action.payload,
			};
		}
		case CLEAR_WORKSPACE_CONVERSATION_HISTORY: {
			return {
				...state,
				isWorkspaceConversationHistoryCleared: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
