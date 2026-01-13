import { useEffect } from "react";
import { Laptop } from "lucide-react";

import { useAuth } from "@auth/AuthProvider";
import { useAppDispatch, useAppSelector } from "@store/index";
import { ShowErrorToaster } from "@shared/toaster";
import { GetAllConfigurations } from "@store/common/actions";
import { FullScreenLoading } from "@components/common/spinner";
import { WorkspacesConstants } from "@helpers/constants";
import MainLayout from "@components/common/main-layout";

export default function WorkspacesComponent() {
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

	async function GetAllConfigurationsData() {
		const token = await fetchToken();
		token && dispatch(GetAllConfigurations(token));
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

	const renderAuthorizedWorkspaces = () => {
		return (
			<MainLayout title="Skill Marketplace">
				<div className="flex flex-col items-center justify-center h-[calc(100vh-10rem)] bg-white/5 rounded-3xl border border-white/10 p-8 text-center backdrop-blur-sm">
					<div className="bg-gradient-to-r from-blue-500 to-purple-600 p-6 rounded-full mb-6">
						<Laptop className="h-40 w-40" />
					</div>
					<h1 className="text-3xl font-bold bg-gradient-to-r from-white via-blue-100 to-purple-100 bg-clip-text text-transparent mb-4">
						{WorkspacesConstants.ComingSoonConstants.Header}
					</h1>
					<p className="text-white/60 max-w-md mx-auto text-lg">
						{WorkspacesConstants.ComingSoonConstants.SubHeading}
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
			message={WorkspacesConstants.LoadingConstants.MainLoader}
		/>
	) : (
		renderAuthorizedWorkspaces()
	);
}
