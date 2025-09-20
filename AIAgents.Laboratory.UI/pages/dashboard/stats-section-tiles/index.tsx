import { useAppSelector } from "@/store";
import { useEffect, useState } from "react";

export default function StatsSectionTilesComponent() {
	const [agentsCount, setAgentsCount] = useState(0);

	const AgentsListStoreData = useAppSelector(
		(state) => state.AgentsReducer.agentsListData
	);

	useEffect(() => {
		setAgentsCount(AgentsListStoreData.length);
	}, [AgentsListStoreData]);

	return (
		<div className="mt-8 grid grid-cols-1 md:grid-cols-3 gap-4">
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
			<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
				<div className="flex items-center space-x-3">
					<div className="w-3 h-3 bg-purple-400 rounded-full animate-pulse"></div>
					<div>
						<p className="text-white/60 text-xs">API Calls Today</p>
						<p className="text-white font-semibold">1,247</p>
					</div>
				</div>
			</div>
		</div>
	);
}
