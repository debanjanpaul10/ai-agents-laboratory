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
    TOGGLE_REGISTERED_APPLICATIONS_LOADER,
    GET_ALL_REGISTERED_APPLICATIONS,
    ADD_NEW_REGISTERED_APPLICATION,
    GET_REGISTERED_APPLICATION_BY_ID,
    UPDATE_REGISTERED_APPLICATION,
    DELETE_REGISTERED_APPLICATION,
    TOGGLE_REGISTER_NEW_APPLICATION_DRAWER,
} from "@store/tools-skills/actionTypes";
import {
    AddNewToolSkillApiAsync,
    GetAllMcpToolsAvailableApiAsync,
    GetAllToolSkillsApiAsync,
    GetToolSkillBySkillIdApiAsync,
    UpdateExistingToolSkillDataApiAsync,
    DeleteExistingToolSkillBySkillIdApiAsync,
    GetAllRegisteredApplicationsApiAsync,
    RegisterNewApplicationApiAsync,
    GetRegisteredApplicationByIdApiAsync,
    UpdateExistingRegisteredApplicationApiAsync,
    DeleteRegisteredApplicationByIdApiAsync,
} from "@shared/api-service";
import { ShowErrorToaster, ShowSuccessToaster } from "@shared/toaster";
import { ToolSkillDTO } from "@models/response/tool-skill-dto";
import { McpServerToolRequestDTO } from "@models/request/mcp-server-tool-request-dto";
import { ToolSkillsToasterConstants } from "@helpers/toaster-constants";
import { RegisteredApplicationDTO } from "@models/request/registered-application.dto";

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

export function ToggleRegisterApplicationsLoader(isLoading: boolean) {
    return {
        type: TOGGLE_REGISTERED_APPLICATIONS_LOADER,
        payload: isLoading,
    };
}

export function ToggleRegisterNewApplicationDrawer(isOpen: boolean) {
    return {
        type: TOGGLE_REGISTER_NEW_APPLICATION_DRAWER,
        payload: isOpen,
    }
}

export function GetAllToolSkillsAsync() {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleToolSkillsLoader(true));
            const response = await GetAllToolSkillsApiAsync();
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

export function GetToolSkillBySkillIdAsync(skillId: string) {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleToolSkillsLoader(true));

            const response = await GetToolSkillBySkillIdApiAsync(skillId);
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

export function AddNewToolSkillAsync(newToolSkill: ToolSkillDTO | FormData) {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleCreateSkillLoader(true));
            const response = await AddNewToolSkillApiAsync(newToolSkill);
            if (response?.isSuccess && response?.responseData) {
                dispatch({
                    type: ADD_NEW_TOOL_SKILL,
                    payload: response.responseData,
                });

                dispatch(ToggleAddSkillDrawer(false));
                dispatch(GetAllToolSkillsAsync() as any);
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
) {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleMcpToolsLoader(true));
            dispatch(ToggleMcpToolsDrawer(true));

            const response =
                await GetAllMcpToolsAvailableApiAsync(mcpServerTool);
            if (response?.isSuccess && response?.responseData)
                dispatch(
                    GetAllMcpToolsAvailableSuccessAsync(response.responseData),
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
) {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleEditSkillLoader(true));
            const response =
                await UpdateExistingToolSkillDataApiAsync(updateToolSkill);
            if (response?.isSuccess && response?.responseData) {
                dispatch(GetAllToolSkillsAsync() as any);
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

export function DeleteExistingToolSkillAsync(skillId: string) {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleToolSkillsLoader(true));
            const response =
                await DeleteExistingToolSkillBySkillIdApiAsync(skillId);
            if (response?.isSuccess) {
                dispatch(GetAllToolSkillsAsync() as any);
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

export function GetAllRegisteredApplicationsAsync() {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleRegisterApplicationsLoader(true));
            const response = await GetAllRegisteredApplicationsApiAsync();
            if (response?.isSuccess)
                dispatch({
                    type: GET_ALL_REGISTERED_APPLICATIONS,
                    payload: response.responseData,
                });
        } catch (error: any) {
            console.error(error);
            if (error.message) ShowErrorToaster(error.message);
        } finally {
            dispatch(ToggleRegisterApplicationsLoader(false));
        }
    };
}

export function RegisterNewApplicationAsync(
    newApplicationModel: RegisteredApplicationDTO,
) {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleRegisterApplicationsLoader(true));
            const response =
                await RegisterNewApplicationApiAsync(newApplicationModel);
            if (response?.isSuccess) {
                dispatch({
                    type: ADD_NEW_REGISTERED_APPLICATION,
                    payload: response.responseData,
                });
                ShowSuccessToaster(
                    ToolSkillsToasterConstants.CREATE_NEW_APPLICATION,
                );
                dispatch(GetAllRegisteredApplicationsAsync() as any);
            }
        } catch (error: any) {
            console.error(error);
            if (error.message) ShowErrorToaster(error.message);
        } finally {
            dispatch(ToggleRegisterApplicationsLoader(false));
        }
    };
}

export function GetRegisteredApplicationByIdAsync(applicationId: number) {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleRegisterApplicationsLoader(true));
            const response =
                await GetRegisteredApplicationByIdApiAsync(applicationId);
            if (response?.isSuccess)
                dispatch({
                    type: GET_REGISTERED_APPLICATION_BY_ID,
                    payload: response.responseData,
                });
        } catch (error: any) {
            console.error(error);
            if (error.message) ShowErrorToaster(error.message);
        } finally {
            dispatch(ToggleRegisterApplicationsLoader(false));
        }
    };
}

export function UpdateExistingRegisteredApplicationAsync(
    updateApplicationDtoModel: RegisteredApplicationDTO,
) {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleRegisterApplicationsLoader(true));
            const response = await UpdateExistingRegisteredApplicationApiAsync(
                updateApplicationDtoModel,
            );
            if (response?.isSuccess) {
                dispatch({
                    type: UPDATE_REGISTERED_APPLICATION,
                    payload: response.responseData,
                });
                ShowSuccessToaster(
                    ToolSkillsToasterConstants.UPDATE_APPLICATION,
                );
                dispatch(GetAllRegisteredApplicationsAsync() as any);
            }
        } catch (error: any) {
            console.error(error);
            if (error.message) ShowErrorToaster(error.message);
        } finally {
            dispatch(ToggleRegisterApplicationsLoader(false));
        }
    };
}

export function DeleteRegisteredApplicationByIdAsync(applicationId: number) {
    return async (dispatch: Dispatch<Action>) => {
        try {
            dispatch(ToggleRegisterApplicationsLoader(true));
            const response =
                await DeleteRegisteredApplicationByIdApiAsync(applicationId);
            if (response?.isSuccess) {
                dispatch({
                    type: DELETE_REGISTERED_APPLICATION,
                    payload: response.responseData,
                });
                ShowSuccessToaster(
                    ToolSkillsToasterConstants.DELETE_APPLICATION,
                );
                dispatch(GetAllRegisteredApplicationsAsync() as any);
            }
        } catch (error: any) {
            console.error(error);
            if (error.message) ShowErrorToaster(error.message);
        } finally {
            dispatch(ToggleRegisterApplicationsLoader(false));
        }
    };
}
