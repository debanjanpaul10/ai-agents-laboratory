import React, { useState } from "react";
import { Button, Input } from "@heroui/react";
import { Send, MessageSquare, Bot, Zap, ArrowRight } from "lucide-react";
import { AgentDataDTO } from "@/models/agent-data-dto";

interface TestAgentComponentProps {
	selectedAgent: AgentDataDTO | null;
	editFormData: AgentDataDTO;
	onClose: () => void;
}

export default function TestAgentComponent({
	editFormData,
	onClose,
}: TestAgentComponentProps) {
	const [testInput, setTestInput] = useState("");
	const [testOutput, setTestOutput] = useState("");
	const [isLoading, setIsLoading] = useState(false);
	const [conversationHistory, setConversationHistory] = useState<
		Array<{ role: "user" | "agent"; message: string; timestamp: Date }>
	>([]);

	const handleTestSubmit = async () => {
		if (!testInput.trim()) return;

		setIsLoading(true);

		// Add user message to conversation
		const userMessage = {
			role: "user" as const,
			message: testInput,
			timestamp: new Date(),
		};

		setConversationHistory((prev) => [...prev, userMessage]);

		// Simulate API call - replace with actual agent testing logic
		setTimeout(() => {
			const agentResponse = {
				role: "agent" as const,
				message: `This is a simulated response from ${
					editFormData.agentName || "the agent"
				} based on the meta prompt: "${editFormData.agentMetaPrompt?.substring(
					0,
					100
				)}..."`,
				timestamp: new Date(),
			};

			setConversationHistory((prev) => [...prev, agentResponse]);
			setTestInput("");
			setIsLoading(false);
		}, 1500);
	};

	const clearConversation = () => {
		setConversationHistory([]);
		setTestInput("");
		setTestOutput("");
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
				{conversationHistory.length === 0 ? (
					<div className="flex flex-col items-center justify-center h-full text-center">
						<div className="bg-white/5 backdrop-blur-sm rounded-full p-6 mb-4">
							<MessageSquare className="w-12 h-12 text-white/40" />
						</div>
						<h3 className="text-white/80 text-lg font-medium mb-2">
							Start Testing
						</h3>
						<p className="text-white/60 text-sm max-w-sm">
							Send a message to test how your agent responds based
							on its configuration.
						</p>
					</div>
				) : (
					<div className="space-y-4">
						{conversationHistory.map((message, index) => (
							<div
								key={index}
								className={`flex ${
									message.role === "user"
										? "justify-end"
										: "justify-start"
								}`}
							>
								<div
									className={`max-w-[80%] p-3 rounded-lg ${
										message.role === "user"
											? "bg-cyan-500/20 text-white border border-cyan-500/30"
											: "bg-white/5 text-white border border-white/10"
									}`}
								>
									<p className="text-sm">{message.message}</p>
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
											Agent is thinking...
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
							value={testInput}
							onChange={(e) => setTestInput(e.target.value)}
							placeholder="Type your test message..."
							onKeyPress={(e) =>
								e.key === "Enter" && handleTestSubmit()
							}
							disabled={isLoading}
							classNames={{
								input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
								inputWrapper:
									"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-cyan-500/50",
							}}
						/>
					</div>
					<Button
						onPress={handleTestSubmit}
						disabled={!testInput.trim() || isLoading}
						className="bg-gradient-to-r from-cyan-500 to-blue-600 text-white font-semibold hover:from-cyan-600 hover:to-blue-700 transition-all duration-300 px-4 py-3"
					>
						<Send className="w-4 h-4" />
					</Button>
				</div>

				{conversationHistory.length > 0 && (
					<div className="mt-3 flex justify-center">
						<Button
							variant="light"
							onPress={clearConversation}
							className="text-white/70 hover:text-white text-sm"
						>
							Clear Conversation
						</Button>
					</div>
				)}
			</div>
		</div>
	);
}
