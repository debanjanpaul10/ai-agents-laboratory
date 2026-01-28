import { Action, Dispatch } from "redux";

import {
	AddBugReportDataApiAsync,
	GetConfigurationByKeyNameApiAsync,
	GetConfigurationsDataApiAsync,
	GetTopActiveAgentsDataApiAsync,
	SubmitFeatureRequestDataApiAsync,
} from "@shared/api-service";
import {
	ADD_NEW_BUG_REPORT,
	ADD_NEW_FEATURE_REQUEST,
	GET_ALL_CONFIGURATIONS,
	GET_CONFIGURATION_BY_KEY_NAME,
	GET_TOP_ACTIVE_AGENTS,
	TOGGLE_DIRECT_CHAT_DRAWER,
	TOGGLE_DIRECT_CHAT_LOADER,
	TOGGLE_FEEDBACK_DRAWER,
	TOGGLE_FEEDBACK_LOADER,
	TOGGLE_MAIN_SPINNER,
} from "@store/common/actionTypes";
import { ShowErrorToaster, ShowSuccessToaster } from "@shared/toaster";
import { AddBugReportDTO } from "@models/request/add-bug-report-dto";
import { FEEDBACK_TYPES } from "@shared/types";
import { NewFeatureRequestDTO } from "@models/request/new-feature-request-dto";
import { CommonToasterConstants } from "@helpers/toaster-constants";

export function ToggleDirectChatDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_DIRECT_CHAT_DRAWER,
		payload: isOpen,
	};
}
export function ToggleMainLoader(isLoading: boolean) {
	return {
		type: TOGGLE_MAIN_SPINNER,
		payload: isLoading,
	};
}

export function ToggleDirectChatLoader(isLoading: boolean) {
	return {
		type: TOGGLE_DIRECT_CHAT_LOADER,
		payload: isLoading,
	};
}

export function ToggleFeedbackDrawer(
	isDrawerOpen: boolean,
	drawerType: FEEDBACK_TYPES,
) {
	return {
		type: TOGGLE_FEEDBACK_DRAWER,
		payload: {
			isDrawerOpen,
			drawerType,
		},
	};
}

export function ToggleFeedbackLoader(isDrawerLoading: boolean) {
	return {
		type: TOGGLE_FEEDBACK_LOADER,
		payload: isDrawerLoading,
	};
}

export function GetAllConfigurations() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetConfigurationsDataApiAsync();
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_ALL_CONFIGURATIONS,
					payload: response.responseData,
				});

				return response.responseData as {};
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		}
	};
}

export function GetConfigurationByKeyName(keyName: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetConfigurationByKeyNameApiAsync(keyName);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_CONFIGURATION_BY_KEY_NAME,
					payload: response.responseData,
				});

				return response.responseData as {};
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		}
	};
}

export function AddBugReportDataAsync(bugReportData: AddBugReportDTO) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleFeedbackLoader(true));
			const response = await AddBugReportDataApiAsync(bugReportData);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: ADD_NEW_BUG_REPORT,
					payload: response.responseData,
				});
				ShowSuccessToaster(CommonToasterConstants.BUG_REPORT_ADDED);
				dispatch(ToggleFeedbackDrawer(false, FEEDBACK_TYPES.BUGREPORT));
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleFeedbackLoader(false));
		}
	};
}

export function SubmitFeatureRequestDataAsync(
	newFeatureRequest: NewFeatureRequestDTO,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleFeedbackLoader(true));
			const response =
				await SubmitFeatureRequestDataApiAsync(newFeatureRequest);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: ADD_NEW_FEATURE_REQUEST,
					payload: response.responseData,
				});
				ShowSuccessToaster(
					CommonToasterConstants.FEATURE_REQUEST_ADDED,
				);
				dispatch(
					ToggleFeedbackDrawer(false, FEEDBACK_TYPES.NEWFEATURE),
				);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleFeedbackLoader(false));
		}
	};
}

export function GetTopActiveAgentsDataAsync() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleMainLoader(true));
			const response = await GetTopActiveAgentsDataApiAsync();
			if (response?.isSuccess && response?.responseData)
				dispatch({
					type: GET_TOP_ACTIVE_AGENTS,
					payload: response.responseData,
				});
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleMainLoader(false));
		}
	};
}
