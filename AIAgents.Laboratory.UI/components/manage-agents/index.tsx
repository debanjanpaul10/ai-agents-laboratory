import { useEffect, useState } from "react";
import {
	Bot,
	X,
	Settings,
	User,
	Calendar,
	BotMessageSquare,
	Save,
	Sparkles,
	ArrowRight,
} from "lucide-react";
import { Button, Input, Textarea } from "@heroui/react";

import { ManageAgentConstants } from "@/helpers/constants";
import { useAppDispatch, useAppSelector } from "@/store";
import { ToggleAgentsListDrawer } from "@/store/common/actions";
import { AgentDataDTO } from "@/models/agent-data-dto";

export default function ManageAgentsComponent() {
	const dispatch = useAppDispatch();

	const [agentsListDrawerOpen, setAgentsListDrawerOpen] = useState(false);
	const [agentsDataList, setAgentsDataList] = useState<AgentDataDTO[]>([]);
	const [editDrawerOpen, setEditDrawerOpen] = useState(false);
	const [selectedAgent, setSelectedAgent] = useState<AgentDataDTO | null>(
		null
	);
	const [editFormData, setEditFormData] = useState({
		agentName: "",
		applicationName: "",
		metaPrompt: "",
		createdBy: "",
	});

	const AgentsListStoreData = useAppSelector(
		(state) => state.AgentsReducer.agentsListData
	);
	const IsAgentsListDrawerOpenStoreData = useAppSelector(
		(state) => state.CommonReducer.isAgentsListDrawerOpen
	);

	useEffect(() => {
		if (agentsListDrawerOpen !== IsAgentsListDrawerOpenStoreData) {
			setAgentsListDrawerOpen(IsAgentsListDrawerOpenStoreData);
		}
	}, [IsAgentsListDrawerOpenStoreData]);

	useEffect(() => {
		if (
			agentsDataList !== AgentsListStoreData &&
			AgentsListStoreData.length > 0
		) {
			setAgentsDataList(AgentsListStoreData);
		}
	}, [AgentsListStoreData]);

	useEffect(() => {
		if (agentsListDrawerOpen) {
			document.body.style.overflow = "hidden";
		} else {
			document.body.style.overflow = "unset";
		}

		// Cleanup on unmount
		return () => {
			document.body.style.overflow = "unset";
		};
	}, [agentsListDrawerOpen]);

	const onClose = () => {
		dispatch(ToggleAgentsListDrawer(false));
		setEditDrawerOpen(false);
		setSelectedAgent(null);
	};

	const handleAgentClick = (agent: AgentDataDTO) => {
		setSelectedAgent(agent);
		setEditFormData({
			agentName: agent.agentName || "",
			applicationName: agent.applicationName || "",
			metaPrompt: agent.agentMetaPrompt || "",
			createdBy: agent.createdBy || "",
		});
		setEditDrawerOpen(true);
	};

	const handleEditClose = () => {
		setEditDrawerOpen(false);
		setSelectedAgent(null);
	};

	const handleEditSave = () => {
		// Handle save logic here
		console.log("Saving agent data:", editFormData);
		// You can dispatch an action to save the data
		handleEditClose();
	};

	const handleInputChange = (field: string, value: string) => {
		setEditFormData((prev) => ({ ...prev, [field]: value }));
	};

	const renderEditAgentDrawer = () => {
		return (
			editDrawerOpen &&
			selectedAgent && (
				<div className="flex flex-col transition-all duration-500 w-full max-w-lg">
					{/* Edit Header */}
					<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
						<div className="flex items-center space-x-3">
							<div className="bg-gradient-to-r from-green-500 to-blue-600 p-2 rounded-xl">
								<BotMessageSquare className="w-5 h-5 text-white" />
							</div>
							<div>
								<h2 className="text-xl font-bold bg-gradient-to-r from-white via-green-100 to-blue-100 bg-clip-text text-transparent">
									Modify Agent configuration
								</h2>
							</div>
						</div>
						<button
							onClick={handleEditClose}
							className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
						>
							<ArrowRight className="w-4 h-4" />
						</button>
					</div>

					{/* Edit Form Content */}
					<div className="flex-1 overflow-y-auto p-6 space-y-6 min-h-0">
						{/* Agent Name Field */}
						<div className="space-y-2">
							<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
								<Sparkles className="w-4 h-4 text-green-400" />
								<span>Agent Name</span>
							</label>
							<div className="relative group">
								<div className="absolute -inset-0.5 bg-gradient-to-r from-green-500/20 to-blue-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
								<Input
									value={editFormData.agentName}
									onChange={(e) =>
										handleInputChange(
											"agentName",
											e.target.value
										)
									}
									placeholder="Enter agent name..."
									className="relative"
									classNames={{
										input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
										inputWrapper:
											"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-green-500/50 min-h-[48px]",
									}}
								/>
							</div>
						</div>

						{/* Application Name Field */}
						<div className="space-y-2">
							<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
								<Settings className="w-4 h-4 text-blue-400" />
								<span>Application Name</span>
							</label>
							<div className="relative group">
								<div className="absolute -inset-0.5 bg-gradient-to-r from-blue-500/20 to-purple-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
								<Input
									value={editFormData.applicationName}
									onChange={(e) =>
										handleInputChange(
											"applicationName",
											e.target.value
										)
									}
									placeholder="Enter application name..."
									className="relative"
									classNames={{
										input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
										inputWrapper:
											"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-blue-500/50 min-h-[48px]",
									}}
								/>
							</div>
						</div>

						{/* Author Field */}
						<div className="space-y-2">
							<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
								<User className="w-4 h-4 text-purple-400" />
								<span>Author</span>
							</label>
							<div className="relative group">
								<div className="absolute -inset-0.5 bg-gradient-to-r from-purple-500/20 to-pink-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
								<Input
									value={editFormData.createdBy}
									onChange={(e) =>
										handleInputChange(
											"createdBy",
											e.target.value
										)
									}
									placeholder="Enter author name..."
									className="relative"
									classNames={{
										input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
										inputWrapper:
											"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-purple-500/50 min-h-[48px]",
									}}
								/>
							</div>
						</div>

						{/* Meta Prompt Field */}
						<div className="space-y-2">
							<label className="text-white/80 text-sm font-medium">
								Agent Meta Prompt
							</label>
							<div className="relative group">
								<div className="absolute -inset-0.5 bg-gradient-to-r from-orange-500/20 to-red-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
								<Textarea
									value={editFormData.metaPrompt}
									onChange={(e) =>
										handleInputChange(
											"metaPrompt",
											e.target.value
										)
									}
									placeholder="Define your agent's behavior, personality, and capabilities..."
									minRows={6}
									maxRows={12}
									className="relative"
									classNames={{
										input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 resize-none px-4 py-3",
										inputWrapper:
											"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-orange-500/50",
									}}
								/>
							</div>
							<p className="text-white/40 text-xs">
								Describe how your agent should behave, what it
								should know, and how it should respond to users.
							</p>
						</div>

						{/* Agent Info Display */}
						<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
							<h3 className="text-white/80 font-medium mb-2 flex items-center space-x-2">
								<Calendar className="w-4 h-4 text-yellow-400" />
								<span>Agent Information</span>
							</h3>
							<div className="space-y-1 text-sm">
								<p className="text-white/60">
									<span className="text-white/80">
										Agent ID:
									</span>{" "}
									{selectedAgent.agentId || "N/A"}
								</p>
								<p className="text-white/60">
									<span className="text-white/80">
										Created:
									</span>{" "}
									{new Date().toLocaleDateString()}
								</p>
								<p className="text-white/60">
									<span className="text-white/80">
										Status:
									</span>{" "}
									<span className="text-green-400">
										Active
									</span>
								</p>
							</div>
						</div>
					</div>

					{/* Edit Footer */}
					<div className="p-3 flex-shrink-0">
						<div className="flex items-center justify-start space-x-3">
							<div className="relative group">
								<div className="absolute -inset-0.5 bg-gradient-to-r from-green-500 to-blue-600 rounded-xl blur opacity-75 group-hover:opacity-100 transition duration-300"></div>
								<Button
									onPress={handleEditSave}
									radius="full"
									className="flex items-center bg-gradient-to-r from-green-500 to-blue-600 text-white font-semibold hover:from-green-600 hover:to-blue-700 transition-all duration-300 px-6 py-3 min-h-[44px]"
								>
									<Save className="w-4 h-4 mr-2" />
									Save Changes
								</Button>
							</div>
						</div>
					</div>
				</div>
			)
		);
	};

	const renderAgentsListPanel = () => {
		return (
			<div className="flex flex-col transition-all duration-500 w-full max-w-lg">
				{/* Agents List Header */}
				<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
					<div className="flex items-center space-x-3">
						<div className="bg-gradient-to-r from-purple-500 to-blue-600 p-2 rounded-xl">
							<Bot className="w-5 h-5 text-white" />
						</div>
						<div>
							<h2 className="text-xl font-bold bg-gradient-to-r from-white via-blue-100 to-purple-100 bg-clip-text text-transparent">
								{ManageAgentConstants.Headers.SubText}
							</h2>
						</div>
					</div>
					<div className="flex items-center space-x-2">
						{/* Close button */}
						<button
							onClick={onClose}
							className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
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
										onClick={() => handleAgentClick(agent)}
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

				{/* Agents List Footer */}
				<div className="p-6 flex-shrink-0">
					<div className="flex items-center justify-between">
						<div className="text-white/60 text-sm">
							{agentsDataList.length} agent
							{agentsDataList.length !== 1 ? "s" : ""} total
						</div>
					</div>
				</div>
			</div>
		);
	};

	return agentsListDrawerOpen ? (
		<>
			<div
				className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300 max-w-full"
				onClick={onClose}
			/>
			<div
				className={
					"fixed right-0 top-0 h-screen z-50 transition-all duration-500 ease-in-out"
				}
			>
				{/* Glow effect */}
				<div className="absolute inset-0 bg-gradient-to-l from-purple-600/20 via-blue-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10"></div>

				{/* Main drawer content */}
				<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex">
					{renderEditAgentDrawer()}
					{renderAgentsListPanel()}
				</div>
			</div>
		</>
	) : null;
}
