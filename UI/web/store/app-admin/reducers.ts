import {
	GET_ALL_BUG_REPORTS,
	GET_ALL_FEATURE_REQUESTS,
	IS_ADMIN_ACCESS_ENABLED_FOR_USER,
	IS_ADMIN_PAGE_SPINNER_LOADING,
	IS_BUG_REPORT_SPINNER_LOADING,
	IS_FEATURE_REQUEST_SPINNER_LOADING,
} from "@store/app-admin/actionTypes";

const initialState: any = {
	bugReportList: [],
	featureRequestList: [],
	isBugReportLoading: false,
	isFeatureReaquestLoading: false,
	isAdminAccessEnabledForUser: false,
	isAdminPageLoading: false,
};

export function ApplicationAdminReducer(state = initialState, action: any) {
	switch (action.type) {
		case GET_ALL_BUG_REPORTS: {
			return {
				...state,
				bugReportList: action.payload,
			};
		}
		case GET_ALL_FEATURE_REQUESTS: {
			return {
				...state,
				featureRequestList: action.payload,
			};
		}
		case IS_ADMIN_ACCESS_ENABLED_FOR_USER: {
			return {
				...state,
				isAdminAccessEnabledForUser: action.payload,
			};
		}

		case IS_BUG_REPORT_SPINNER_LOADING: {
			return {
				...state,
				isBugReportLoading: action.payload,
			};
		}
		case IS_FEATURE_REQUEST_SPINNER_LOADING: {
			return {
				...state,
				isFeatureReaquestLoading: action.payload,
			};
		}
		case IS_ADMIN_PAGE_SPINNER_LOADING: {
			return {
				...state,
				isAdminPageLoading: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
