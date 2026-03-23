import { useEffect, useState } from "react";
import { Shield, Home, Bug, Lightbulb } from "lucide-react";
import { Button } from "@heroui/react";
import { useRouter } from "next/router";

import { useAuth } from "@auth/AuthProvider";
import MainLayout from "@components/common/main-layout";
import { FullScreenLoading } from "@components/common/spinner";
import { AdminPageConstants, DashboardConstants } from "@helpers/constants";
import { GetAllConfigurations } from "@store/common/actions";
import { useAppDispatch, useAppSelector } from "@store/index";
import { IsAdminAccessEnabledAsync } from "@store/app-admin/actions";
import BugReportsAdminComponent from "@components/admin/bug-reports";
import FeatureRequestsAdminComponent from "@components/admin/feature-requests";

export default function AdminpageComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();
	const router = useRouter();

	const [activeTab, setActiveTab] = useState<
		"bug-reports" | "feature-requests"
	>("bug-reports");

	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations,
	);
	const IsAdminAccessEnabledStoreData = useAppSelector(
		(state) => state.ApplicationAdminReducer.isAdminAccessEnabledForUser,
	);

	useEffect(() => {
		if (authContext.isAuthenticated && !authContext.isLoading) {
			dispatch(IsAdminAccessEnabledAsync());
			if (
				!ConfigurationsStoreData ||
				Object.keys(ConfigurationsStoreData).length === 0
			)
				GetAllConfigurationsData();
		}
	}, [authContext.isAuthenticated, authContext.isLoading]);

	function GetAllConfigurationsData() {
		dispatch(GetAllConfigurations());
	}

	if (authContext.isAuthenticated && IsAdminAccessEnabledStoreData) {
		return (
			<MainLayout isFullWidth={true} contentClassName="p-0">
				<div className="w-full h-screen overflow-hidden bg-gradient-to-br from-gray-900 via-slate-900 to-black">
					<div className="p-6">
						<div className="mb-8">
							<h1 className="text-3xl font-bold text-white mb-2">
								Admin Dashboard
							</h1>
							<p className="text-gray-400">
								Manage the application and its features
							</p>
						</div>

						{/* Tab Navigation */}
						<div className="flex space-x-1 mb-6">
							<Button
								onPress={() => setActiveTab("bug-reports")}
								className={`px-6 py-3 rounded-xl font-medium transition-all duration-300 ${
									activeTab === "bug-reports"
										? "bg-red-500/20 border border-red-500/50 text-red-300 shadow-lg"
										: "bg-white/5 border border-white/10 hover:border-red-500/30 hover:bg-red-500/10 text-gray-300 hover:text-red-300"
								}`}
							>
								<Bug className="w-4 h-4 mr-2" />
								Bug Reports
							</Button>
							<Button
								onPress={() => setActiveTab("feature-requests")}
								className={`px-6 py-3 rounded-xl font-medium transition-all duration-300 ${
									activeTab === "feature-requests"
										? "bg-blue-500/20 border border-blue-500/50 text-blue-300 shadow-lg"
										: "bg-white/5 border border-white/10 hover:border-blue-500/30 hover:bg-blue-500/10 text-gray-300 hover:text-blue-300"
								}`}
							>
								<Lightbulb className="w-4 h-4 mr-2" />
								Feature Requests
							</Button>
						</div>

						{/* Tab Content */}
						<div className="bg-white/5 border border-white/10 rounded-xl p-6 backdrop-blur-sm">
							{activeTab === "bug-reports" && (
								<BugReportsAdminComponent />
							)}
							{activeTab === "feature-requests" && (
								<FeatureRequestsAdminComponent />
							)}
						</div>
					</div>
				</div>
			</MainLayout>
		);
	}

	if (authContext.isAuthenticated && !IsAdminAccessEnabledStoreData) {
		return (
			<MainLayout isFullWidth={true} contentClassName="p-6">
				<div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-slate-900 to-slate-800">
					<div className="text-center max-w-md">
						<div className="flex justify-center mb-6">
							<div className="bg-red-500/20 p-6 rounded-full">
								<Shield className="h-16 w-16 text-red-500" />
							</div>
						</div>

						<h1 className="text-4xl font-bold text-white mb-2">
							{AdminPageConstants.UnauthorizedAccess.Header}
						</h1>

						<p className="text-gray-300 text-lg mb-2">
							{AdminPageConstants.UnauthorizedAccess.SubHeader}
						</p>

						<p className="text-gray-400 mb-8">
							{AdminPageConstants.UnauthorizedAccess.Subtitle}
						</p>

						<Button
							onPress={() => router.push("/dashboard")}
							className="bg-gradient-to-r from-blue-500 to-purple-600 hover:from-blue-600 hover:to-purple-700 text-white font-semibold py-3 px-6 rounded-lg transition-all"
						>
							<Home className="h-5 w-5 mr-2" />
							Back to Dashboard
						</Button>
					</div>
				</div>
			</MainLayout>
		);
	}

	return (
		<FullScreenLoading
			isLoading={true}
			message={DashboardConstants.LoadingConstants.LoginRedirectLoader}
		/>
	);
}
