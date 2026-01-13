import Link from "next/link";
import { useRouter } from "next/router";
import {
	LayoutDashboard,
	Users,
	Store,
	LogOut,
	Info,
	Laptop,
} from "lucide-react";
import Image from "next/image";
import { Button } from "@heroui/react";
import { useMsal } from "@azure/msal-react";

import AppLogo from "@public/images/icon.png";
import { useAppSelector } from "@store/index";

export default function Sidebar() {
	const router = useRouter();
	const { instance } = useMsal();
	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations
	);

	const isActive = (path: string) => router.pathname === path;

	const handleLogout = () => {
		instance.logoutRedirect().catch((e) => {
			console.error(e);
		});
		localStorage.clear();
	};

	const handleHowTo = () => {
		// Open how-to guide in a new tab
		if (ConfigurationsStoreData?.HowToFileLink) {
			window.open(ConfigurationsStoreData.HowToFileLink, "_blank");
		}
	};

	const navItems = [
		{
			name: "Dashboard",
			href: "/dashboard",
			icon: LayoutDashboard,
		},
		{
			name: "Agents Management",
			href: "/manage-agents",
			icon: Users,
		},
		{
			name: "Skills Marketplace",
			href: "/marketplace",
			icon: Store,
		},
		{
			name: "Agents Workspaces",
			href: "/workspaces",
			icon: Laptop,
		},
	];

	return (
		<aside className="fixed left-0 top-0 z-40 h-screen w-64 border-r border-white/10 bg-black/60 backdrop-blur-xl">
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
						AI Lab
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
					<Button
						onPress={handleHowTo}
						className="w-full justify-start bg-transparent hover:bg-white/5 text-gray-400 hover:text-white"
					>
						<Info className="h-5 w-5 mr-3" />
						How To
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
	);
}
