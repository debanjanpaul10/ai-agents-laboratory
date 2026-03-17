import { useEffect } from "react";

import { useAuth } from "@auth/AuthProvider";
import MainLayout from "@components/common/main-layout";
import { FullScreenLoading } from "@components/common/spinner";
import { RegisterApplicationsConstants } from "@helpers/constants";
import { GetAllConfigurations } from "@store/common/actions";
import { useAppDispatch, useAppSelector } from "@store/index";
import { GetAllRegisteredApplicationsAsync } from "@store/tools-skills/actions";

export default function RegisterApplicationsComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const IsRegisteredApplicationsLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isRegisteredApplicationsLoading,
	);
	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations,
	);

	useEffect(() => {
		if (authContext.isAuthenticated && !authContext.isLoading) {
			GetAllRegisteredApplicationsData();
			if (
				!ConfigurationsStoreData ||
				Object.keys(ConfigurationsStoreData).length === 0
			)
				GetAllConfigurationsData();
		}
	}, [authContext.isAuthenticated, authContext.isLoading]);

	const GetAllRegisteredApplicationsData = () => {
		dispatch(GetAllRegisteredApplicationsAsync());
	};

	const GetAllConfigurationsData = () => {
		dispatch(GetAllConfigurations());
	};

	const renderRegisterApplications = () =>
		IsRegisteredApplicationsLoading ? (
			<FullScreenLoading
				isLoading={IsRegisteredApplicationsLoading}
				message={
					RegisterApplicationsConstants.LoadingConstants.MainLoader
				}
			/>
		) : (
			<MainLayout contentClassName="p-0" isFullWidth={true}>
				<div className="w-full h-screen overflow-hidden bg-gradient-to-br from-gray-900 via-slate-900 to-black">
					Hello world
				</div>
			</MainLayout>
		);

	return authContext.isAuthenticated ? (
		renderRegisterApplications()
	) : (
		<FullScreenLoading
			isLoading={true}
			message={
				RegisterApplicationsConstants.LoadingConstants
					.LoginRedirectLoader
			}
		/>
	);
}
