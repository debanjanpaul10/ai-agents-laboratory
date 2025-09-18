"use client";

import { useEffect } from "react";
import { useMsal } from "@azure/msal-react";
import { Bolt, Bot, FilePlus, LogOut, Spotlight } from "lucide-react";

import { useAuth } from "@/auth/AuthProvider";
import ActiveAgentsComponent from "@/components/dashboard/activeAgents";
import { useAppDispatch, useAppSelector } from "@/store";
import { GetAllAgentsDataAsync } from "@/store/agents/actions";
import { FullScreenLoading } from "@/components/ui/loading-spinner";

export default function DashboardComponent() {
	const dispatch = useAppDispatch();
	const { instance, accounts } = useMsal();
	const authContext = useAuth();

	const IsLoadingStoreData = useAppSelector(
		(state) => state.AgentsReducer.isLoading
	);

	useEffect(() => {
		// Only fetch data when user is authenticated and not loading
		if (authContext.isAuthenticated && !authContext.isLoading)
			GetAllAgentsData();
	}, [authContext.isAuthenticated, authContext.isLoading]);

	const handleLogout = () => {
		instance.logoutRedirect().catch((e) => {
			console.error(e);
		});
	};

	const GetAllAgentsData = async () => {
		const token = await fetchToken();
		token && dispatch(GetAllAgentsDataAsync(token));
	};

	const fetchToken = async () => {
		try {
			if (authContext.isAuthenticated && !authContext.isLoading)
				return await authContext.getAccessToken();
		} catch (error) {
			console.error(error);
		}
	};

	const handleManageAgents = () => {
		// Navigate to manage agents page
		console.log("Navigate to manage agents");
		// router.push('/agents/manage');
	};

	const handleCreateAgent = () => {
		// Navigate to create agent page
		console.log("Navigate to create agent");
		// router.push('/agents/create');
	};

	// Show loading screen during authentication or API call
	if (authContext.isLoading || IsLoadingStoreData) {
		return (
			<FullScreenLoading
				isLoading={true}
				message={
					authContext.isLoading
						? "Authenticating..."
						: "Loading AI Agents Data..."
				}
			/>
		);
	}

	// If not authenticated, don't render dashboard
	if (!authContext.isAuthenticated) {
		return (
			<FullScreenLoading
				isLoading={true}
				message="Redirecting to login..."
			/>
		);
	}

	// Show dashboard only after data is loaded
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
						<div className="flex justify-between items-center mb-8">
							<div className="flex items-center space-x-4">
								<div className="bg-gradient-to-r from-blue-500 to-purple-600 p-3 rounded-2xl">
									<Bot />
								</div>
								<div>
									<h1 className="text-4xl font-bold bg-gradient-to-r from-white via-blue-100 to-cyan-100 bg-clip-text text-transparent drop-shadow-2xl">
										AI Agents Laboratory
									</h1>
									<p className="text-white/60 text-sm mt-1">
										Advanced AI Management Platform
									</p>
								</div>
							</div>

							{/* Logout button with glassmorphic effect */}
							<div className="relative group/button">
								<div className="absolute -inset-0.5 bg-gradient-to-r from-red-500 to-pink-500 rounded-xl blur opacity-75 group-hover/button:opacity-100 transition duration-300"></div>
								<button
									onClick={handleLogout}
									className="relative bg-red-500/20 backdrop-blur-sm hover:bg-red-500/30 text-white font-semibold py-3 px-6 rounded-xl border border-red-500/30 hover:border-red-400/50 transition-all duration-300 hover:scale-105 shadow-lg hover:shadow-red-500/25"
								>
									<span className="flex items-center space-x-2">
										<LogOut />
										<span>Logout</span>
									</span>
								</button>
							</div>
						</div>

						{/* Welcome card */}
						{accounts[0] && (
							<div className="mb-8 relative group/welcome">
								<div className="absolute -inset-0.5 bg-gradient-to-r from-blue-500/30 to-purple-500/30 rounded-2xl blur opacity-50 group-hover/welcome:opacity-75 transition duration-500"></div>
								<div className="relative bg-white/5 backdrop-blur-sm rounded-2xl p-6 border border-white/10 hover:border-white/20 transition-all duration-300">
									<div className="flex items-center space-x-4">
										<div className="w-12 h-12 bg-gradient-to-r from-blue-500 to-purple-600 rounded-full flex items-center justify-center">
											<span className="text-white font-bold text-lg">
												{(
													accounts[0].name ||
													accounts[0].username
												)
													.charAt(0)
													.toUpperCase()}
											</span>
										</div>
										<div>
											<h2 className="text-xl font-semibold text-white mb-1">
												Welcome,{" "}
												{accounts[0].name ||
													accounts[0].username}
												!
											</h2>
											<p className="text-white/70 flex items-center space-x-2">
												<Spotlight />
												<span>
													{accounts[0].username}
												</span>
											</p>
										</div>
									</div>
								</div>
							</div>
						)}

						{/* Feature cards grid */}
						<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
							<ActiveAgentsComponent />

							{/* Manage Agents Card */}
							<div
								className="relative group/card cursor-pointer"
								onClick={handleManageAgents}
							>
								<div className="absolute -inset-0.5 bg-gradient-to-r from-green-500/30 via-emerald-600/30 to-teal-500/30 rounded-2xl blur opacity-50 group-hover/card:opacity-75 transition duration-500"></div>
								<div className="relative bg-gradient-to-br from-gray-800/90 via-gray-900/90 to-black/90 backdrop-blur-xl text-white p-6 rounded-2xl border border-green-500/20 hover:border-green-400/40 transition-all duration-500 hover:scale-105 min-h-[280px] flex flex-col shadow-2xl">
									<div className="flex items-center justify-between mb-4">
										<div className="bg-green-500/20 backdrop-blur-sm p-3 rounded-xl border border-green-500/30">
											<Bolt />
										</div>
										<div className="w-2 h-2 bg-green-400 rounded-full animate-pulse delay-500"></div>
									</div>
									<h3 className="text-xl font-bold mb-3 text-white">
										Manage Agents
									</h3>
									<p className="text-gray-300 text-sm leading-relaxed mb-6 flex-1">
										Configure, monitor, and control your
										existing AI agents. View performance
										metrics and adjust settings.
									</p>

									{/* Action indicators */}
									<div className="space-y-2">
										<div className="flex items-center text-gray-400 text-xs">
											<svg
												className="w-3 h-3 mr-2 text-green-400"
												fill="none"
												stroke="currentColor"
												viewBox="0 0 24 24"
											>
												<path
													strokeLinecap="round"
													strokeLinejoin="round"
													strokeWidth={2}
													d="M9 5l7 7-7 7"
												/>
											</svg>
											View all agents
										</div>
										<div className="flex items-center text-gray-400 text-xs">
											<svg
												className="w-3 h-3 mr-2 text-green-400"
												fill="none"
												stroke="currentColor"
												viewBox="0 0 24 24"
											>
												<path
													strokeLinecap="round"
													strokeLinejoin="round"
													strokeWidth={2}
													d="M9 5l7 7-7 7"
												/>
											</svg>
											Edit configurations
										</div>
										<div className="flex items-center text-gray-400 text-xs">
											<svg
												className="w-3 h-3 mr-2 text-green-400"
												fill="none"
												stroke="currentColor"
												viewBox="0 0 24 24"
											>
												<path
													strokeLinecap="round"
													strokeLinejoin="round"
													strokeWidth={2}
													d="M9 5l7 7-7 7"
												/>
											</svg>
											Monitor performance
										</div>
									</div>

									<div className="mt-4 flex items-center text-gray-400 text-xs">
										<span className="w-2 h-2 bg-green-400 rounded-full mr-2 animate-pulse"></span>
										Click to manage
									</div>
								</div>
							</div>

							{/* Create New Agent Card */}
							<div
								className="relative group/card cursor-pointer"
								onClick={handleCreateAgent}
							>
								<div className="absolute -inset-0.5 bg-gradient-to-r from-purple-500/30 via-violet-600/30 to-indigo-500/30 rounded-2xl blur opacity-50 group-hover/card:opacity-75 transition duration-500"></div>
								<div className="relative bg-gradient-to-br from-gray-800/90 via-gray-900/90 to-black/90 backdrop-blur-xl text-white p-6 rounded-2xl border border-purple-500/20 hover:border-purple-400/40 transition-all duration-500 hover:scale-105 min-h-[280px] flex flex-col shadow-2xl">
									<div className="flex items-center justify-between mb-4">
										<div className="bg-purple-500/20 backdrop-blur-sm p-3 rounded-xl border border-purple-500/30">
											<FilePlus />
										</div>
										<div className="w-2 h-2 bg-purple-400 rounded-full animate-pulse delay-1000"></div>
									</div>
									<h3 className="text-xl font-bold mb-3 text-white">
										Create New Agent
									</h3>
									<p className="text-gray-300 text-sm leading-relaxed mb-6 flex-1">
										Build a new AI agent from scratch.
										Configure capabilities, set parameters,
										and deploy to production.
									</p>

									{/* Creation steps */}
									<div className="space-y-2">
										<div className="flex items-center text-gray-400 text-xs">
											<svg
												className="w-3 h-3 mr-2 text-purple-400"
												fill="none"
												stroke="currentColor"
												viewBox="0 0 24 24"
											>
												<path
													strokeLinecap="round"
													strokeLinejoin="round"
													strokeWidth={2}
													d="M9 5l7 7-7 7"
												/>
											</svg>
											Choose agent type
										</div>
										<div className="flex items-center text-gray-400 text-xs">
											<svg
												className="w-3 h-3 mr-2 text-purple-400"
												fill="none"
												stroke="currentColor"
												viewBox="0 0 24 24"
											>
												<path
													strokeLinecap="round"
													strokeLinejoin="round"
													strokeWidth={2}
													d="M9 5l7 7-7 7"
												/>
											</svg>
											Configure settings
										</div>
										<div className="flex items-center text-gray-400 text-xs">
											<svg
												className="w-3 h-3 mr-2 text-purple-400"
												fill="none"
												stroke="currentColor"
												viewBox="0 0 24 24"
											>
												<path
													strokeLinecap="round"
													strokeLinejoin="round"
													strokeWidth={2}
													d="M9 5l7 7-7 7"
												/>
											</svg>
											Deploy & test
										</div>
									</div>

									<div className="mt-4 flex items-center text-gray-400 text-xs">
										<span className="w-2 h-2 bg-purple-400 rounded-full mr-2 animate-pulse"></span>
										Click to create
									</div>
								</div>
							</div>
						</div>

						{/* Bottom stats section */}
						<div className="mt-8 grid grid-cols-1 md:grid-cols-3 gap-4">
							<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
								<div className="flex items-center space-x-3">
									<div className="w-3 h-3 bg-green-400 rounded-full animate-pulse"></div>
									<div>
										<p className="text-white/60 text-xs">
											System Status
										</p>
										<p className="text-white font-semibold">
											All Systems Operational
										</p>
									</div>
								</div>
							</div>
							<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
								<div className="flex items-center space-x-3">
									<div className="w-3 h-3 bg-blue-400 rounded-full animate-pulse"></div>
									<div>
										<p className="text-white/60 text-xs">
											Active Agents
										</p>
										<p className="text-white font-semibold">
											12 Running
										</p>
									</div>
								</div>
							</div>
							<div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
								<div className="flex items-center space-x-3">
									<div className="w-3 h-3 bg-purple-400 rounded-full animate-pulse"></div>
									<div>
										<p className="text-white/60 text-xs">
											API Calls Today
										</p>
										<p className="text-white font-semibold">
											1,247
										</p>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}
