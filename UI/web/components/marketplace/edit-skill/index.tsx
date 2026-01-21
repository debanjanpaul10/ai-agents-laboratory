import { useState } from "react";
import { Button, Input } from "@heroui/react";
import { useMsal } from "@azure/msal-react";
import {
	Globe,
	Save,
	Settings,
	Sparkles,
	Terminal,
	Trash,
	Type,
	Wrench,
	X,
	Bot,
	ChevronRight,
	Users,
} from "lucide-react";

import { useAuth } from "@auth/AuthProvider";
import { useAppDispatch, useAppSelector } from "@store/index";
import { EditSkillFlyoutComponentProps } from "@shared/types";
import { ToolSkillDTO } from "@models/response/tool-skill-dto";
import { AssociatedAgentsSkillDataDTO } from "@models/response/associated-agents-skill-data.dto";
import {
	DeleteExistingToolSkillAsync,
	GetAllMcpToolsAvailableAsync,
	UpdateExistingToolSkillAsync,
} from "@store/tools-skills/actions";
import { FullScreenLoading } from "@components/common/spinner";
import { MarketplaceConstants } from "@helpers/constants";
import { McpServerToolRequestDTO } from "@models/request/mcp-server-tool-request-dto";
import DeletePopupComponent from "@components/common/delete-popup";

export default function EditSkillFlyoutComponent({
	editFormData,
	selectedSkill,
	setEditFormData,
	setSelectedSkill,
	isEditDrawerOpen,
	onEditClose,
	isDisabled,
}: EditSkillFlyoutComponentProps) {
	const dispatch = useAppDispatch();
	const authContext = useAuth();
	const { accounts } = useMsal();

	const [isDeletePopupOpen, setIsDeletePopupOpen] = useState(false);

	const IsEditSkillLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isEditSkillLoading,
	);
	const IsMcpToolsLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isMcpToolsLoading,
	);
	const IsToolSkillLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isToolSkillsLoading,
	);

	const handleInputChange = (field: keyof ToolSkillDTO, value: string) => {
		setEditFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleEditClose = () => {
		setSelectedSkill(null);
		onEditClose();
	};

	async function handleEditSave() {
		const form = new FormData();
		form.append("toolSkillGuid", editFormData.toolSkillGuid);
		form.append("toolSkillDisplayName", editFormData.toolSkillDisplayName);
		form.append(
			"toolSkillTechnicalName",
			editFormData.toolSkillTechnicalName,
		);
		form.append(
			"toolSkillMcpServerUrl",
			editFormData.toolSkillMcpServerUrl,
		);

		const token = await authContext.getAccessToken();
		token && dispatch(UpdateExistingToolSkillAsync(form, token));
	}

	async function HandleSkillDelete() {
		const token = await authContext.getAccessToken();
		var skillId = editFormData.toolSkillGuid;
		token && dispatch(DeleteExistingToolSkillAsync(skillId, token));
		setIsDeletePopupOpen(false);
		handleEditClose();
	}

	async function GetAllMcpToolsAvailable(mcpServerUrl: string) {
		const token = await authContext.getAccessToken();
		const mcpServerRequest: McpServerToolRequestDTO = {
			serverUrl: mcpServerUrl,
		};
		token &&
			dispatch(GetAllMcpToolsAvailableAsync(mcpServerRequest, token));
	}

	const renderSkillInformationTile = (selectedSkill: ToolSkillDTO) => {
		return (
			<div className="bg-white/5 backdrop-blur-sm rounded-2xl p-6 border border-white/10">
				<h3 className="text-white/80 font-bold mb-4 flex items-center space-x-2">
					<Settings className="w-4 h-4 text-indigo-400" />
					<span className="text-xs uppercase tracking-wider">
						Skill Information
					</span>
				</h3>
				<div className="space-y-3 text-sm">
					<div className="flex justify-between items-center">
						<span className="text-white/40">Skill GUID:</span>
						<span className="text-white/80 font-mono text-xs bg-white/5 px-2 py-1 rounded">
							{selectedSkill.toolSkillGuid || "N/A"}
						</span>
					</div>
					<div className="flex justify-between items-center">
						<span className="text-white/40">Created:</span>
						<span className="text-white/80">
							{new Date(selectedSkill.dateCreated).toDateString()}
						</span>
					</div>
					<div className="flex justify-between items-center">
						<span className="text-white/40">Created By:</span>
						<span className="text-white/80">
							{selectedSkill.createdBy}
						</span>
					</div>
				</div>
			</div>
		);
	};

	const renderAssociatedAgents = (agents: AssociatedAgentsSkillDataDTO[]) => {
		return (
			<div className="bg-white/5 backdrop-blur-sm rounded-2xl p-6 border border-white/10">
				<h3 className="text-white/80 font-bold mb-4 flex items-center space-x-2">
					<Users className="w-4 h-4 text-purple-400" />
					<span className="text-xs uppercase tracking-wider">
						Associated Agents
					</span>
				</h3>
				{!agents || agents.length === 0 ? (
					<div className="py-4 text-center">
						<p className="text-white/30 text-xs italic">
							No agents currently using this skill
						</p>
					</div>
				) : (
					<div className="space-y-2">
						{agents.map((agent) => (
							<div
								key={agent.agentGuid}
								className="flex items-center justify-between p-3 rounded-xl bg-white/5 border border-white/5 hover:bg-white/10 hover:border-indigo-500/30 transition-all duration-300 group"
							>
								<div className="flex items-center space-x-3">
									<div className="w-8 h-8 rounded-lg bg-indigo-500/10 flex items-center justify-center border border-indigo-500/20 group-hover:bg-indigo-500/20 transition-colors">
										<Bot className="w-4 h-4 text-indigo-400" />
									</div>
									<div className="flex flex-col">
										<span className="text-white/80 text-sm font-medium group-hover:text-white transition-colors">
											{agent.agentName}
										</span>
										<span className="text-white/20 text-[10px] font-mono">
											{agent.agentGuid}
										</span>
									</div>
								</div>
							</div>
						))}
					</div>
				)}
			</div>
		);
	};

	const renderFooter = () => {
		return (
			<div className="border-t border-white/5 p-8 flex-shrink-0 bg-black/40 backdrop-blur-xl">
				<div className="flex items-center space-x-4">
					<Button
						onPress={handleEditSave}
						disabled={
							isDisabled ||
							accounts[0].username !== editFormData.createdBy
						}
						isLoading={IsEditSkillLoading}
						className="flex-1 h-14 bg-gradient-to-r from-emerald-600 via-teal-600 to-cyan-600 text-white font-bold text-lg hover:shadow-2xl hover:shadow-emerald-500/30 transition-all duration-300 rounded-2xl group border border-white/10 disabled:opacity-50"
					>
						{!IsEditSkillLoading && (
							<Save className="w-5 h-5 mr-2 group-hover:scale-110 transition-transform" />
						)}
						<span>Save Changes</span>
					</Button>

					<Button
						onPress={() => setIsDeletePopupOpen(true)}
						disabled={
							isDisabled ||
							accounts[0].username !== editFormData.createdBy
						}
						className="h-14 px-6 bg-red-500/10 text-red-400 font-semibold hover:bg-red-500/20 transition-all duration-300 rounded-2xl border border-red-500/20 hover:text-red-300 flex items-center space-x-2"
					>
						<Trash className="w-5 h-5" />
						<span>Delete</span>
					</Button>
				</div>
			</div>
		);
	};

	return IsEditSkillLoading ? (
		<FullScreenLoading
			isLoading={IsEditSkillLoading}
			message={MarketplaceConstants.LoadingConstants.UpdateSkill}
		/>
	) : (
		isEditDrawerOpen && selectedSkill && (
			<>
				<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
					{/* Header */}
					<div className="flex items-center justify-between p-8 border-b border-white/5 flex-shrink-0 bg-white/[0.02] backdrop-blur-md">
						<div className="flex items-center space-x-4">
							<div className="bg-gradient-to-br from-emerald-500 via-teal-500 to-cyan-500 p-3 rounded-2xl shadow-lg shadow-emerald-500/20 ring-1 ring-white/20">
								<Settings className="w-6 h-6 text-white" />
							</div>
							<div>
								<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-emerald-100 to-teal-100 bg-clip-text text-transparent tracking-tight">
									Edit Skill
								</h2>
								<p className="text-white/40 text-sm font-medium">
									Modify{" "}
									{editFormData.toolSkillDisplayName ||
										"skill"}{" "}
									details.
								</p>
							</div>
						</div>
						<button
							onClick={handleEditClose}
							className="p-2.5 rounded-xl bg-white/5 hover:bg-red-500/10 border border-white/10 hover:border-red-500/20 transition-all duration-300 text-white/50 hover:text-red-400 group"
							disabled={isDisabled}
						>
							<X className="w-5 h-5 group-hover:rotate-90 transition-transform duration-300" />
						</button>
					</div>

					{/* Form content */}
					<div className="flex-1 overflow-y-auto p-8 space-y-8 min-h-0 scrollbar-thin scrollbar-thumb-white/10">
						<div className="space-y-6">
							<div className="flex items-center space-x-2 pb-2 border-b border-white/5">
								<Sparkles className="w-4 h-4 text-emerald-400" />
								<span className="text-white/60 text-xs font-bold uppercase tracking-wider">
									Skill Details
								</span>
							</div>

							<div className="space-y-6">
								<div className="space-y-2">
									<label className="text-white/80 font-semibold text-sm ml-1">
										Display Name
									</label>
									<Input
										placeholder="Enter display name..."
										variant="bordered"
										size="lg"
										value={
											editFormData.toolSkillDisplayName
										}
										onValueChange={(v) =>
											handleInputChange(
												"toolSkillDisplayName",
												v,
											)
										}
										disabled={
											accounts[0].username !==
											editFormData.createdBy
										}
										startContent={
											<Type className="w-5 h-5 text-indigo-400 mr-3" />
										}
										classNames={{
											input: "text-white placeholder:text-white/20 p-3",
											inputWrapper:
												"bg-white/5 border-white/10 hover:border-emerald-500/30 focus-within:!border-emerald-500/50 transition-all min-h-[56px] rounded-2xl",
										}}
									/>
								</div>

								<div className="space-y-2">
									<label className="text-white/80 font-semibold text-sm ml-1">
										Technical Name
									</label>
									<Input
										placeholder="Enter technical name..."
										variant="bordered"
										size="lg"
										value={
											editFormData.toolSkillTechnicalName
										}
										onValueChange={(v) =>
											handleInputChange(
												"toolSkillTechnicalName",
												v,
											)
										}
										disabled={
											accounts[0].username !==
											editFormData.createdBy
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
									<label className="text-white/80 font-semibold text-sm ml-1">
										MCP Server URL
									</label>
									<div className="flex gap-3 items-center">
										<Input
											placeholder="Enter MCP URL..."
											variant="bordered"
											size="lg"
											value={
												editFormData.toolSkillMcpServerUrl
											}
											onValueChange={(v) =>
												handleInputChange(
													"toolSkillMcpServerUrl",
													v,
												)
											}
											disabled={
												accounts[0].username !==
												editFormData.createdBy
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
													editFormData.toolSkillMcpServerUrl,
												);
											}}
											isLoading={IsMcpToolsLoading}
											disabled={
												!editFormData.toolSkillMcpServerUrl
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

						{/* Tool Information */}
						{renderSkillInformationTile(selectedSkill)}

						{/* Associated Agents */}
						{renderAssociatedAgents(selectedSkill.associatedAgents)}
					</div>

					{/* Footer */}
					{renderFooter()}
				</div>

				<DeletePopupComponent
					isOpen={isDeletePopupOpen}
					onClose={() => setIsDeletePopupOpen(false)}
					onDelete={HandleSkillDelete}
					title="Delete Skill"
					description={`Are you sure you want to delete "${editFormData.toolSkillDisplayName}"? This action cannot be undone and will remove the skill from any associated agents.`}
					isLoading={IsToolSkillLoading}
				/>
			</>
		)
	);
}
