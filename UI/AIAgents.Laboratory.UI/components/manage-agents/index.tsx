import { useEffect, useState } from "react";

import { useAppDispatch, useAppSelector } from "@store/index";
import { ToggleAgentsListDrawer } from "@store/common/actions";
import { AgentDataDTO } from "@models/agent-data-dto";
import AgentsListComponent from "@components/manage-agents/agents-list";
import ModifyAgentComponent from "@components/manage-agents/modify-agent";
import TestAgentComponent from "@components/manage-agents/test-agent";
import KnowledgeBaseFlyoutComponent from "@components/common/knowledge-base-flyout";

export default function ManageAgentsComponent() {
	const dispatch = useAppDispatch();

	const [agentsListDrawerOpen, setAgentsListDrawerOpen] = useState(false);
	const [agentsDataList, setAgentsDataList] = useState<AgentDataDTO[]>([]);

	const [selectedAgent, setSelectedAgent] = useState<AgentDataDTO | null>(
		null
	);
	const [editFormData, setEditFormData] = useState<AgentDataDTO>({
		agentName: "",
		applicationName: "",
		agentMetaPrompt: "",
		createdBy: "",
		agentId: "",
		dateCreated: new Date(),
		knowledgeBaseDocument: [],
		isPrivate: false,
		mcpServerUrl: "",
	});
	const [isEditDrawerOpen, setIsEditDrawerOpen] = useState(false);
	const [isTestDrawerOpen, setIsTestDrawerOpen] = useState(false);
	const [isKnowledgeBaseFlyoutOpen, setIsKnowledgeBaseFlyoutOpen] =
		useState(false);
	const [selectedKnowledgeFiles, setSelectedKnowledgeFiles] = useState<
		File[]
	>([]);
	const [removedExistingDocuments, setRemovedExistingDocuments] = useState<
		string[]
	>([]);

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
		setSelectedKnowledgeFiles([]); // Clear knowledge files when closing
		setRemovedExistingDocuments([]); // Clear removed existing documents when closing
		setIsKnowledgeBaseFlyoutOpen(false); // Close knowledge base flyout
	};

	const handleAgentClick = (agent: AgentDataDTO) => {
		setSelectedAgent(agent);
		setEditFormData({
			agentName: agent.agentName || "",
			applicationName: agent.applicationName || "",
			agentMetaPrompt: agent.agentMetaPrompt || "",
			createdBy: agent.createdBy || "",
			agentId: agent.agentId || "",
			dateCreated: agent.dateCreated,
			knowledgeBaseDocument: agent.knowledgeBaseDocument || null,
			isPrivate: agent.isPrivate,
			mcpServerUrl: agent.mcpServerUrl || "",
		});
		setSelectedKnowledgeFiles([]); // Clear knowledge files when switching agents
		setRemovedExistingDocuments([]); // Clear removed existing documents when switching agents
		setIsKnowledgeBaseFlyoutOpen(false); // Close knowledge base flyout
		setIsEditDrawerOpen(true);
	};

	const handleEditClose = () => {
		setIsEditDrawerOpen(false);
		setSelectedAgent(null);
		setSelectedKnowledgeFiles([]); // Clear knowledge files when closing
		setRemovedExistingDocuments([]); // Clear removed existing documents when closing
		setIsKnowledgeBaseFlyoutOpen(false); // Close knowledge base flyout
		if (isTestDrawerOpen) {
			setIsTestDrawerOpen(false);
		}
	};

	const toggleKnowledgebaseFlyout = (isOpen: boolean) => {
		setIsKnowledgeBaseFlyoutOpen(isOpen);
	};

	const handleExistingDocumentsChange = (removedFileNames: string[]) => {
		setRemovedExistingDocuments(removedFileNames);
	};

	// Convert existing knowledge base document to the new format
	const getExistingDocuments = () => {
		if (
			!selectedAgent?.knowledgeBaseDocument ||
			selectedAgent?.knowledgeBaseDocument.length === 0
		)
			return [];

		return selectedAgent.knowledgeBaseDocument;
	};

	return (
		agentsListDrawerOpen && (
			<>
				<div
					className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300 max-w-full"
					onClick={onClose}
				/>
				{/* Test Agent Drawer - Leftmost only when test is open) */}
				{isTestDrawerOpen && (
					<div className="fixed left-0 top-0 md:w-1/3 h-screen z-50 transition-all duration-500 ease-in-out">
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

				{/* Knowledge base flyout */}
				{isKnowledgeBaseFlyoutOpen && (
					<div
						className={`fixed top-0 md:w-1/3 h-screen z-50 transition-all duration-500 ease-in-out ${
							isTestDrawerOpen ? "left-0" : "left-0"
						}`}
					>
						<div className="absolute inset-0 bg-gradient-to-r from-cyan-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-r border-white/10 shadow-2xl">
							<KnowledgeBaseFlyoutComponent
								isOpen={isKnowledgeBaseFlyoutOpen}
								onClose={() => toggleKnowledgebaseFlyout(false)}
								onFilesChange={setSelectedKnowledgeFiles}
								selectedFiles={selectedKnowledgeFiles}
								existingDocuments={getExistingDocuments()}
								onExistingDocumentsChange={
									handleExistingDocumentsChange
								}
							/>
						</div>
					</div>
				)}

				{/* Modify Agent Drawer - Middle (when edit is open) */}
				{isEditDrawerOpen && (
					<div
						className={`fixed top-0 md:w-1/3 h-screen z-50 transition-all duration-500 ease-in-out ${
							isTestDrawerOpen
								? "left-1/3"
								: isKnowledgeBaseFlyoutOpen
								? "left-1/3"
								: "right-1/3"
						}`}
					>
						<div className="absolute inset-0 bg-gradient-to-r from-green-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-x border-white/10 shadow-2xl">
							<ModifyAgentComponent
								editFormData={editFormData}
								selectedAgent={selectedAgent}
								setEditFormData={setEditFormData}
								setSelectedAgent={setSelectedAgent}
								isEditDrawerOpen={isEditDrawerOpen}
								onTestAgent={() => setIsTestDrawerOpen(true)}
								onEditClose={handleEditClose}
								isDisabled={
									isTestDrawerOpen ||
									isKnowledgeBaseFlyoutOpen
								}
								onOpenKnowledgeBase={() =>
									toggleKnowledgebaseFlyout(true)
								}
								selectedKnowledgeFiles={selectedKnowledgeFiles}
								removedExistingDocuments={
									removedExistingDocuments
								}
							/>
						</div>
					</div>
				)}

				{/* Agents List Drawer - Rightmost (always visible when main drawer is open) */}
				<div className="fixed right-0 top-0 md:w-1/3 h-screen z-50 transition-all duration-500 ease-in-out">
					<div className="absolute inset-0 bg-gradient-to-l from-purple-600/20 via-blue-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10"></div>
					<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
						<AgentsListComponent
							agentsDataList={agentsDataList}
							handleAgentClick={handleAgentClick}
							onClose={onClose}
							isDisabled={
								isTestDrawerOpen ||
								isEditDrawerOpen ||
								isKnowledgeBaseFlyoutOpen
							}
						/>
					</div>
				</div>
			</>
		)
	);
}
