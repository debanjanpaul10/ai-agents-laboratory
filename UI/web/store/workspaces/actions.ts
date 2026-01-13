import { Action, Dispatch } from "redux";
import { GET_ALL_WORKSPACES, TOGGLE_WORKSPACES_LOADER } from "./actionTypes";
import { ShowErrorToaster } from "@shared/toaster";
import { GetAllWorkspacesDataApiAsync } from "@shared/api-service";

export function ToggleWorkspacesLoader(isLoading: boolean) {
	return {
		type: TOGGLE_WORKSPACES_LOADER,
		payload: isLoading,
	};
}

export function GetAllWorkspacesDataAsync(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleWorkspacesLoader(true));
			const response = await GetAllWorkspacesDataApiAsync(accessToken);
			if (response?.isSuccess && response?.responseData)
				dispatch({
					type: GET_ALL_WORKSPACES,
					payload: response.responseData,
				});
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleWorkspacesLoader(false));
		}
	};
}
