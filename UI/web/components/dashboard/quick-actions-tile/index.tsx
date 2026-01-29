import {
	Zap,
	ArrowRight,
	MessageCircleCode,
	Bug,
	PackagePlus,
} from "lucide-react";

import { useAppDispatch } from "@store/index";
import {
	ToggleDirectChatDrawer,
	ToggleFeedbackDrawer,
} from "@store/common/actions";
import { FEEDBACK_TYPES } from "@shared/types";
import { DashboardConstants } from "@helpers/constants";

export default function QuickActionsTileComponent() {
	const dispatch = useAppDispatch();

	const toggleDirectChatDrawer = () => {
		dispatch(ToggleDirectChatDrawer(true));
	};

	const toggleBugReportDrawer = () => {
		dispatch(ToggleFeedbackDrawer(true, FEEDBACK_TYPES.BUGREPORT));
	};

	const toggleFeatureRequestDrawer = () => {
		dispatch(ToggleFeedbackDrawer(true, FEEDBACK_TYPES.NEWFEATURE));
	};

	const actions = [
		{
			label: "Chat with an AI Agent",
			description:
				"Chat with our sarcastic and exemplary AI conversation agent",
			icon: <MessageCircleCode className="w-5 h-5 text-white" />,
			onClick: toggleDirectChatDrawer,
			gradient: "from-cyan-500 to-blue-500",
			shadow: "shadow-cyan-500/25",
		},
		{
			label: "Request a new feature",
			description: "Request for a new feature that you would like to see",
			icon: <PackagePlus className="w-5 h-5 text-white" />,
			onClick: () => {
				toggleFeatureRequestDrawer();
			},
			gradient: "from-emerald-500 to-teal-500",
			shadow: "shadow-emerald-500/25",
		},
		{
			label: "Report a Bug",
			description: "Help us improve the platform by telling on our flaws",
			icon: <Bug className="w-5 h-5 text-white" />,
			onClick: () => {
				toggleBugReportDrawer();
			},
			gradient: "from-red-500 to-red-500",
			shadow: "shadow-red-500/25",
		},
	];

	return (
		<div className="relative group/card h-full">
			<div className="absolute inset-0 bg-gradient-to-r from-purple-500/20 via-fuchsia-600/20 to-pink-500/20 rounded-2xl blur-sm opacity-50 group-hover/card:opacity-75 transition duration-500 -z-10"></div>
			<div className="relative bg-gradient-to-br from-gray-800/90 via-gray-900/90 to-black/90 backdrop-blur-xl text-white p-6 rounded-2xl border border-purple-500/20 hover:border-purple-400/40 transition-all duration-500 hover:scale-[1.02] h-full flex flex-col shadow-2xl">
				{/* Header */}
				<div className="flex items-center justify-between mb-6">
					<div className="flex items-center space-x-3">
						<div className="bg-purple-500/20 backdrop-blur-sm p-3 rounded-xl border border-purple-500/30">
							<Zap className="w-5 h-5 text-purple-400" />
						</div>
						<h3 className="text-xl font-bold text-white">
							{DashboardConstants.QuickActionsTile.Header}
						</h3>
					</div>
					<div className="w-2 h-2 bg-purple-400 rounded-full animate-pulse"></div>
				</div>

				{/* Actions Grid */}
				<div className="flex flex-col space-y-3 flex-1">
					{actions.map((action, _) => {
						return (
							<button
								key={action.label}
								onClick={action.onClick}
								className="w-full bg-gray-700/30 hover:bg-gray-700/50 backdrop-blur-sm rounded-xl p-3 border border-gray-600/30 hover:border-white/20 transition-all duration-300 group cursor-pointer"
							>
								<div className="flex items-center justify-between w-full">
									<div className="flex items-center space-x-4">
										<div
											className={`p-2.5 rounded-lg bg-gradient-to-br ${action.gradient} ${action.shadow} shadow-lg`}
										>
											{action.icon}
										</div>
										<div className="text-left">
											<p className="font-semibold text-white text-sm">
												{action.label}
											</p>
											<p className="text-gray-400 text-xs">
												{action.description}
											</p>
										</div>
									</div>
									<ArrowRight className="w-4 h-4 text-gray-500 group-hover:text-white group-hover:translate-x-1 transition-all" />
								</div>
							</button>
						);
					})}
				</div>

				<div className="mt-6 pt-4 border-t border-white/5">
					<p className="text-xs text-center text-gray-500 animate-pulse">
						{DashboardConstants.QuickActionsTile.SubText}
					</p>
				</div>
			</div>
		</div>
	);
}
