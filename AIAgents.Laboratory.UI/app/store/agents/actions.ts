import { Action, Dispatch } from "redux";
import { GET_ALL_AGENTS_DATA, TOGGLE_SPINNER } from "./actionTypes";
import { GetAgentsApiAsync } from "@/lib/ai-agents-api-service";

export function ToggleMainLoader(isLoading: boolean) {
	return {
		type: TOGGLE_SPINNER,
		payload: isLoading,
	};
}

export function GetAllAgentsDataAsync(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleMainLoader(true));
			const response = await GetAgentsApiAsync(accessToken);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_ALL_AGENTS_DATA,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			throw error;
		} finally {
			dispatch(ToggleMainLoader(false));
		}
	};
}
