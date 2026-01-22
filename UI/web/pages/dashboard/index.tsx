import { useEffect } from "react";

import { useAuth } from "@auth/AuthProvider";
import ActiveAgentsTileComponent from "@components/dashboard/active-agents-tile";
import { useAppDispatch, useAppSelector } from "@store/index";
import { FullScreenLoading } from "@components/common/spinner";
import SystemHealthTileComponent from "@components/dashboard/system-health-tile";
import FooterComponent from "@components/common/footer";
import WelcomeCardComponent from "@components/dashboard/welcome-card";
import QuickActionsTileComponent from "@components/dashboard/quick-actions-tile";
import DirectChatComponent from "@components/direct-chat";
import { DashboardConstants } from "@helpers/constants";
import {
	GetAllConfigurations,
	GetTopActiveAgentsDataAsync,
} from "@store/common/actions";
import MainLayout from "@components/common/main-layout";
import FeedbackComponent from "@components/feedback";

export default function DashboardComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const IsLoadingStoreData = useAppSelector(
		(state) => state.CommonReducer.isLoading,
	);

	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations,
	);

	useEffect(() => {
		if (authContext.isAuthenticated && !authContext.isLoading) {
			GetTopActiveAgentsData();
			if (
				!ConfigurationsStoreData ||
				Object.keys(ConfigurationsStoreData).length === 0
			) {
				GetAllConfigurationsData();
			}
		}
	}, [authContext.isAuthenticated, authContext.isLoading]);

	async function GetTopActiveAgentsData() {
		const token = await fetchToken();
		token && dispatch(GetTopActiveAgentsDataAsync(token));
	}

	async function GetAllConfigurationsData() {
		const token = await fetchToken();
		token && dispatch(GetAllConfigurations(token));
	}

	async function fetchToken() {
		try {
			if (authContext.isAuthenticated && !authContext.isLoading)
				return await authContext.getAccessToken();
		} catch (error) {
			console.error(error);
		}
	}

	return !authContext.isAuthenticated ? (
		<FullScreenLoading
			isLoading={true}
			message={DashboardConstants.LoadingConstants.LoginRedirectLoader}
		/>
	) : IsLoadingStoreData ? (
		<FullScreenLoading
			isLoading={IsLoadingStoreData}
			message={DashboardConstants.LoadingConstants.MainLoader}
		/>
	) : (
		<MainLayout isFullWidth={true} contentClassName="p-6">
			<div className="space-y-6">
				<WelcomeCardComponent />

				{/* Main Grid */}
				<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 auto-rows-fr">
					<ActiveAgentsTileComponent />
					<QuickActionsTileComponent />
					<SystemHealthTileComponent />
				</div>
			</div>
			<DirectChatComponent />
			<FeedbackComponent />
			<FooterComponent />
		</MainLayout>
	);
}
