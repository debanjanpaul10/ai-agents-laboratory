import { useEffect } from "react";
import { Store } from "lucide-react";

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
			<MainLayout>
				<div className="flex flex-col items-center justify-center h-[calc(100vh-4rem)] bg-white/5 rounded-3xl border border-white/10 p-8 text-center backdrop-blur-sm">
					<div className="bg-gradient-to-r from-blue-500 to-purple-600 p-6 rounded-full mb-6">
						{/* Using a lucide icon for Store if available, or just text */}
						<Store className="h-40 w-40" />
					</div>
					<h1 className="text-3xl font-bold bg-gradient-to-r from-white via-blue-100 to-purple-100 bg-clip-text text-transparent mb-4">
						{MarketplaceConstants.ComingSoonConstants.Header}
					</h1>
					<p className="text-white/60 max-w-md mx-auto text-lg">
						{MarketplaceConstants.ComingSoonConstants.SubHeading}
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
