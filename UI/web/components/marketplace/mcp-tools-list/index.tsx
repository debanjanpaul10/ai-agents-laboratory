import { McpServerToolsDTO } from "@models/mcp-server-tools-dto";
import { useAppSelector } from "@store/index";
import { useEffect, useState } from "react";
import { X, Wrench, Info, Zap } from "lucide-react";

export default function McpToolsListFlyoutComponent({
	isOpen,
	onClose,
}: {
	isOpen: boolean;
	onClose: () => void;
}) {
	const [mcpServerTools, setMcpServerTools] = useState<McpServerToolsDTO[]>(
		[]
	);

	const McpServerToolsStoreData = useAppSelector<McpServerToolsDTO[]>(
		(state) => state.ToolSkillsReducer.mcpServerTools
	);

	const IsLoading = useAppSelector<boolean>(
		(state) => state.ToolSkillsReducer.isMcpToolsLoading
	);

	useEffect(() => {
		if (
			Array.isArray(McpServerToolsStoreData)
		) {
			setMcpServerTools(McpServerToolsStoreData);
		}
	}, [McpServerToolsStoreData]);

	if (!isOpen) return null;

	return (
		<>
			<div className="fixed top-0 right-0 md:right-1/3 md:w-2/3 w-full h-screen z-[60] transition-all duration-500 ease-in-out">
				<div className="absolute inset-0 bg-gradient-to-l from-indigo-600/20 via-blue-600/20 to-transparent blur-sm opacity-50 -z-10"></div>
				<div className="relative h-full bg-gradient-to-br from-slate-900/95 via-gray-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
					{/* Header */}
					<div className="flex items-center justify-between p-6 border-b border-white/5 flex-shrink-0 bg-white/[0.02]">
						<div className="flex items-center space-x-3">
							<div className="bg-gradient-to-br from-cyan-500 to-blue-600 p-2.5 rounded-xl shadow-lg shadow-cyan-500/20">
								<Wrench className="w-5 h-5 text-white" />
							</div>
							<div>
								<h3 className="text-lg font-bold text-white tracking-tight">
									MCP Server Tools
								</h3>
								<p className="text-white/40 text-xs font-medium">
									{mcpServerTools.length} tools discovered
								</p>
							</div>
						</div>
						<button
							onClick={onClose}
							className="p-2 rounded-lg bg-white/5 hover:bg-white/10 border border-white/10 transition-all duration-300 text-white/50 hover:text-white"
						>
							<X className="w-4 h-4" />
						</button>
					</div>

					{/* Content */}
					<div className="flex-1 overflow-y-auto p-6 space-y-4 scrollbar-thin scrollbar-thumb-white/10">
						{IsLoading ? (
							<div className="space-y-4">
								{[1, 2, 3].map((i) => (
									<div key={i} className="bg-white/5 border border-white/10 rounded-2xl p-4 animate-pulse">
										<div className="flex items-start space-x-3">
											<div className="bg-white/10 w-9 h-9 rounded-lg" />
											<div className="flex-1 space-y-2">
												<div className="h-4 bg-white/10 rounded w-1/2" />
												<div className="h-3 bg-white/10 rounded w-3/4" />
											</div>
										</div>
									</div>
								))}
							</div>
						) : mcpServerTools.length > 0 ? (
							mcpServerTools.map((tool, index) => (
								<div
									key={index}
									className="group relative bg-white/5 border border-white/10 rounded-2xl p-4 hover:bg-white/10 hover:border-cyan-500/30 transition-all duration-300"
								>
									<div className="flex items-start space-x-3">
										<div className="bg-cyan-500/10 p-2 rounded-lg group-hover:bg-cyan-500/20 transition-colors">
											<Zap className="w-4 h-4 text-cyan-400" />
										</div>
										<div className="flex-1 min-w-0">
											<h4 className="text-white font-semibold text-sm truncate">
												{tool.toolName}
											</h4>
											<p className="text-white/50 text-xs mt-1 leading-relaxed line-clamp-3">
												{tool.toolDescription || "No description provided."}
											</p>
										</div>
									</div>
								</div>
							))
						) : (
							<div className="flex flex-col items-center justify-center h-full space-y-4 opacity-50">
								<div className="p-4 bg-white/5 rounded-full">
									<Info className="w-8 h-8 text-white/20" />
								</div>
								<p className="text-white/40 text-sm font-medium">No tools found</p>
							</div>
						)}
					</div>

					{/* Footer */}
					<div className="p-6 border-t border-white/5 bg-black/20 backdrop-blur-md">
						<p className="text-[10px] text-white/30 uppercase tracking-[0.2em] font-bold text-center">
							Secure MCP Connection Active
						</p>
					</div>
				</div>
			</div>
		</>
	);
}
