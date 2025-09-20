import { useEffect, useState } from "react";
import { Bot, X } from "lucide-react";

import { useAuth } from "@/auth/AuthProvider";
import { ManageAgentConstants } from "@/helpers/constants";
import { useAppDispatch, useAppSelector } from "@/store";
import { ToggleAgentsListDrawer } from "@/store/common/actions";

export default function ManageAgentsComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const [agentsListDrawerOpen, setAgentsListDrawerOpen] = useState(false);

	const AgentsListStoreData = useAppSelector(
		(state) => state.AgentsReducer.agentsListData
	);
	const IsAgentsListDrawerOpenStoreData = useAppSelector(
		(state) => state.CommonReducer.isAgentsListDrawerOpen
	);

	useEffect(() => {
		if (agentsListDrawerOpen !== IsAgentsListDrawerOpenStoreData) {
			setAgentsListDrawerOpen(IsAgentsListDrawerOpenStoreData);
		}
	}, [IsAgentsListDrawerOpenStoreData]);

	useEffect(() => {
		if (agentsListDrawerOpen) {
			document.body.style.overflow = "hidden";
		} else {
			document.body.style.overflow = "unset";
		}

		// Cleanup on unmount
		return () => {
			document.body.style.overflow = "unset";
		};
	}, [agentsListDrawerOpen]);

	const onClose = () => {
		dispatch(ToggleAgentsListDrawer(false));
	};

	if (!agentsListDrawerOpen) return null;

	return (
		<>
			{/* Backdrop overlay */}
			<div
				className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300"
				onClick={onClose}
			/>

			{/* Drawer */}
			<div
				className={`fixed right-0 top-0 h-screen z-50 transition-all duration-500 ease-in-out ${"w-full max-w-md"}`}
			>
				{/* Glow effect */}
				<div className="absolute -inset-1 bg-gradient-to-l from-purple-600/20 via-blue-600/20 to-cyan-600/20 blur-lg opacity-75"></div>

				{/* Main drawer content */}
				<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
					{/* Header */}
					<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
						<div className="flex items-center space-x-3">
							<div className="bg-gradient-to-r from-purple-500 to-blue-600 p-2 rounded-xl">
								<Bot className="w-5 h-5 text-white" />
							</div>
							<div>
								<h2 className="text-xl font-bold bg-gradient-to-r from-white via-blue-100 to-purple-100 bg-clip-text text-transparent">
									{ManageAgentConstants.Headers.SubText}
								</h2>
							</div>
						</div>
						<div className="flex items-center space-x-2">
							{/* Close button */}
							<button
								onClick={onClose}
								className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
							>
								<X className="w-4 h-4" />
							</button>
						</div>
					</div>

					{/* Form content */}

					{/* Footer with buttons */}
				</div>
			</div>
		</>
	);
}
