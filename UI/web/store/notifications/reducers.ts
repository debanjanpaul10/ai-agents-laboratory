import {
    MARK_NOTIFICATION_AS_READ,
    POLL_NOTIFICATIONS,
    TOGGLE_NOTIFICATIONS_LOADER,
    TOGGLE_NOTIFICATIONS_PANEL,
} from "./actionTypes";

const initialState: any = {
    notifications: [],
    areNotificationsLoading: false,
    notificationsMarkedRead: {},
    isNotificationsPanelOpen: false,
};

export function NotificationsReducer(state = initialState, action: any) {
    switch (action.type) {
        case TOGGLE_NOTIFICATIONS_LOADER: {
            return {
                ...state,
                areNotificationsLoading: action.payload,
            };
        }

        case POLL_NOTIFICATIONS: {
            return {
                ...state,
                notifications: action.payload,
            };
        }
        case MARK_NOTIFICATION_AS_READ: {
            return {
                ...state,
                notificationsMarkedRead: action.payload,
            };
        }
        case TOGGLE_NOTIFICATIONS_PANEL: {
            return {
                ...state,
                isNotificationsPanelOpen: action.payload,
            };
        }

        default: {
            return {
                ...state,
            };
        }
    }
}
