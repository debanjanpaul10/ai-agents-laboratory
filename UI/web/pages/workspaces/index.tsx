import { useEffect, useState } from "react";
import { Plus } from "lucide-react";
import { Button } from "@heroui/react";
import { useRouter } from "next/router";

import { useAuth } from "@auth/AuthProvider";
import { useAppDispatch, useAppSelector } from "@store/index";
import { ShowErrorToaster } from "@shared/toaster";
import { GetAllConfigurations } from "@store/common/actions";
import { FullScreenLoading } from "@components/common/spinner";
import { WorkspacesConstants } from "@helpers/constants";
import MainLayout from "@components/common/main-layout";
import {
	GetAllWorkspacesDataAsync,
	ToggleCreateWorkspaceDrawer,
	DeleteExistingWorkspaceAsync,
} from "@store/workspaces/actions";
import WorkspacesListComponent from "@components/workspaces/workspaces-list";
import CreateWorkspaceComponent from "@components/workspaces/create-workspace";
import EditWorkspaceFlyoutComponent from "@components/workspaces/edit-workspace";
import DeletePopupComponent from "@components/common/delete-popup";
import { AgentsWorkspaceDTO } from "@models/response/agents-workspace-dto";
import AssociateAgentsFlyoutComponent from "@components/workspaces/associate-agents";

export default function WorkspacesComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();
	const router = useRouter();

	const [isEditDrawerOpen, setIsEditDrawerOpen] = useState<boolean>(false);
	const [isDeletePopupOpen, setIsDeletePopupOpen] = useState<boolean>(false);
	const [selectedWorkspace, setSelectedWorkspace] =
		useState<AgentsWorkspaceDTO | null>(null);
	const [editFormData, setEditFormData] = useState<AgentsWorkspaceDTO>({
		activeAgentsListInWorkspace: [],
		agentWorkspaceGuid: "",
		agentWorkspaceName: "",
		isGroupChatEnabled: false,
		createdBy: "",
		dateCreated: new Date(),
		dateModified: new Date(),
		modifiedBy: "",
		workspaceUsers: [],
	});

	const [associateAgentsState, setAssociateAgentsState] = useState<{
		isOpen: boolean;
		selectedGuids: Set<string>;
		onComplete: ((selected: Set<string>) => void) | null;
	}>({
		isOpen: false,
		selectedGuids: new Set(),
		onComplete: null,
	});

	const IsWorkspacesLoading = useAppSelector(
		(state) => state.WorkspacesReducer.isWorkspacesLoading,
	);
	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations,
	);
	const WorkspacesListStoreData = useAppSelector(
		(state) => state.WorkspacesReducer.allWorkspaces,
	);
	const IsAddWorkspaceDrawerOpenStoreData = useAppSelector(
		(state) => state.WorkspacesReducer.isAddWorkspaceDrawerOpen,
	);

	useEffect(() => {
		if (authContext.isAuthenticated && !authContext.isLoading) {
			GetAllWorkspacesData();
			if (
				!ConfigurationsStoreData ||
				Object.keys(ConfigurationsStoreData).length === 0
			)
				GetAllConfigurationsData();
		}
	}, [authContext.isAuthenticated, authContext.isLoading]);

	async function fetchToken() {
		try {
			if (authContext.isAuthenticated && !authContext.isLoading)
				return await authContext.getAccessToken();
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		}
	}

	async function GetAllConfigurationsData() {
		const token = await fetchToken();
		token && dispatch(GetAllConfigurations(token));
	}

	async function GetAllWorkspacesData() {
		const token = await fetchToken();
		token && dispatch(GetAllWorkspacesDataAsync(token));
	}

	const handleUnAuthorizedUser = () => {
		return (
			<FullScreenLoading
				isLoading={true}
				message={
					WorkspacesConstants.LoadingConstants.LoginRedirectLoader
				}
			/>
		);
	};

	const handleCreateNewWorkspaceClick = () => {
		dispatch(ToggleCreateWorkspaceDrawer(true));
	};

	const handleEditWorkspace = (workspace: AgentsWorkspaceDTO) => {
		setSelectedWorkspace(workspace);
		setEditFormData(workspace);
		setIsEditDrawerOpen(true);
	};

	const handleOpenAssociateAgents = (
		currentSelection: Set<string>,
		onComplete: (selected: Set<string>) => void,
	) => {
		setAssociateAgentsState({
			isOpen: true,
			selectedGuids: currentSelection,
			onComplete,
		});
	};

	const handleAssociateAgentsClose = () => {
		setAssociateAgentsState((prev) => ({ ...prev, isOpen: false }));
	};

	const handleAssociateAgentsSelectionComplete = (
		selectedGuids: Set<string>,
	) => {
		if (associateAgentsState.onComplete) {
			associateAgentsState.onComplete(selectedGuids);
		}
	};

	const confirmDeleteWorkspace = async () => {
		if (!selectedWorkspace?.agentWorkspaceGuid) return;
		const token = await fetchToken();
		if (token) {
			await dispatch(
				DeleteExistingWorkspaceAsync(
					selectedWorkspace.agentWorkspaceGuid,
					token,
				) as any,
			);
			setIsDeletePopupOpen(false);
			setSelectedWorkspace(null);
		}
	};

	const isAnyDrawerOpen =
		isEditDrawerOpen || IsAddWorkspaceDrawerOpenStoreData;

	return !authContext.isAuthenticated ? (
		handleUnAuthorizedUser()
	) : IsWorkspacesLoading ? (
		<FullScreenLoading
			isLoading={IsWorkspacesLoading}
			message={WorkspacesConstants.LoadingConstants.MainLoader}
		/>
	) : (
		<MainLayout contentClassName="p-0" isFullWidth={true}>
			<div className="w-full h-screen overflow-hidden bg-gradient-to-br from-gray-900 via-slate-900 to-black">
				<WorkspacesListComponent
					workspacesList={WorkspacesListStoreData}
					handleWorkspaceClick={(workspaceId: string) =>
						router.push(`/workspace/${workspaceId}`)
					}
					onEditWorkspace={handleEditWorkspace}
					onClose={() => {}}
					isDisabled={isAnyDrawerOpen}
					showCloseButton={false}
					actionButton={
						<Button
							onPress={handleCreateNewWorkspaceClick}
							className="bg-white/5 border border-white/10 hover:border-blue-500/50 hover:bg-blue-500/10 text-white font-medium px-6 rounded-xl transition-all duration-300 group shadow-lg"
						>
							<Plus className="w-4 h-4 mr-2 text-blue-400 group-hover:text-blue-300 group-hover:scale-110 transition-all" />
							<span>Add New Workspace</span>
						</Button>
					}
				/>
				<CreateWorkspaceComponent
					onOpenAssociateAgents={handleOpenAssociateAgents}
				/>
				<AssociateAgentsFlyoutComponent
					isOpen={associateAgentsState.isOpen}
					onClose={handleAssociateAgentsClose}
					selectedAgentGuids={associateAgentsState.selectedGuids}
					onSelectionComplete={handleAssociateAgentsSelectionComplete}
				/>

				{/* Delete Popup */}
				<DeletePopupComponent
					isOpen={isDeletePopupOpen}
					onClose={() => setIsDeletePopupOpen(false)}
					onDelete={confirmDeleteWorkspace}
					title="Delete Workspace"
					description="Are you sure you want to delete this workspace? This action cannot be undone and will remove all associated data."
					isLoading={IsWorkspacesLoading}
				/>
			</div>

			{/* Backdrop Overlay */}
			{isAnyDrawerOpen && (
				<div
					className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-all duration-300"
					onClick={() => {}}
				/>
			)}

			{/* Edit Workspace Drawer */}
			{isEditDrawerOpen && (
				<div className="fixed top-0 right-0 h-screen z-50 transition-all duration-500 ease-in-out md:w-1/2 w-full">
					<div className="absolute inset-0 bg-gradient-to-r from-emerald-600/20 via-teal-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10"></div>
					<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
						<EditWorkspaceFlyoutComponent
							editFormData={editFormData}
							selectedWorkspace={selectedWorkspace}
							setEditFormData={setEditFormData}
							setSelectedWorkspace={setSelectedWorkspace}
							isEditDrawerOpen={isEditDrawerOpen}
							onEditClose={() => setIsEditDrawerOpen(false)}
							isDisabled={false}
							onOpenAssociateAgents={handleOpenAssociateAgents}
						/>
					</div>
				</div>
			)}
		</MainLayout>
	);
}
