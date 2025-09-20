import { useEffect, useState } from "react";
import { Sparkles, Minimize2 } from "lucide-react";
import { Textarea } from "@heroui/react";

import { useAppDispatch, useAppSelector } from "@/store";
import { ToggleAgentsListDrawer } from "@/store/common/actions";
import { AgentDataDTO } from "@/models/agent-data-dto";
import AgentsListComponent from "./agents-list";
import ModifyAgentComponent from "./modify-agent";
import TestAgentComponent from "./test-agent";

export default function ManageAgentsComponent() {
	const dispatch = useAppDispatch();

	const [agentsListDrawerOpen, setAgentsListDrawerOpen] = useState(false);
	const [agentsDataList, setAgentsDataList] = useState<AgentDataDTO[]>([]);

	const [expandedPromptModal, setExpandedPromptModal] = useState(false);
	const [selectedAgent, setSelectedAgent] = useState<AgentDataDTO | null>(
		null
	);
	const [editFormData, setEditFormData] = useState<AgentDataDTO>({
		agentName: "",
		applicationName: "",
		agentMetaPrompt: "",
		createdBy: "",
		agentId: "",
	});
	const [isEditDrawerOpen, setIsEditDrawerOpen] = useState(false);
	const [isTestDrawerOpen, setIsTestDrawerOpen] = useState(false);

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
		setIsEditDrawerOpen(false);
		setSelectedAgent(null);
	};

	const handleAgentClick = (agent: AgentDataDTO) => {
		setSelectedAgent(agent);
		setEditFormData({
			agentName: agent.agentName || "",
			applicationName: agent.applicationName || "",
			agentMetaPrompt: agent.agentMetaPrompt || "",
			createdBy: agent.createdBy || "",
			agentId: agent.agentId || "",
		});
		setIsEditDrawerOpen(true);
	};

	const handleInputChange = (field: string, value: string) => {
		setEditFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleExpandPrompt = () => {
		setExpandedPromptModal(true);
	};

	const handleCollapsePrompt = () => {
		setExpandedPromptModal(false);
	};

	const handleEditClose = () => {
		setIsEditDrawerOpen(false);
		setSelectedAgent(null);
		// Also close test drawer if it's open
		if (isTestDrawerOpen) {
			setIsTestDrawerOpen(false);
		}
	};

	const renderExpandedMetapromptEditor = () => {
		return (
			expandedPromptModal && (
				<>
					{/* Modal Backdrop */}
					<div
						className="fixed inset-0 bg-black/80 backdrop-blur-sm z-70 transition-opacity duration-300"
						onClick={handleCollapsePrompt}
					/>

					{/* Modal Content */}
					<div className="fixed inset-0 flex items-center justify-center z-80 p-8">
						<div className="w-full max-w-4xl h-full max-h-[90vh] bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border border-white/10 shadow-2xl rounded-2xl flex flex-col">
							{/* Modal Header */}
							<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
								<div className="flex items-center space-x-3">
									<div className="bg-gradient-to-r from-orange-500 to-red-600 p-2 rounded-xl">
										<Sparkles className="w-5 h-5 text-white" />
									</div>
									<div>
										<h2 className="text-xl font-bold bg-gradient-to-r from-white via-orange-100 to-red-100 bg-clip-text text-transparent">
											Agent Meta Prompt
										</h2>
									</div>
								</div>
								<button
									onClick={handleCollapsePrompt}
									className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
									title="Collapse prompt editor"
								>
									<Minimize2 className="w-4 h-4" />
								</button>
							</div>

							{/* Modal Content */}
							<div className="flex-1 p-6 min-h-0 flex flex-col">
								<div className="flex-1 min-h-0">
									<Textarea
										value={editFormData.agentMetaPrompt}
										onChange={(e) =>
											handleInputChange(
												"metaPrompt",
												e.target.value
											)
										}
										placeholder="Define your agent's behavior, personality, and capabilities in detail..."
										className="h-full"
										classNames={{
											base: "h-full",
											input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 resize-none px-4 py-3 h-full min-h-full",
											inputWrapper:
												"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-orange-500/50 h-full min-h-full",
										}}
									/>
								</div>
							</div>
						</div>
					</div>
				</>
			)
		);
	};

	return agentsListDrawerOpen ? (
		<>
			<div
				className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300 max-w-full"
				onClick={onClose}
			/>
			{/* Test Agent Drawer - Leftmost only when test is open) */}
			{isTestDrawerOpen && (
				<div className="fixed left-0 top-0 w-1/3 h-screen z-50 transition-all duration-500 ease-in-out">
					<div className="absolute inset-0 bg-gradient-to-r from-cyan-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
					<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-r border-white/10 shadow-2xl">
						<TestAgentComponent
							selectedAgent={selectedAgent}
							editFormData={editFormData}
							onClose={() => setIsTestDrawerOpen(false)}
						/>
					</div>
				</div>
			)}

			{/* Modify Agent Drawer - Middle (when edit is open) */}
			{isEditDrawerOpen && (
				<div
					className={`fixed top-0 w-1/3 h-screen z-50 transition-all duration-500 ease-in-out ${
						isTestDrawerOpen ? "left-1/3" : "right-1/3"
					}`}
				>
					<div className="absolute inset-0 bg-gradient-to-r from-green-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
					<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-x border-white/10 shadow-2xl">
						<ModifyAgentComponent
							editFormData={editFormData}
							handleExpandPrompt={handleExpandPrompt}
							selectedAgent={selectedAgent}
							setEditFormData={setEditFormData}
							setSelectedAgent={setSelectedAgent}
							isEditDrawerOpen={isEditDrawerOpen}
							onTestAgent={() => setIsTestDrawerOpen(true)}
							onEditClose={handleEditClose}
							isDisabled={isTestDrawerOpen}
						/>
					</div>
				</div>
			)}

			{/* Agents List Drawer - Rightmost (always visible when main drawer is open) */}
			<div className="fixed right-0 top-0 w-1/3 h-screen z-50 transition-all duration-500 ease-in-out">
				<div className="absolute inset-0 bg-gradient-to-l from-purple-600/20 via-blue-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10"></div>
				<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
					<AgentsListComponent
						agentsDataList={agentsDataList}
						handleAgentClick={handleAgentClick}
						onClose={onClose}
						isDisabled={isTestDrawerOpen || isEditDrawerOpen}
					/>
				</div>
			</div>

			{renderExpandedMetapromptEditor()}
		</>
	) : null;
}
