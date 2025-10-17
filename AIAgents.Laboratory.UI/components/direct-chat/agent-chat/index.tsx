import { useState } from "react";
import { Button, Input, Tooltip } from "@heroui/react";
import {
	BotMessageSquare,
	Info,
	MessageSquare,
	RefreshCcw,
	Send,
	X,
} from "lucide-react";
import { Action, ThunkDispatch } from "@reduxjs/toolkit";

import { DashboardConstants, ManageAgentConstants } from "@helpers/constants";
import { ChatMessage } from "@shared/types";
import { generateMessageId } from "@shared/utils";
import { DirectChatRequestDTO } from "@models/direct-chat-request-dto";
import { useAuth } from "@auth/AuthProvider";
import { useAppDispatch } from "@store/index";
import {
	ClearConversationHistoryAsync,
	GetDirectChatResponseAsync,
} from "@store/agents/actions";

export default function AgentChatComponent({
	toggleChatbotInformation,
	onClose,
}: {
	toggleChatbotInformation: any;
	onClose: any;
}) {
	const { getAccessToken } = useAuth();
	const dispatch = useAppDispatch();

	const [messages, setMessages] = useState<Array<ChatMessage>>([]);
	const [userInput, setUserInput] = useState("");
	const [isLoading, setIsLoading] = useState(false);

	async function clearConversation() {
		setMessages([]);
		setUserInput("");

		const accessToken = await getAccessToken();
		accessToken && dispatch(ClearConversationHistoryAsync(accessToken));
	}

	async function sendChatbotRequest() {
		if (!userInput.trim()) return;

		const userMessage: ChatMessage = {
			id: generateMessageId(),
			type: "user" as const,
			content: userInput,
			timestamp: new Date(),
		};

		setMessages((prev) => [...prev, userMessage]);
		setUserInput("");
		setIsLoading(true);

		try {
			const chatRequest: DirectChatRequestDTO = {
				userMessage: userMessage.content.trim(),
			};
			const accessToken = await getAccessToken();
			if (accessToken) {
				const aiResponse = (await (
					dispatch as ThunkDispatch<any, any, Action>
				)(GetDirectChatResponseAsync(chatRequest, accessToken))) as
					| string
					| null;

				if (aiResponse) {
					const botMessage = {
						id: generateMessageId(),
						type: "bot" as const,
						content: aiResponse,
						timestamp: new Date(),
					};

					setMessages((prev) => [...prev, botMessage]);
				}
			}
		} catch (error) {
			console.error(error);
		} finally {
			setIsLoading(false);
		}
	}

	return (
		<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
			<div className="h-full flex flex-col">
				{/* HEADER */}
				<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
					<div className="flex items-center space-x-3">
						<div className="bg-gradient-to-r from-cyan-500 to-blue-600 p-2 rounded-xl">
							<BotMessageSquare className="w-5 h-5 text-white" />
						</div>
						<div>
							<h2 className="text-xl font-bold bg-gradient-to-r from-white via-cyan-100 to-blue-100 bg-clip-text text-transparent">
								{
									DashboardConstants.DirectChatConstants
										.Header.MainHeader
								}
							</h2>
							<p className="text-white/60 text-sm">
								{
									DashboardConstants.DirectChatConstants
										.Header.SubHeader
								}
							</p>
						</div>
					</div>
					<div className="flex items-center space-x-2">
						<Tooltip content="Chatbot information">
							<Button
								onPress={toggleChatbotInformation}
								className="p-2 rounded-lg bg-white/5 hover:bg-green-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-green-400"
							>
								<Info className="w-4 h-4" />
							</Button>
						</Tooltip>

						<Tooltip content="Clear conversation history">
							<Button
								onPress={clearConversation}
								className="p-2 rounded-lg bg-white/5 hover:bg-yellow-500/20 border border-white/10 hover:border-yellow-500/30 transition-all duration-200 text-white/70 hover:text-yellow-400"
							>
								<RefreshCcw className="w-4 h-4" />
							</Button>
						</Tooltip>
						<Tooltip content="Close chat">
							<Button
								onPress={onClose}
								className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
							>
								<X className="w-4 h-4" />
							</Button>
						</Tooltip>
					</div>
				</div>

				{/* CONVERSATION AREA */}
				<div className="flex-1 p-4 overflow-y-auto min-h-0">
					{messages.length === 0 ? (
						<div className="flex flex-col items-center justify-center h-full text-center">
							<div className="bg-white/5 backdrop-blur-sm rounded-full p-6 mb-4">
								<MessageSquare className="w-12 h-12 text-white/40" />
							</div>
							<h3 className="text-white/80 text-lg font-medium mb-2">
								{
									DashboardConstants.DirectChatConstants
										.ConversationContent.Heading
								}
							</h3>
							<p className="text-white/60 text-sm max-w-sm">
								{
									DashboardConstants.DirectChatConstants
										.ConversationContent.SubText
								}
							</p>
						</div>
					) : (
						<div className="space-y-4">
							{messages.map((message, index) => (
								<div
									key={index}
									className={`flex ${
										message.type === "user"
											? "justify-end"
											: "justify-start"
									}`}
								>
									<div
										className={`max-w-[80%] p-3 rounded-lg ${
											message.type === "user"
												? "bg-cyan-500/20 text-white border border-cyan-500/30"
												: "bg-white/5 text-white border border-white/10"
										}`}
									>
										<p className="text-sm">
											{message.content}
										</p>
										<p className="text-xs text-white/40 mt-1">
											{message.timestamp.toLocaleTimeString()}
										</p>
									</div>
								</div>
							))}
							{isLoading && (
								<div className="flex justify-start">
									<div className="bg-white/5 text-white border border-white/10 p-3 rounded-lg">
										<div className="flex items-center space-x-2">
											<div className="animate-spin rounded-full h-4 w-4 border-b-2 border-cyan-400"></div>
											<span className="text-sm">
												{
													ManageAgentConstants
														.TestAgentConstants
														.Loading.AgentResponse
												}
											</span>
										</div>
									</div>
								</div>
							)}
						</div>
					)}
				</div>

				{/* INPUT AREA */}
				<div className="border-t border-white/10 p-4 flex-shrink-0">
					<div className="flex space-x-2">
						<div className="flex-1">
							<Input
								value={userInput}
								onChange={(e) => setUserInput(e.target.value)}
								placeholder={
									ManageAgentConstants.TestAgentConstants
										.PlaceHolders.TypeMessage
								}
								onKeyDown={(event: any) => {
									if (
										event.key === "Enter" &&
										!event.shiftKey
									) {
										sendChatbotRequest();
									}
								}}
								radius="full"
								disabled={isLoading}
								classNames={{
									input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-2 py-2",
									inputWrapper:
										"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-cyan-500/50",
								}}
							/>
						</div>
						<Button
							onPress={sendChatbotRequest}
							disabled={!userInput.trim() || isLoading}
							radius="full"
							className="bg-gradient-to-r from-cyan-500 to-blue-600 text-white font-semibold hover:from-cyan-600 hover:to-blue-700 transition-all duration-300 px-4 py-3"
							title="Send message"
						>
							<Send className="w-4 h-4" />
						</Button>
					</div>
				</div>
			</div>
		</div>
	);
}
