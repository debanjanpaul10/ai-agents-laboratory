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
	drawerType: FEEDBACK_TYPES
) {
	return {
		type: TOGGLE_FEEDBACK_DRAWER,
		payload: {
			isDrawerOpen,
			drawerType,
		},
	};
}

export function ToggleFeedbackLoader(
	isDrawerLoading: boolean,
	drawerType: FEEDBACK_TYPES
) {
	return {
		type: TOGGLE_FEEDBACK_LOADER,
		payload: {
			isDrawerLoading,
			drawerType,
		},
	};
}

export function GetAllConfigurations(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetConfigurationsDataApiAsync(accessToken);
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

export function GetConfigurationByKeyName(
	keyName: string,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetConfigurationByKeyNameApiAsync(
				keyName,
				accessToken
			);
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

export function AddBugReportDataAsync(
	bugReportData: AddBugReportDTO,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleFeedbackLoader(true, FEEDBACK_TYPES.BUGREPORT));
			const response = await AddBugReportDataApiAsync(
				bugReportData,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: ADD_NEW_BUG_REPORT,
					payload: response.responseData,
				});
				ShowSuccessToaster(CommonToasterConstants.BUG_REPORT_ADDED);
				dispatch(ToggleFeedbackDrawer(false, FEEDBACK_TYPES.BUGREPORT));

				return response.responseData as {};
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleFeedbackLoader(false, FEEDBACK_TYPES.BUGREPORT));
		}
	};
}

export function SubmitFeatureRequestDataAsync(
	newFeatureRequest: NewFeatureRequestDTO,
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleFeedbackLoader(true, FEEDBACK_TYPES.NEWFEATURE));
			const response = await SubmitFeatureRequestDataApiAsync(
				newFeatureRequest,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: ADD_NEW_FEATURE_REQUEST,
					payload: response.responseData,
				});
				ShowSuccessToaster(
					CommonToasterConstants.FEATURE_REQUEST_ADDED
				);
				dispatch(
					ToggleFeedbackDrawer(false, FEEDBACK_TYPES.NEWFEATURE)
				);

				return response.responseData as {};
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleFeedbackLoader(false, FEEDBACK_TYPES.NEWFEATURE));
		}
	};
}

export function GetTopActiveAgentsDataAsync(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleMainLoader(true));
			const response = await GetTopActiveAgentsDataApiAsync(accessToken);
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
