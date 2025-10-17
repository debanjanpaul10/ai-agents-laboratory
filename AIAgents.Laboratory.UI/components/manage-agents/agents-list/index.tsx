import React, { useEffect, useState } from "react";
import {
	Bot,
	X,
	Settings,
	User,
	Calendar,
	BotMessageSquare,
} from "lucide-react";

import { AgentDataDTO } from "@models/agent-data-dto";
import { ManageAgentConstants } from "@helpers/constants";
import { AgentsListComponentProps } from "@shared/types";

export default function AgentsListComponent({
	agentsDataList,
	handleAgentClick,
	onClose,
	isDisabled,
}: AgentsListComponentProps) {
	const [currentAgentsData, setCurrentAgentsData] = useState<AgentDataDTO[]>(
		[]
	);

	useEffect(() => {
		if (
			agentsDataList !== null &&
			agentsDataList.length > 0 &&
			agentsDataList !== currentAgentsData
		) {
			setCurrentAgentsData(agentsDataList);
		}
	}, [agentsDataList]);

	return (
		<div className="fixed inset-0 w-full h-full max-w-full max-h-full z-[100] bg-gray-900 flex items-center justify-center">
			<div className="w-full h-full md:w-auto md:h-auto bg-gradient-to-b from-gray-900 via-slate-900 to-black rounded-none md:rounded-2xl shadow-xl overflow-y-auto">
				{/* Header */}
				<div className="flex items-center justify-between md:p-6 p-4 border-b border-white/10 flex-shrink-0">
					<div className="flex items-center space-x-3">
						<div className="bg-gradient-to-r from-purple-500 to-blue-600 p-2 rounded-xl">
							<Bot className="w-5 h-5 text-white" />
						</div>
						<div>
							<h2 className="text-xl font-bold bg-gradient-to-r from-white via-blue-100 to-purple-100 bg-clip-text text-transparent">
								{ManageAgentConstants.Headers.SubText}
							</h2>
							<p className="text-white/60 text-sm">
								Select an agent to modify
							</p>
						</div>
					</div>
					<div className="flex items-center space-x-2">
						<button
							onClick={onClose}
							disabled={isDisabled}
							className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400 disabled:opacity-50 disabled:cursor-not-allowed"
						>
							<X className="w-4 h-4" />
						</button>
					</div>
				</div>

				{/* Agents List Content */}
				<div className="flex-1 overflow-y-auto p-6 space-y-4 min-h-0">
					{agentsDataList.length > 0 ? (
						agentsDataList.map(
							(agent: AgentDataDTO, index: number) => (
								<div
									key={agent.agentId || index}
									className="relative group"
								>
									{/* Agent Card Glow */}
									<div className="absolute inset-0 bg-gradient-to-r from-blue-500/10 to-purple-500/10 rounded-xl blur-sm opacity-0 group-hover:opacity-100 transition duration-300 -z-10"></div>

									{/* Agent Card */}
									<div
										className="relative bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10 hover:border-white/20 transition-all duration-300 cursor-pointer"
										onClick={() =>
											!isDisabled &&
											handleAgentClick(agent)
										}
									>
										{/* Agent Header */}
										<div className="flex items-start justify-between mb-3">
											<div className="flex items-center space-x-3">
												<div className="bg-gradient-to-r from-blue-500 to-purple-600 p-2 rounded-lg">
													<BotMessageSquare className="w-4 h-4 text-white" />
												</div>
												<div>
													<h3 className="text-white font-semibold text-lg">
														{agent.agentName ||
															`Agent ${
																index + 1
															}`}
													</h3>
													<div className="flex items-center space-x-4 mt-1">
														<div className="flex items-center space-x-1 text-white/60 text-sm">
															<Settings className="w-3 h-3" />
															<span>
																{agent.applicationName ||
																	"Unknown App"}
															</span>
														</div>
														<div className="flex items-center space-x-1 text-white/60 text-sm">
															<User className="w-3 h-3" />
															<span>
																{agent.createdBy ||
																	"Unknown"}
															</span>
														</div>
													</div>
												</div>
											</div>
											<div className="flex items-center space-x-1">
												<div className="w-2 h-2 bg-green-400 rounded-full animate-pulse"></div>
												<span className="text-green-400 text-xs font-medium">
													Active
												</span>
											</div>
										</div>

										{/* Agent Actions */}
										<div className="flex items-center justify-between pt-3 border-t border-white/10">
											<div className="text-white/40 text-xs">
												<Calendar className="w-3 h-3 inline mr-1" />
												{new Date().toLocaleDateString()}
											</div>
										</div>
									</div>
								</div>
							)
						)
					) : (
						<div className="flex flex-col items-center justify-center py-12 text-center">
							<div className="bg-white/5 backdrop-blur-sm rounded-full p-6 mb-4">
								<Bot className="w-12 h-12 text-white/40" />
							</div>
							<h3 className="text-white/80 text-lg font-medium mb-2">
								No Agents Found
							</h3>
							<p className="text-white/60 text-sm max-w-sm">
								You haven't created any AI agents yet. Create
								your first agent to get started.
							</p>
						</div>
					)}
				</div>

				{/* Footer */}
				<div className="border-t border-white/10 p-6 flex-shrink-0">
					<div className="flex items-center justify-between">
						<div className="text-white/60 text-sm">
							{agentsDataList.length} agent
							{agentsDataList.length !== 1 ? "s" : ""} total
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}
