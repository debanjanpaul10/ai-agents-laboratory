import React, { useRef, useState } from "react";
import { Button } from "@heroui/react";
import { Action, ThunkDispatch } from "@reduxjs/toolkit";
import { Send, MessageSquare, Bot, Zap, ArrowRight } from "lucide-react";

import { ChatRequestDTO } from "@models/chat-request-dto";
import { useAuth } from "@auth/AuthProvider";
import { InvokeChatAgentAsync } from "@store/chat/actions";
import { useAppDispatch } from "@store/index";
import { ChatMessage, TestAgentComponentProps } from "@shared/types";
import { ManageAgentConstants } from "@helpers/constants";
import { GenerateMessageId } from "@shared/utils";
import { MarkdownRenderer } from "@components/common/markdown-renderer";

export default function TestAgentComponent({
	editFormData,
	onClose,
}: TestAgentComponentProps) {
	const { getAccessToken } = useAuth();
	const dispatch = useAppDispatch();

	const [messages, setMessages] = useState<Array<ChatMessage>>([]);
	const [userInput, setUserInput] = useState("");
	const [isLoading, setIsLoading] = useState(false);
	const [showScrollbar, setShowScrollbar] = useState(false);
	const textareaRef = useRef<HTMLTextAreaElement>(null);

	const sendChatbotRequest = async () => {
		if (!userInput.trim()) return;

		const userMessage: ChatMessage = {
			id: GenerateMessageId(),
			type: "user" as const,
			content: userInput,
		};
		await SendChatbotMessageAsync(userMessage);
	};

	async function SendChatbotMessageAsync(userMessage: ChatMessage) {
		setMessages((prev) => [...prev, userMessage]);
		setUserInput("");

		// Reset textarea height and scrollbar after clearing input
		if (textareaRef.current) {
			textareaRef.current.style.height = "auto";
			setShowScrollbar(false);
		}

		setIsLoading(true);

		try {
			const chatRequest: ChatRequestDTO = {
				userMessage: userMessage.content.trim(),
				conversationId: GenerateMessageId(),
				agentId: editFormData.agentId,
				agentName: editFormData.agentName,
			};
			const accessToken = await getAccessToken();
			if (accessToken) {
				const aiResponse = (await (
					dispatch as ThunkDispatch<any, any, Action>
				)(InvokeChatAgentAsync(chatRequest, accessToken))) as
					| string
					| null;

				if (aiResponse) {
					const botMessage = {
						id: GenerateMessageId(),
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

	const clearConversation = () => {
		setMessages([]);
		setUserInput("");
	};

	const handleInputChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
		setUserInput(e.target.value);

		// Auto-resize textarea
		if (textareaRef.current) {
			textareaRef.current.style.height = "auto";
			textareaRef.current.style.height = `${Math.min(
				textareaRef.current.scrollHeight,
				800
			)}px`;

			// Show scrollbar only when content exceeds 200px
			setShowScrollbar(textareaRef.current.scrollHeight > 200);
		}
	};

	const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
		if (e.key === "Enter" && !e.shiftKey) {
			e.preventDefault();
			sendChatbotRequest();
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
							disabled={isLoading}
							rows={1}
							className="w-full resize-none bg-white/5 border border-white/10 hover:border-white/20 focus:border-cyan-500/50 focus:outline-none text-white placeholder:text-white/40 px-4 py-3 transition-all duration-200 disabled:opacity-50"
							style={{
								maxHeight: "200px",
								overflowY: showScrollbar ? "auto" : "hidden",
							}}
						/>
					</div>
					<Button
						onPress={sendChatbotRequest}
						disabled={!userInput.trim() || isLoading}
						radius="full"
						title="Send message"
						className="mb-3 bg-gradient-to-r from-cyan-500 to-blue-600 text-white font-semibold hover:from-cyan-600 hover:to-blue-700 transition-all duration-300 px-3 py-3 disabled:opacity-50"
					>
						<Send className="w-4 h-4" />
					</Button>
				</div>

				{messages.length > 0 && (
					<div className="mt-3 flex justify-center">
						<Button
							variant="solid"
							onPress={clearConversation}
							className="text-red/70 hover:text-red text-sm"
						>
							{
								ManageAgentConstants.TestAgentConstants
									.PlaceHolders.ClearConversation
							}
						</Button>
					</div>
				)}
			</div>
		);
	};

	return (
		<div className="h-full flex flex-col">
			{/* Header */}
			<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
				<div className="flex items-center space-x-3">
					<div className="bg-gradient-to-r from-cyan-500 to-blue-600 p-2 rounded-xl">
						<Zap className="w-5 h-5 text-white" />
					</div>
					<div>
						<h2 className="text-xl font-bold bg-gradient-to-r from-white via-cyan-100 to-blue-100 bg-clip-text text-transparent">
							Test AI Agent
						</h2>
						<p className="text-white/60 text-sm">
							Test {editFormData.agentName || "your agent"} in
							real-time
						</p>
					</div>
				</div>
				<button
					onClick={onClose}
					className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
					title="Close test panel"
				>
					<ArrowRight className="w-4 h-4" />
				</button>
			</div>
			{/* Agent Info */}
			<div className="p-4 border-b border-white/10 bg-white/5">
				<div className="flex items-center space-x-3">
					<Bot className="w-4 h-4 text-cyan-400" />
					<div>
						<p className="text-white font-medium text-sm">
							{editFormData.agentName || "Unnamed Agent"}
						</p>
						<p className="text-white/60 text-xs">
							{editFormData.applicationName ||
								"No application specified"}
						</p>
					</div>
				</div>
			</div>
			{/* Conversation Area */}
			<div className="flex-1 p-4 overflow-y-auto min-h-0">
				{messages.length === 0 ? (
					<div className="flex flex-col items-center justify-center h-full text-center">
						<div className="bg-white/5 backdrop-blur-sm rounded-full p-6 mb-4">
							<MessageSquare className="w-12 h-12 text-white/40" />
						</div>
						<h3 className="text-white/80 text-lg font-medium mb-2">
							{
								ManageAgentConstants.TestAgentConstants
									.PlaceHolders.ChatBodyHeader
							}
						</h3>
						<p className="text-white/60 text-sm max-w-sm">
							{
								ManageAgentConstants.TestAgentConstants
									.PlaceHolders.SubText
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
			{/* Input Area */}
			{renderInputArea()}
		</div>
	);
}
