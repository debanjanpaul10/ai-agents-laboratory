"use client";

import { useEffect, useState } from "react";
import { BrainCircuit } from "lucide-react";

import { AgentData } from "@lib/types";
import { useAppSelector } from "@/store";
import { DashboardConstants } from "@/helpers/constants";

export default function ActiveAgentsTileComponent() {
	const [agentsDataList, setAgentsDataList] = useState<AgentData[]>([]);

	const AgentsStoreData = useAppSelector(
		(state) => state.AgentsReducer.agentsListData
	);

	useEffect(() => {
		if (AgentsStoreData !== null && AgentsStoreData.length > 0) {
			setAgentsDataList(AgentsStoreData);
		}
	}, [AgentsStoreData]);

	return (
		<div className="relative group/card">
			<div className="absolute inset-0 bg-gradient-to-r from-blue-500/20 via-blue-600/20 to-cyan-500/20 rounded-2xl blur-sm opacity-50 group-hover/card:opacity-75 transition duration-500 -z-10"></div>
			<div className="relative bg-gradient-to-br from-gray-800/90 via-gray-900/90 to-black/90 backdrop-blur-xl text-white p-6 rounded-2xl border border-blue-500/20 hover:border-blue-400/40 transition-all duration-500 hover:scale-105 min-h-[280px] flex flex-col shadow-2xl">
				<div className="flex items-center justify-between mb-4">
					<div className="flex items-center space-x-3">
						<div className="bg-blue-500/20 backdrop-blur-sm p-3 rounded-xl border border-blue-500/30">
							<BrainCircuit />
						</div>
						<h3 className="text-xl font-bold text-white">
							Top Active Agents
						</h3>
					</div>
					<div className="w-2 h-2 bg-blue-400 rounded-full animate-pulse"></div>
				</div>
				<p className="text-gray-300 text-sm leading-relaxed mb-3 flex-1">
					{DashboardConstants.ActiveAgentsTile.SubText}
				</p>
				{/* Agents List */}
				<div className="space-y-2">
					{agentsDataList.length > 0 ? (
						agentsDataList
							.slice(0, 3)
							.map((agent: AgentData, index: number) => (
								<div
									key={agent.agentId || index}
									className="bg-gray-700/50 backdrop-blur-sm rounded-lg p-2 border border-gray-600/30 hover:border-blue-500/30 transition-all duration-300"
								>
									<div className="flex items-center justify-between">
										<div className="flex-1">
											<p className="text-white font-medium text-xs truncate">
												{agent.agentName ||
													`Agent ${index + 1}`}
											</p>
											<p className="text-gray-400 text-xs truncate">
												{agent.applicationName ||
													"Unknown App"}
											</p>
										</div>
										<div className="w-2 h-2 bg-green-400 rounded-full animate-pulse ml-2"></div>
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
						</div>
					)}
				</div>

				<div className="mt-4 flex items-center text-gray-400 text-xs">
					<span className="w-2 h-2 bg-blue-400 rounded-full mr-2 animate-pulse"></span>
					{agentsDataList.length > 0
						? `${agentsDataList.length} agents running`
						: DashboardConstants.ActiveAgentsTile.ReadyToDeployText}
				</div>
			</div>
		</div>
	);
}
