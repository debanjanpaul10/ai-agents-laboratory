import { useRouter } from "next/router";
import { useEffect, useState } from "react";

import MainLayout from "@components/common/main-layout";
import { WorkspacesConstants } from "@helpers/constants";
import { useAppDispatch, useAppSelector } from "@store/index";
import { useAuth } from "@auth/AuthProvider";
import { ShowErrorToaster, ShowSuccessToaster } from "@shared/toaster";
import { GetWorkspaceByWorkspaceIdAsync } from "@store/workspaces/actions";
import { FullScreenLoading } from "@components/common/spinner";
import { GetAllConfigurations } from "@store/common/actions";
import { WorkspaceAgentsDataDTO } from "@models/response/workspace-agents-data.dto";
import { AgentsWorkspaceDTO } from "@models/response/agents-workspace-dto";
import AssociatedAgentsChatPaneComponent from "@components/workspace/associated-agents-chat";
import AssociatedAgentsListPaneComponent from "@components/workspace/associated-agents-list";

export default function WorkspaceComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();
	const router = useRouter();

	const [selectedAgent, setSelectedAgent] =
		useState<WorkspaceAgentsDataDTO | null>(null);

	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations,
	);
	const IsWorkspaceLoadingStoreData = useAppSelector<boolean>(
		(state) => state.WorkspacesReducer.isWorkspacesLoading,
	);
	const WorkspaceDetailsData = useAppSelector<AgentsWorkspaceDTO>(
		(state) => state.WorkspacesReducer.workspaceData,
	);

	useEffect(() => {
		if (authContext.isAuthenticated && !authContext.isLoading) {
			if (router.query.id) {
				GetWorkspaceByWorkspaceId(router.query.id.toString());
			}
			if (
				!ConfigurationsStoreData ||
				Object.keys(ConfigurationsStoreData).length === 0
			)
				GetAllConfigurationsData();
		}
	}, [router.query.id]);

	async function fetchToken() {
		try {
			if (authContext.isAuthenticated && !authContext.isLoading)
				return await authContext.getAccessToken();
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		}
	}

	async function GetWorkspaceByWorkspaceId(workspaceId: string) {
		const token = await fetchToken();
		token && dispatch(GetWorkspaceByWorkspaceIdAsync(workspaceId, token));
	}

	async function GetAllConfigurationsData() {
		const token = await fetchToken();
		token && dispatch(GetAllConfigurations(token));
	}

	const handleAgentSelection = (agent: WorkspaceAgentsDataDTO) => {
		if (selectedAgent && selectedAgent.agentGuid !== agent.agentGuid) {
			ShowSuccessToaster("Previous conversation history cleared");
		}
		setSelectedAgent(agent);
	};

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

	return !authContext.isAuthenticated ? (
		handleUnAuthorizedUser()
	) : IsWorkspaceLoadingStoreData ? (
		<FullScreenLoading
			isLoading={IsWorkspaceLoadingStoreData}
			message={WorkspacesConstants.LoadingConstants.MainLoader}
		/>
	) : (
		<MainLayout isFullWidth={true} contentClassName="p-0">
			<div className="h-[calc(100vh)] flex flex-col md:flex-row gap-0 rounded-3xl overflow-hidden border border-white/10 shadow-2xl">
				<div className="w-1/2 h-full flex-shrink-0">
					<AssociatedAgentsListPaneComponent
						selectedAgent={selectedAgent}
						setSelectedAgent={handleAgentSelection}
						workspaceDetailsData={WorkspaceDetailsData}
					/>
				</div>

				<div className="w-1/2 h-full">
					<AssociatedAgentsChatPaneComponent
						key={selectedAgent?.agentGuid}
						selectedAgent={selectedAgent}
					/>
				</div>
			</div>
		</MainLayout>
	);
}
