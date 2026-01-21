import { useAppDispatch, useAppSelector } from "@store/index";
import {
	Settings,
	X,
	Sparkles,
	User,
	Calendar,
	ScrollText,
	Shield,
	Maximize2,
	Save,
} from "lucide-react";
import { useState, useEffect } from "react";
import { useMsal } from "@azure/msal-react";
import { useAuth } from "@auth/AuthProvider";
import { Button } from "@heroui/react";

import ExpandMetapromptEditorComponent from "@components/common/expand-metaprompt-editor";
import { UpdateExistingAgentDataAsync } from "@store/agents/actions";

export default function ChatbotInformationComponent({
	isAgentInfoDrawerOpen,
	onAgentInfoDrawerClose,
}: {
	isAgentInfoDrawerOpen: boolean;
	onAgentInfoDrawerClose: any;
}) {
	const dispatch = useAppDispatch();
	const authContext = useAuth();
	const { accounts } = useMsal();

	const ConversationAgentDetailsStoreData = useAppSelector(
		(state) => state.AgentsReducer.agentData
	);

	const [expandedPromptModal, setExpandedPromptModal] = useState(false);
	const [editedMetaPrompt, setEditedMetaPrompt] = useState(
		ConversationAgentDetailsStoreData?.agentMetaPrompt || ""
	);

	const handleAgentInfoDrawerClose = () => {
		onAgentInfoDrawerClose();
	};

	const handleExpandPrompt = () => {
		setExpandedPromptModal(true);
	};

	const handleCollapsePrompt = () => {
		setExpandedPromptModal(false);
	};

	const handleInputChange = (field: string, value: string) => {
		if (field === "agentMetaPrompt") {
			setEditedMetaPrompt(value);
		}
	};

	const handleSave = async () => {
		if (!ConversationAgentDetailsStoreData) return;

		const form = new FormData();
		form.append("agentId", ConversationAgentDetailsStoreData.agentId);
		form.append("agentMetaPrompt", editedMetaPrompt);
		form.append("agentName", ConversationAgentDetailsStoreData.agentName);
		form.append(
			"applicationName",
			ConversationAgentDetailsStoreData.applicationName
		);

		const accessToken = await authContext.getAccessToken();
		accessToken &&
			dispatch(UpdateExistingAgentDataAsync(form, accessToken));
	};

	const isOwner =
		accounts[0]?.username === ConversationAgentDetailsStoreData?.createdBy;
	const hasChanges =
		editedMetaPrompt !== ConversationAgentDetailsStoreData?.agentMetaPrompt;

	// Sync editedMetaPrompt when agent data changes
	useEffect(() => {
		if (ConversationAgentDetailsStoreData?.agentMetaPrompt) {
			setEditedMetaPrompt(
				ConversationAgentDetailsStoreData.agentMetaPrompt
			);
		}
	}, [ConversationAgentDetailsStoreData?.agentMetaPrompt]);

	const formatDate = (dateString: string) => {
		try {
			return new Date(dateString).toLocaleDateString("en-US", {
				year: "numeric",
				month: "long",
				day: "numeric",
			});
		} catch {
			return "N/A";
		}
	};

	return (
		isAgentInfoDrawerOpen && (
			<div className="h-full flex flex-col">
				{/* Header */}
				<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
					<div className="flex items-center space-x-3">
						<div className="bg-gradient-to-r from-green-500 to-blue-600 p-2 rounded-xl">
							<Settings className="w-5 h-5 text-white" />
						</div>
						<div>
							<h2 className="text-xl font-bold bg-gradient-to-r from-white via-green-100 to-blue-100 bg-clip-text text-transparent">
								AI Chatbot Agent Information
							</h2>
							<p className="text-white/60 text-sm">
								{ConversationAgentDetailsStoreData?.agentName ||
									"Agent Details"}
							</p>
						</div>
					</div>
					<button
						onClick={handleAgentInfoDrawerClose}
						className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
					>
						<X className="w-4 h-4" />
					</button>
				</div>

				{/* Content */}
				<div className="flex-1 overflow-y-auto p-3 space-y-3 min-h-0">
					{/* Agent Name */}
					<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
						<h3 className="text-white/80 font-medium mb-3 flex items-center space-x-2">
							<Sparkles className="w-4 h-4 text-green-400" />
							<span>Agent Name</span>
						</h3>
						<p className="text-white text-base">
							{ConversationAgentDetailsStoreData?.agentName ||
								"N/A"}
						</p>
					</div>

					{/* Application Name */}
					<div className="bg-white/5 backdrop-blur-sm rounded-xl p-3 border border-white/10">
						<h3 className="text-white/80 font-medium mb-3 flex items-center space-x-2">
							<Settings className="w-4 h-4 text-blue-400" />
							<span>Application Name</span>
						</h3>
						<p className="text-white text-base">
							{ConversationAgentDetailsStoreData?.applicationName ||
								"N/A"}
						</p>
					</div>

					{/* Agent Meta Prompt */}
					<div className="bg-white/5 backdrop-blur-sm rounded-xl p-3 border border-white/10">
						<div className="flex items-center justify-between mb-3">
							<h3 className="text-white/80 font-medium flex items-center space-x-2">
								<ScrollText className="w-4 h-4 text-orange-400" />
								<span>Agent Meta Prompt</span>
							</h3>
							<button
								onClick={handleExpandPrompt}
								className="p-2 rounded-lg bg-white/5 hover:bg-orange-500/20 border border-white/10 hover:border-orange-500/30 transition-all duration-200 text-white/70 hover:text-orange-400"
								title="Expand prompt"
							>
								<Maximize2 className="w-4 h-4" />
							</button>
						</div>
						<div className="bg-black/20 rounded-lg p-3 border border-white/5 max-h-40 overflow-y-auto">
							<p className="text-white/70 text-sm whitespace-pre-wrap break-words">
								{editedMetaPrompt || "N/A"}
							</p>
						</div>
					</div>

					{/* Expanded Metaprompt Modal */}
					{expandedPromptModal && (
						<ExpandMetapromptEditorComponent
							expandedPromptModal={expandedPromptModal}
							handleCollapsePrompt={handleCollapsePrompt}
							agentMetaprompt={editedMetaPrompt}
							handleInputChange={handleInputChange}
							createdBy={
								ConversationAgentDetailsStoreData?.createdBy ||
								""
							}
							isNewAgent={false}
						/>
					)}

					{/* Agent Details */}
					<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
						<h3 className="text-white/80 font-medium mb-3 flex items-center space-x-2">
							<Shield className="w-4 h-4 text-yellow-400" />
							<span>Agent Details</span>
						</h3>
						<div className="space-y-3 text-sm">
							<div className="flex items-start space-x-2">
								<Settings className="w-4 h-4 text-purple-400 mt-0.5 flex-shrink-0" />
								<div className="flex-1">
									<span className="text-white/80 font-medium">
										Agent ID:
									</span>
									<p className="text-white/60 break-all">
										{ConversationAgentDetailsStoreData?.agentId ||
											"N/A"}
									</p>
								</div>
							</div>
							<div className="flex items-start space-x-2">
								<User className="w-4 h-4 text-blue-400 mt-0.5 flex-shrink-0" />
								<div className="flex-1">
									<span className="text-white/80 font-medium">
										Created By:
									</span>
									<p className="text-white/60 break-all">
										{ConversationAgentDetailsStoreData?.createdBy ||
											"N/A"}
									</p>
								</div>
							</div>
							<div className="flex items-start space-x-2">
								<Calendar className="w-4 h-4 text-green-400 mt-0.5 flex-shrink-0" />
								<div className="flex-1">
									<span className="text-white/80 font-medium">
										Date Created:
									</span>
									<p className="text-white/60">
										{ConversationAgentDetailsStoreData?.dateCreated
											? formatDate(
													ConversationAgentDetailsStoreData.dateCreated
											  )
											: "N/A"}
									</p>
								</div>
							</div>
						</div>
					</div>
				</div>

				{/* Footer - Save Button */}
				<div className="border-t border-white/10 p-2 flex-shrink-0">
					<div className="flex items-center justify-start space-x-3">
						<div className="relative">
							<Button
								onPress={handleSave}
								disabled={!(isOwner && hasChanges)}
								radius="full"
								className="group relative bg-gradient-to-r from-emerald-400 via-green-500 to-teal-500 text-white font-semibold hover:from-emerald-500 hover:via-green-600 hover:to-teal-600 shadow-lg hover:shadow-green-500/50 transition-all duration-300 overflow-hidden whitespace-nowrap disabled:opacity-50"
							>
								<div className="flex items-center">
									<Save className="w-4 h-4 flex-shrink-0" />
									<span className="max-w-0 overflow-hidden group-hover:max-w-[100px] transition-all duration-300 ease-in-out ml-0 group-hover:ml-2">
										Save
									</span>
								</div>
							</Button>
						</div>
					</div>
				</div>
			</div>
		)
	);
}
