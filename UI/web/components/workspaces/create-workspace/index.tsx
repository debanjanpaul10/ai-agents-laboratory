import {
	Laptop,
	X,
	Plus,
	Trash2,
	Users,
	Bot,
	UserPlus,
	Sparkles,
	RotateCcw,
	Type,
} from "lucide-react";
import { useEffect, useState } from "react";
import { Button, Input, Chip } from "@heroui/react";
import { cn } from "@heroui/react";

import { useAuth } from "@auth/AuthProvider";
import { FullScreenLoading } from "@components/common/spinner";
import { DashboardConstants, WorkspacesConstants } from "@helpers/constants";
import { AgentsWorkspaceDTO } from "@models/response/agents-workspace-dto";
import { useAppDispatch, useAppSelector } from "@store/index";
import {
	CreateNewWorkspaceAsync,
	ToggleAssociateAgentsDrawer,
	ToggleCreateWorkspaceDrawer,
} from "@store/workspaces/actions";
import { WorkspaceAgentsDataDTO } from "@models/response/workspace-agents-data.dto";
import { GetAllAgentsDataAsync } from "@store/agents/actions";


export default function CreateWorkspaceComponent({
	onOpenAssociateAgents,
}: {
	onOpenAssociateAgents: (
		currentSelection: Set<string>,
		onComplete: (selected: Set<string>) => void,
	) => void;
}) {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const [isDrawerOpen, setIsDrawerOpen] = useState<boolean>(false);
	const [formData, setFormData] = useState<AgentsWorkspaceDTO>({
		activeAgentsListInWorkspace: [],
		agentWorkspaceGuid: "",
		agentWorkspaceName: "",
		createdBy: "",
		dateCreated: new Date(),
		dateModified: new Date(),
		modifiedBy: "",
		workspaceUsers: [],
	});

	const [newUserEmail, setNewUserEmail] = useState("");
	const [selectedAgentGuids, setSelectedAgentGuids] = useState<Set<string>>(
		new Set(),
	);

	const IsAddWorkspaceDrawerOpenStoreData = useAppSelector(
		(state) => state.WorkspacesReducer.isAddWorkspaceDrawerOpen,
	);

	const IsCreateWorkspaceLoadingStoreData = useAppSelector(
		(state) => state.WorkspacesReducer.isAddWorkspaceLoading,
	);

	const agentsListData = useAppSelector(
		(state) => state.AgentsReducer.agentsListData,
	);

	const IsAgentsListLoading = useAppSelector(
		(state) => state.AgentsReducer.isAgentsListLoading,
	);

	useEffect(() => {
		if (isDrawerOpen !== IsAddWorkspaceDrawerOpenStoreData) {
			setIsDrawerOpen(IsAddWorkspaceDrawerOpenStoreData);
			if (IsAddWorkspaceDrawerOpenStoreData) handleClearData();
		}
	}, [IsAddWorkspaceDrawerOpenStoreData, authContext, agentsListData.length]);

	const onCloseFlyout = () => {
		dispatch(ToggleCreateWorkspaceDrawer(false));
		dispatch(ToggleAssociateAgentsDrawer(false));
	};

	const handleInputChange = (
		field: keyof AgentsWorkspaceDTO,
		value: string,
	) => {
		setFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleAddUser = () => {
		if (
			newUserEmail.trim() &&
			!formData.workspaceUsers.includes(newUserEmail.trim())
		) {
			setFormData((prev) => ({
				...prev,
				workspaceUsers: [...prev.workspaceUsers, newUserEmail.trim()],
			}));
			setNewUserEmail("");
		}
	};

	const handleRemoveUser = (email: string) => {
		setFormData((prev) => ({
			...prev,
			workspaceUsers: prev.workspaceUsers.filter((u) => u !== email),
		}));
	};

	async function handleCreateWorkspace() {
		const agentsToSubmit: WorkspaceAgentsDataDTO[] = Array.from(
			selectedAgentGuids,
		).map((guid) => {
			const agent = agentsListData.find(
				(a: any) => (a.agentGuid || a.agentId) === guid,
			);
			return {
				agentGuid: guid,
				agentName: agent?.agentName || "",
			};
		});

		const workspaceToSubmit: AgentsWorkspaceDTO = {
			...formData,
			activeAgentsListInWorkspace: agentsToSubmit,
		};

		const token = await authContext.getAccessToken();
		if (token) {
			dispatch(CreateNewWorkspaceAsync(workspaceToSubmit, token) as any);
			onCloseFlyout();
		}
	}

	async function GetAllAvailableAgents() {
		const accessToken = await authContext.getAccessToken();
		accessToken && dispatch(GetAllAgentsDataAsync(accessToken, true));
	}

	const handleClearData = () => {
		setFormData({
			activeAgentsListInWorkspace: [],
			agentWorkspaceGuid: "",
			agentWorkspaceName: "",
			createdBy: "",
			dateCreated: new Date(),
			dateModified: new Date(),
			modifiedBy: "",
			workspaceUsers: [],
		});
		setSelectedAgentGuids(new Set());
		setNewUserEmail("");
	};

	const toggleAgentSelection = (guid: string) => {
		setSelectedAgentGuids((prev) => {
			const next = new Set(prev);
			if (next.has(guid)) {
				next.delete(guid);
			} else {
				next.add(guid);
			}
			return next;
		});
	};

	const handleAgentSelectionComplete = (selectedGuids: Set<string>) => {
		setSelectedAgentGuids(selectedGuids);
	};

	const isFormValid =
		formData.agentWorkspaceName.trim() !== "" &&
		selectedAgentGuids.size !== 0 &&
		formData.workspaceUsers.length !== 0;

	return IsCreateWorkspaceLoadingStoreData ? (
		<FullScreenLoading
			isLoading={IsCreateWorkspaceLoadingStoreData}
			message={DashboardConstants.LoadingConstants.CreateNewWorkspace}
		/>
	) : (
		isDrawerOpen && (
			<>
				<div
					className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300 max-w-full"
					onClick={onCloseFlyout}
				/>
				<div className="fixed right-0 top-0 md:w-1/2 w-full h-screen z-50 transition-all duration-500 ease-in-out">
					<div className="absolute inset-0 bg-gradient-to-l from-purple-600/20 via-blue-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10"></div>
					<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
						{/* Header */}
						<div className="flex items-center justify-between p-8 border-b border-white/5 flex-shrink-0 bg-white/[0.02] backdrop-blur-md">
							<div className="flex items-center space-x-4">
								<div className="bg-gradient-to-br from-indigo-500 via-purple-500 to-pink-500 p-3 rounded-2xl shadow-lg shadow-purple-500/20 ring-1 ring-white/20">
									<Laptop className="w-6 h-6 text-white" />
								</div>
								<div>
									<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-indigo-100 to-purple-100 bg-clip-text text-transparent tracking-tight">
										{
											WorkspacesConstants
												.CreateWorkspaceConstants.Header
										}
									</h2>
									<p className="text-white/40 text-sm font-medium">
										{
											WorkspacesConstants
												.CreateWorkspaceConstants
												.SubHeader
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

						{/* Form Content */}
						<div className="flex-1 overflow-y-auto p-8 space-y-8 min-h-0 scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
							{/* Workspace Details Section */}
							<div className="space-y-6">
								<div className="flex items-center space-x-2 pb-2 border-b border-white/5">
									<Sparkles className="w-4 h-4 text-indigo-400" />
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
											value={formData.agentWorkspaceName}
											onValueChange={(v) =>
												handleInputChange(
													"agentWorkspaceName",
													v,
												)
											}
											placeholder="Enter a memorable name for your workspace..."
											variant="bordered"
											size="lg"
											startContent={
												<Type className="w-5 h-5 text-indigo-400 mr-3" />
											}
											classNames={{
												input: "text-white placeholder:text-white/20 p-3",
												inputWrapper:
													"bg-white/5 border-white/10 hover:border-indigo-500/30 focus-within:!border-indigo-500/50 transition-all min-h-[56px] rounded-2xl",
											}}
										/>
									</div>
								</div>

								<div className="space-y-4">
									<label className="text-white/80 font-semibold text-sm ml-1">
										Select Workspace Agents
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
												Array.from(
													selectedAgentGuids,
												).map((guid) => {
													const agent =
														agentsListData.find(
															(a: any) =>
																(a.agentGuid ||
																	a.agentId) ===
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
												})
											) : (
												<span className="text-white/20 text-sm ml-2">
													Associate agents with this
													workspace...
												</span>
											)}
										</div>
										<Button
											isIconOnly
											className="h-[56px] w-[56px] min-w-[56px] bg-white/5 border border-white/10 hover:border-cyan-400/50 hover:bg-cyan-400/10 text-cyan-400 transition-all duration-300 rounded-2xl group/btn"
											onPress={async () => {
												await GetAllAvailableAgents();
												onOpenAssociateAgents(
													selectedAgentGuids,
													handleAgentSelectionComplete,
												);
											}}
											isLoading={IsAgentsListLoading}
											title="View All Agents"
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
												className="h-[56px] w-[56px] min-w-[56px] bg-gradient-to-br from-cyan-500 to-blue-600 text-white rounded-2xl shadow-lg shadow-cyan-500/20 hover:scale-105 active:scale-95 transition-all duration-300"
											>
												<UserPlus className="w-6 h-6" />
											</Button>
										</div>
									</div>

									<div className="grid grid-cols-1 gap-3 max-h-[240px] overflow-y-auto pr-2 scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
										{formData.workspaceUsers.map((user) => (
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
													className="p-2 rounded-xl text-white/20 hover:text-red-400 hover:bg-red-400/10 transition-all duration-300"
												>
													<Trash2 className="w-4 h-4" />
												</button>
											</div>
										))}
										{formData.workspaceUsers.length ===
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

							{/* Pro Tip Box */}
							<div className="relative group">
								<div className="absolute inset-0 bg-indigo-500/10 blur-xl opacity-50 group-hover:opacity-100 transition duration-500"></div>
								<div className="relative bg-gradient-to-br from-indigo-500/10 to-purple-500/5 border border-indigo-500/20 rounded-2xl p-5 backdrop-blur-sm">
									<div className="flex items-start space-x-3">
										<div className="bg-indigo-500/20 p-2 rounded-lg">
											<Bot className="w-4 h-4 text-indigo-400" />
										</div>
										<p className="text-indigo-100/70 text-sm leading-relaxed">
											<span className="text-indigo-400 font-bold block mb-1">
												Pro Tip:
											</span>
											Workspaces help you organize agents
											and collaborate with teammates in an
											isolated environment.
										</p>
									</div>
								</div>
							</div>
						</div>

						{/* Action Footer */}
						<div className="border-t border-white/5 p-8 flex-shrink-0 bg-black/40 backdrop-blur-xl">
							<div className="flex items-center space-x-4">
								<Button
									onPress={handleCreateWorkspace}
									disabled={!isFormValid}
									className="px-6 h-14 bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-600 text-white font-bold text-lg hover:shadow-2xl hover:shadow-purple-500/30 transition-all duration-300 rounded-2xl group border border-white/10 disabled:opacity-50 disabled:cursor-not-allowed"
								>
									<Plus className="w-5 h-5 mr-2 group-hover:scale-110 transition-transform" />
									<span>Create Workspace</span>
								</Button>
								<Button
									onPress={handleClearData}
									className="h-14 px-6 bg-indigo-500/5 text-indigo-400 font-semibold hover:bg-indigo-500/10 transition-all duration-300 rounded-2xl border border-indigo-500/20 hover:text-indigo-300 flex items-center space-x-2"
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
