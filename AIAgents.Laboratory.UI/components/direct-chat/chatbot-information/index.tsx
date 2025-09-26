import { Settings, X } from "lucide-react";

export default function ChatbotInformationComponent({
	isAgentInfoDrawerOpen,
	onAgentInfoDrawerClose,
}: {
	isAgentInfoDrawerOpen: boolean;
	onAgentInfoDrawerClose: any;
}) {
	const handleAgentInfoDrawerClose = () => {
		onAgentInfoDrawerClose();
	};

	return (
		isAgentInfoDrawerOpen && (
			<div className="h-full flex flex-col">
				{/* Header */}
				<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
					<div className="flex items-center space-x-3">
						<div className="bg-gradient-to-r from-green-500 to-blue-600 p-2 rounded-xl">
							<Settings className="w-5 h-5 text-white" />
						</div>
						<div>
							<h2 className="text-xl font-bold bg-gradient-to-r from-white via-green-100 to-blue-100 bg-clip-text text-transparent">
								AI Chatbot Agent Information
							</h2>
						</div>
					</div>
					<button
						onClick={handleAgentInfoDrawerClose}
						className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
					>
						<X className="w-4 h-4" />
					</button>
				</div>
			</div>
		)
	);
}
