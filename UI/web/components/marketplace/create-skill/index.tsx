import { useEffect, useState } from "react";
import { Button, Input } from "@heroui/react";
import {
	Bot,
	Sparkles,
	X,
	Terminal,
	Globe,
	Type,
	Wrench,
	RotateCcw,
} from "lucide-react";

import { useAppDispatch, useAppSelector } from "@store/index";
import {
	AddNewToolSkillAsync,
	GetAllMcpToolsAvailableAsync,
	ToggleAddSkillDrawer,
} from "@store/tools-skills/actions";
import { DashboardConstants, MarketplaceConstants } from "@helpers/constants";
import { useAuth } from "@auth/AuthProvider";
import { ToolSkillDTO } from "@models/response/tool-skill-dto";
import { FullScreenLoading } from "@components/common/spinner";
import { McpServerToolRequestDTO } from "@models/request/mcp-server-tool-request-dto";

export default function CreateSkillComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const [isDrawerOpen, setIsDrawerOpen] = useState<boolean>(false);
	const [formData, setFormData] = useState<ToolSkillDTO>({
		associatedAgents: [],
		createdBy: "",
		dateCreated: new Date(),
		dateModified: new Date(),
		modifiedBy: "",
		toolSkillDisplayName: "",
		toolSkillGuid: "",
		toolSkillMcpServerUrl: "",
		toolSkillTechnicalName: "",
	});

	const IsDrawerOpenStoreData = useAppSelector(
		(state) => state.ToolSkillsReducer.isAddSkillDrawerOpen,
	);
	const IsCreateSkillLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isCreateSkillLoading,
	);
	const IsMcpToolsLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isMcpToolsLoading,
	);

	useEffect(() => {
		if (isDrawerOpen !== IsDrawerOpenStoreData) {
			setIsDrawerOpen(IsDrawerOpenStoreData);
			if (IsDrawerOpenStoreData) {
				// Reset form when opening
				setFormData({
					associatedAgents: [],
					createdBy: authContext.user?.email || "",
					dateCreated: new Date(),
					dateModified: new Date(),
					modifiedBy: authContext.user?.email || "",
					toolSkillDisplayName: "",
					toolSkillGuid: "",
					toolSkillMcpServerUrl: "",
					toolSkillTechnicalName: "",
				});
			}
		}
	}, [IsDrawerOpenStoreData, authContext.user]);

	const onCloseFlyout = () => {
		dispatch(ToggleAddSkillDrawer(false));
	};

	const handleInputChange = (field: keyof ToolSkillDTO, value: string) => {
		setFormData((prev) => ({ ...prev, [field]: value }));
	};

	function handleCreateSkill() {
		const form = new FormData();
		form.append("toolSkillDisplayName", formData.toolSkillDisplayName);
		form.append("toolSkillTechnicalName", formData.toolSkillTechnicalName);
		form.append("toolSkillMcpServerUrl", formData.toolSkillMcpServerUrl);

		dispatch(AddNewToolSkillAsync(form));
		handleClearData();
	}

	const handleClearData = () => {
		setFormData({
			associatedAgents: [],
			createdBy: authContext.user?.email || "",
			dateCreated: new Date(),
			dateModified: new Date(),
			modifiedBy: authContext.user?.email || "",
			toolSkillDisplayName: "",
			toolSkillGuid: "",
			toolSkillMcpServerUrl: "",
			toolSkillTechnicalName: "",
		});
	};

	function GetAllMcpToolsAvailable(mcpServerUrl: string) {
		const mcpServerRequest: McpServerToolRequestDTO = {
			serverUrl: mcpServerUrl,
		};
		dispatch(GetAllMcpToolsAvailableAsync(mcpServerRequest));
	}

	const isFormValid =
		formData.toolSkillDisplayName.trim() !== "" &&
		formData.toolSkillTechnicalName.trim() !== "";

	const isFormDirty =
		formData.toolSkillDisplayName.trim() !== "" ||
		formData.toolSkillTechnicalName.trim() !== "" ||
		formData.toolSkillMcpServerUrl.trim() !== "";

	return IsCreateSkillLoading ? (
		<FullScreenLoading
			isLoading={IsCreateSkillLoading}
			message={DashboardConstants.LoadingConstants.CreateNewSkill}
		/>
	) : (
		isDrawerOpen && (
			<>
				<div className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300 max-w-full" />
				<div className="fixed right-0 top-0 md:w-1/2 h-screen z-50 transition-all duration-500 ease-in-out">
					<div className="absolute inset-0 bg-gradient-to-l from-purple-600/20 via-blue-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10"></div>
					<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
						{/* Header */}
						<div className="flex items-center justify-between p-8 border-b border-white/5 flex-shrink-0 bg-white/[0.02] backdrop-blur-md">
							<div className="flex items-center space-x-4">
								<div className="bg-gradient-to-br from-indigo-500 via-purple-500 to-pink-500 p-3 rounded-2xl shadow-lg shadow-purple-500/20 ring-1 ring-white/20">
									<Bot className="w-6 h-6 text-white" />
								</div>
								<div>
									<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-indigo-100 to-purple-100 bg-clip-text text-transparent tracking-tight">
										{
											MarketplaceConstants
												.AddSkillConstants.Header
										}
									</h2>
									<p className="text-white/40 text-sm font-medium">
										{
											MarketplaceConstants
												.AddSkillConstants.SubHeader
										}
									</p>
								</div>
							</div>
							<button
								onClick={onCloseFlyout}
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
										Skill Details
									</span>
								</div>

								<div className="space-y-6">
									<div className="space-y-2">
										<label
											className="text-white/80 font-semibold text-sm ml-1"
											htmlFor="toolSkillDisplayName"
										>
											Display Name
										</label>
										<Input
											placeholder={
												MarketplaceConstants
													.AddSkillConstants
													.Placeholders.DisplayName
											}
											variant="bordered"
											size="lg"
											value={
												formData.toolSkillDisplayName
											}
											onValueChange={(v) =>
												handleInputChange(
													"toolSkillDisplayName",
													v,
												)
											}
											startContent={
												<Type className="w-5 h-5 text-indigo-400 mr-3" />
											}
											classNames={{
												input: "text-white placeholder:text-white/20 p-3",
												inputWrapper:
													"bg-white/5 border-white/10 hover:border-purple-500/30 focus-within:!border-purple-500/50 transition-all min-h-[56px] rounded-2xl",
											}}
										/>
									</div>

									<div className="space-y-2">
										<label
											className="text-white/80 font-semibold text-sm ml-1"
											htmlFor="toolSkillTechnicalName"
										>
											Technical Name
										</label>
										<Input
											placeholder={
												MarketplaceConstants
													.AddSkillConstants
													.Placeholders.TechnicalName
											}
											variant="bordered"
											size="lg"
											value={
												formData.toolSkillTechnicalName
											}
											onValueChange={(v) =>
												handleInputChange(
													"toolSkillTechnicalName",
													v,
												)
											}
											startContent={
												<Terminal className="w-5 h-5 text-blue-400 mr-3" />
											}
											classNames={{
												input: "text-white font-mono placeholder:text-white/20 p-3",
												inputWrapper:
													"bg-white/5 border-white/10 hover:border-blue-500/30 focus-within:!border-blue-500/50 transition-all min-h-[56px] rounded-2xl",
											}}
										/>
									</div>

									<div className="space-y-2">
										<label
											className="text-white/80 font-semibold text-sm ml-1"
											htmlFor="toolSkillMcpServerUrl"
										>
											MCP Server URL
										</label>
										<div className="flex gap-3 items-center">
											<Input
												placeholder={
													MarketplaceConstants
														.AddSkillConstants
														.Placeholders.McpUrl
												}
												variant="bordered"
												size="lg"
												value={
													formData.toolSkillMcpServerUrl
												}
												onValueChange={(v) =>
													handleInputChange(
														"toolSkillMcpServerUrl",
														v,
													)
												}
												startContent={
													<Globe className="w-5 h-5 text-cyan-400 mr-3" />
												}
												classNames={{
													input: "text-white placeholder:text-white/20 p-3",
													inputWrapper:
														"bg-white/5 border-white/10 hover:border-cyan-500/30 focus-within:!border-cyan-500/50 transition-all min-h-[56px] rounded-2xl",
												}}
												className="flex-1"
											/>
											<Button
												isIconOnly
												className="h-[56px] w-[56px] min-w-[56px] bg-white/5 border border-white/10 hover:border-cyan-400/50 hover:bg-cyan-400/10 text-cyan-400 transition-all duration-300 rounded-2xl group/btn disabled:opacity-50 disabled:cursor-not-allowed"
												onPress={() => {
													GetAllMcpToolsAvailable(
														formData.toolSkillMcpServerUrl,
													);
												}}
												isLoading={IsMcpToolsLoading}
												disabled={
													!formData.toolSkillMcpServerUrl
												}
												title="View Available Tools"
											>
												{!IsMcpToolsLoading && (
													<Wrench className="w-5 h-5 group-hover/btn:rotate-45 transition-transform duration-300" />
												)}
											</Button>
										</div>
									</div>
								</div>
							</div>

							<div className="relative group">
								<div className="absolute inset-0 bg-blue-500/10 blur-xl opacity-50 group-hover:opacity-100 transition duration-500"></div>
								<div className="relative bg-gradient-to-br from-blue-500/10 to-indigo-500/5 border border-blue-500/20 rounded-2xl p-5 backdrop-blur-sm">
									<div className="flex items-start space-x-3">
										<div className="bg-blue-500/20 p-2 rounded-lg">
											<Bot className="w-4 h-4 text-blue-400" />
										</div>
										<p className="text-blue-100/70 text-sm leading-relaxed">
											<span className="text-blue-400 font-bold block mb-1">
												Pro Tip:
											</span>
											{
												MarketplaceConstants
													.AddSkillConstants.Info
											}
										</p>
									</div>
								</div>
							</div>
						</div>

						{/* Footer with buttons */}
						<div className="border-t border-white/5 p-8 flex-shrink-0 bg-black/40 backdrop-blur-xl">
							<div className="flex items-center space-x-4">
								<Button
									onPress={handleCreateSkill}
									isLoading={IsCreateSkillLoading}
									disabled={
										!isFormValid ||
										!isFormDirty ||
										IsCreateSkillLoading
									}
									className="px-6 h-14 bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-600 text-white font-bold text-lg hover:shadow-2xl hover:shadow-purple-500/30 transition-all duration-300 rounded-2xl group border border-white/10 disabled:opacity-50 disabled:cursor-not-allowed"
								>
									{!IsCreateSkillLoading && (
										<Sparkles className="w-5 h-5 mr-2 group-hover:scale-110 transition-transform" />
									)}
									<span>Create New Skill</span>
								</Button>
								<Button
									onPress={handleClearData}
									title="Clear form data"
									className="h-14 px-6 bg-indigo-500/5 text-indigo-400 font-semibold hover:bg-indigo-500/10 transition-all duration-300 rounded-2xl border border-indigo-500/20 hover:text-indigo-300 flex items-center space-x-2"
									disabled={IsCreateSkillLoading}
								>
									<RotateCcw className="w-5 h-5" />
									<span>Reset</span>
								</Button>
							</div>
						</div>
					</div>
				</div>
			</>
		)
	);
}
