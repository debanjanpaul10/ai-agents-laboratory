import { useEffect, useState } from "react";
import { useRouter } from "next/router";
import { Button, Tooltip } from "@heroui/react";
import {
	Bot,
	BotMessageSquare,
	Calendar,
	MoveLeft,
	User,
	Users,
} from "lucide-react";

import { RouteConstants, RunWorkspaceConstants } from "@helpers/constants";
import { WorkspaceAgentsDataDTO } from "@models/response/workspace-agents-data.dto";

export default function AssociatedAgentsListPaneComponent({
	workspaceDetailsData,
	selectedAgent,
	setSelectedAgent,
}: {
	workspaceDetailsData: any;
	selectedAgent: any;
	setSelectedAgent: (agent: WorkspaceAgentsDataDTO) => void;
}) {
	const router = useRouter();

	const [activeAssociatedAgents, setActiveAssociatedAgents] = useState<
		WorkspaceAgentsDataDTO[]
	>([]);

	useEffect(() => {
		setActiveAssociatedAgents(
			workspaceDetailsData?.activeAgentsListInWorkspace || [],
		);
	}, [workspaceDetailsData?.activeAgentsListInWorkspace]);

	const handleAgentClick = (agent: WorkspaceAgentsDataDTO) => {
		setSelectedAgent(agent);
	};

	return (
		<div className="flex flex-col h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-r border-white/10">
			{/* Header */}
			<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
				<div className="flex items-center space-x-3">
					<div className="bg-gradient-to-r from-purple-500 to-pink-600 p-2 rounded-xl">
						<Users className="w-5 h-5 text-white" />
					</div>
					<div>
						<h2 className="text-xl font-bold bg-gradient-to-r from-white via-purple-100 to-pink-100 bg-clip-text text-transparent">
							{workspaceDetailsData.agentWorkspaceName}
						</h2>
						<p className="text-white/60 text-sm">
							{activeAssociatedAgents.length} agent
							{activeAssociatedAgents.length !== 1
								? "s"
								: ""}{" "}
							available
						</p>
					</div>
				</div>
				<Tooltip content="Back to workspaces">
					<Button
						onPress={() => {
							router.push(RouteConstants.Workspaces);
						}}
						isIconOnly
						className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
					>
						<MoveLeft className="w-4 h-4" />
					</Button>
				</Tooltip>
			</div>

			{/* Workspace Info */}
			<div className="p-4 border-b border-white/10 bg-white/5">
				<h3 className="text-white font-medium text-sm mb-1">
					<div className="flex items-center space-x-2">
						<Calendar className="w-3.5 h-3.5" />
						<span>
							Created on{" "}
							{new Date(
								workspaceDetailsData.dateCreated,
							).toLocaleDateString()}
						</span>
					</div>
				</h3>
				<h3 className="text-white/60 font-medium text-sm mb-1">
					<div className="flex items-center space-x-2">
						<User className="w-3.5 h-3.5" />
						<span>
							Created by{" "}
							{workspaceDetailsData?.createdBy || "Unknown"}
						</span>
					</div>
				</h3>
			</div>

			{/* Agents List */}
			<div className="flex-1 overflow-y-auto p-4 space-y-2">
				{activeAssociatedAgents.length === 0 ? (
					<div className="flex flex-col items-center justify-center h-full text-center p-6">
						<div className="bg-white/5 backdrop-blur-sm rounded-full p-6 mb-4">
							<Bot className="w-12 h-12 text-white/40" />
						</div>
						<h3 className="text-white/80 text-lg font-medium mb-2">
							{RunWorkspaceConstants.AgentsPane.NoAgentsHeader}
						</h3>
						<p className="text-white/60 text-sm max-w-sm">
							{RunWorkspaceConstants.AgentsPane.NoAgentsSubHeader}
						</p>
					</div>
				) : (
					activeAssociatedAgents.map(
						(agent: WorkspaceAgentsDataDTO) => (
							<button
								key={agent.agentGuid}
								onClick={() => handleAgentClick(agent)}
								className={`w-full text-left p-4 rounded-lg border transition-all duration-200 cursor-pointer ${
									selectedAgent?.agentGuid === agent.agentGuid
										? "bg-gradient-to-r from-purple-500/20 to-pink-500/20 border-purple-500/50 shadow-lg shadow-purple-500/20"
										: "bg-white/5 border-white/10 hover:bg-white/10 hover:border-white/20"
								}`}
							>
								<div className="flex items-center space-x-3">
									<div
										className={`p-2 rounded-lg ${
											selectedAgent?.agentGuid ===
											agent.agentGuid
												? "bg-gradient-to-r from-purple-500 to-pink-600"
												: "bg-white/10"
										}`}
									>
										<BotMessageSquare className="w-5 h-5 text-white" />
									</div>
									<div className="flex-1 min-w-0">
										<h4 className="text-white font-medium text-sm truncate">
											{agent.agentName}
										</h4>
										<p className="text-white/60 text-xs truncate">
											Click to chat
										</p>
									</div>
								</div>
							</button>
						),
					)
				)}
			</div>
		</div>
	);
}
