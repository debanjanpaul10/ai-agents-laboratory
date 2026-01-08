import { useEffect, useState } from "react";
import { Download, Files, FileText, Images, ScanEye, View } from "lucide-react";

import { useAppDispatch, useAppSelector } from "@store/index";
import { ToggleAgentsListDrawer } from "@store/common/actions";
import { AgentDataDTO } from "@models/agent-data-dto";
import AgentsListComponent from "@components/manage-agents/agents-list";
import ModifyAgentComponent from "@components/manage-agents/modify-agent";
import TestAgentComponent from "@components/manage-agents/test-agent";
import FileUploadFlyoutComponent from "@components/common/file-upload-flyout";
import {
	AiVisionImagesFlyoutPropsConstants,
	KnowledgeBaseFlyoutPropsConstants,
} from "@helpers/constants";

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

		// KNOWLEDGE BASE
		setSelectedKnowledgeFiles([]);
		setRemovedExistingDocuments([]);
		setIsKnowledgeBaseFlyoutOpen(false);

		// AI VISION IMAGES
		setSelectedVisionImages([]);
		setRemovedExistingImages([]);
		setIsVisionFlyoutOpen(false);
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

		// Transform knowledge base documents to match FileUploadFlyoutComponent expected format
		return selectedAgent.knowledgeBaseDocument.map((doc: any) => {
			// If it's already a File object, return as-is (it has name and size)
			if (doc instanceof File) {
				return doc;
			}

			// Otherwise, transform API response format to expected format
			return {
				name: doc.documentName || doc.name || doc.fileName || "Unknown",
				size: doc.size || doc.fileSize || 0,
				documentUrl: doc.documentUrl || doc.url || "",
				contentType: doc.contentType || "",
				...doc, // Preserve any other properties
			};
		});
	};

	const getExistingImages = () => {
		if (
			!selectedAgent?.aiVisionImagesData ||
			selectedAgent?.aiVisionImagesData.length === 0
		)
			return [];

		// Transform vision images to match FileUploadFlyoutComponent expected format
		return selectedAgent.aiVisionImagesData.map((image: any) => ({
			name: image.imageName || image.name || "Unknown",
			size: image.size || 0, // Size not available from API, defaulting to 0
			imageUrl: image.imageUrl || image.url || "",
			...image, // Preserve any other properties
		}));
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

				{/* Ai Vision Images Flyout */}
				{isVisionFlyoutOpen && (
					<div
						className={`fixed top-0 md:w-1/3 h-screen z-50 transition-all duration-500 ease-in-out ${
							isTestDrawerOpen ? "left-0" : "left-0"
						}`}
					>
						<div className="absolute inset-0 bg-gradient-to-r from-cyan-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-r border-white/10 shadow-2xl">
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
