import { ChevronsRight, FilePlus } from "lucide-react";

import { useAppDispatch } from "@store/index";
import { ToggleNewAgentDrawer } from "@store/common/actions";
import { DashboardConstants } from "@helpers/constants";

export default function CreateAgentsTileComponent() {
	const dispatch = useAppDispatch();

	const handleCreateAgent = () => {
		dispatch(ToggleNewAgentDrawer(true));
	};

	return (
		<div
			className="relative group/card cursor-pointer"
			onClick={handleCreateAgent}
		>
			<div className="absolute inset-0 bg-gradient-to-r from-purple-500/20 via-violet-600/20 to-indigo-500/20 rounded-2xl blur-sm opacity-50 group-hover/card:opacity-75 transition duration-500 -z-10"></div>
			<div className="relative bg-gradient-to-br from-gray-800/90 via-gray-900/90 to-black/90 backdrop-blur-xl text-white p-6 rounded-2xl border border-purple-500/20 hover:border-purple-400/40 transition-all duration-500 hover:scale-105 min-h-[280px] flex flex-col shadow-2xl">
				<div className="flex items-center justify-between mb-4">
					<div className="flex items-center space-x-3">
						<div className="bg-purple-500/20 backdrop-blur-sm p-3 rounded-xl border border-purple-500/30">
							<FilePlus />
						</div>
						<h3 className="text-xl font-bold text-white">
							Create New Agent
						</h3>
					</div>
					<div className="w-2 h-2 bg-purple-400 rounded-full animate-pulse delay-1000"></div>
				</div>
				<p className="text-gray-300 text-sm leading-relaxed mb-6 flex-1">
					{DashboardConstants.CreateAgentTile.SubText}
				</p>
				<div className="space-y-2">
					{DashboardConstants.CreateAgentTile.Steps.map(
						(item: string, index: number) => {
							return (
								<div
									className="flex items-center text-gray-400 text-xs"
									key={index}
								>
									<ChevronsRight className="text-purple-400" />
									&nbsp;
									{item}
								</div>
							);
						}
					)}
				</div>

				<div className="mt-4 flex items-center text-gray-400 text-xs">
					<span className="w-2 h-2 bg-purple-400 rounded-full mr-2 animate-pulse"></span>
					{DashboardConstants.CreateAgentTile.ActionText}
				</div>
			</div>
		</div>
	);
}
