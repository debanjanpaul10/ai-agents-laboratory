import { useEffect, useState } from "react";
import { Button, Input } from "@heroui/react";
import {
	X,
	Bot,
	Sparkles,
	ScrollText,
	Expand,
	Files,
	Link,
	GlobeLock,
	Images,
	ScanEye,
} from "lucide-react";

import { useAppDispatch, useAppSelector } from "@store/index";
import { CreateAgentDTO } from "@models/create-agent-dto";
import {
	CreateAgentConstants,
	DashboardConstants,
	ManageAgentConstants,
} from "@helpers/constants";
import { CreateNewAgentAsync } from "@store/agents/actions";
import { useAuth } from "@auth/AuthProvider";
import { FullScreenLoading } from "@components/common/spinner";
import ExpandMetapromptEditorComponent from "@components/common/expand-metaprompt-editor";
import { CreateAgentFlyoutProps } from "@shared/types";

export default function CreateAgentFlyoutComponent({
	isOpen,
	onClose,
	selectedKnowledgeFiles,
	onOpenKnowledgeBase,
	onClearKnowledgeFiles,
	selectedAiVisionImages,
	onOpenAiVisionFlyout,
	onClearAiVisionImages,
}: CreateAgentFlyoutProps) {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const [expandedPromptModal, setExpandedPromptModal] =
		useState<boolean>(false);
	const [formData, setFormData] = useState<CreateAgentDTO>({
		agentName: "",
		agentMetaPrompt: "",
		applicationName: "",
		knowledgeBaseDocument: null,
		isPrivate: false,
		mcpServerUrl: "",
		visionImages: null,
	});

	const IsCreateAgentLoading = useAppSelector(
		(state) => state.AgentsReducer.isAgentCreateSpinnerLoading
	);
	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations
	);

	// Disable background scrolling when drawer is open
	useEffect(() => {
		if (isOpen) {
			document.body.style.overflow = "hidden";
		} else {
			document.body.style.overflow = "unset";
		}

		// Cleanup on unmount
		return () => {
			document.body.style.overflow = "unset";
		};
	}, [isOpen]);

	const handleClearData = () => {
		setFormData({
			agentName: "",
			agentMetaPrompt: "",
			applicationName: "",
			knowledgeBaseDocument: null,
			isPrivate: false,
			mcpServerUrl: "",
			visionImages: null,
		});
		onClearKnowledgeFiles();
		onClearAiVisionImages();
	};

	async function handleSubmit() {
		const form = new FormData();
		form.append("agentMetaPrompt", formData.agentMetaPrompt);
		form.append("agentName", formData.agentName);
		form.append("applicationName", formData.applicationName);
		form.append("isPrivate", formData.isPrivate.toString());

		// Handle multiple knowledge base files
		if (selectedKnowledgeFiles.length > 0) {
			selectedKnowledgeFiles.forEach((file) => {
				form.append("knowledgeBaseDocument", file);
			});
		}

		if (selectedAiVisionImages.length > 0)
			selectedAiVisionImages.forEach((file) => {
				form.append("visionImages", file);
			});

		if (formData.mcpServerUrl)
			form.append("mcpServerUrl", formData.mcpServerUrl);

		const token = await authContext.getAccessToken();
		token && dispatch(CreateNewAgentAsync(form, token));
		handleClearData();
	}

	const handleInputChange = (
		field: string,
		value: string | File | boolean | null
	) => {
		setFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleCollapsePrompt = () => {
		setExpandedPromptModal(false);
	};

	const handleExpandPrompt = () => {
		setExpandedPromptModal(true);
	};

	const renderAgentKnowledgeBaseData = () => {
		return (
			<div className="space-y-2">
				<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
					<Files className="w-4 h-4 text-yellow-400" />
					<span>Agent Knowledge Base</span>
				</label>
				<div className="relative group">
					<div className="absolute -inset-0.5 bg-gradient-to-r from-yellow-500/20 to-orange-500/20 rounded-xl blur opacity-50 group-hover:opacity-75 transition duration-300"></div>
					<button
						onClick={onOpenKnowledgeBase}
						className="relative w-full bg-white/5 border border-white/10 hover:border-white/20 hover:border-yellow-500/50 rounded-xl p-4 text-left transition-all duration-200 min-h-[48px] flex items-center justify-between"
					>
						<div className="flex items-center space-x-3">
							<Files className="w-5 h-5 text-yellow-400" />
							<div>
								<span className="text-white font-medium">
									{selectedKnowledgeFiles.length > 0
										? `${
												selectedKnowledgeFiles.length
										  } file${
												selectedKnowledgeFiles.length !==
												1
													? "s"
													: ""
										  } selected`
										: "Choose files"}
								</span>
								{selectedKnowledgeFiles.length > 0 && (
									<p className="text-white/60 text-sm">
										Click to manage files
									</p>
								)}
							</div>
						</div>
						<div className="text-white/40">
							<svg
								className="w-5 h-5"
								fill="none"
								stroke="currentColor"
								viewBox="0 0 24 24"
							>
								<path
									strokeLinecap="round"
									strokeLinejoin="round"
									strokeWidth={2}
									d="M9 5l7 7-7 7"
								/>
							</svg>
						</div>
					</button>
				</div>
				<p className="text-white/40 text-xs">
					{ManageAgentConstants.ModifyAgentConstants.KBInfo}
				</p>
			</div>
		);
	};

	const renderAiVisionUploadData = () => {
		return (
			<div className="space-y-2">
				<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
					<ScanEye className="w-4 h-4 text-red-400" />
					<span>AI Vision Images</span>
				</label>
				<div className="relative group">
					<div className="absolute -inset-0.5 bg-gradient-to-r from-red-500/20 to-pink-500/20 rounded-xl blur opacity-50 group-hover:opacity-75 transition duration-300"></div>
					<button
						onClick={onOpenAiVisionFlyout}
						className="relative w-full bg-white/5 border border-white/10 hover:border-white/20 hover:border-red-500/50 rounded-xl p-4 text-left transition-all duration-200 min-h-[48px] flex items-center justify-between"
					>
						<div className="flex items-center space-x-3">
							<Images className="w-5 h-5 text-red-400" />
							<div>
								<span className="text-white font-medium">
									{selectedAiVisionImages.length > 0
										? `${
												selectedAiVisionImages.length
										  } file${
												selectedAiVisionImages.length !==
												1
													? "s"
													: ""
										  } selected`
										: "Choose images"}
								</span>
								{selectedAiVisionImages.length > 0 && (
									<p className="text-white/60 text-sm">
										Click to manage images
									</p>
								)}
							</div>
						</div>
						<div className="text-white/40">
							<svg
								className="w-5 h-5"
								fill="none"
								stroke="currentColor"
								viewBox="0 0 24 24"
							>
								<path
									strokeLinecap="round"
									strokeLinejoin="round"
									strokeWidth={2}
									d="M9 5l7 7-7 7"
								/>
							</svg>
						</div>
					</button>
				</div>
				<p className="text-white/40 text-xs">
					{ManageAgentConstants.ModifyAgentConstants.VisionInfo}
				</p>
			</div>
		);
	};

	if (!isOpen) return null;

	return IsCreateAgentLoading ? (
		<FullScreenLoading
			isLoading={IsCreateAgentLoading}
			message={DashboardConstants.LoadingConstants.SaveNewAgentLoader}
		/>
	) : (
		<>
			{/* Main drawer content */}
			<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
				{/* Header */}
				<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
					<div className="flex items-center space-x-3">
						<div className="bg-gradient-to-r from-purple-500 to-blue-600 p-2 rounded-xl">
							<Bot className="w-5 h-5 text-white" />
						</div>
						<div>
							<h2 className="text-xl font-bold bg-gradient-to-r from-white via-blue-100 to-purple-100 bg-clip-text text-transparent">
								{CreateAgentConstants.Headers.SubText}
							</h2>
						</div>
					</div>
					<div className="flex items-center space-x-2">
						<Button
							onPress={onClose}
							title="Close window"
							className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
						>
							<X className="w-4 h-4" />
						</Button>
					</div>
				</div>

				{/* Form content */}
				<div className="flex-1 overflow-y-auto p-6 space-y-6 min-h-0">
					{/* Agent Name Field */}
					<div className="space-y-2">
						<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
							<Sparkles className="w-4 h-4 text-purple-400" />
							<span>Agent Name</span>
						</label>
						<div className="relative group">
							<div className="absolute -inset-0.5 bg-gradient-to-r from-purple-500/20 to-blue-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
							<Input
								value={formData.agentName}
								onChange={(e) =>
									handleInputChange(
										"agentName",
										e.target.value
									)
								}
								placeholder={
									CreateAgentConstants.InputFields
										.AgentNamePlaceholder
								}
								className="relative"
								radius="full"
								classNames={{
									input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
									inputWrapper:
										"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-purple-500/50 min-h-[48px]",
								}}
							/>
						</div>
					</div>

					{/* Application Name Field */}
					<div className="space-y-2">
						<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
							<Bot className="w-4 h-4 text-blue-400" />
							<span>Application Name</span>
						</label>
						<div className="relative group">
							<div className="absolute -inset-0.5 bg-gradient-to-r from-blue-500/20 to-cyan-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
							<Input
								value={formData.applicationName}
								onChange={(e) =>
									handleInputChange(
										"applicationName",
										e.target.value
									)
								}
								placeholder={
									CreateAgentConstants.InputFields
										.ApplicationNamePlaceholder
								}
								className="relative"
								radius="full"
								classNames={{
									input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
									inputWrapper:
										"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-purple-500/50 min-h-[48px]",
								}}
							/>
						</div>
					</div>

					{/* Is Private Field */}
					<div className="py-2">
						<div className="flex items-center justify-between py-2">
							<div className="flex items-center space-x-2">
								<GlobeLock className="w-4 h-4 text-pink-400" />
								<span className="text-white/80 text-sm font-medium">
									Private Agent
								</span>
							</div>
							<label className="relative inline-flex items-center cursor-pointer">
								<input
									type="checkbox"
									checked={formData.isPrivate}
									onChange={(e) =>
										handleInputChange(
											"isPrivate",
											e.target.checked
										)
									}
									className="sr-only peer"
								/>
								<div className="relative w-11 h-6 bg-white/10 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-amber-500"></div>
							</label>
						</div>
						<p className="text-white/40 text-xs">
							{
								ManageAgentConstants.ModifyAgentConstants
									.PrivateField
							}
						</p>
					</div>

					{/* Meta Prompt Field */}
					<div className="space-y-2">
						<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
							<ScrollText className="w-4 h-4 text-green-400" />
							<span>Agent Meta Prompt</span>
						</label>
						<div className="relative">
							<textarea
								value={formData.agentMetaPrompt}
								onChange={(e) =>
									handleInputChange(
										"agentMetaPrompt",
										e.target.value
									)
								}
								placeholder={
									CreateAgentConstants.InputFields
										.AgentMetaPromptPlaceholder
								}
								rows={6}
								className="w-full bg-white/5 border border-white/10 text-white placeholder:text-white/40 resize-none px-4 py-3 pr-12 rounded-xl hover:border-white/20 focus:border-orange-500/50 focus:outline-none transition-colors duration-200"
							/>

							{/* Expand Button */}
							<button
								onClick={handleExpandPrompt}
								className="absolute top-2 right-2 p-2 rounded-lg bg-gray-800/90 hover:bg-orange-500/80 border border-orange-500/50 hover:border-orange-400 transition-all duration-200 text-orange-400 hover:text-white z-20 shadow-lg hover:shadow-orange-500/25 backdrop-blur-sm disabled:opacity-50 disabled:cursor-not-allowed"
								title="Expand prompt editor"
							>
								<Expand className="w-4 h-4" />
							</button>
						</div>
					</div>

					{/* Knowledge Base */}
					{ConfigurationsStoreData.IsKnowledgeBaseServiceEnabled ===
						"true" && renderAgentKnowledgeBaseData()}

					{/* AI Vision Images */}
					{ConfigurationsStoreData.IsAiVisionServiceEnabled ===
						"true" && renderAiVisionUploadData()}

					{/* MCP Server URL */}
					<div className="space-y-2">
						<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
							<Link className="w-4 h-4 text-green-400" />
							<span>MCP Server URL</span>
						</label>
						<div className="relative">
							<Input
								value={formData.mcpServerUrl}
								onChange={(e) =>
									handleInputChange(
										"mcpServerUrl",
										e.target.value
									)
								}
								placeholder={
									CreateAgentConstants.InputFields
										.McpServerURL
								}
								className="relative"
								radius="full"
								classNames={{
									input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
									inputWrapper:
										"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-purple-500/50 min-h-[48px]",
								}}
							/>
						</div>
						<p className="text-white/40 text-xs">
							{ManageAgentConstants.ModifyAgentConstants.MCPUrl}
						</p>
					</div>
				</div>

				{/* Footer with buttons */}
				<div className="border-t border-white/10 p-6 flex-shrink-0">
					<div className="flex items-center justify-start space-x-4">
						<Button
							onPress={handleSubmit}
							title="Create AI Agent"
							className="flex items-center bg-gradient-to-r from-purple-500 to-blue-600 text-white font-semibold hover:from-purple-600 hover:to-blue-700 transition-all duration-300 px-6 py-3 min-h-[44px]"
							radius="full"
							disabled={
								!formData.agentName.trim() ||
								!formData.agentMetaPrompt.trim() ||
								!formData.applicationName.trim()
							}
						>
							<Sparkles className="w-4 h-4" /> &nbsp;
							<span>Create Agent</span>
						</Button>
						<Button
							onPress={handleClearData}
							title="Clear data"
							className="flex items-center bg-gradient-to-r from-red-500 to-red-600 text-white font-semibold hover:from-red-600 hover:to-red-700 transition-all duration-300 px-6 py-3 min-h-[44px]"
							radius="full"
							disabled={
								!formData.agentName.trim() ||
								!formData.agentMetaPrompt.trim() ||
								!formData.applicationName.trim()
							}
						>
							<span>Clear</span>
						</Button>
					</div>
				</div>

				<ExpandMetapromptEditorComponent
					agentMetaprompt={formData.agentMetaPrompt}
					expandedPromptModal={expandedPromptModal}
					handleCollapsePrompt={handleCollapsePrompt}
					handleInputChange={handleInputChange}
					createdBy={""}
					isNewAgent={true}
				/>
			</div>
		</>
	);
}
