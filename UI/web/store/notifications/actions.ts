import { Action, Dispatch } from "redux";

import {
	CLEAR_ALL_NOTIFICATIONS,
	MARK_NOTIFICATION_AS_READ,
	POLL_NOTIFICATIONS,
	RECEIVE_NOTIFICATION,
	TOGGLE_NOTIFICATIONS_LOADER,
	TOGGLE_NOTIFICATIONS_PANEL,
} from "@store/notifications/actionTypes";
import { ShowErrorToaster } from "@shared/toaster";
import {
	DeleteAllNotificationsForUserApiAsync,
	MarkNotificationAsReadApiAsync,
	PollNotificationsApiAsync,
} from "@shared/api-service";
import { NotificationsResponseDTO } from "@models/response/notifications-response-dto.model";

export function ToggleNotificationsLoader(isLoading: boolean) {
	return {
		type: TOGGLE_NOTIFICATIONS_LOADER,
		payload: isLoading,
	};
}

export function ToggleNotificationsPanel(isOpen: boolean) {
	return {
		type: TOGGLE_NOTIFICATIONS_PANEL,
		payload: isOpen,
	};
}

export function PollNotificationsAsync() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleNotificationsLoader(true));
			const response = await PollNotificationsApiAsync();
			if (response?.isSuccess && response?.responseData)
				dispatch({
					type: POLL_NOTIFICATIONS,
					payload: response.responseData as [
						NotificationsResponseDTO,
					],
				});
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleNotificationsLoader(false));
		}
	};
}

export function ReceiveNotification(notification: NotificationsResponseDTO) {
	return {
		type: RECEIVE_NOTIFICATION,
		payload: notification,
	};
}

export function MarkNotificationAsReadAsync(notificationId: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleNotificationsLoader(true));
			const response =
				await MarkNotificationAsReadApiAsync(notificationId);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: MARK_NOTIFICATION_AS_READ,
					payload: response.responseData as boolean,
				});
				dispatch(PollNotificationsAsync() as any);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleNotificationsLoader(false));
		}
	};
}

export function DeleteAllNotificationsForUserAsync() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleNotificationsLoader(true));
			const response = await DeleteAllNotificationsForUserApiAsync();
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: CLEAR_ALL_NOTIFICATIONS,
					payload: response.responseData,
				});
				dispatch(PollNotificationsAsync() as any);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleNotificationsLoader(false));
		}
	};
}
