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
	WandSparkles,
	Terminal,
} from "lucide-react";

import { useAppDispatch, useAppSelector } from "@store/index";
import { CreateAgentDTO } from "@models/request/create-agent-dto";
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
	selectedSkillGuids,
	onOpenAssociateSkills,
	onClearSkillGuids,
}: CreateAgentFlyoutProps) {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const [expandedPromptModal, setExpandedPromptModal] =
		useState<boolean>(false);
	const [formData, setFormData] = useState<CreateAgentDTO>({
		agentName: "",
		agentMetaPrompt: "",
		agentDescription: "",
		applicationName: "",
		knowledgeBaseDocument: null,
		isPrivate: false,
		visionImages: null,
		associatedSkillGuids: [],
	});

	const IsCreateAgentLoading = useAppSelector(
		(state) => state.AgentsReducer.isAgentCreateSpinnerLoading,
	);
	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations,
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
	}, [isOpen, selectedSkillGuids]);

	useEffect(() => {
		setFormData((prev) => ({
			...prev,
			associatedSkillGuids: selectedSkillGuids,
		}));
	}, [selectedSkillGuids]);

	const handleClearData = () => {
		setFormData({
			agentName: "",
			agentMetaPrompt: "",
			agentDescription: "",
			applicationName: "",
			knowledgeBaseDocument: null,
			isPrivate: false,
			visionImages: null,
			associatedSkillGuids: [],
		});
		onClearKnowledgeFiles();
		onClearAiVisionImages();
		onClearSkillGuids();
	};

	async function handleSubmit() {
		const form = new FormData();
		form.append("agentMetaPrompt", formData.agentMetaPrompt);
		form.append("agentName", formData.agentName);
		form.append("agentDescription", formData.agentDescription);
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

		if (formData.associatedSkillGuids.length > 0) {
			formData.associatedSkillGuids.forEach((guid) => {
				form.append("associatedSkillGuids", guid);
			});
		}

		const token = await authContext.getAccessToken();
		token && dispatch(CreateNewAgentAsync(form, token));
		handleClearData();
	}

	const handleInputChange = (
		field: string,
		value: string | File | boolean | null,
	) => {
		setFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleCollapsePrompt = () => {
		setExpandedPromptModal(false);
	};

	const handleExpandPrompt = () => {
		setExpandedPromptModal(true);
	};

	const isFormDirty =
		formData.agentName.trim() !== "" ||
		formData.agentMetaPrompt.trim() !== "" ||
		formData.agentDescription.trim() !== "" ||
		formData.applicationName.trim() !== "" ||
		formData.isPrivate !== false ||
		selectedSkillGuids.length > 0 ||
		selectedKnowledgeFiles.length > 0 ||
		selectedAiVisionImages.length > 0;

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

	const renderAssociateToolSkillsData = () => {
		return (
			<div className="space-y-2">
				<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
					<Terminal className="w-4 h-4 text-cyan-400" />
					<span>Associate Tool Skills</span>
				</label>
				<div className="relative group">
					<div className="absolute -inset-0.5 bg-gradient-to-r from-cyan-500/20 to-blue-500/20 rounded-xl blur opacity-50 group-hover:opacity-75 transition duration-300"></div>
					<button
						onClick={onOpenAssociateSkills}
						className="relative w-full bg-white/5 border border-white/10 hover:border-white/20 hover:border-cyan-500/50 rounded-xl p-4 text-left transition-all duration-200 min-h-[48px] flex items-center justify-between"
					>
						<div className="flex items-center space-x-3">
							<Terminal className="w-5 h-5 text-cyan-400" />
							<div>
								<span className="text-white font-medium">
									{selectedSkillGuids.length > 0
										? `${selectedSkillGuids.length} skill${
												selectedSkillGuids.length !== 1
													? "s"
													: ""
											} associated`
										: "Associate skills"}
								</span>
								{selectedSkillGuids.length > 0 && (
									<p className="text-white/60 text-sm">
										Click to manage skills
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
				<p className="text-white/40 text-[10px] ml-1 pt-1 italic">
					Empower your agent with specific tool capabilities.
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
				<div className="flex items-center justify-between p-8 border-b border-white/5 flex-shrink-0 bg-white/[0.02] backdrop-blur-md">
					<div className="flex items-center space-x-4">
						<div className="bg-gradient-to-br from-indigo-500 via-purple-500 to-pink-500 p-3 rounded-2xl shadow-lg shadow-purple-500/20 ring-1 ring-white/20">
							<Bot className="w-6 h-6 text-white" />
						</div>
						<div>
							<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-indigo-100 to-purple-100 bg-clip-text text-transparent tracking-tight">
								{CreateAgentConstants.Headers.SubText}
							</h2>
							<p className="text-white/40 text-sm font-medium">
								Configure and deploy a new autonomous AI agent.
							</p>
						</div>
					</div>
					<button
						onClick={onClose}
						className="p-2.5 rounded-xl bg-white/5 hover:bg-red-500/10 border border-white/10 hover:border-red-500/20 transition-all duration-300 text-white/50 hover:text-red-400 group"
					>
						<X className="w-5 h-5 group-hover:rotate-90 transition-transform duration-300" />
					</button>
				</div>

				{/* Form content */}
				<div className="flex-1 overflow-y-auto p-8 space-y-8 min-h-0 scrollbar-thin scrollbar-thumb-white/10">
					<div className="space-y-6">
						<div className="flex items-center space-x-2 pb-2 border-b border-white/5">
							<Sparkles className="w-4 h-4 text-purple-400" />
							<span className="text-white/60 text-xs font-bold uppercase tracking-wider">
								Basic Configuration
							</span>
						</div>

						{/* Agent Name Field */}
						<div className="space-y-2">
							<label className="text-white/80 font-semibold text-sm ml-1">
								Agent Name
							</label>
							<Input
								value={formData.agentName}
								onChange={(e) =>
									handleInputChange(
										"agentName",
										e.target.value,
									)
								}
								placeholder={
									CreateAgentConstants.InputFields
										.AgentNamePlaceholder
								}
								variant="bordered"
								size="lg"
								startContent={
									<Sparkles className="w-5 h-5 text-purple-400 mr-2" />
								}
								classNames={{
									input: "text-white placeholder:text-white/20 p-3",
									inputWrapper:
										"bg-white/5 border-white/10 hover:border-purple-500/30 focus-within:!border-purple-500/50 transition-all min-h-[56px] rounded-2xl",
								}}
							/>
						</div>

						{/* Application Name Field */}
						<div className="space-y-2">
							<label className="text-white/80 font-semibold text-sm ml-1">
								Application Name
							</label>
							<Input
								value={formData.applicationName}
								onChange={(e) =>
									handleInputChange(
										"applicationName",
										e.target.value,
									)
								}
								placeholder={
									CreateAgentConstants.InputFields
										.ApplicationNamePlaceholder
								}
								variant="bordered"
								size="lg"
								startContent={
									<Bot className="w-5 h-5 text-blue-400 mr-2" />
								}
								classNames={{
									input: "text-white placeholder:text-white/20 p-3",
									inputWrapper:
										"bg-white/5 border-white/10 hover:border-blue-500/30 focus-within:!border-blue-500/50 transition-all min-h-[56px] rounded-2xl",
								}}
							/>
						</div>
					</div>

					<div className="space-y-6">
						<div className="flex items-center space-x-2 pb-2 border-b border-white/5">
							<WandSparkles className="w-4 h-4 text-orange-400" />
							<span className="text-white/60 text-xs font-bold uppercase tracking-wider">
								Agent Personality
							</span>
						</div>

						{/* Agent Description Field */}
						<div className="space-y-2">
							<label className="text-white/80 font-semibold text-sm ml-1">
								Agent Description
							</label>
							<div className="relative group">
								<textarea
									value={formData.agentDescription}
									onChange={(e) =>
										handleInputChange(
											"agentDescription",
											e.target.value,
										)
									}
									rows={4}
									placeholder={
										CreateAgentConstants.InputFields
											.AgentDescriptionPlaceholder
									}
									className="w-full bg-white/5 border border-white/10 text-white placeholder:text-white/20 resize-none px-4 py-3 rounded-2xl hover:border-orange-500/30 focus:border-orange-500/50 focus:outline-none transition-all duration-200"
								/>
							</div>
						</div>

						{/* Meta Prompt Field */}
						<div className="space-y-2">
							<label className="text-white/80 font-semibold text-sm ml-1 flex items-center justify-between">
								<span>Agent Meta Prompt</span>
								<span className="text-[10px] text-white/30 uppercase tracking-widest bg-white/5 px-2 py-0.5 rounded-full">
									System Instructions
								</span>
							</label>
							<div className="relative group">
								<textarea
									value={formData.agentMetaPrompt}
									onChange={(e) =>
										handleInputChange(
											"agentMetaPrompt",
											e.target.value,
										)
									}
									placeholder={
										CreateAgentConstants.InputFields
											.AgentMetaPromptPlaceholder
									}
									rows={8}
									className="w-full bg-white/5 border border-white/10 text-white placeholder:text-white/20 resize-none px-4 py-3 pr-14 rounded-2xl hover:border-green-500/30 focus:border-green-500/50 focus:outline-none transition-all duration-200 font-mono text-sm leading-relaxed"
								/>

								{/* Expand Button */}
								<button
									onClick={handleExpandPrompt}
									className="absolute top-3 right-3 p-2.5 rounded-xl bg-gray-900/80 hover:bg-green-500/20 border border-white/10 hover:border-green-500/30 transition-all duration-300 text-white/40 hover:text-green-400 z-20 backdrop-blur-sm group"
									title="Expand prompt editor"
								>
									<Expand className="w-4 h-4 group-hover:scale-110 transition-transform" />
								</button>
							</div>
						</div>
					</div>

					<div className="space-y-6">
						<div className="flex items-center space-x-2 pb-2 border-b border-white/5">
							<GlobeLock className="w-4 h-4 text-pink-400" />
							<span className="text-white/60 text-xs font-bold uppercase tracking-wider">
								Capabilities & Security
							</span>
						</div>

						{/* Is Private Field */}
						<div className="bg-white/5 border border-white/10 rounded-2xl p-4 transition-all hover:bg-white/[0.07]">
							<div className="flex items-center justify-between">
								<div className="flex items-center space-x-3">
									<div className="bg-pink-500/20 p-2 rounded-lg">
										<GlobeLock className="w-4 h-4 text-pink-400" />
									</div>
									<div>
										<span className="text-white/90 font-semibold">
											Private Agent
										</span>
										<p className="text-white/40 text-xs mt-0.5">
											{
												ManageAgentConstants
													.ModifyAgentConstants
													.PrivateField
											}
										</p>
									</div>
								</div>
								<label className="relative inline-flex items-center cursor-pointer">
									<input
										type="checkbox"
										checked={formData.isPrivate}
										onChange={(e) =>
											handleInputChange(
												"isPrivate",
												e.target.checked,
											)
										}
										className="sr-only peer"
									/>
									<div className="relative w-12 h-6 bg-white/10 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-6 peer-checked:after:border-white after:content-[''] after:absolute after:top-[4px] after:left-[4px] after:bg-white/50 after:rounded-full after:h-4 after:w-4 after:transition-all peer-checked:bg-pink-500 peer-checked:after:bg-white"></div>
								</label>
							</div>
						</div>

						{/* Knowledge Base */}
						{ConfigurationsStoreData.IsKnowledgeBaseServiceEnabled ===
							"true" && renderAgentKnowledgeBaseData()}

						{/* AI Vision Images */}
						{ConfigurationsStoreData.IsAiVisionServiceEnabled ===
							"true" && renderAiVisionUploadData()}

						{/* Associate Tool Skills */}
						{renderAssociateToolSkillsData()}
					</div>
				</div>

				{/* Footer with buttons */}
				<div className="border-t border-white/5 p-8 flex-shrink-0 bg-black/40 backdrop-blur-xl">
					<div className="flex items-center space-x-4">
						<Button
							onPress={handleSubmit}
							title="Create AI Agent"
							className="px-6 h-14 bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-600 text-white font-bold text-lg hover:shadow-2xl hover:shadow-purple-500/30 transition-all duration-300 rounded-2xl group border border-white/10 disabled:opacity-50 disabled:cursor-not-allowed"
							disabled={
								!formData.agentName.trim() ||
								!formData.agentMetaPrompt.trim() ||
								!formData.applicationName.trim() ||
								!isFormDirty ||
								IsCreateAgentLoading
							}
						>
							<Sparkles className="w-5 h-5 mr-2 group-hover:scale-110 transition-transform" />
							<span>Deploy Agent</span>
						</Button>
						<Button
							onPress={handleClearData}
							title="Clear form data"
							disabled={!isFormDirty || IsCreateAgentLoading}
							className="h-14 px-6 bg-white/5 text-white/50 font-semibold hover:bg-white/10 transition-all duration-300 rounded-2xl border border-white/10 hover:text-white flex items-center space-x-2 disabled:opacity-50 disabled:cursor-not-allowed"
						>
							<Terminal className="w-4 h-4" />
							<span>Reset</span>
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
