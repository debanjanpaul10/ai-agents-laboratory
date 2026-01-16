import { useEffect, useState } from "react";
import { Activity, Server, Cpu, Bot } from "lucide-react";

import { useAppSelector } from "@store/index";
import { TopActiveAgentsDTO } from "@models/response/top-active-agents-dto";

export default function SystemHealthTileComponent() {
	const [activeAgentsCount, setActiveAgentsCount] = useState<number>(0);

	const TopActiveAgentsStoreData = useAppSelector<TopActiveAgentsDTO>(
		(state) => state.CommonReducer.topActiveAgents
	);
	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations
	);

	useEffect(() => {
		if (
			TopActiveAgentsStoreData !== null &&
			TopActiveAgentsStoreData.topActiveAgents?.length > 0
		)
			setActiveAgentsCount(TopActiveAgentsStoreData.activeAgentsCount);
	}, [TopActiveAgentsStoreData]);

	const renderHealthItem = (
		icon: React.ReactNode,
		label: string,
		value: string | number,
		statusColor: string = "bg-green-400"
	) => (
		<div className="bg-gray-700/50 backdrop-blur-sm rounded-lg p-3 border border-gray-600/30 hover:border-blue-500/30 transition-all duration-300 flex items-center justify-between group">
			<div className="flex items-center space-x-3">
				<div className="p-2 bg-white/5 rounded-lg group-hover:bg-white/10 transition-colors">
					{icon}
				</div>
				<div>
					<p className="text-gray-400 text-xs">{label}</p>
					<p className="text-white font-semibold text-sm">{value}</p>
				</div>
			</div>
			<div
				className={`w-2 h-2 ${statusColor} rounded-full animate-pulse shadow-[0_0_8px_rgba(74,222,128,0.5)]`}
			></div>
		</div>
	);

	return (
		<div className="relative group/card h-full">
			<div className="absolute inset-0 bg-gradient-to-r from-emerald-500/20 via-green-600/20 to-teal-500/20 rounded-2xl blur-sm opacity-50 group-hover/card:opacity-75 transition duration-500 -z-10"></div>
			<div className="relative bg-gradient-to-br from-gray-800/90 via-gray-900/90 to-black/90 backdrop-blur-xl text-white p-6 rounded-2xl border border-emerald-500/20 hover:border-emerald-400/40 transition-all duration-500 hover:scale-[1.02] h-full flex flex-col shadow-2xl">
				{/* Header */}
				<div className="flex items-center justify-between mb-6">
					<div className="flex items-center space-x-3">
						<div className="bg-emerald-500/20 backdrop-blur-sm p-3 rounded-xl border border-emerald-500/30">
							<Activity className="w-5 h-5 text-emerald-400" />
						</div>
						<h3 className="text-xl font-bold text-white">
							System Health
						</h3>
					</div>
					<div className="w-2 h-2 bg-green-400 rounded-full animate-pulse"></div>
				</div>

				<div className="space-y-3 flex-1">
					{renderHealthItem(
						<Server className="w-4 h-4 text-emerald-400" />,
						"System Status",
						"All Systems Operational",
						"bg-emerald-400"
					)}
					{renderHealthItem(
						<Cpu className="w-4 h-4 text-blue-400" />,
						"Active Agents",
						`${activeAgentsCount} Running`,
						"bg-blue-400"
					)}
					{renderHealthItem(
						<Bot className="w-4 h-4 text-purple-400" />,
						"AI Provider",
						ConfigurationsStoreData?.CurrentAiServiceProvider ||
							"Not Configured",
						ConfigurationsStoreData?.CurrentAiServiceProvider
							? "bg-purple-400"
							: "bg-yellow-400"
					)}
				</div>

				<div className="mt-6 pt-4 border-t border-white/5">
					<div className="flex justify-between items-center text-xs text-gray-400">
						<span>Last check: Just now</span>
						<span className="text-emerald-400">100% Uptime</span>
					</div>
				</div>
			</div>
		</div>
	);
}
