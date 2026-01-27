import { useMsal } from "@azure/msal-react";
import { Button, Chip, cn, Input } from "@heroui/react";
import {
	Bot,
	Info,
	Save,
	Settings,
	Sparkles,
	Trash,
	Trash2,
	Type,
	UserPlus,
	Users,
	X,
} from "lucide-react";
import { useMemo, useState } from "react";

import DeletePopupComponent from "@components/common/delete-popup";
import { FullScreenLoading } from "@components/common/spinner";
import { WorkspacesConstants } from "@helpers/constants";
import { AgentsWorkspaceDTO } from "@models/response/agents-workspace-dto";
import { WorkspaceAgentsDataDTO } from "@models/response/workspace-agents-data.dto";
import { EditWorkspaceFlyoutComponentProps } from "@shared/types";
import { useAppDispatch, useAppSelector } from "@store/index";
import { GetAllAgentsDataAsync } from "@store/agents/actions";
import {
	DeleteExistingWorkspaceAsync,
	UpdateExistingWorkspaceDataAsync,
} from "@store/workspaces/actions";
import GroupChatPopupComponent from "@components/common/group-chat-popup";

export default function EditWorkspaceFlyoutComponent({
	editFormData,
	selectedWorkspace,
	setEditFormData,
	setSelectedWorkspace,
	isEditDrawerOpen,
	onEditClose,
	isDisabled,
	onOpenAssociateAgents,
}: EditWorkspaceFlyoutComponentProps) {
	const dispatch = useAppDispatch();
	const { accounts } = useMsal();

	const [isDeletePopupOpen, setIsDeletePopupOpen] = useState<boolean>(false);
	const [newUserEmail, setNewUserEmail] = useState<string>("");
	const [isGroupChatEnablePopupOpen, setIsGroupChatEnablePopupOpen] =
		useState<boolean>(false);

	const IsEditWorkspaceLoading = useAppSelector<boolean>(
		(state) => state.WorkspacesReducer.isEditWorkspaceLoading,
	);
	const agentsListData = useAppSelector(
		(state) => state.AgentsReducer.agentsListData,
	);
	const IsAgentsListLoading = useAppSelector(
		(state) => state.AgentsReducer.isAgentsListLoading,
	);

	const selectedAgentGuids = useMemo(() => {
		return new Set(
			editFormData.activeAgentsListInWorkspace.map((a) => a.agentGuid),
		);
	}, [editFormData.activeAgentsListInWorkspace]);

	const handleInputChange = (
		field: keyof AgentsWorkspaceDTO,
		value: string | boolean,
	) => {
		setEditFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleEditClose = () => {
		setSelectedWorkspace(null);
		onEditClose();
	};

	function HandleWorkspaceDelete() {
		var workspaceGuidId = editFormData.agentWorkspaceGuid;
		dispatch(DeleteExistingWorkspaceAsync(workspaceGuidId));
		setIsDeletePopupOpen(false);
		handleEditClose();
	}

	function handleEditSave() {
		dispatch(UpdateExistingWorkspaceDataAsync(editFormData));
		handleEditClose();
	}

	function GetAllAvailableAgents() {
		dispatch(GetAllAgentsDataAsync(true));
	}

	const handleAddUser = () => {
		if (
			newUserEmail.trim() &&
			!editFormData.workspaceUsers.includes(newUserEmail.trim())
		) {
			setEditFormData((prev) => ({
				...prev,
				workspaceUsers: [...prev.workspaceUsers, newUserEmail.trim()],
			}));
			setNewUserEmail("");
		}
	};

	const handleRemoveUser = (email: string) => {
		setEditFormData((prev) => ({
			...prev,
			workspaceUsers: prev.workspaceUsers.filter((u) => u !== email),
		}));
	};

	const handleAgentSelectionComplete = (selectedGuids: Set<string>) => {
		const newAgentsList: WorkspaceAgentsDataDTO[] = Array.from(
			selectedGuids,
		).map((guid) => {
			const existing = editFormData.activeAgentsListInWorkspace.find(
				(a) => a.agentGuid === guid,
			);
			if (existing) return existing;
			const agent = agentsListData.find(
				(a: any) => (a.agentGuid || a.agentId) === guid,
			);
			return {
				agentGuid: guid,
				agentName: agent?.agentName || "Unknown Agent",
			};
		});
		setEditFormData((prev) => ({
			...prev,
			activeAgentsListInWorkspace: newAgentsList,
		}));
	};

	const toggleAgentSelection = (guid: string) => {
		const currentGuids = new Set(selectedAgentGuids);
		if (currentGuids.has(guid)) {
			currentGuids.delete(guid);
		} else {
			currentGuids.add(guid);
		}
		handleAgentSelectionComplete(currentGuids);
	};

	return IsEditWorkspaceLoading ? (
		<FullScreenLoading
			isLoading={IsEditWorkspaceLoading}
			message={WorkspacesConstants.LoadingConstants.UpdateWorkspace}
		/>
	) : (
		isEditDrawerOpen && selectedWorkspace && (
			<>
				<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
					{/* HEADER */}
					<div className="flex items-center justify-between p-8 border-b border-white/5 flex-shrink-0 bg-white/[0.02] backdrop-blur-md">
						<div className="flex items-center space-x-4">
							<div className="bg-gradient-to-br from-emerald-500 via-teal-500 to-cyan-500 p-3 rounded-2xl shadow-lg shadow-emerald-500/20 ring-1 ring-white/20">
								<Settings className="w-6 h-6 text-white" />
							</div>
							<div>
								<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-emerald-100 to-teal-100 bg-clip-text text-transparent tracking-tight">
									Edit Workspace
								</h2>
								<p className="text-white/40 text-sm font-medium">
									Modify{" "}
									{editFormData.agentWorkspaceName ||
										"Workspace"}{" "}
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

					{/* CONTENT */}
					<div className="flex-1 overflow-y-auto p-8 space-y-8 min-h-0 scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
						{/* Workspace Details Section */}
						<div className="space-y-6">
							<div className="flex items-center space-x-2 pb-2 border-b border-white/5">
								<Sparkles className="w-4 h-4 text-emerald-400" />
								<span className="text-white/60 text-xs font-bold uppercase tracking-wider">
									Workspace Details
								</span>
							</div>

							<div className="space-y-4">
								<div className="space-y-2">
									<label className="text-white/80 font-semibold text-sm ml-1">
										Workspace Name
									</label>
									<Input
										value={editFormData.agentWorkspaceName}
										onValueChange={(v) =>
											handleInputChange(
												"agentWorkspaceName",
												v,
											)
										}
										placeholder="Enter a memorable name for your workspace..."
										variant="bordered"
										size="lg"
										disabled={isDisabled}
										startContent={
											<Type className="w-5 h-5 text-emerald-400 mr-3" />
										}
										classNames={{
											input: "text-white placeholder:text-white/20 p-3",
											inputWrapper:
												"bg-white/5 border-white/10 hover:border-emerald-500/30 focus-within:!border-emerald-500/50 transition-all min-h-[56px] rounded-2xl",
										}}
									/>
								</div>
							</div>

							<div className="space-y-4 bg-white/5 p-3 rounded-2xl">
								<div className="flex items-center justify-between">
									<label className="text-white/80 font-semibold text-sm ml-1">
										Agent Group Chat{" "}
										<span className="text-red-400">
											(Preview)
										</span>
									</label>
									<label className="relative inline-flex items-end cursor-pointer">
										<input
											type="checkbox"
											checked={
												editFormData.isGroupChatEnabled
											}
											onChange={(e) => {
												if (e.target.checked === true)
													setIsGroupChatEnablePopupOpen(
														true,
													);
												else
													handleInputChange(
														"isGroupChatEnabled",
														false,
													);
											}}
											className="sr-only peer"
										/>
										<div className="relative w-12 h-6 bg-white/10 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-6 peer-checked:after:border-white after:content-[''] after:absolute after:top-[4px] after:left-[4px] after:bg-white/50 after:rounded-full after:h-4 after:w-4 after:transition-all peer-checked:bg-pink-500 peer-checked:after:bg-white"></div>
									</label>
								</div>
							</div>

							<div className="space-y-4">
								<label className="text-white/80 font-semibold text-sm ml-1">
									Workspace Agents
								</label>
								<div className="flex gap-3">
									<div
										className={cn(
											"flex-1 bg-white/5 border border-white/10 hover:border-purple-500/30 transition-all min-h-[56px] rounded-2xl p-3 flex flex-wrap gap-2 items-center group",
											selectedAgentGuids.size === 0 &&
												"border-dashed",
										)}
									>
										{selectedAgentGuids.size > 0 ? (
											Array.from(selectedAgentGuids).map(
												(guid) => {
													const agent =
														editFormData.activeAgentsListInWorkspace.find(
															(a) =>
																a.agentGuid ===
																guid,
														);
													return (
														<Chip
															key={guid}
															variant="flat"
															className="bg-purple-500/10 text-purple-300 border border-purple-500/20 px-3 py-1"
															onClose={() => {
																toggleAgentSelection(
																	guid,
																);
															}}
															size="sm"
															startContent={
																<Bot className="w-3 h-3 mr-1" />
															}
														>
															{agent?.agentName ||
																guid}
														</Chip>
													);
												},
											)
										) : (
											<span className="text-white/20 text-sm ml-2">
												Associate agents with this
												workspace...
											</span>
										)}
									</div>
									<Button
										isIconOnly
										className="h-[56px] w-[56px] min-w-[56px] bg-white/5 border border-white/10 hover:border-cyan-400/50 hover:bg-cyan-400/10 text-cyan-400 transition-all duration-300 rounded-2xl group/btn disabled:opacity-50"
										onPress={async () => {
											await GetAllAvailableAgents();
											onOpenAssociateAgents(
												selectedAgentGuids,
												handleAgentSelectionComplete,
											);
										}}
										isLoading={IsAgentsListLoading}
										title="Manage Agents"
										disabled={isDisabled}
									>
										{!IsAgentsListLoading && (
											<Bot className="w-5 h-5 group-hover/btn:rotate-12 transition-transform duration-300" />
										)}
									</Button>
								</div>
							</div>
						</div>

						{/* User Management Section */}
						<div className="space-y-6">
							<div className="flex items-center space-x-2 pb-2 border-b border-white/5">
								<Sparkles className="w-4 h-4 text-cyan-400" />
								<span className="text-white/60 text-xs font-bold uppercase tracking-wider">
									Workspace Members
								</span>
							</div>

							<div className="space-y-4">
								<div className="space-y-2">
									<label className="text-white/80 font-semibold text-sm ml-1">
										Add Member Email
									</label>
									<div className="flex space-x-3">
										<Input
											value={newUserEmail}
											onValueChange={setNewUserEmail}
											onKeyDown={(e) =>
												e.key === "Enter" &&
												handleAddUser()
											}
											placeholder="Add user email or ID..."
											variant="bordered"
											size="lg"
											disabled={isDisabled}
											startContent={
												<Users className="w-5 h-5 text-cyan-400 mr-3" />
											}
											classNames={{
												input: "text-white placeholder:text-white/20 p-3",
												inputWrapper:
													"bg-white/5 border-white/10 hover:border-cyan-500/30 focus-within:!border-cyan-500/50 transition-all min-h-[56px] rounded-2xl flex-1",
											}}
										/>
										<Button
											onPress={handleAddUser}
											disabled={isDisabled}
											className="h-[56px] w-[56px] min-w-[56px] bg-gradient-to-br from-cyan-500 to-blue-600 text-white rounded-2xl shadow-lg shadow-cyan-500/20 hover:scale-105 active:scale-95 transition-all duration-300 disabled:opacity-50"
										>
											<UserPlus className="w-6 h-6" />
										</Button>
									</div>
								</div>

								<div className="grid grid-cols-1 gap-3 max-h-[240px] overflow-y-auto pr-2 scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
									{editFormData.workspaceUsers.map((user) => (
										<div
											key={user}
											className="flex items-center justify-between p-3.5 bg-white/5 border border-white/5 rounded-2xl group hover:bg-white/[0.08] hover:border-white/10 transition-all duration-300"
										>
											<div className="flex items-center space-x-3">
												<div className="w-8 h-8 rounded-full bg-cyan-500/20 flex items-center justify-center border border-cyan-500/30">
													<Users className="w-4 h-4 text-cyan-400" />
												</div>
												<span className="text-white/80 font-medium">
													{user}
												</span>
											</div>
											<button
												onClick={() =>
													handleRemoveUser(user)
												}
												disabled={isDisabled}
												className="p-2 rounded-xl text-white/20 hover:text-red-400 hover:bg-red-400/10 transition-all duration-300 disabled:opacity-0"
											>
												<Trash2 className="w-4 h-4" />
											</button>
										</div>
									))}
									{editFormData.workspaceUsers.length ===
										0 && (
										<div className="flex flex-col items-center justify-center py-8 bg-white/[0.02] border border-dashed border-white/10 rounded-2xl">
											<Users className="w-10 h-10 text-white/10 mb-2" />
											<p className="text-white/30 text-sm">
												No users added yet
											</p>
										</div>
									)}
								</div>
							</div>
						</div>

						{/* Info Tile */}
						<div className="bg-white/5 backdrop-blur-sm rounded-2xl p-6 border border-white/10">
							<h3 className="text-white/80 font-bold mb-4 flex items-center space-x-2">
								<Info className="w-4 h-4 text-cyan-400" />
								<span className="text-xs uppercase tracking-wider">
									Workspace Information
								</span>
							</h3>
							<div className="space-y-3 text-sm">
								<div className="flex justify-between items-center">
									<span className="text-white/40">
										Workspace GUID:
									</span>
									<span className="text-white/80 font-mono text-xs bg-white/5 px-2 py-1 rounded select-all">
										{editFormData.agentWorkspaceGuid ||
											"N/A"}
									</span>
								</div>
								<div className="flex justify-between items-center">
									<span className="text-white/40">
										Created:
									</span>
									<span className="text-white/80">
										{new Date(
											editFormData.dateCreated,
										).toDateString()}
									</span>
								</div>
								<div className="flex justify-between items-center">
									<span className="text-white/40">
										Created By:
									</span>
									<span className="text-white/80">
										{editFormData.createdBy || "Unknown"}
									</span>
								</div>
							</div>
						</div>
					</div>

					{/* ACTION FOOTER */}
					<div className="border-t border-white/5 p-8 flex-shrink-0 bg-black/40 backdrop-blur-xl">
						<div className="flex items-center space-x-4">
							<Button
								onPress={handleEditSave}
								disabled={
									isDisabled ||
									(editFormData.createdBy !==
										accounts[0]?.username &&
										editFormData.createdBy !== "Unknown")
								}
								isLoading={IsEditWorkspaceLoading}
								className="px-6 h-14 bg-gradient-to-r from-emerald-600 via-teal-600 to-cyan-600 text-white font-bold text-lg hover:shadow-2xl hover:shadow-emerald-500/30 transition-all duration-300 rounded-2xl group border border-white/10 disabled:opacity-50 disabled:cursor-not-allowed"
							>
								{!IsEditWorkspaceLoading && (
									<Save className="w-5 h-5 mr-2 group-hover:scale-110 transition-transform" />
								)}
								<span>Save Changes</span>
							</Button>

							<Button
								onPress={() => setIsDeletePopupOpen(true)}
								disabled={
									isDisabled ||
									(editFormData.createdBy !==
										accounts[0]?.username &&
										editFormData.createdBy !== "Unknown")
								}
								className="h-14 px-6 bg-red-500/10 text-red-400 font-semibold hover:bg-red-500/20 transition-all duration-300 rounded-2xl border border-red-500/20 hover:text-red-300 flex items-center space-x-2 disabled:opacity-50 disabled:cursor-not-allowed"
							>
								<Trash className="w-5 h-5" />
								<span>Delete</span>
							</Button>
						</div>
					</div>
				</div>

				<DeletePopupComponent
					isOpen={isDeletePopupOpen}
					onClose={() => setIsDeletePopupOpen(false)}
					onAction={HandleWorkspaceDelete}
					title="Delete Workspace"
					description={`Are you sure you want to delete "${editFormData.agentWorkspaceName}"? This action cannot be undone and will remove all associated data.`}
					isLoading={IsEditWorkspaceLoading}
				/>

				<GroupChatPopupComponent
					isOpen={isGroupChatEnablePopupOpen}
					onClose={() => setIsGroupChatEnablePopupOpen(false)}
					onAction={() => {
						handleInputChange("isGroupChatEnabled", true);
						setIsGroupChatEnablePopupOpen(false);
					}}
					title="Enable group chat?"
					description={
						"Are you sure you want to enable this feature? It is still in preview stages! There might be some inaccuracies and bugs!"
					}
					isLoading={IsEditWorkspaceLoading}
				/>
			</>
		)
	);
}
