import { Action, Dispatch } from "redux";

import {
	GET_ALL_BUG_REPORTS,
	GET_ALL_FEATURE_REQUESTS,
	IS_ADMIN_ACCESS_ENABLED_FOR_USER,
	IS_ADMIN_PAGE_SPINNER_LOADING,
	IS_BUG_REPORT_SPINNER_LOADING,
	IS_FEATURE_REQUEST_SPINNER_LOADING,
} from "@store/app-admin/actionTypes";
import { ShowErrorToaster } from "@shared/toaster";
import {
	GetAllBugReportsDataApiAsync,
	GetAllSubmittedFeatureRequestsApiAsync,
	IsAdminAccessEnabledApiAsync,
} from "@shared/api-service";

export function ToggleBugReportSpinner(isLoading: boolean) {
	return {
		type: IS_BUG_REPORT_SPINNER_LOADING,
		payload: isLoading,
	};
}

export function ToggleFeatureRequestSpinner(isLoading: boolean) {
	return {
		type: IS_FEATURE_REQUEST_SPINNER_LOADING,
		payload: isLoading,
	};
}

export function ToggleAdminPageSpinner(isLoading: boolean) {
	return {
		type: IS_ADMIN_PAGE_SPINNER_LOADING,
		payload: isLoading,
	};
}

export function IsAdminAccessEnabledAsync() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleAdminPageSpinner(true));
			const response = await IsAdminAccessEnabledApiAsync();
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: IS_ADMIN_ACCESS_ENABLED_FOR_USER,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleAdminPageSpinner(false));
		}
	};
}

export function GetAllSubmittedFeatureRequestsAsync() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleFeatureRequestSpinner(true));
			const response = await GetAllSubmittedFeatureRequestsApiAsync();
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_ALL_FEATURE_REQUESTS,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleFeatureRequestSpinner(false));
		}
	};
}

export function GetAllBugReportsDataAsync() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleBugReportSpinner(true));
			const response = await GetAllBugReportsDataApiAsync();
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_ALL_BUG_REPORTS,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleBugReportSpinner(false));
		}
	};
}
