import { useEffect, useState } from "react";
import { useAppSelector } from "@store/index";

export default function StatsSectionTilesComponent() {
	const [agentsCount, setAgentsCount] = useState<number>(0);

	const AgentsListStoreData = useAppSelector(
		(state) => state.AgentsReducer.agentsListData
	);
	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations
	);

	useEffect(() => {
		setAgentsCount(AgentsListStoreData.length);
	}, [AgentsListStoreData]);

	return (
		<div className="mt-8 grid grid-cols-1 md:grid-cols-3 gap-4">
			{/* System Status */}
			<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
				<div className="flex items-center space-x-3">
					<div className="w-3 h-3 bg-green-400 rounded-full animate-pulse"></div>
					<div>
						<p className="text-white/60 text-xs">System Status</p>
						<p className="text-white font-semibold">
							All Systems Operational
						</p>
					</div>
				</div>
			</div>

			{/* Active Agents Count */}
			<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
				<div className="flex items-center space-x-3">
					<div className="w-3 h-3 bg-blue-400 rounded-full animate-pulse"></div>
					<div>
						<p className="text-white/60 text-xs">Active Agents</p>
						<p className="text-white font-semibold">
							{agentsCount}
						</p>
					</div>
				</div>
			</div>

			{/* AI Service Provider */}
			<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
				<div className="flex items-center space-x-3">
					<div className="w-3 h-3 bg-purple-400 rounded-full animate-pulse"></div>
					<div>
						<p className="text-white/60 text-xs">
							Current AI Service provider
						</p>
						<p className="text-white font-semibold">
							{ConfigurationsStoreData.CurrentAiServiceProvider}
						</p>
					</div>
				</div>
			</div>
		</div>
	);
}
