import { useEffect, useState } from "react";
import {
	Download,
	Files,
	FileText,
	Images,
	ScanEye,
	View,
	Plus,
} from "lucide-react";
import { Button } from "@heroui/react";

import { useAppDispatch, useAppSelector } from "@store/index";
import {
	GetAllAgentsDataAsync,
	ToggleNewAgentDrawer,
} from "@store/agents/actions";
import { GetAllConfigurations } from "@store/common/actions";
import { AgentDataDTO } from "@models/agent-data-dto";
import AgentsListComponent from "@components/manage-agents/agents-list";
import ModifyAgentComponent from "@components/manage-agents/modify-agent";
import TestAgentComponent from "@components/manage-agents/test-agent";
import CreateAgentComponent from "@components/manage-agents/create-agent";
import FileUploadFlyoutComponent from "@components/common/file-upload-flyout";
import {
	AiVisionImagesFlyoutPropsConstants,
	KnowledgeBaseFlyoutPropsConstants,
	DashboardConstants,
} from "@helpers/constants";
import MainLayout from "@components/common/main-layout";
import { useAuth } from "@auth/AuthProvider";
import { FullScreenLoading } from "@components/common/spinner";

export default function ManageAgentsPage() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const [agentsDataList, setAgentsDataList] = useState<AgentDataDTO[]>([]);
	const [selectedAgent, setSelectedAgent] = useState<AgentDataDTO | null>(
		null
	);
	const [editFormData, setEditFormData] = useState<AgentDataDTO>({
		agentName: "",
		agentDescription: "",
		applicationName: "",
		agentMetaPrompt: "",
		createdBy: "",
		agentId: "",
		dateCreated: new Date(),
		dateModified: new Date(),
		knowledgeBaseDocument: [],
		isPrivate: false,
		mcpServerUrl: "",
		visionImages: null,
		aiVisionImagesData: [],
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
	const [isVisionFlyoutOpen, setIsVisionFlyoutOpen] =
		useState<boolean>(false);
	const [selectedVisionImages, setSelectedVisionImages] = useState<File[]>(
		[]
	);
	const [removedExistingImages, setRemovedExistingImages] = useState<
		string[]
	>([]);

	const AgentsListStoreData = useAppSelector(
		(state) => state.AgentsReducer.agentsListData
	);
	const IsLoadingStoreData = useAppSelector(
		(state) => state.CommonReducer.isLoading
	);

	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations
	);

	useEffect(() => {
		if (authContext.isAuthenticated && !authContext.isLoading) {
			GetAllAgentsData();
			if (
				!ConfigurationsStoreData ||
				Object.keys(ConfigurationsStoreData).length === 0
			) {
				GetAllConfigurationsData();
			}
		}
	}, [authContext.isAuthenticated, authContext.isLoading]);

	async function GetAllAgentsData() {
		const token = await fetchToken();
		token && dispatch(GetAllAgentsDataAsync(token));
	}

	async function GetAllConfigurationsData() {
		const token = await fetchToken();
		token && dispatch(GetAllConfigurations(token));
	}

	async function fetchToken() {
		try {
			if (authContext.isAuthenticated && !authContext.isLoading)
				return await authContext.getAccessToken();
		} catch (error) {
			console.error(error);
		}
	}

	useEffect(() => {
		if (agentsDataList !== AgentsListStoreData) {
			setAgentsDataList(AgentsListStoreData);
		}
	}, [AgentsListStoreData]);

	const handleCreateAgent = () => {
		dispatch(ToggleNewAgentDrawer(true));
	};

	const handleAgentClick = (agent: AgentDataDTO) => {
		setSelectedAgent(agent);
		setEditFormData({
			agentName: agent.agentName || "",
			agentDescription: agent.agentDescription || "",
			applicationName: agent.applicationName || "",
			agentMetaPrompt: agent.agentMetaPrompt || "",
			createdBy: agent.createdBy || "",
			agentId: agent.agentId || "",
			dateCreated: agent.dateCreated,
			dateModified: agent.dateModified,
			knowledgeBaseDocument: agent.knowledgeBaseDocument || null,
			isPrivate: agent.isPrivate,
			mcpServerUrl: agent.mcpServerUrl || "",
			visionImages: agent.visionImages || null,
			aiVisionImagesData: agent.aiVisionImagesData || [],
		});

		// KB FILES
		setSelectedKnowledgeFiles([]);
		setRemovedExistingDocuments([]);
		setIsKnowledgeBaseFlyoutOpen(false);

		// AI VISION IMAGES
		setSelectedVisionImages([]);
		setRemovedExistingImages([]);
		setIsVisionFlyoutOpen(false);

		setIsEditDrawerOpen(true);
	};

	const handleEditClose = () => {
		setIsEditDrawerOpen(false);
		setSelectedAgent(null);

		// KB FILES
		setSelectedKnowledgeFiles([]);
		setRemovedExistingDocuments([]);
		setIsKnowledgeBaseFlyoutOpen(false);

		// AI VISION IMAGES
		setSelectedVisionImages([]);
		setRemovedExistingImages([]);
		setIsVisionFlyoutOpen(false);

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

	const toggleAiVisionFlyout = (isOpen: boolean) => {
		setIsVisionFlyoutOpen(isOpen);
	};

	const handleExistingImagesChange = (removedImageNames: string[]) => {
		setRemovedExistingImages(removedImageNames);
	};

	const getExistingDocuments = () => {
		if (
			!selectedAgent?.knowledgeBaseDocument ||
			selectedAgent?.knowledgeBaseDocument.length === 0
		)
			return [];

		return selectedAgent.knowledgeBaseDocument.map((doc: any) => {
			if (doc instanceof File) {
				return doc;
			}
			return {
				name: doc.documentName || doc.name || doc.fileName || "Unknown",
				size: doc.size || doc.fileSize || 0,
				documentUrl: doc.documentUrl || doc.url || "",
				contentType: doc.contentType || "",
				...doc,
			};
		});
	};

	const getExistingImages = () => {
		if (
			!selectedAgent?.aiVisionImagesData ||
			selectedAgent?.aiVisionImagesData.length === 0
		)
			return [];

		return selectedAgent.aiVisionImagesData.map((image: any) => ({
			name: image.imageName || image.name || "Unknown",
			size: image.size || 0,
			imageUrl: image.imageUrl || image.url || "",
			...image,
		}));
	};

	const handleUnAuthorizedUser = () => {
		return (
			<FullScreenLoading
				isLoading={true}
				message={
					DashboardConstants.LoadingConstants.LoginRedirectLoader
				}
			/>
		);
	};

	const renderAuthorizedManageAgents = () => {
		return (
			<MainLayout contentClassName="p-0" isFullWidth={true}>
				<div className="w-full h-screen overflow-hidden bg-gradient-to-br from-gray-900 via-slate-900 to-black">
					<AgentsListComponent
						agentsDataList={agentsDataList}
						handleAgentClick={handleAgentClick}
						onClose={() => {}}
						isDisabled={isAnyDrawerOpen}
						showCloseButton={false}
						actionButton={
							<Button
								onPress={handleCreateAgent}
								className="bg-white/5 border border-white/10 hover:border-blue-500/50 hover:bg-blue-500/10 text-white font-medium px-6 rounded-xl transition-all duration-300 group shadow-lg"
							>
								<Plus className="w-4 h-4 mr-2 text-blue-400 group-hover:text-blue-300 group-hover:scale-110 transition-all" />
								Add New Agent
							</Button>
						}
					/>
				</div>

				<CreateAgentComponent />

				{/* Backdrop Overlay */}
				{isAnyDrawerOpen && (
					<div
						className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-all duration-300"
						onClick={() => {
							/* Optional: Close all drawers logic if desired, keeping non-clickable for now as requested user might just want visual block */
						}}
					/>
				)}

				{/* Drawers  */}

				{/* Modify Agent Drawer - Base Drawer (z-50) */}
				{isEditDrawerOpen && (
					<div className="fixed top-0 right-0 h-screen z-50 transition-all duration-500 ease-in-out md:w-1/3 w-full">
						<div className="absolute inset-0 bg-gradient-to-r from-green-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
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
									isKnowledgeBaseFlyoutOpen ||
									isVisionFlyoutOpen
								}
								onOpenKnowledgeBase={() =>
									toggleKnowledgebaseFlyout(true)
								}
								selectedKnowledgeFiles={selectedKnowledgeFiles}
								removedExistingDocuments={
									removedExistingDocuments
								}
								onOpenVisionImagesFlyout={() => {
									toggleAiVisionFlyout(true);
								}}
								selectedVisionImages={selectedVisionImages}
								removedExistingImages={removedExistingImages}
							/>
						</div>
					</div>
				)}

				{/* Test Agent Drawer - Secondary (z-60) */}
				{isTestDrawerOpen && (
					<div className="fixed top-0 right-0 md:right-1/3 md:w-2/3 w-full h-screen z-[60] transition-all duration-500 ease-in-out">
						<div className="absolute inset-0 bg-gradient-to-r from-cyan-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
							<TestAgentComponent
								selectedAgent={selectedAgent}
								editFormData={editFormData}
								onClose={() => setIsTestDrawerOpen(false)}
							/>
						</div>
					</div>
				)}

				{/* Knowledge Base Flyout - Secondary (z-60) */}
				{isKnowledgeBaseFlyoutOpen && (
					<div className="fixed top-0 right-0 md:right-1/3 md:w-1/3 w-full h-screen z-[60] transition-all duration-500 ease-in-out">
						<div className="absolute inset-0 bg-gradient-to-r from-cyan-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
							<FileUploadFlyoutComponent
								isOpen={isKnowledgeBaseFlyoutOpen}
								onClose={() => toggleKnowledgebaseFlyout(false)}
								onFilesChange={setSelectedKnowledgeFiles}
								selectedFiles={selectedKnowledgeFiles}
								existingFiles={getExistingDocuments()}
								onExistingFilesChange={
									handleExistingDocumentsChange
								}
								removedExistingFiles={removedExistingDocuments}
								config={{
									headerConstants:
										KnowledgeBaseFlyoutPropsConstants,
									icons: {
										title: Files,
										body: FileText,
										download: Download,
									},
									supportedTypes:
										".doc,.docx,.pdf,.txt,.xls,.xlsx,.json",
								}}
							/>
						</div>
					</div>
				)}

				{/* Vision Flyout - Secondary (z-60) */}
				{isVisionFlyoutOpen && (
					<div className="fixed top-0 right-0 md:right-1/3 md:w-1/3 w-full h-screen z-[60] transition-all duration-500 ease-in-out">
						<div className="absolute inset-0 bg-gradient-to-r from-cyan-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
							<FileUploadFlyoutComponent
								isOpen={isVisionFlyoutOpen}
								onClose={() => toggleAiVisionFlyout(false)}
								onFilesChange={setSelectedVisionImages}
								selectedFiles={selectedVisionImages}
								existingFiles={getExistingImages()}
								onExistingFilesChange={
									handleExistingImagesChange
								}
								removedExistingFiles={removedExistingImages}
								config={{
									headerConstants:
										AiVisionImagesFlyoutPropsConstants,
									icons: {
										title: ScanEye,
										body: Images,
										download: View,
									},
									supportedTypes: ".jpg,.jpeg,.png,.svg",
								}}
							/>
						</div>
					</div>
				)}
			</MainLayout>
		);
	};

	const isAnyDrawerOpen =
		isEditDrawerOpen ||
		isTestDrawerOpen ||
		isKnowledgeBaseFlyoutOpen ||
		isVisionFlyoutOpen;

	return !authContext.isAuthenticated ? (
		handleUnAuthorizedUser()
	) : IsLoadingStoreData ? (
		<FullScreenLoading
			isLoading={IsLoadingStoreData}
			message={DashboardConstants.LoadingConstants.MainLoader}
		/>
	) : (
		renderAuthorizedManageAgents()
	);
}
