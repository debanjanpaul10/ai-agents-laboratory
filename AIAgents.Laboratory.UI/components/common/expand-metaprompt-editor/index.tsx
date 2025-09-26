import { MouseEventHandler } from "react";
import { Modal } from "@heroui/react";
import { Minimize2, ScrollText } from "lucide-react";
import { useMsal } from "@azure/msal-react";

export default function ExpandMetapromptEditorComponent({
	expandedPromptModal,
	handleCollapsePrompt,
	agentMetaprompt,
	handleInputChange,
	createdBy,
}: {
	expandedPromptModal: boolean;
	handleCollapsePrompt: MouseEventHandler;
	agentMetaprompt: string;
	handleInputChange: any;
	createdBy: string;
}) {
	const { accounts } = useMsal();

	return (
		<Modal isOpen={expandedPromptModal}>
			{/* Modal Backdrop */}
			<div className="fixed inset-0 bg-black/80 backdrop-blur-sm z-70 transition-opacity duration-300" />

			{/* Modal Content */}
			<div className="fixed inset-0 flex items-center justify-center z-80 p-8">
				<div className="w-full max-w-4xl h-full max-h-[90vh] bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border border-white/10 shadow-2xl rounded-2xl flex flex-col">
					{/* Modal Header */}
					<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
						<div className="flex items-center space-x-3">
							<div className="bg-green-400 rounded-full p-2 rounded-xl">
								<ScrollText className="w-5 h-5 text-white-400" />
							</div>
							<div>
								<h2 className="text-xl font-bold bg-gradient-to-r from-white via-orange-100 to-red-100 bg-clip-text text-transparent">
									Agent Meta Prompt
								</h2>
							</div>
						</div>
						<button
							onClick={handleCollapsePrompt}
							className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
							title="Collapse prompt editor"
						>
							<Minimize2 className="w-4 h-4" />
						</button>
					</div>

					{/* Modal Content */}
					<div className="flex-1 p-6 min-h-0 flex flex-col">
						<div className="flex-1 min-h-0">
							<div className="relative h-full group">
								{/* Glow effect */}
								<div className="absolute -inset-0.5 bg-gradient-to-r from-orange-500/20 via-red-500/20 to-purple-500/20 rounded-xl blur opacity-75 group-focus-within:opacity-100 transition duration-300" />

								{/* Editor */}
								<textarea
									value={agentMetaprompt}
									onChange={(e) =>
										handleInputChange(
											"agentMetaPrompt",
											e.target.value
										)
									}
									placeholder="Define your agent's behavior, personality, and capabilities in detail..."
									className="w-full h-full bg-black/40 backdrop-blur-xl text-white placeholder:text-white/40 p-6 rounded-xl border border-white/10 hover:border-white/20 focus:border-orange-500/50 focus:outline-none resize-none font-mono relative z-10"
									disabled={
										accounts[0].username !== createdBy
									}
								/>
							</div>
						</div>
					</div>
				</div>
			</div>
		</Modal>
	);
}
