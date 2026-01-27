import { useEffect, useRef, useState } from "react";
import { Button, Tooltip } from "@heroui/react";
import { Bot, MessageSquare, Send, Users } from "lucide-react";
import { Action, ThunkDispatch } from "@reduxjs/toolkit";

import {
	ManageAgentConstants,
	RunWorkspaceConstants,
} from "@helpers/constants";
import { ChatMessage } from "@shared/types";
import { MarkdownRenderer } from "@components/common/markdown-renderer";
import { GenerateMessageId } from "@shared/utils";
import { ChatRequestDTO } from "@models/request/chat-request-dto";
import { useAppDispatch } from "@store/index";
import { ShowErrorToaster } from "@shared/toaster";
import { InvokeChatAgentAsync } from "@store/chat/actions";
import { WorkspaceAgentsDataDTO } from "@models/response/workspace-agents-data.dto";
import { GetWorkspaceGroupChatResponseAsync } from "@store/workspaces/actions";
import { WorkspaceAgentChatRequestDTO } from "@models/request/workspace-agent-chat-request.dto";
import { AgentsWorkspaceDTO } from "@models/response/agents-workspace-dto";
import { GroupChatResponseDTO } from "@models/response/group-chat-response.dto";

export default function AssociatedAgentsChatPaneComponent({
	selectedAgent,
	workspaceDetailsData,
}: {
	selectedAgent: WorkspaceAgentsDataDTO | null;
	workspaceDetailsData: AgentsWorkspaceDTO;
}) {
	const dispatch = useAppDispatch();

	const [showScrollbar, setShowScrollbar] = useState<boolean>(false);
	const [userInput, setUserInput] = useState<string>("");
	const [messages, setMessages] = useState<Array<ChatMessage>>([]);
	const [isLoading, setIsLoading] = useState(false);
	const [isGroupChatAgent, setIsGroupChatAgent] = useState<boolean>(false);

	const textareaRef = useRef<HTMLTextAreaElement>(null);
	const messagesEndRef = useRef<HTMLDivElement>(null);

	useEffect(() => {
		messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
	}, [messages]);

	useEffect(() => {
		setIsGroupChatAgent(
			selectedAgent?.agentGuid ===
				RunWorkspaceConstants.ChatPane.GroupChatAgent.Guid,
		);
	}, [selectedAgent]);

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

	const clearConversation = () => {
		setMessages([]);
		setUserInput("");
	};

	const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
		if (e.key === "Enter" && !e.shiftKey) {
			e.preventDefault();
			handleSendChatRequest();
		}
	};

	const handleSendChatRequest = () => {
		if (!selectedAgent) return;
		if (isGroupChatAgent) SendGroupChatRequest();
		else SendChatbotRequest();
	};

	async function SendGroupChatRequest() {
		if (!userInput.trim() || !selectedAgent) return;

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

		try {
			setIsLoading(true);
			const chatRequest: WorkspaceAgentChatRequestDTO = {
				agentId: "",
				applicationName: "",
				conversationId: GenerateMessageId(),
				userMessage: userInput,
				workspaceId: workspaceDetailsData.agentWorkspaceGuid,
			};

			const aiResponse = (await (
				dispatch as ThunkDispatch<any, any, Action>
			)(
				GetWorkspaceGroupChatResponseAsync(chatRequest),
			)) as GroupChatResponseDTO | null;

			if (aiResponse?.agentResponse) {
				let content = aiResponse.agentResponse;
				if (
					aiResponse.agentsInvoked &&
					aiResponse.agentsInvoked.length > 0
				) {
					content = `${aiResponse.agentResponse}\n\n<small>Agents invoked: ${aiResponse.agentsInvoked.join(", ")}</small>`;
				}

				const botMessage = {
					id: GenerateMessageId(),
					type: "bot" as const,
					content: content,
				};

				setMessages((prev) => [...prev, botMessage]);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			setIsLoading(false);
		}
	}

	async function SendChatbotRequest() {
		if (!userInput.trim() || !selectedAgent) return;

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

		try {
			setIsLoading(true);
			const chatRequest: ChatRequestDTO = {
				userMessage: userMessage.content.trim(),
				conversationId: GenerateMessageId(),
				agentId: selectedAgent.agentGuid,
				agentName: selectedAgent.agentName,
			};
			const aiResponse = (await (
				dispatch as ThunkDispatch<any, any, Action>
			)(InvokeChatAgentAsync(chatRequest))) as string | null;

			if (aiResponse) {
				const botMessage = {
					id: GenerateMessageId(),
					type: "bot" as const,
					content: aiResponse,
				};

				setMessages((prev) => [...prev, botMessage]);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			setIsLoading(false);
		}
	}

	return !selectedAgent ? (
		<div className="flex flex-col items-center justify-center h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl text-center p-8">
			<div className="bg-white/5 backdrop-blur-sm rounded-full p-8 mb-6">
				<MessageSquare className="w-16 h-16 text-white/40" />
			</div>
			<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-cyan-100 to-blue-100 bg-clip-text text-transparent mb-4">
				{RunWorkspaceConstants.ChatPane.SelectAgentHeader}
			</h2>
			<p className="text-white/60 max-w-md">
				{RunWorkspaceConstants.ChatPane.SelectAgentSubheader}
			</p>
		</div>
	) : (
		<div className="h-full flex flex-col bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl">
			{/* Chat Header */}
			<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
				<div className="flex items-center space-x-3">
					<div
						className={`p-2 rounded-xl ${
							isGroupChatAgent
								? "bg-gradient-to-r from-purple-500 to-pink-600"
								: "bg-gradient-to-r from-cyan-500 to-blue-600"
						}`}
					>
						{isGroupChatAgent ? (
							<Users className="w-5 h-5 text-white" />
						) : (
							<Bot className="w-5 h-5 text-white" />
						)}
					</div>
					<div>
						<h2 className="text-xl font-bold bg-gradient-to-r from-white via-cyan-100 to-blue-100 bg-clip-text text-transparent">
							{selectedAgent.agentName}
						</h2>
						<p className="text-white/60 text-sm">
							{isGroupChatAgent
								? "Active conversation with all agents"
								: RunWorkspaceConstants.ChatPane.SubTitle}
						</p>
					</div>
				</div>
				{messages.length > 0 && (
					<Tooltip content="Clear conversation">
						<Button
							onPress={clearConversation}
							className="p-2 rounded-lg bg-white/5 hover:bg-yellow-500/20 border border-white/10 hover:border-yellow-500/30 transition-all duration-200 text-white/70 hover:text-yellow-400"
						>
							Clear
						</Button>
					</Tooltip>
				)}
			</div>

			{/* Conversation Area */}
			<div className="flex-1 p-4 overflow-y-auto min-h-0">
				{messages.length === 0 ? (
					<div className="flex flex-col items-center justify-center h-full text-center">
						<div className="bg-white/5 backdrop-blur-sm rounded-full p-6 mb-4">
							<MessageSquare className="w-12 h-12 text-white/40" />
						</div>
						<h3 className="text-white/80 text-lg font-medium mb-2">
							{isGroupChatAgent
								? "Group Chat"
								: RunWorkspaceConstants.Headers.Header}
						</h3>
						<p className="text-white/60 text-sm max-w-sm">
							{isGroupChatAgent
								? "Collaborate with all your agents in one place."
								: RunWorkspaceConstants.Headers.SubHeader}
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
						<div ref={messagesEndRef} />
					</div>
				)}
			</div>

			{/* Input Area */}
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
						onPress={handleSendChatRequest}
						disabled={!userInput.trim() || isLoading}
						radius="full"
						title="Send message"
						className={`mb-3 text-white font-semibold transition-all duration-300 px-3 py-3 disabled:opacity-50 ${
							isGroupChatAgent
								? "bg-gradient-to-r from-purple-500 to-pink-600 hover:from-purple-600 hover:to-pink-700"
								: "bg-gradient-to-r from-cyan-500 to-blue-600 hover:from-cyan-600 hover:to-blue-700"
						}`}
					>
						<Send className="w-4 h-4" />
					</Button>
				</div>
			</div>
		</div>
	);
}
