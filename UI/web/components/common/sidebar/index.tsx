import Link from "next/link";
import { useRouter } from "next/router";
import {
	LayoutDashboard,
	Users,
	Store,
	LogOut,
	Info,
	Laptop,
	UserCircle,
	MonitorSmartphone,
	Bell,
} from "lucide-react";
import Image from "next/image";
import { Button } from "@heroui/react";
import { useMsal } from "@azure/msal-react";

import AppLogo from "@public/images/icon.png";
import { useAppDispatch, useAppSelector } from "@store/index";
import { metadata } from "@helpers/constants";
import {
	PollNotificationsAsync,
	ReceiveNotification,
	ToggleNotificationsPanel,
} from "@store/notifications/actions";
import NotificationsDrawerComponent from "@components/common/notifications";
import { startNotificationsStream } from "@shared/notifications-stream";
import { useEffect, useMemo, useRef } from "react";

export default function SidebarComponent() {
	const router = useRouter();
	const { instance } = useMsal();
	const dispatch = useAppDispatch();
	const abortRef = useRef<AbortController | null>(null);

	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations,
	);
	const notifications = useAppSelector(
		(state) => state.NotificationsReducer.notifications,
	);
	const unreadCount = useMemo(
		() => (notifications ?? []).filter((n: any) => n?.isActive).length,
		[notifications],
	);

	useEffect(() => {
		// Initial sync so badge/drawer has data immediately.
		dispatch(PollNotificationsAsync() as any);

		abortRef.current?.abort();
		const controller = new AbortController();
		abortRef.current = controller;

		let isMounted = true;
		let backoffMs = 500;

		const loop = async () => {
			while (isMounted && !controller.signal.aborted) {
				try {
					await startNotificationsStream({
						signal: controller.signal,
						onNotification: (n) =>
							dispatch(ReceiveNotification(n) as any),
					});
					// Stream ended unexpectedly; reconnect.
				} catch {
					// ignore; reconnect with backoff
				}

				await new Promise((r) => setTimeout(r, backoffMs));
				backoffMs = Math.min(backoffMs * 2, 10_000);
			}
		};

		const timeoutId = setTimeout(() => {
			loop();
		}, 100);

		return () => {
			clearTimeout(timeoutId);
			isMounted = false;
			controller.abort();
		};
	}, [dispatch]);

	const isActive = (path: string) => router.pathname === path;

	const handleLogout = () => {
		instance.logoutRedirect().catch((e) => {
			console.error(e);
		});
		localStorage.clear();
	};

	const handleHowTo = () => {
		if (ConfigurationsStoreData?.HowToFileLink) {
			window.open(ConfigurationsStoreData.HowToFileLink, "_blank");
		}
	};

	const handleAdminPage = () => {
		router.push("/admin");
	};

	const handleNotifications = () => {
		dispatch(PollNotificationsAsync());
		dispatch(ToggleNotificationsPanel(true));
	};

	const navItems = [
		{ name: "Dashboard", href: "/dashboard", icon: LayoutDashboard },
		{ name: "Agents Management", href: "/manage-agents", icon: Users },
		{ name: "Skills Marketplace", href: "/marketplace", icon: Store },
		{ name: "Agents Workspaces", href: "/workspaces", icon: Laptop },
		{
			name: "Register Applications",
			href: "/register-applications",
			icon: MonitorSmartphone,
		},
	];

	return (
		<>
			<aside className="fixed left-0 top-0 z-40 h-screen w-90 border-r border-white/10 bg-black/60 backdrop-blur-xl">
				<div className="flex h-full flex-col px-3 py-4">
					<div className="mb-8 flex items-center px-2">
						<div className="bg-gradient-to-r from-blue-500 to-purple-600 p-2 rounded-xl mr-3">
							<Image
								src={AppLogo}
								alt="App Icon"
								height={30}
								width={30}
							/>
						</div>
						<span className="self-center whitespace-nowrap text-xl font-bold bg-gradient-to-r from-white via-blue-100 to-cyan-100 bg-clip-text text-transparent">
							{metadata.title}
						</span>
					</div>

					<ul className="space-y-2 font-medium flex-1">
						{navItems.map((item) => (
							<li key={item.name}>
								<Link
									href={item.href}
									className={`group flex items-center rounded-lg p-2 transition-all hover:bg-white/10 ${
										isActive(item.href)
											? "bg-white/10 text-blue-400"
											: "text-gray-400"
									}`}
								>
									<item.icon
										className={`h-5 w-5 transition duration-75 ${
											isActive(item.href)
												? "text-blue-400"
												: "text-gray-400 group-hover:text-white"
										}`}
									/>
									<span className="ml-3">{item.name}</span>
								</Link>
							</li>
						))}
					</ul>

					<div className="border-t border-white/10 pt-4 space-y-2">
						{/* Notifications */}
						<Button
							onPress={handleNotifications}
							className="w-full justify-start bg-transparent hover:bg-purple-500/10 text-gray-400 hover:text-purple-400 relative"
						>
							<Bell className="h-5 w-5 mr-3" />
							Notifications
							{unreadCount > 0 && (
								<span className="absolute right-3 top-1/2 -translate-y-1/2 min-w-5 h-5 px-1.5 rounded-full bg-pink-500/90 text-white text-xs leading-5 text-center">
									{unreadCount}
								</span>
							)}
						</Button>

						<Button
							onPress={handleHowTo}
							className="w-full justify-start bg-transparent hover:bg-white/5 text-gray-400 hover:text-white"
						>
							<Info className="h-5 w-5 mr-3" />
							How To
						</Button>

						<Button
							onPress={handleAdminPage}
							className="w-full justify-start bg-transparent hover:bg-blue-500/10 text-gray-400 hover:text-blue-400"
						>
							<UserCircle className="h-5 w-5 mr-3" />
							Admin
						</Button>

						<Button
							onPress={handleLogout}
							className="w-full justify-start bg-transparent hover:bg-red-500/10 text-gray-400 hover:text-red-400"
						>
							<LogOut className="h-5 w-5 mr-3" />
							Logout
						</Button>
					</div>
				</div>
			</aside>

			<NotificationsDrawerComponent />
		</>
	);
}
