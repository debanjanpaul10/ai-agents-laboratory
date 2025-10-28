import { ChangeEvent, useEffect, useState } from "react";
import { Button, Input } from "@heroui/react";
import { X, Bot, Sparkles, ScrollText, Expand, Files } from "lucide-react";

import { useAppDispatch, useAppSelector } from "@store/index";
import { ToggleNewAgentDrawer } from "@store/common/actions";
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

export default function CreateAgentComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const [isDrawerOpen, setIsDrawerOpen] = useState(false);
	const [expandedPromptModal, setExpandedPromptModal] =
		useState<boolean>(false);
	const [formData, setFormData] = useState<CreateAgentDTO>({
		agentName: "",
		agentMetaPrompt: "",
		applicationName: "",
		knowledgeBaseDocument: null,
		isPrivate: false,
	});

	const IsDrawerOpenStoreData = useAppSelector(
		(state) => state.CommonReducer.isNewAgentDrawerOpen
	);
	const IsCreateAgentLoading = useAppSelector(
		(state) => state.AgentsReducer.isAgentCreateSpinnerLoading
	);

	useEffect(() => {
		if (isDrawerOpen !== IsDrawerOpenStoreData) {
			setIsDrawerOpen(IsDrawerOpenStoreData);
		}
	}, [IsDrawerOpenStoreData]);

	// Disable background scrolling when drawer is open
	useEffect(() => {
		if (isDrawerOpen) {
			document.body.style.overflow = "hidden";
		} else {
			document.body.style.overflow = "unset";
		}

		// Cleanup on unmount
		return () => {
			document.body.style.overflow = "unset";
		};
	}, [isDrawerOpen]);

	const onClose = () => {
		dispatch(ToggleNewAgentDrawer(false));
	};

	const handleSubmit = async () => {
		const form = new FormData();
		form.append("agentMetaPrompt", formData.agentMetaPrompt);
		form.append("agentName", formData.agentName);
		form.append("applicationName", formData.applicationName);
		if (formData.knowledgeBaseDocument) {
			form.append(
				"knowledgeBaseDocument",
				formData.knowledgeBaseDocument
			);
		}

		const token = await authContext.getAccessToken();
		token && dispatch(CreateNewAgentAsync(form, token));
		dispatch(ToggleNewAgentDrawer(false));
		setFormData({
			agentName: "",
			agentMetaPrompt: "",
			applicationName: "",
			knowledgeBaseDocument: null,
			isPrivate: false,
		});
	};

	const handleInputChange = (field: string, value: string | File | null) => {
		setFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleCollapsePrompt = () => {
		setExpandedPromptModal(false);
	};

	const handleExpandPrompt = () => {
		setExpandedPromptModal(true);
	};

	const handleFileUpload = (e: ChangeEvent<HTMLInputElement>) => {
		const fileInput = e.target as HTMLInputElement;
		const file = fileInput.files?.[0] || null;

		if (file && file.size > 10 * 1024 * 1024) {
			alert("File size cannot exceed 10MB.");
			fileInput.value = "";
			handleInputChange("knowledgeBaseDocument", null);
			return;
		}

		handleInputChange("knowledgeBaseDocument", file);
	};

	return (
		isDrawerOpen &&
		(IsCreateAgentLoading ? (
			<FullScreenLoading
				isLoading={IsCreateAgentLoading}
				message={DashboardConstants.LoadingConstants.SaveNewAgentLoader}
			/>
		) : (
			<>
				{/* Backdrop overlay */}
				<div
					className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300"
					onClick={onClose}
				/>

				{/* Drawer */}
				<div className="fixed right-0 top-0 h-screen z-50 transition-all duration-500 ease-in-out w-full max-w-md">
					{/* Glow effect */}
					<div className="absolute -inset-1 bg-gradient-to-l from-purple-600/20 via-blue-600/20 to-cyan-600/20 blur-lg opacity-75"></div>

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
							<div className="space-y-2">
								<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
									<Files className="w-4 h-4 text-pink-400" />
									<span>Agent Knowledge Base</span>
								</label>
								<div className="relative group">
									<div className="absolute -inset-0.5 bg-gradient-to-r from-green-500/20 to-emerald-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
									<Input
										onChange={(e) => {
											handleFileUpload(e);
										}}
										type="file"
										accept=".txt,.pdf,.doc,.docx"
										className="relative"
										radius="full"
										classNames={{
											input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 py-2 cursor-pointer file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-green-500/20 file:text-green-400 hover:file:bg-green-500/30",
											inputWrapper:
												"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-green-500/50 min-h-[48px] cursor-pointer",
										}}
									/>
								</div>
								<p className="text-white/40 text-xs">
									{
										ManageAgentConstants
											.ModifyAgentConstants.KBInfo
									}
								</p>
							</div>
						</div>

						{/* Footer with buttons */}
						<div className="border-t border-white/10 p-6 flex-shrink-0">
							<div className="flex items-center justify-start space-x-3">
								<div className="relative group">
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
								</div>
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
				</div>
			</>
		))
	);
}
