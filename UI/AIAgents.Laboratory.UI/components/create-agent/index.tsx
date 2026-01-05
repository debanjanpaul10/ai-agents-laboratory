import { useEffect, useState } from "react";

import { useAppDispatch, useAppSelector } from "@store/index";
import { ToggleNewAgentDrawer } from "@store/common/actions";
import CreateAgentFlyoutComponent from "@components/create-agent/agent-creator-flyout";
import KnowledgeBaseFlyoutComponent from "@components/common/knowledge-base-flyout";

export default function CreateAgentComponent() {
	const dispatch = useAppDispatch();

	const [isDrawerOpen, setIsDrawerOpen] = useState<boolean>(false);
	const [knowledgeBaseFlyoutOpen, setKnowledgeBaseFlyoutOpen] =
		useState<boolean>(false);
	const [selectedKnowledgeFiles, setSelectedKnowledgeFiles] = useState<
		File[]
	>([]);

	const IsDrawerOpenStoreData = useAppSelector(
		(state) => state.CommonReducer.isNewAgentDrawerOpen
	);

	useEffect(() => {
		if (isDrawerOpen !== IsDrawerOpenStoreData) {
			setIsDrawerOpen(IsDrawerOpenStoreData);
			// Close knowledge base flyout when main drawer closes
			if (!IsDrawerOpenStoreData) {
				setKnowledgeBaseFlyoutOpen(false);
			}
		}
	}, [IsDrawerOpenStoreData]);

	const onClose = () => {
		dispatch(ToggleNewAgentDrawer(false));
		setKnowledgeBaseFlyoutOpen(false); // Close knowledge base flyout when main flyout closes
	};

	const toggleKnowledgebaseFlyout = (isOpen: boolean) => {
		setKnowledgeBaseFlyoutOpen(isOpen);
	};

	const handleClearKnowledgeFiles = () => {
		setSelectedKnowledgeFiles([]);
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
							<KnowledgeBaseFlyoutComponent
								isOpen={knowledgeBaseFlyoutOpen}
								onClose={() => toggleKnowledgebaseFlyout(false)}
								onFilesChange={setSelectedKnowledgeFiles}
								selectedFiles={selectedKnowledgeFiles}
								existingDocuments={[]}
								onExistingDocumentsChange={() => {}} // No-op for create agent
								removedExistingDocs={[]} // No existing docs in create flow
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
						/>
					</div>
				</div>
			</>
		)
	);
}
