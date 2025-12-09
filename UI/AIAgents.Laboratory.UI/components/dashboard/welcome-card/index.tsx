import { useMsal } from "@azure/msal-react";
import { Bug, MessageCircleCode, PackagePlus, Spotlight } from "lucide-react";
import { Button, Tooltip } from "@heroui/react";

import {
	ToggleDirectChatDrawer,
	ToggleFeedbackDrawer,
} from "@store/common/actions";
import { useAppDispatch, useAppSelector } from "@store/index";
import { FEEDBACK_TYPES } from "@shared/types";

export default function WelcomeCardComponent() {
	const { accounts } = useMsal();
	const dispatch = useAppDispatch();

	const ConfigurationStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations
	);

	const toggleDirectChatDrawer = () => {
		dispatch(ToggleDirectChatDrawer(true));
	};

	const toggleBugReportDrawer = () => {
		dispatch(ToggleFeedbackDrawer(true, FEEDBACK_TYPES.BUGREPORT));
	};

	const toggleFeatureRequestDrawer = () => {
		dispatch(ToggleFeedbackDrawer(true, FEEDBACK_TYPES.NEWFEATURE));
	};

	const renderFeedbackFeatures = () => {
		return ConfigurationStoreData.IsFeedbackFeatureEnabled === "true" ? (
			<>
				{/* BUG REPORT BUTTON */}
				<div className="relative">
					<Tooltip content="Submit a bug report">
						<Button
							onPress={toggleBugReportDrawer}
							radius="full"
							className="group relative bg-gradient-to-r from-red-400 to-red-500 text-white font-semibold hover:from-red-500 hover:to-red-600 shadow-lg hover:shadow-rose-500/50 transition-all duration-300 overflow-hidden whitespace-nowrap disabled:opacity-50"
						>
							<span className="flex items-center justify-center space-x-2">
								<Bug />
							</span>
						</Button>
					</Tooltip>
				</div>
				{/* FEATURE REQUEST BUTTON */}
				<div className="relative">
					<Tooltip content="Request a new feature">
						<Button
							onPress={toggleFeatureRequestDrawer}
							radius="full"
							className="group relative bg-gradient-to-r from-emerald-400 to-teal-500 text-white font-semibold hover:from-emerald-500 hover:to-teal-600 shadow-lg hover:shadow-green-500/50 transition-all duration-300 overflow-hidden whitespace-nowrap disabled:opacity-50"
						>
							<span className="flex items-center justify-center space-x-2">
								<PackagePlus />
							</span>
						</Button>
					</Tooltip>
				</div>
			</>
		) : null;
	};

	return (
		accounts[0] && (
			<div className="mb-8 relative group/welcome">
				<div className="absolute -inset-0.5 bg-gradient-to-r from-blue-500/30 to-purple-500/30 rounded-2xl blur opacity-50 group-hover/welcome:opacity-75 transition duration-500"></div>
				<div className="relative bg-white/5 backdrop-blur-sm rounded-2xl p-4 md:p-6 border border-white/10 hover:border-white/20 transition-all duration-300">
					<div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
						<div className="flex items-center space-x-4">
							<div className="w-10 h-10 md:w-12 md:h-12 bg-gradient-to-r from-blue-500 to-purple-600 rounded-full flex items-center justify-center">
								<span className="text-white font-bold text-base md:text-lg">
									{(accounts[0].name || accounts[0].username)
										.charAt(0)
										.toUpperCase()}
								</span>
							</div>
							<div>
								<h2 className="text-base md:text-xl font-semibold text-white mb-1">
									Welcome,{" "}
									{accounts[0].name || accounts[0].username}!
								</h2>
								<p className="text-white/70 flex items-center space-x-2 text-xs md:text-base">
									<Spotlight />
									<span>{accounts[0].username}</span>
								</p>
							</div>
						</div>

						<div className="flex items-center space-x-2">
							{/* AI AGENT CHAT BUTTON */}
							<div className="relative">
								<Tooltip content="Chat with an exemplary AI conversation agent">
									<Button
										onPress={toggleDirectChatDrawer}
										radius="full"
										className="group relative bg-gradient-to-r from-cyan-400 to-blue-500 text-white font-semibold hover:from-cyan-500 hover:to-blue-600 shadow-lg hover:shadow-blue-500/50 transition-all duration-300 overflow-hidden whitespace-nowrap disabled:opacity-50"
									>
										<span className="flex items-center justify-center space-x-2">
											<MessageCircleCode />
											<span>Chat with AI Agent</span>
										</span>
									</Button>
								</Tooltip>
							</div>
							{renderFeedbackFeatures()}
						</div>
					</div>
				</div>
			</div>
		)
	);
}
