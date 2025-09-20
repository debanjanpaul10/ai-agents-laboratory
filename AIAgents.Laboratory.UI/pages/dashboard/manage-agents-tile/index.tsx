import { Bolt, ChevronsRight } from "lucide-react";

import { useAppDispatch } from "@/store";
import { ToggleAgentsListDrawer } from "@/store/common/actions";
import { DashboardConstants } from "@/helpers/constants";

export default function ManageAgentsTileComponent() {
	const dispatch = useAppDispatch();

	const handleManageAgents = () => {
		dispatch(ToggleAgentsListDrawer(true));
	};

	return (
		<div
			className="relative group/card cursor-pointer"
			onClick={handleManageAgents}
		>
			<div className="absolute inset-0 bg-gradient-to-r from-green-500/20 via-emerald-600/20 to-teal-500/20 rounded-2xl blur-sm opacity-50 group-hover/card:opacity-75 transition duration-500 -z-10"></div>
			<div className="relative bg-gradient-to-br from-gray-800/90 via-gray-900/90 to-black/90 backdrop-blur-xl text-white p-6 rounded-2xl border border-green-500/20 hover:border-green-400/40 transition-all duration-500 hover:scale-105 min-h-[280px] flex flex-col shadow-2xl">
				<div className="flex items-center justify-between mb-4">
					<div className="flex items-center space-x-3">
						<div className="bg-green-500/20 backdrop-blur-sm p-3 rounded-xl border border-green-500/30">
							<Bolt />
						</div>
						<h3 className="text-xl font-bold text-white">
							Manage Agents
						</h3>
					</div>
					<div className="w-2 h-2 bg-green-400 rounded-full animate-pulse delay-500"></div>
				</div>
				<p className="text-gray-300 text-sm leading-relaxed mb-6 flex-1">
					{DashboardConstants.ManageAgentsTile.SubText}
				</p>

				{/* Action indicators */}
				<div className="space-y-2">
					{DashboardConstants.ManageAgentsTile.Steps.map(
						(item: string) => {
							return (
								<div className="flex items-center text-gray-400 text-xs">
									<ChevronsRight className="text-green-400" />
									&nbsp;
									{item}
								</div>
							);
						}
					)}
				</div>

				<div className="mt-4 flex items-center text-gray-400 text-xs">
					<span className="w-2 h-2 bg-green-400 rounded-full mr-2 animate-pulse"></span>
					{DashboardConstants.ManageAgentsTile.ActionText}
				</div>
			</div>
		</div>
	);
}
