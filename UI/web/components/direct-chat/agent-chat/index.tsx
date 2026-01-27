import { useEffect, useRef, useState } from "react";
import { Button, Tooltip } from "@heroui/react";
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
import { GenerateMessageId } from "@shared/utils";
import { DirectChatRequestDTO } from "@models/request/direct-chat-request-dto";
import { useAppDispatch, useAppSelector } from "@store/index";
import {
	ClearConversationHistoryAsync,
	GetDirectChatResponseAsync,
} from "@store/chat/actions";
import { FullScreenLoading } from "@components/common/spinner";
import { GetAgentDataByIdAsync } from "@store/agents/actions";
import { MarkdownRenderer } from "@components/common/markdown-renderer";

export default function AgentChatComponent({
	toggleChatbotInformation,
	onClose,
	isAgentInfoDrawerOpen,
}: {
	toggleChatbotInformation: any;
	onClose: any;
	isAgentInfoDrawerOpen: boolean;
}) {
	const dispatch = useAppDispatch();

	const [messages, setMessages] = useState<Array<ChatMessage>>([]);
	const [userInput, setUserInput] = useState("");
	const [isLoading, setIsLoading] = useState(false);
	const [showScrollbar, setShowScrollbar] = useState(false);
	const textareaRef = useRef<HTMLTextAreaElement>(null);

	const ConversationHistoryStoreData = useAppSelector(
		(state) => state.ChatReducer.conversationHistory,
	);
	const IsDirectChatLoadingStoreData = useAppSelector(
		(state) => state.CommonReducer.isDirectChatLoading,
	);
	const ConversationAgentIdStoreData = useAppSelector(
		(state) => state.CommonReducer.configurationValue,
	);
	const IsEditAgentDataLoading = useAppSelector(
		(state) => state.AgentsReducer.isEditAgentDataLoading,
	);

	const mapChatHistoryToMessages = (
		chatHistory: Array<{ role: string; content: string }>,
	) => {
		return chatHistory.map((msg) => ({
			id: GenerateMessageId(),
			type: msg.role === "user" ? ("user" as const) : ("bot" as const),
			content: msg.content,
		}));
	};

	useEffect(() => {
		if (
			ConversationAgentIdStoreData.AIChatbotAgentId !== null &&
			ConversationAgentIdStoreData.AIChatbotAgentId !== undefined &&
			ConversationAgentIdStoreData.AIChatbotAgentId !== ""
		)
			getAgentInformation();
	}, [ConversationAgentIdStoreData.AIChatbotAgentId]);

	useEffect(() => {
		if (
			ConversationHistoryStoreData?.chatHistory &&
			Array.isArray(ConversationHistoryStoreData.chatHistory) &&
			ConversationHistoryStoreData.chatHistory.length > 0
		) {
			const mappedMessages = mapChatHistoryToMessages(
				ConversationHistoryStoreData.chatHistory,
			);
			setMessages(mappedMessages);
		}
	}, [ConversationHistoryStoreData]);

	function getAgentInformation() {
		dispatch(
			GetAgentDataByIdAsync(
				ConversationAgentIdStoreData.AIChatbotAgentId,
			),
		);
	}

	function clearConversation() {
		setMessages([]);
		setUserInput("");

		dispatch(ClearConversationHistoryAsync());
	}

	async function sendChatbotRequest() {
		if (!userInput.trim()) return;

		const userMessage: ChatMessage = {
			id: GenerateMessageId(),
			type: "user" as const,
			content: userInput,
		};

		setMessages((prev) => [...prev, userMessage]);
		setUserInput("");

		// Reset textarea height and scrollbar after clearing input
		if (textareaRef.current) {
			textareaRef.current.style.height = "auto";
			setShowScrollbar(false);
		}

		setIsLoading(true);

		try {
			const chatRequest: DirectChatRequestDTO = {
				userMessage: userMessage.content.trim(),
			};
			const aiResponse = (await (
				dispatch as ThunkDispatch<any, any, Action>
			)(GetDirectChatResponseAsync(chatRequest))) as string | null;

			if (aiResponse) {
				const botMessage = {
					id: GenerateMessageId(),
					type: "bot" as const,
					content: aiResponse,
				};

				setMessages((prev) => [...prev, botMessage]);
			}
		} catch (error) {
			console.error(error);
		} finally {
			setIsLoading(false);
		}
	}

	const renderChatbotHeader = () => {
		return (
			<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
				<div className="flex items-center space-x-3">
					<div className="bg-gradient-to-r from-cyan-500 to-blue-600 p-2 rounded-xl">
						<BotMessageSquare className="w-5 h-5 text-white" />
					</div>
					<div>
						<h2 className="text-xl font-bold bg-gradient-to-r from-white via-cyan-100 to-blue-100 bg-clip-text text-transparent">
							{
								DashboardConstants.DirectChatConstants.Header
									.MainHeader
							}
						</h2>
						<p className="text-white/60 text-sm">
							{
								DashboardConstants.DirectChatConstants.Header
									.SubHeader
							}
						</p>
					</div>
				</div>
				<div className="flex items-center space-x-2">
					<Tooltip content="Chatbot information">
						<Button
							disabled={isAgentInfoDrawerOpen}
							onPress={toggleChatbotInformation}
							className="p-2 rounded-lg bg-white/5 hover:bg-green-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-green-400 disabled:opacity-50"
						>
							<Info className="w-4 h-4" />
						</Button>
					</Tooltip>

					<Tooltip content="Clear conversation history">
						<Button
							disabled={isAgentInfoDrawerOpen}
							onPress={clearConversation}
							className="p-2 rounded-lg bg-white/5 hover:bg-yellow-500/20 border border-white/10 hover:border-yellow-500/30 transition-all duration-200 text-white/70 hover:text-yellow-400 disabled:opacity-50"
						>
							<RefreshCcw className="w-4 h-4" />
						</Button>
					</Tooltip>
					<Tooltip content="Close chat">
						<Button
							disabled={isAgentInfoDrawerOpen}
							onPress={onClose}
							className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400 disabled:opacity-50"
						>
							<X className="w-4 h-4" />
						</Button>
					</Tooltip>
				</div>
			</div>
		);
	};

	const renderConversationArea = () => {
		return (
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
									{message.type === "user" ? (
										<p className="text-sm whitespace-pre-wrap break-words">
											{message.content}
										</p>
									) : (
										<div className="text-sm">
											<MarkdownRenderer
												content={message.content}
											/>
										</div>
									)}
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
													.TestAgentConstants.Loading
													.AgentResponse
											}
										</span>
									</div>
								</div>
							</div>
						)}
					</div>
				)}
			</div>
		);
	};

	const handleInputChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
		setUserInput(e.target.value);

		// Auto-resize textarea
		if (textareaRef.current) {
			textareaRef.current.style.height = "auto";
			textareaRef.current.style.height = `${Math.min(
				textareaRef.current.scrollHeight,
				800,
			)}px`;

			// Show scrollbar only when content exceeds 200px
			setShowScrollbar(textareaRef.current.scrollHeight > 200);
		}
	};

	const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
		if (e.key === "Enter" && !e.shiftKey) {
			e.preventDefault();
			sendChatbotRequest();

			// Reset textarea height after sending
			if (textareaRef.current) {
				textareaRef.current.style.height = "auto";
				setShowScrollbar(false);
			}
		}
	};

	const renderInputArea = () => {
		return (
			<div className="border-t border-white/10 p-4 flex-shrink-0">
				<div className="flex space-x-2 items-end">
					<div className="flex-1">
						<textarea
							ref={textareaRef}
							value={userInput}
							onChange={handleInputChange}
							onKeyDown={handleKeyDown}
							placeholder={
								ManageAgentConstants.TestAgentConstants
									.PlaceHolders.TypeMessage
							}
							disabled={isLoading || isAgentInfoDrawerOpen}
							rows={1}
							className="w-full resize-none bg-white/5 border border-white/10 hover:border-white/20 focus:border-cyan-500/50 focus:outline-none text-white placeholder:text-white/40 px-4 py-3 transition-all duration-200 overflow-y-hidden disabled:opacity-50"
							style={{
								maxHeight: "200px",
								overflowY: showScrollbar ? "auto" : "hidden",
							}}
						/>
					</div>
					<Button
						onPress={sendChatbotRequest}
						disabled={
							!userInput.trim() ||
							isLoading ||
							isAgentInfoDrawerOpen
						}
						radius="full"
						className="mb-3 bg-gradient-to-r from-cyan-500 to-blue-600 text-white font-semibold hover:from-cyan-600 hover:to-blue-700 transition-all duration-300 px-3 py-3 disabled:opacity-50"
						title="Send message"
					>
						<Send className="w-4 h-4" />
					</Button>
				</div>
			</div>
		);
	};

	return IsDirectChatLoadingStoreData || IsEditAgentDataLoading ? (
		<FullScreenLoading isLoading={true} message="Chatbot is loading" />
	) : (
		<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
			<div className="h-full flex flex-col">
				{renderChatbotHeader()}

				{renderConversationArea()}

				{renderInputArea()}
			</div>
		</div>
	);
}
