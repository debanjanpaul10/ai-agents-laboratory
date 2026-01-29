import { useEffect, useState } from "react";

import { useAppDispatch, useAppSelector } from "@store/index";
import {
	GetConfigurationByKeyName,
	ToggleDirectChatDrawer,
} from "@store/common/actions";
import AgentChatComponent from "@components/direct-chat/agent-chat";
import ChatbotInformationComponent from "@components/direct-chat/chatbot-information";
import { GetConversationHistoryDataForUserAsync } from "@store/chat/actions";
import { ApplicationConstants } from "@helpers/constants";

export default function DirectChatComponent() {
	const dispatch = useAppDispatch();

	const [isDrawerOpen, setIsDrawerOpen] = useState<boolean>(false);
	const [isAgentInfoDrawerOpen, setIsAgentInfoDrawerOpen] =
		useState<boolean>(false);

	const IsDrawerOpenStoreData = useAppSelector(
		(state) => state.CommonReducer.isDirectChatOpen,
	);

	useEffect(() => {
		if (IsDrawerOpenStoreData !== isDrawerOpen) {
			setIsDrawerOpen(IsDrawerOpenStoreData);
		}
	}, [IsDrawerOpenStoreData]);

	useEffect(() => {
		if (isDrawerOpen) {
			document.body.style.overflow = "hidden";
			dispatch(GetConversationHistoryDataForUserAsync());
			dispatch(
				GetConfigurationByKeyName(
					ApplicationConstants.ChatbotAgentConfigKeyName,
				),
			);
		} else {
			document.body.style.overflow = "unset";
		}

		return () => {
			document.body.style.overflow = "unset";
		};
	}, [isDrawerOpen]);

	const onClose = () => {
		dispatch(ToggleDirectChatDrawer(false));
	};

	const toggleChatbotInformation = () => {
		setIsAgentInfoDrawerOpen(true);
	};

	const onAgentInfoDrawerClose = () => {
		setIsAgentInfoDrawerOpen(false);
	};

	return (
		isDrawerOpen && (
			<>
				<div className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300 max-w-full" />

				{isAgentInfoDrawerOpen && (
					<div className="fixed top-0 md:w-1/2 h-screen z-50 transition-all duration-500 ease-in-out right-1/2">
						<div className="absolute inset-0 bg-gradient-to-r from-green-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-x border-white/10 shadow-2xl">
							<ChatbotInformationComponent
								isAgentInfoDrawerOpen={isAgentInfoDrawerOpen}
								onAgentInfoDrawerClose={onAgentInfoDrawerClose}
							/>
						</div>
					</div>
				)}

				<div className="fixed right-0 top-0 md:w-1/2 h-screen z-50 transition-all duration-500 ease-in-out">
					<div className="absolute inset-0 bg-gradient-to-l from-purple-600/20 via-blue-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10"></div>
					<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
						<AgentChatComponent
							toggleChatbotInformation={toggleChatbotInformation}
							onClose={onClose}
							isAgentInfoDrawerOpen={isAgentInfoDrawerOpen}
						/>
					</div>
				</div>
			</>
		)
	);
}
