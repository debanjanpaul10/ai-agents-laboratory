import {
    MARK_NOTIFICATION_AS_READ,
    POLL_NOTIFICATIONS,
    RECEIVE_NOTIFICATION,
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

        case RECEIVE_NOTIFICATION: {
            const incoming = action.payload;
            const existing = state.notifications ?? [];
            const index = existing.findIndex((n: any) => n?.id === incoming?.id);
            const next =
                index >= 0
                    ? [
                            incoming,
                            ...existing.slice(0, index),
                            ...existing.slice(index + 1),
                        ]
                    : [incoming, ...existing];

            return {
                ...state,
                notifications: next,
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
