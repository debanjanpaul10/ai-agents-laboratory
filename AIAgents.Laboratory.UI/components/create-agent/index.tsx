"use client";

import { useEffect, useState } from "react";
import { Button, Input, Textarea } from "@heroui/react";
import { X, Maximize2, Minimize2, Bot, Sparkles } from "lucide-react";

import { useAppDispatch, useAppSelector } from "@/store";
import { ToggleNewAgentDrawer } from "@/store/common/actions";
import { CreateAgentDTO } from "@/models/create-agent-dto";
import { CreateAgentConstants } from "@/helpers/constants";
import { CreateNewAgentAsync } from "@/store/agents/actions";
import { useAuth } from "@/auth/AuthProvider";
import { FullScreenLoading } from "@components/ui/loading-spinner";

export default function CreateAgentComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const [isDrawerOpen, setIsDrawerOpen] = useState(false);
	const [isExpanded, setIsExpanded] = useState(false);
	const [formData, setFormData] = useState<CreateAgentDTO>({
		agentName: "",
		agentMetaPrompt: "",
		applicationName: "",
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
		setIsExpanded(false);
	};

	const handleSubmit = async () => {
		const newAgentData: CreateAgentDTO = {
			agentMetaPrompt: formData.agentMetaPrompt,
			agentName: formData.agentName,
			applicationName: formData.applicationName,
		};

		const token = await authContext.getAccessToken();
		token && dispatch(CreateNewAgentAsync(newAgentData, token));
		dispatch(ToggleNewAgentDrawer(false));
		setFormData({
			agentName: "",
			agentMetaPrompt: "",
			applicationName: "",
		});
	};

	const handleInputChange = (field: string, value: string) => {
		setFormData((prev) => ({ ...prev, [field]: value }));
	};

	if (!isDrawerOpen) return null;

	return IsCreateAgentLoading ? (
		<FullScreenLoading
			isLoading={IsCreateAgentLoading}
			message={"Saving new agent..."}
		/>
	) : (
		<>
			{/* Backdrop overlay */}
			<div
				className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300"
				onClick={onClose}
			/>

			{/* Drawer */}
			<div
				className={`fixed right-0 top-0 h-screen z-50 transition-all duration-500 ease-in-out ${
					isExpanded ? "w-full max-w-4xl" : "w-full max-w-md"
				}`}
			>
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
							{/* Expand/Collapse button */}
							<button
								onClick={() => setIsExpanded(!isExpanded)}
								className="p-2 rounded-lg bg-white/5 hover:bg-white/10 border border-white/10 hover:border-white/20 transition-all duration-200 text-white/70 hover:text-white"
							>
								{isExpanded ? (
									<Minimize2 className="w-4 h-4" />
								) : (
									<Maximize2 className="w-4 h-4" />
								)}
							</button>

							{/* Close button */}
							<button
								onClick={onClose}
								className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
							>
								<X className="w-4 h-4" />
							</button>
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
									classNames={{
										input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
										inputWrapper:
											"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-blue-500/50 min-h-[48px]",
									}}
								/>
							</div>
						</div>

						{/* Meta Prompt Field */}
						<div className="space-y-2">
							<label className="text-white/80 text-sm font-medium">
								Agent Meta Prompt
							</label>
							<div className="relative group">
								<div className="absolute -inset-0.5 bg-gradient-to-r from-green-500/20 to-purple-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
								<Textarea
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
									minRows={isExpanded ? 12 : 6}
									maxRows={isExpanded ? 20 : 10}
									className="relative"
									classNames={{
										input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 resize-none px-4 py-3",
										inputWrapper:
											"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-green-500/50",
									}}
								/>
							</div>
						</div>
					</div>

					{/* Footer with buttons */}
					<div className="border-t border-white/10 p-6 flex-shrink-0">
						<div className="flex items-center justify-start space-x-3">
							<div className="relative group">
								<div className="absolute -inset-0.5 bg-gradient-to-r from-purple-500 to-blue-600 rounded-xl blur opacity-75 group-hover:opacity-100 transition duration-300"></div>
								<Button
									onPress={handleSubmit}
									className="flex items-center bg-gradient-to-r from-purple-500 to-blue-600 text-white font-semibold hover:from-purple-600 hover:to-blue-700 transition-all duration-300 px-6 py-3 min-h-[44px]"
									radius="full"
									disabled={
										!formData.agentName.trim() ||
										!formData.agentMetaPrompt.trim() ||
										!formData.applicationName.trim()
									}
								>
									<Sparkles className="w-4 h-4" />
									<span>Create Agent</span>
								</Button>
							</div>
						</div>
					</div>
				</div>
			</div>
		</>
	);
}
