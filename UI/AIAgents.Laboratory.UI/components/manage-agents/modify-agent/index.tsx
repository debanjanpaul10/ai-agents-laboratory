import React, { useState } from "react";
import { Button, Input } from "@heroui/react";
import {
	Save,
	Play,
	Settings,
	User,
	Sparkles,
	Expand,
	ArrowRight,
	Trash,
	ScrollText,
	Files,
	Link,
	GlobeLock,
	Images,
	ScanEye,
} from "lucide-react";
import { useMsal } from "@azure/msal-react";

import { ModifyAgentComponentProps } from "@shared/types";
import { useAppDispatch, useAppSelector } from "@store/index";
import { useAuth } from "@auth/AuthProvider";
import {
	DeleteExistingAgentDataAsync,
	UpdateExistingAgentDataAsync,
} from "@store/agents/actions";
import { FullScreenLoading } from "@components/common/spinner";
import { DashboardConstants, ManageAgentConstants } from "@helpers/constants";
import { AgentDataDTO } from "@models/agent-data-dto";
import ExpandMetapromptEditorComponent from "@components/common/expand-metaprompt-editor";
import { ToggleAgentsListDrawer } from "@store/common/actions";

export default function ModifyAgentComponent({
	editFormData,
	selectedAgent,
	setEditFormData,
	setSelectedAgent,
	isEditDrawerOpen,
	onTestAgent,
	onEditClose,
	isDisabled,
	onOpenKnowledgeBase,
	selectedKnowledgeFiles,
	removedExistingDocuments,
	onOpenVisionImagesFlyout,
	removedExistingImages,
	selectedVisionImages,
}: ModifyAgentComponentProps) {
	const dispatch = useAppDispatch();
	const authContext = useAuth();
	const { accounts } = useMsal();

	const [expandedPromptModal, setExpandedPromptModal] =
		useState<boolean>(false);

	const IsEditAgentDataLoading = useAppSelector(
		(state) => state.AgentsReducer.isEditAgentDataLoading
	);
	const ConfigurationStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations
	);

	const handleInputChange = (
		field: string,
		value: string | File | boolean | null
	) => {
		setEditFormData((prev: any) => ({ ...prev, [field]: value }));
	};

	async function handleEditSave() {
		const form = new FormData();
		form.append("agentId", editFormData.agentId);
		form.append("agentMetaPrompt", editFormData.agentMetaPrompt);
		form.append("agentName", editFormData.agentName);
		form.append("applicationName", editFormData.applicationName);
		form.append("isPrivate", editFormData.isPrivate.toString());

		// Handle multiple knowledge base files (new uploads)
		if (selectedKnowledgeFiles.length > 0) {
			selectedKnowledgeFiles.forEach((file) => {
				form.append(`knowledgeBaseDocument`, file);
			});
		}

		// Send list of removed existing knowledge base documents (by file name)
		if (removedExistingDocuments.length > 0) {
			removedExistingDocuments.forEach((fileName) => {
				form.append("removedKnowledgeBaseDocuments", fileName);
			});
		}

		if (editFormData.mcpServerUrl)
			form.append("mcpServerUrl", editFormData.mcpServerUrl);

		const accessToken = await authContext.getAccessToken();
		accessToken &&
			dispatch(UpdateExistingAgentDataAsync(form, accessToken));
	}

	const handleEditClose = () => {
		setSelectedAgent(null);
		onEditClose();
	};

	async function handleAgentDelete() {
		const token = await authContext.getAccessToken();
		token &&
			dispatch(DeleteExistingAgentDataAsync(editFormData.agentId, token));
		dispatch(ToggleAgentsListDrawer(false));
		handleEditClose();
	}

	const handleExpandPrompt = () => {
		setExpandedPromptModal(true);
	};

	const handleCollapsePrompt = () => {
		setExpandedPromptModal(false);
	};

	const renderAgentInformationTile = (selectedAgent: AgentDataDTO) => {
		return (
			<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
				<h3 className="text-white/80 font-medium mb-2 flex items-center space-x-2">
					<Settings className="w-4 h-4 text-yellow-400" />
					<span>Agent Information</span>
				</h3>
				<div className="space-y-1 text-sm">
					<p className="text-white/60">
						<span className="text-white/80">Agent ID:</span>{" "}
						{selectedAgent.agentId || "N/A"}
					</p>
					<p className="text-white/60">
						<span className="text-white/80">Created:</span>{" "}
						{`${new Date(
							selectedAgent.dateCreated
						).toDateString()}`}
					</p>
					<p className="text-white/60">
						<span className="text-white/80">Status:</span>{" "}
						<span className="text-green-400 animate-pulse">
							Active
						</span>
					</p>
				</div>
			</div>
		);
	};

	const renderFooter = () => {
		return (
			<div className="border-t border-white/10 p-6 flex-shrink-0">
				<div className="flex items-center justify-start space-x-3">
					{/* Save Button */}
					<div className="relative">
						<Button
							onPress={handleEditSave}
							disabled={
								isDisabled ||
								accounts[0].username !== editFormData.createdBy
							}
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

					{/* Test Button */}
					<div className="relative">
						<Button
							onPress={onTestAgent}
							disabled={isDisabled}
							radius="full"
							className="group relative bg-gradient-to-r from-blue-400 via-indigo-500 to-purple-500 text-white font-semibold hover:from-blue-500 hover:via-indigo-600 hover:to-purple-600 shadow-lg hover:shadow-indigo-500/50 transition-all duration-300 overflow-hidden whitespace-nowrap disabled:opacity-50"
						>
							<div className="flex items-center">
								<Play className="w-4 h-4 flex-shrink-0" />
								<span className="max-w-0 overflow-hidden group-hover:max-w-[100px] transition-all duration-300 ease-in-out ml-0 group-hover:ml-2">
									Test
								</span>
							</div>
						</Button>
					</div>

					{/* Delete Button */}
					<div className="relative">
						<Button
							onPress={handleAgentDelete}
							disabled={
								accounts[0].username !== editFormData.createdBy
							}
							radius="full"
							className="group relative bg-gradient-to-r from-red-400 via-rose-500 to-pink-500 text-white font-semibold hover:from-red-500 hover:via-rose-600 hover:to-pink-600 shadow-lg hover:shadow-rose-500/50 transition-all duration-300 overflow-hidden whitespace-nowrap disabled:opacity-50"
						>
							<div className="flex items-center">
								<Trash className="w-4 h-4 flex-shrink-0" />
								<span className="max-w-0 overflow-hidden group-hover:max-w-[100px] transition-all duration-300 ease-in-out ml-0 group-hover:ml-2">
									Delete
								</span>
							</div>
						</Button>
					</div>
				</div>
			</div>
		);
	};

	const renderAgentKnowledgeBaseData = () => {
		// Derive counts from existing and newly selected files
		const existingDocs =
			(selectedAgent?.knowledgeBaseDocument as File[] | null) ?? [];
		const visibleExistingDocs = existingDocs.filter(
			(doc) => !removedExistingDocuments.includes(doc.name)
		);
		const existingCount = visibleExistingDocs.length;
		const totalCount = selectedKnowledgeFiles.length + existingCount;
		const hasAnyFiles = totalCount > 0;

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
									{totalCount > 0
										? `${totalCount} file${
												totalCount !== 1 ? "s" : ""
										  } selected`
										: "Choose files"}
								</span>
								{hasAnyFiles && (
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

	const renderAiVisionImagesData = () => {
		const existingImages =
			(selectedAgent?.visionImages as File[] | null) ?? [];
		const visibleExistingImages = existingImages.filter(
			(image) => !removedExistingImages.includes(image.name)
		);
		const existingCount = visibleExistingImages.length;
		const totalCount = selectedVisionImages.length + existingCount;
		const hasAnyFiles = totalCount > 0;

		return (
			<div className="space-y-2">
				<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
					<ScanEye className="w-4 h-4 text-red-400" />
					<span>AI Vision Images</span>
				</label>
				<div className="relative group">
					<div className="absolute -inset-0.5 bg-gradient-to-r from-red-500/20 to-pink-500/20 rounded-xl blur opacity-50 group-hover:opacity-75 transition duration-300"></div>
					<button
						onClick={onOpenVisionImagesFlyout}
						className="relative w-full bg-white/5 border border-white/10 hover:border-white/20 hover:border-red-500/50 rounded-xl p-4 text-left transition-all duration-200 min-h-[48px] flex items-center justify-between"
					>
						<div className="flex items-center space-x-3">
							<Images className="w-5 h-5 text-red-400" />
							<div>
								<span className="text-white font-medium">
									{totalCount > 0
										? `${totalCount} file${
												totalCount !== 1 ? "s" : ""
										  } selected`
										: "Choose images"}
								</span>
								{hasAnyFiles && (
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

	const renderEditAgentsData = () => {
		return (
			isEditDrawerOpen &&
			selectedAgent && (
				<div
					className={`h-full flex flex-col ${
						isDisabled ? "opacity-50 pointer-events-none" : ""
					}`}
				>
					{/* Header */}
					<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
						<div className="flex items-center space-x-3">
							<div className="bg-gradient-to-r from-green-500 to-blue-600 p-2 rounded-xl">
								<Settings className="w-5 h-5 text-white" />
							</div>
							<div>
								<h2 className="text-xl font-bold bg-gradient-to-r from-white via-green-100 to-blue-100 bg-clip-text text-transparent">
									{
										ManageAgentConstants
											.ModifyAgentConstants.MainHeader
									}
								</h2>
								<p className="text-white/60 text-sm">
									Edit {editFormData.agentName || "agent"}{" "}
									settings
								</p>
							</div>
						</div>
						<button
							onClick={handleEditClose}
							className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
							disabled={isDisabled}
						>
							<ArrowRight className="w-4 h-4" />
						</button>
					</div>
					{/* Form Content */}
					<div className="flex-1 overflow-y-auto p-6 space-y-6 min-h-0">
						{/* Agent Name Field */}
						<div className="space-y-2">
							<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
								<Sparkles className="w-4 h-4 text-green-400" />
								<span>Agent Name</span>
							</label>
							<Input
								value={editFormData.agentName}
								onChange={(e) =>
									handleInputChange(
										"agentName",
										e.target.value
									)
								}
								radius="full"
								placeholder={
									ManageAgentConstants.ModifyAgentConstants
										.Placeholders.AgentName
								}
								disabled={
									isDisabled ||
									accounts[0].username !==
										editFormData.createdBy
								}
								classNames={{
									input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
									inputWrapper:
										"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-green-500/50 min-h-[48px]",
								}}
							/>
						</div>

						{/* Application Name Field */}
						<div className="space-y-2">
							<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
								<Settings className="w-4 h-4 text-blue-400" />
								<span>Application Name</span>
							</label>
							<Input
								value={editFormData.applicationName}
								onChange={(e) =>
									handleInputChange(
										"applicationName",
										e.target.value
									)
								}
								radius="full"
								placeholder={
									ManageAgentConstants.ModifyAgentConstants
										.Placeholders.ApplicationName
								}
								disabled={
									isDisabled ||
									accounts[0].username !==
										editFormData.createdBy
								}
								classNames={{
									input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
									inputWrapper:
										"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-blue-500/50 min-h-[48px]",
								}}
							/>
						</div>

						{/* Author Field */}
						<div className="space-y-2">
							<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
								<User className="w-4 h-4 text-purple-400" />
								<span>Author</span>
							</label>
							<Input
								value={editFormData.createdBy}
								onChange={(e) =>
									handleInputChange(
										"createdBy",
										e.target.value
									)
								}
								radius="full"
								disabled={true}
								classNames={{
									input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
									inputWrapper:
										"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-purple-500/50 min-h-[48px]",
								}}
							/>
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
										checked={editFormData.isPrivate}
										onChange={(e) =>
											handleInputChange(
												"isPrivate",
												e.target.checked
											)
										}
										disabled={
											isDisabled ||
											accounts[0].username !==
												editFormData.createdBy
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
									value={editFormData.agentMetaPrompt}
									onChange={(e) =>
										handleInputChange(
											"agentMetaPrompt",
											e.target.value
										)
									}
									placeholder={
										ManageAgentConstants
											.ModifyAgentConstants.Placeholders
											.AgentMetaprompt
									}
									rows={6}
									disabled={
										isDisabled ||
										accounts[0].username !==
											editFormData.createdBy
									}
									className="w-full bg-white/5 border border-white/10 text-white placeholder:text-white/40 resize-none px-4 py-3 pr-12 rounded-xl hover:border-white/20 focus:border-orange-500/50 focus:outline-none transition-colors duration-200"
								/>
								{/* Expand Button */}
								<button
									onClick={handleExpandPrompt}
									disabled={isDisabled}
									className="absolute top-2 right-2 p-2 rounded-lg bg-gray-800/90 hover:bg-orange-500/80 border border-orange-500/50 hover:border-orange-400 transition-all duration-200 text-orange-400 hover:text-white z-20 shadow-lg hover:shadow-orange-500/25 backdrop-blur-sm disabled:opacity-50 disabled:cursor-not-allowed"
									title="Expand prompt editor"
								>
									<Expand className="w-4 h-4" />
								</button>
							</div>
							<p className="text-white/40 text-xs">
								{ManageAgentConstants.ModifyAgentConstants.Info}
							</p>
						</div>

						{/* Agent Knowledge Base */}
						{ConfigurationStoreData.IsKnowledgeBaseServiceEnabled ===
							"true" && renderAgentKnowledgeBaseData()}

						{/* AI VISION IMAGES */}
						{ConfigurationStoreData.IsAiVisionServiceEnabled ===
							"true" && renderAiVisionImagesData()}

						{/* MCP Server URL */}
						<div className="space-y-2">
							<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
								<Link className="w-4 h-4 text-green-400" />
								<span>MCP Server URL</span>
							</label>
							<div className="relative">
								<Input
									value={editFormData.mcpServerUrl}
									onChange={(e) =>
										handleInputChange(
											"mcpServerUrl",
											e.target.value
										)
									}
									placeholder={
										ManageAgentConstants
											.ModifyAgentConstants.Placeholders
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
								{
									ManageAgentConstants.ModifyAgentConstants
										.MCPUrl
								}
							</p>
						</div>

						{/* Agent Info Display */}
						{renderAgentInformationTile(selectedAgent)}
					</div>
					{/* Footer */}
					{renderFooter()}

					<ExpandMetapromptEditorComponent
						agentMetaprompt={editFormData.agentMetaPrompt}
						expandedPromptModal={expandedPromptModal}
						handleCollapsePrompt={handleCollapsePrompt}
						handleInputChange={handleInputChange}
						createdBy={editFormData.createdBy}
						isNewAgent={false}
					/>
				</div>
			)
		);
	};

	return IsEditAgentDataLoading ? (
		<FullScreenLoading
			isLoading={true}
			message={DashboardConstants.LoadingConstants.SaveAgentDataLoader}
		/>
	) : (
		renderEditAgentsData()
	);
}
