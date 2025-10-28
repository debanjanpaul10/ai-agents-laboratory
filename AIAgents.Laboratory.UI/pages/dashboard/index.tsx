import { useEffect } from "react";
import { useMsal } from "@azure/msal-react";
import { LogOut } from "lucide-react";
import Image from "next/image";
import { Button } from "@heroui/react";

import AppLogo from "@public/images/icon.png";
import { useAuth } from "@auth/AuthProvider";
import ActiveAgentsTileComponent from "@components/dashboard/active-agents-tile";
import { useAppDispatch, useAppSelector } from "@store/index";
import {
	GetAllAgentsDataAsync,
	GetConversationHistoryDataForUserAsync,
} from "@store/agents/actions";
import { FullScreenLoading } from "@components/common/spinner";
import ManageAgentsTileComponent from "@components/dashboard/manage-agents-tile";
import CreateAgentsTileComponent from "@components/dashboard/create-agents-tile";
import StatsSectionTilesComponent from "@components/dashboard/stats-section-tiles";
import FooterComponent from "@components/common/footer";
import WelcomeCardComponent from "@components/dashboard/welcome-card";
import DirectChatComponent from "@components/direct-chat";
import { DashboardConstants } from "@helpers/constants";

export default function DashboardComponent() {
	const dispatch = useAppDispatch();
	const { instance } = useMsal();
	const authContext = useAuth();

	const IsLoadingStoreData = useAppSelector(
		(state) => state.CommonReducer.isLoading
	);

	useEffect(() => {
		if (authContext.isAuthenticated && !authContext.isLoading) {
			GetAllAgentsData();
			LoadConversationHistory();
		}
	}, [authContext.isAuthenticated, authContext.isLoading]);

	const handleLogout = () => {
		instance.logoutRedirect().catch((e) => {
			console.error(e);
		});
		localStorage.clear();
	};

	const GetAllAgentsData = async () => {
		const token = await fetchToken();
		token && dispatch(GetAllAgentsDataAsync(token));
	};

	const LoadConversationHistory = async () => {
		const token = await fetchToken();
		token && dispatch(GetConversationHistoryDataForUserAsync(token));
	};

	const fetchToken = async () => {
		try {
			if (authContext.isAuthenticated && !authContext.isLoading)
				return await authContext.getAccessToken();
		} catch (error) {
			console.error(error);
		}
	};

	const handleUnAuthorizedUser = () => {
		return (
			<FullScreenLoading
				isLoading={true}
				message={
					DashboardConstants.LoadingConstants.LoginRedirectLoader
				}
			/>
		);
	};

	const renderAuthorizedDashboard = () => {
		return (
			<div className="min-h-screen bg-gradient-to-br from-gray-900 via-slate-900 to-black p-8 relative overflow-hidden">
				{/* Animated background elements */}
				<div className="absolute top-20 left-20 w-64 h-64 bg-gradient-to-br from-blue-500/10 to-purple-600/10 rounded-full blur-3xl animate-pulse"></div>
				<div className="absolute bottom-32 right-32 w-48 h-48 bg-gradient-to-br from-green-400/10 to-blue-500/10 rounded-full blur-2xl animate-pulse delay-1000"></div>
				<div className="absolute top-1/2 left-1/4 w-32 h-32 bg-gradient-to-br from-purple-400/10 to-pink-500/10 rounded-full blur-xl animate-pulse delay-2000"></div>

				<div className="max-w-7xl mx-auto relative z-10">
					{/* Main glassmorphic container */}
					<div className="relative group">
						{/* Glow effect behind main container */}
						<div className="absolute -inset-1 bg-gradient-to-r from-blue-600/20 via-purple-600/20 to-cyan-600/20 rounded-3xl blur-lg opacity-75 group-hover:opacity-100 transition duration-1000 animate-pulse"></div>

						<div className="relative bg-white/5 backdrop-blur-xl rounded-3xl border border-white/10 shadow-2xl p-8">
							{/* Header */}
							<div className="flex justify-between items-start mb-8">
								<div className="flex items-center space-x-4">
									<div className="bg-gradient-to-r from-blue-500 to-purple-600 p-3 rounded-2xl">
										<Image
											src={AppLogo}
											alt="App Icon"
											height={50}
											width={50}
										/>
									</div>
									<div>
										<h1 className="text-2xl md:text-4xl font-bold bg-gradient-to-r from-white via-blue-100 to-cyan-100 bg-clip-text text-transparent drop-shadow-2xl">
											AI Agents Laboratory
										</h1>
										<p className="text-white/60 text-xs md:text-sm mt-1">
											Advanced AI Management Platform
										</p>
									</div>
								</div>

								{/* Logout button with glassmorphic effect - responsive sizing */}
								<div className="relative group/button">
									<div className="absolute -inset-0.5 bg-gradient-to-r from-red-500 to-pink-500 rounded-xl blur opacity-75 group-hover/button:opacity-100 transition duration-300"></div>
									<Button
										onPress={handleLogout}
										radius="full"
										title="Logout from application"
										className="relative bg-red-500/20 backdrop-blur-sm hover:bg-red-500/30 text-white font-semibold rounded-xl border border-red-500/30 hover:border-red-400/50 transition-all duration-300 hover:scale-105 shadow-lg hover:shadow-red-500/25"
									>
										<span className="flex items-center space-x-1 md:space-x-2">
											<LogOut className="w-4 h-4 md:w-5 md:h-5" />
											<span className="text-xs md:text-base">
												Logout
											</span>
										</span>
									</Button>
								</div>
							</div>

							<WelcomeCardComponent />

							{/* Feature cards grid */}
							<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
								<ActiveAgentsTileComponent />
								<CreateAgentsTileComponent />
								<ManageAgentsTileComponent />
							</div>

							{/* Bottom stats section */}
							<StatsSectionTilesComponent />
						</div>
					</div>
				</div>
				<DirectChatComponent />
				<FooterComponent />
			</div>
		);
	};

	return !authContext.isAuthenticated ? (
		handleUnAuthorizedUser()
	) : IsLoadingStoreData ? (
		<FullScreenLoading
			isLoading={IsLoadingStoreData}
			message={DashboardConstants.LoadingConstants.MainLoader}
		/>
	) : (
		renderAuthorizedDashboard()
	);
}
