import { useEffect, useState } from "react";
import { Bot, ChevronsRight, Clock, Settings, Sparkles } from "lucide-react";

import { AgentData } from "@shared/types";
import { useAppSelector } from "@store/index";
import { DashboardConstants } from "@helpers/constants";
import { TopActiveAgentsDTO } from "@models/top-active-agents-dto";

export default function ActiveAgentsTileComponent() {
	const [agentsDataList, setAgentsDataList] = useState<AgentData[]>([]);
	const [activeAgentsCount, setActiveAgentsCount] = useState<number>(0);

	const TopActiveAgentsStoreData = useAppSelector<TopActiveAgentsDTO>(
		(state) => state.CommonReducer.topActiveAgents
	);

	useEffect(() => {
		if (TopActiveAgentsStoreData?.topActiveAgents?.length > 0) {
			setAgentsDataList(TopActiveAgentsStoreData.topActiveAgents);
			setActiveAgentsCount(TopActiveAgentsStoreData.activeAgentsCount);
		}
	}, [TopActiveAgentsStoreData]);

	return (
		<div className="relative group/card h-full">
			<div className="absolute inset-0 bg-gradient-to-r from-blue-500/20 via-blue-600/20 to-cyan-500/20 rounded-2xl blur-sm opacity-50 group-hover/card:opacity-75 transition duration-500 -z-10"></div>
			<div className="relative bg-gradient-to-br from-gray-800/90 via-gray-900/90 to-black/90 backdrop-blur-xl text-white p-6 rounded-2xl border border-blue-500/20 hover:border-blue-400/40 transition-all duration-500 hover:scale-[1.02] h-full flex flex-col shadow-2xl">
				<div className="flex items-center justify-between mb-4">
					<div className="flex items-center space-x-3">
						<div className="bg-blue-500/20 backdrop-blur-sm p-3 rounded-xl border border-blue-500/30">
							<Bot />
						</div>
						<h3 className="text-xl font-bold text-white">
							Top Active Agents
						</h3>
					</div>
					<div className="w-2 h-2 bg-blue-400 rounded-full animate-pulse"></div>
				</div>
				<p className="text-gray-300 text-sm leading-relaxed mb-3">
					{DashboardConstants.ActiveAgentsTile.SubText}
				</p>
				{/* Agents List */}
				<div className="space-y-2 flex-1">
					{agentsDataList.length > 0 ? (
						agentsDataList.map((agent: any, index: number) => (
							<div
								key={agent.agentId || index}
								className="bg-gray-700/50 backdrop-blur-sm rounded-lg p-2 border border-gray-600/30 hover:border-blue-500/30 transition-all duration-300"
							>
								<div className="flex items-center justify-between">
									<div className="flex-1">
										<p className="text-white font-medium text-xs truncate flex items-center gap-2 py-1">
											<Sparkles className="w-4 h-4 flex-shrink-0 animate-pulse" />
											<span className="truncate">
												{agent.agentName ||
													`Agent ${index + 1}`}
											</span>
										</p>
										<div className="flex items-center justify-between gap-4 py-1 text-gray-400 text-xs">
											<span className="flex items-center gap-2 truncate">
												<Settings className="w-4 h-4 flex-shrink-0" />
												<span className="truncate">
													{agent.applicationName}
												</span>
											</span>
											<span className="flex items-center gap-2 whitespace-nowrap">
												<Clock className="w-4 h-4 flex-shrink-0" />
												<span>
													{new Date(
														agent.dateModified
													).toDateString()}
												</span>
											</span>
										</div>
									</div>
								</div>
							</div>
						))
					) : (
						<div className="text-center py-4">
							<div className="w-8 h-8 bg-gray-700/50 rounded-full flex items-center justify-center mx-auto mb-2 border border-gray-600/30">
								<svg
									className="w-4 h-4 text-gray-400"
									fill="none"
									stroke="currentColor"
									viewBox="0 0 24 24"
								>
									<path
										strokeLinecap="round"
										strokeLinejoin="round"
										strokeWidth={2}
										d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"
									/>
								</svg>
							</div>
							<p className="text-gray-400 text-xs">
								{
									DashboardConstants.ActiveAgentsTile
										.NoAgentsText
								}
							</p>

							<div className="space-y-2 mt-3">
								{DashboardConstants.ActiveAgentsTile.Steps.map(
									(item: string, index: number) => {
										return (
											<div
												className="flex items-center text-gray-400 text-xs"
												key={index}
											>
												<ChevronsRight className="text-blue-400" />
												&nbsp;
												{item}
											</div>
										);
									}
								)}
							</div>
						</div>
					)}
				</div>

				<div className="mt-4 flex items-center text-gray-400 text-xs">
					<span className="w-2 h-2 bg-blue-400 rounded-full mr-2 animate-pulse"></span>
					{activeAgentsCount > 0
						? `${activeAgentsCount} agents running`
						: DashboardConstants.ActiveAgentsTile.ReadyToDeployText}
				</div>
			</div>
		</div>
	);
}
