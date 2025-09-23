import React, { useState } from "react";
import { Button, Input } from "@heroui/react";
import { Action, ThunkDispatch } from "@reduxjs/toolkit";
import { Send, MessageSquare, Bot, Zap, ArrowRight } from "lucide-react";

import { ChatRequestDTO } from "@models/chat-request-dto";
import { useAuth } from "@auth/AuthProvider";
import { InvokeChatAgentAsync } from "@store/agents/actions";
import { useAppDispatch } from "@store/index";
import { ChatMessage, TestAgentComponentProps } from "@shared/types";
import { ManageAgentConstants } from "@helpers/constants";

export default function TestAgentComponent({
	editFormData,
	onClose,
}: TestAgentComponentProps) {
	const { getAccessToken } = useAuth();
	const dispatch = useAppDispatch();

	const [messages, setMessages] = useState<Array<ChatMessage>>([]);
	const [userInput, setUserInput] = useState("");
	const [isLoading, setIsLoading] = useState(false);

	const sendChatbotResponse = async () => {
		if (!userInput.trim()) return;

		const userMessage: ChatMessage = {
			id: generateMessageId(),
			type: "user" as const,
			content: userInput,
			timestamp: new Date(),
		};
		await SendChatbotMessageAsync(userMessage);
	};

	async function SendChatbotMessageAsync(userMessage: ChatMessage) {
		setMessages((prev) => [...prev, userMessage]);
		setUserInput("");
		setIsLoading(true);

		try {
			const chatRequest: ChatRequestDTO = {
				userMessage: userMessage.content.trim(),
				conversationId: generateMessageId(),
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

	// Generate a unique ID for messages
	const generateMessageId = () => {
		return `msg_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
	};

	const clearConversation = () => {
		setMessages([]);
		setUserInput("");
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
									<p className="text-sm">{message.content}</p>
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
								if (event.key === "Enter" && !event.shiftKey) {
									sendChatbotResponse();
								}
							}}
							radius="full"
							disabled={isLoading}
							classNames={{
								input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
								inputWrapper:
									"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-cyan-500/50",
							}}
						/>
					</div>
					<Button
						onPress={sendChatbotResponse}
						disabled={!userInput.trim() || isLoading}
						radius="full"
						className="bg-gradient-to-r from-cyan-500 to-blue-600 text-white font-semibold hover:from-cyan-600 hover:to-blue-700 transition-all duration-300 px-4 py-3"
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
		</div>
	);
}
