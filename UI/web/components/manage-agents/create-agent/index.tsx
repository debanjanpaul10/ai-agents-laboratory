import { useEffect, useState } from "react";
import { Download, Files, FileText, Images, ScanEye, View } from "lucide-react";

import { useAppDispatch, useAppSelector } from "@store/index";
import {
	AiVisionImagesFlyoutPropsConstants,
	KnowledgeBaseFlyoutPropsConstants,
} from "@helpers/constants";
import FileUploadFlyoutComponent from "@components/common/file-upload-flyout";
import { ToggleNewAgentDrawer } from "@store/agents/actions";
import CreateAgentFlyoutComponent from "@components/manage-agents/create-agent/agent-creator-flyout";

export default function CreateAgentComponent() {
	const dispatch = useAppDispatch();

	const [isDrawerOpen, setIsDrawerOpen] = useState<boolean>(false);
	const [knowledgeBaseFlyoutOpen, setKnowledgeBaseFlyoutOpen] =
		useState<boolean>(false);
	const [selectedKnowledgeFiles, setSelectedKnowledgeFiles] = useState<
		File[]
	>([]);

	const [aiVisionImagesFlyoutOpen, setAiVisionImagesFlyoutOpen] =
		useState<boolean>(false);
	const [selectedAiVisionImages, setSelectedAiVisionImages] = useState<
		File[]
	>([]);

	const IsDrawerOpenStoreData = useAppSelector(
		(state) => state.AgentsReducer.isNewAgentDrawerOpen
	);

	useEffect(() => {
		if (isDrawerOpen !== IsDrawerOpenStoreData) {
			setIsDrawerOpen(IsDrawerOpenStoreData);
			// Close knowledge base flyout when main drawer closes
			if (!IsDrawerOpenStoreData) {
				setKnowledgeBaseFlyoutOpen(false);
				setAiVisionImagesFlyoutOpen(false);
			}
		}
	}, [IsDrawerOpenStoreData]);

	const onClose = () => {
		dispatch(ToggleNewAgentDrawer(false));
		setKnowledgeBaseFlyoutOpen(false); // Close knowledge base flyout when main flyout closes
		setAiVisionImagesFlyoutOpen(false);
	};

	const toggleKnowledgebaseFlyout = (isOpen: boolean) => {
		setKnowledgeBaseFlyoutOpen(isOpen);
	};

	const handleClearKnowledgeFiles = () => {
		setSelectedKnowledgeFiles([]);
	};

	const toggleAiVisionFlyout = (isOpen: boolean) => {
		setAiVisionImagesFlyoutOpen(isOpen);
	};

	const handleClearAiVisionImages = () => {
		setSelectedAiVisionImages([]);
	};

	return (
		isDrawerOpen && (
			<>
				<div
					className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300 max-w-full"
					onClick={onClose}
				/>

				{knowledgeBaseFlyoutOpen && (
					<div
						className={`fixed top-0 md:w-1/3 h-screen z-50 transition-all duration-500 ease-in-out ${"right-1/3"}`}
					>
						<div className="absolute inset-0 bg-gradient-to-r from-green-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-x border-white/10 shadow-2xl">
							<FileUploadFlyoutComponent
								isOpen={knowledgeBaseFlyoutOpen}
								onClose={() => toggleKnowledgebaseFlyout(false)}
								onFilesChange={setSelectedKnowledgeFiles}
								selectedFiles={selectedKnowledgeFiles}
								existingFiles={[]}
								onExistingFilesChange={() => {}}
								removedExistingFiles={[]}
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

				{aiVisionImagesFlyoutOpen && (
					<div
						className={`fixed top-0 md:w-1/3 h-screen z-50 transition-all duration-500 ease-in-out ${"right-1/3"}`}
					>
						<div className="absolute inset-0 bg-gradient-to-r from-green-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-x border-white/10 shadow-2xl">
							<FileUploadFlyoutComponent
								isOpen={aiVisionImagesFlyoutOpen}
								onClose={() => toggleAiVisionFlyout(false)}
								onFilesChange={setSelectedAiVisionImages}
								selectedFiles={selectedAiVisionImages}
								existingFiles={[]}
								onExistingFilesChange={() => {}}
								removedExistingFiles={[]}
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

				<div className="fixed right-0 top-0 md:w-1/3 h-screen z-50 transition-all duration-500 ease-in-out">
					<div className="absolute inset-0 bg-gradient-to-l from-purple-600/20 via-blue-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10"></div>
					<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
						<CreateAgentFlyoutComponent
							isOpen={isDrawerOpen}
							onClose={onClose}
							selectedKnowledgeFiles={selectedKnowledgeFiles}
							onOpenKnowledgeBase={() =>
								toggleKnowledgebaseFlyout(true)
							}
							onClearKnowledgeFiles={handleClearKnowledgeFiles}
							selectedAiVisionImages={selectedAiVisionImages}
							onOpenAiVisionFlyout={() =>
								toggleAiVisionFlyout(true)
							}
							onClearAiVisionImages={handleClearAiVisionImages}
						/>
					</div>
				</div>
			</>
		)
	);
}
