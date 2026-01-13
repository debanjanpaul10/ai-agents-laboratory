import { Action, Dispatch } from "redux";

import { ReduxStoreType } from "@shared/types";
import {
	GET_ALL_TOOLS_SKILLS,
	GET_TOOL_SKILL_BY_ID,
	TOGGLE_TOOLS_SKILLS_LOADER,
} from "@store/tools-skills/actionTypes";
import {
	GetAllToolSkillsApiAsync,
	GetToolSkillBySkillIdApiAsync,
} from "@shared/api-service";
import { ShowErrorToaster } from "@shared/toaster";

export function ToggleToolSkillsLoader(isLoading: boolean): ReduxStoreType {
	return {
		type: TOGGLE_TOOLS_SKILLS_LOADER,
		payload: isLoading,
	};
}

export function GetAllToolSkillsAsync(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleToolSkillsLoader(true));
			const response = await GetAllToolSkillsApiAsync(accessToken);
			if (response?.isSuccess && response?.responseData)
				dispatch({
					type: GET_ALL_TOOLS_SKILLS,
					payload: response.responseData,
				});
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleToolSkillsLoader(false));
		}
	};
}

export function GetToolSkillBySkillIdAsync(
	skillId: string,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleToolSkillsLoader(true));

			const response = await GetToolSkillBySkillIdApiAsync(
				skillId,
				accessToken
			);
			if (response?.isSuccess && response?.responseData)
				dispatch({
					type: GET_TOOL_SKILL_BY_ID,
					payload: response.responseData,
				});
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleToolSkillsLoader(false));
		}
	};
}
