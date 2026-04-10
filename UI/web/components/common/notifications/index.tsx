import { useEffect } from "react";
import { Bell, Check, Loader2, X } from "lucide-react";
import { Tooltip } from "@heroui/react";

import { useAppDispatch, useAppSelector } from "@store/index";
import {
	MarkNotificationAsReadAsync,
	ToggleNotificationsPanel,
} from "@store/notifications/actions";
import { NotificationsResponseDTO } from "@models/response/notifications-response-dto.model";

export default function NotificationsDrawerComponent() {
	const dispatch = useAppDispatch();

	const isOpen = useAppSelector(
		(state) => state.NotificationsReducer.isNotificationsPanelOpen,
	);
	const notifications: NotificationsResponseDTO[] = useAppSelector(
		(state) => state.NotificationsReducer.notifications,
	);
	const isLoading = useAppSelector(
		(state) => state.NotificationsReducer.areNotificationsLoading,
	);

	useEffect(() => {
		document.body.style.overflow = isOpen ? "hidden" : "unset";
		return () => {
			document.body.style.overflow = "unset";
		};
	}, [isOpen]);

	const handleClose = () => dispatch(ToggleNotificationsPanel(false));

	const handleMarkAsRead = (id: string) =>
		dispatch(MarkNotificationAsReadAsync(id));

	const getNotificationTypeColor = (type: string) => {
		switch (type?.toLowerCase()) {
			case "error":
				return "from-red-500/20 to-pink-500/20 border-red-500/20";
			case "warning":
				return "from-yellow-500/20 to-orange-500/20 border-yellow-500/20";
			case "success":
				return "from-green-500/20 to-emerald-500/20 border-green-500/20";
			default:
				return "from-blue-500/20 to-indigo-500/20 border-blue-500/20";
		}
	};

	const getNotificationTypeDot = (type: string) => {
		switch (type?.toLowerCase()) {
			case "error":
				return "bg-red-400";
			case "warning":
				return "bg-yellow-400";
			case "success":
				return "bg-green-400";
			default:
				return "bg-blue-400";
		}
	};

	const getContentElement = () => {
		if (isLoading) {
			return (
				<div className="flex flex-col items-center justify-center h-full space-y-3 text-white/40">
					<Loader2 className="w-8 h-8 animate-spin text-purple-400" />
					<span className="text-sm">Loading notifications...</span>
				</div>
			);
		}

		if (notifications.length === 0) {
			return (
				<div className="flex flex-col items-center justify-center h-full space-y-3 text-white/30">
					<Bell className="w-12 h-12 opacity-20" />
					<p className="text-sm">No notifications</p>
				</div>
			);
		}

		return notifications.map((notification) => {
			const isUnread = notification.isActive;

			return (
				<div
					key={notification.id}
					className={`relative bg-gradient-to-br ${getNotificationTypeColor(notification.notificationType)} border rounded-xl p-4 transition-all duration-200 hover:bg-white/[0.07] ${isUnread ? "" : "opacity-75 grayscale-[30%]"}`}
				>
					{isUnread ? (
						<Tooltip content="Mark as read" placement="top">
							<button
								onClick={() =>
									handleMarkAsRead(notification.id)
								}
								aria-label="Mark as read"
								className="absolute top-3 right-3 p-1.5 rounded-lg bg-white/5 hover:bg-green-500/20 border border-white/10 hover:border-green-500/30 transition-all duration-200 text-white/30 hover:text-green-400 group"
							>
								<Check className="w-3.5 h-3.5 group-hover:scale-110 transition-transform" />
							</button>
						</Tooltip>
					) : (
						<span className="absolute top-3 right-3 text-white/50 text-[10px] uppercase tracking-wider">
							Read
						</span>
					)}

					<div className="pr-12 space-y-1.5">
						<div className="flex items-center space-x-2">
							{isUnread && (
								<span
									className={`w-2 h-2 rounded-full flex-shrink-0 ${getNotificationTypeDot(notification.notificationType)}`}
								/>
							)}
							<span
								className={`font-semibold text-sm truncate ${isUnread ? "text-white" : "text-white/80"}`}
							>
								{notification.title}
							</span>
						</div>
						<p
							className={`text-xs leading-relaxed ${isUnread ? "text-white/60" : "text-white/70"}`}
						>
							{notification.message}
						</p>
						<div className="flex items-center justify-between pt-1">
							<span
								className={`text-[10px] uppercase tracking-wider ${isUnread ? "text-white/30" : "text-white/50"}`}
							>
								{notification.notificationType || "Info"}
							</span>
							{notification.createdBy && (
								<span
									className={`text-[10px] ${isUnread ? "text-white/25" : "text-white/45"}`}
								>
									{notification.createdBy}
								</span>
							)}
						</div>
					</div>
				</div>
			);
		});
	};

	if (!isOpen) return null;

	return (
		<>
			{/* Backdrop */}
			<button
				onClick={handleClose}
				aria-label="Close notifications panel"
				className="fixed inset-0 bg-black/60 backdrop-blur-md z-[90] transition-opacity duration-300 cursor-default"
			/>

			{/* Drawer */}
			<div className="fixed top-0 right-0 h-screen md:w-[420px] w-full z-[100] transition-all duration-500 ease-in-out">
				<div className="absolute inset-0 bg-gradient-to-l from-indigo-600/20 via-purple-600/20 to-pink-600/20 blur-sm opacity-50 -z-10" />
				<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
					{/* Header */}
					<div className="flex items-center justify-between p-6 border-b border-white/5 flex-shrink-0 bg-white/[0.02]">
						<div className="flex items-center space-x-3">
							<div className="bg-gradient-to-br from-indigo-500 via-purple-500 to-pink-500 p-2.5 rounded-xl shadow-lg shadow-purple-500/20 ring-1 ring-white/20">
								<Bell className="w-5 h-5 text-white" />
							</div>
							<div>
								<h2 className="text-xl font-bold bg-gradient-to-r from-white via-indigo-100 to-purple-100 bg-clip-text text-transparent">
									Notifications
								</h2>
								<p className="text-white/40 text-xs">
									{
										notifications.filter((n) => n.isActive)
											.length
									}{" "}
									unread
								</p>
							</div>
						</div>
						<button
							onClick={handleClose}
							aria-label="Close notifications"
							className="p-2.5 rounded-xl bg-white/5 hover:bg-red-500/10 border border-white/10 hover:border-red-500/20 transition-all duration-300 text-white/50 hover:text-red-400 group"
						>
							<X className="w-5 h-5 group-hover:rotate-90 transition-transform duration-300" />
						</button>
					</div>

					{/* Content */}
					<div className="flex-1 overflow-y-auto p-4 space-y-3 min-h-0">
						{getContentElement()}
					</div>
				</div>
			</div>
		</>
	);
}
