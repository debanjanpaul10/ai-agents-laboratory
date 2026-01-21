import { Action, Dispatch } from "redux";

import { ReduxStoreType } from "@shared/types";
import {
	ADD_NEW_TOOL_SKILL,
	GET_ALL_MCP_SERVER_TOOLS,
	GET_ALL_TOOLS_SKILLS,
	GET_TOOL_SKILL_BY_ID,
	TOGGLE_ADD_SKILL_DRAWER,
	TOGGLE_CREATE_SKILLS_LOADER,
	TOGGLE_EDIT_SKILLS_LOADER,
	TOGGLE_TOOLS_SKILLS_LOADER,
	TOGGLE_MCP_TOOLS_LOADER,
	TOGGLE_MCP_TOOLS_DRAWER,
} from "@store/tools-skills/actionTypes";
import {
	AddNewToolSkillApiAsync,
	GetAllMcpToolsAvailableApiAsync,
	GetAllToolSkillsApiAsync,
	GetToolSkillBySkillIdApiAsync,
	UpdateExistingToolSkillDataApiAsync,
	DeleteExistingToolSkillBySkillIdApiAsync,
} from "@shared/api-service";
import { ShowErrorToaster, ShowSuccessToaster } from "@shared/toaster";
import { ToolSkillDTO } from "@models/response/tool-skill-dto";
import { McpServerToolRequestDTO } from "@models/request/mcp-server-tool-request-dto";
import { ToolSkillsToasterConstants } from "@helpers/toaster-constants";

export function ToggleToolSkillsLoader(isLoading: boolean): ReduxStoreType {
	return {
		type: TOGGLE_TOOLS_SKILLS_LOADER,
		payload: isLoading,
	};
}

export function ToggleEditSkillLoader(isLoading: boolean) {
	return {
		type: TOGGLE_EDIT_SKILLS_LOADER,
		payload: isLoading,
	};
}

export function ToggleCreateSkillLoader(isLoading: boolean) {
	return {
		type: TOGGLE_CREATE_SKILLS_LOADER,
		payload: isLoading,
	};
}

export function ToggleAddSkillDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_ADD_SKILL_DRAWER,
		payload: isOpen,
	};
}

export function ToggleMcpToolsLoader(isLoading: boolean) {
	return {
		type: TOGGLE_MCP_TOOLS_LOADER,
		payload: isLoading,
	};
}

export function ToggleMcpToolsDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_MCP_TOOLS_DRAWER,
		payload: isOpen,
	};
}

export function GetAllMcpToolsAvailableSuccessAsync(data: any) {
	return {
		type: GET_ALL_MCP_SERVER_TOOLS,
		payload: data,
	};
}

export function GetAllMcpToolsAvailableFailedAsync() {
	return {
		type: GET_ALL_MCP_SERVER_TOOLS,
		payload: [],
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

export function AddNewToolSkillAsync(
	newToolSkill: ToolSkillDTO | FormData,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleCreateSkillLoader(true));
			const response = await AddNewToolSkillApiAsync(
				newToolSkill,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: ADD_NEW_TOOL_SKILL,
					payload: response.responseData,
				});

				dispatch(ToggleAddSkillDrawer(false));
				dispatch(GetAllToolSkillsAsync(accessToken) as any);
				ShowSuccessToaster(ToolSkillsToasterConstants.CREATE_SKILL);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleMcpToolsDrawer(false));
			dispatch(ToggleCreateSkillLoader(false));
		}
	};
}

export function GetAllMcpToolsAvailableAsync(
	mcpServerTool: McpServerToolRequestDTO,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleMcpToolsLoader(true));
			dispatch(ToggleMcpToolsDrawer(true));

			const response = await GetAllMcpToolsAvailableApiAsync(
				mcpServerTool,
				accessToken
			);
			if (response?.isSuccess && response?.responseData)
				dispatch(
					GetAllMcpToolsAvailableSuccessAsync(response.responseData)
				);
			else dispatch(ToggleMcpToolsLoader(false));
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
			dispatch(GetAllMcpToolsAvailableFailedAsync());
		} finally {
			dispatch(ToggleMcpToolsLoader(false));
		}
	};
}
export function UpdateExistingToolSkillAsync(
	updateToolSkill: ToolSkillDTO | FormData,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleEditSkillLoader(true));
			const response = await UpdateExistingToolSkillDataApiAsync(
				updateToolSkill,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch(GetAllToolSkillsAsync(accessToken) as any);
				ShowSuccessToaster(ToolSkillsToasterConstants.UPDATE_SKILL);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleEditSkillLoader(false));
		}
	};
}

export function DeleteExistingToolSkillAsync(
	skillId: string,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleToolSkillsLoader(true));
			const response = await DeleteExistingToolSkillBySkillIdApiAsync(
				skillId,
				accessToken
			);
			if (response?.isSuccess) {
				dispatch(GetAllToolSkillsAsync(accessToken) as any);
				ShowSuccessToaster(ToolSkillsToasterConstants.DELETE_SKILL);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleToolSkillsLoader(false));
		}
	};
}
