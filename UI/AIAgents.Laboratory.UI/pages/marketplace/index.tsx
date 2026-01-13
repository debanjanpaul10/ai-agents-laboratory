import { useEffect } from "react";

import { useAuth } from "@auth/AuthProvider";
import { ShowErrorToaster } from "@shared/toaster";
import { useAppDispatch, useAppSelector } from "@store/index";
import { GetAllToolSkillsAsync } from "@store/tools-skills/actions";
import { GetAllConfigurations } from "@store/common/actions";
import { FullScreenLoading } from "@components/common/spinner";
import { MarketplaceConstants } from "@helpers/constants";
import MainLayout from "@components/common/main-layout";

export default function MarketplaceComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const IsSkillsMarketPlaceLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isToolSkillsLoading
	);

	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations
	);

	useEffect(() => {
		if (authContext.isAuthenticated && !authContext.isLoading) {
			GetAllToolSkillsData();
			if (
				!ConfigurationsStoreData ||
				Object.keys(ConfigurationsStoreData).length === 0
			) {
				GetAllConfigurationsData();
			}
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

	async function GetAllToolSkillsData() {
		const token = await fetchToken();
		token && dispatch(GetAllToolSkillsAsync(token));
	}

	async function GetAllConfigurationsData() {
		const token = await fetchToken();
		token && dispatch(GetAllConfigurations(token));
	}

	const handleUnAuthorizedUser = () => {
		return (
			<FullScreenLoading
				isLoading={true}
				message={
					MarketplaceConstants.LoadingConstants.LoginRedirectLoader
				}
			/>
		);
	};

	const renderAuthorizedMarketplace = () => {
		return (
			<MainLayout title="Skill Marketplace">
				<div className="flex flex-col items-center justify-center h-[calc(100vh-10rem)] bg-white/5 rounded-3xl border border-white/10 p-8 text-center backdrop-blur-sm">
					<div className="bg-gradient-to-r from-blue-500 to-purple-600 p-6 rounded-full mb-6">
						{/* Using a lucide icon for Store if available, or just text */}
						<svg
							xmlns="http://www.w3.org/2000/svg"
							width="48"
							height="48"
							viewBox="0 0 24 24"
							fill="none"
							stroke="currentColor"
							strokeWidth="2"
							strokeLinecap="round"
							strokeLinejoin="round"
							className="text-white"
						>
							<path d="m2 7 4.41-4.41A2 2 0 0 1 7.83 2h8.34a2 2 0 0 1 1.42.59L22 7" />
							<path d="M4 12v8a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-8" />
							<path d="M15 22v-4a2 2 0 0 0-2-2h-2a2 2 0 0 0-2 2v4" />
							<path d="M2 7h20" />
							<path d="M22 7v3a2 2 0 0 1-2 2v0a2.7 2 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2 0 0 1 16 12a2.7 2 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2 0 0 1 12 12a2.7 2 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2 0 0 1 8 12a2.7 2 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2 0 0 1 4 12v0a2 2 0 0 1-2-2V7" />
						</svg>
					</div>
					<h1 className="text-3xl font-bold bg-gradient-to-r from-white via-blue-100 to-purple-100 bg-clip-text text-transparent mb-4">
						Skill Marketplace Coming Soon
					</h1>
					<p className="text-white/60 max-w-md mx-auto text-lg">
						Explore and add new capabilities to your AI agents from
						our curated marketplace of skills and tools.
					</p>
				</div>
			</MainLayout>
		);
	};

	return !authContext.isAuthenticated ? (
		handleUnAuthorizedUser()
	) : IsSkillsMarketPlaceLoading ? (
		<FullScreenLoading
			isLoading={IsSkillsMarketPlaceLoading}
			message={MarketplaceConstants.LoadingConstants.MainLoader}
		/>
	) : (
		renderAuthorizedMarketplace()
	);
}
