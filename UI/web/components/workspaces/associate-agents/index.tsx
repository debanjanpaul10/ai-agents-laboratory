import { useEffect, useState } from "react";
import {
	ArrowRight,
	Bot,
	BotMessageSquare,
	Info,
	Check,
	X,
} from "lucide-react";
import { cn } from "@heroui/react";

import { useAppSelector } from "@store/index";

export default function AssociateAgentsFlyoutComponent({
	isOpen,
	onClose,
	selectedAgentGuids,
	onSelectionComplete,
}: {
	readonly isOpen: boolean;
	readonly onClose: () => void;
	readonly selectedAgentGuids: Set<string>;
	readonly onSelectionComplete: (selectedGuids: Set<string>) => void;
}) {
	const [searchQuery, setSearchQuery] = useState("");
	const [localSelectedGuids, setLocalSelectedGuids] = useState<Set<string>>(
		new Set(selectedAgentGuids),
	);

	const AgentsListStoreData = useAppSelector(
		(state) => state.AgentsReducer.agentsListData,
	);
	const IsLoading = useAppSelector(
		(state) => state.AgentsReducer.isAgentsListLoading,
	);

	useEffect(() => {
		if (isOpen) {
			setLocalSelectedGuids(new Set(selectedAgentGuids));
		}
	}, [isOpen, selectedAgentGuids]);

	const filteredAgents = (AgentsListStoreData || []).filter(
		(agent: any) =>
			(agent.agentName || "")
				.toLowerCase()
				.includes(searchQuery.toLowerCase()) ||
			(agent.agentGuid || agent.agentId || "")
				.toLowerCase()
				.includes(searchQuery.toLowerCase()),
	);

	const toggleLocalSelection = (guid: string) => {
		setLocalSelectedGuids((prev) => {
			const next = new Set(prev);
			if (next.has(guid)) {
				next.delete(guid);
			} else {
				next.add(guid);
			}
			return next;
		});
	};

	const handleDone = () => {
		onSelectionComplete(localSelectedGuids);
		onClose();
	};

	const renderFilteredAgentsList = () =>
		filteredAgents && filteredAgents.length > 0 ? (
			<div className="grid grid-cols-1 gap-4">
				{filteredAgents.map((agent: any, index: number) => {
					const agentGuid = agent.agentGuid || agent.agentId || "";
					const isSelected = localSelectedGuids.has(agentGuid);

					return (
						<div
							key={agentGuid || index}
							onClick={() => toggleLocalSelection(agentGuid)}
							className={cn(
								"group relative bg-white/[0.03] border rounded-2xl p-5 cursor-pointer transition-all duration-300",
								isSelected
									? "bg-indigo-500/10 border-indigo-500/50 shadow-[0_0_20px_rgba(99,102,241,0.15)] ring-1 ring-indigo-500/50"
									: "bg-white/[0.03] border-white/5 hover:bg-white/[0.07] hover:border-indigo-500/30",
							)}
						>
							<div className="flex items-start space-x-4">
								<div
									className={cn(
										"p-3 rounded-xl transition-all duration-300",
										isSelected
											? "bg-indigo-500 text-white shadow-lg shadow-indigo-500/30"
											: "bg-white/5 text-indigo-400 group-hover:bg-indigo-500/10",
									)}
								>
									<BotMessageSquare className="w-5 h-5" />
								</div>
								<div className="flex-1 min-w-0">
									<div className="flex items-center justify-between">
										<h4 className="text-white font-bold text-base truncate mb-1">
											{agent.agentName}
										</h4>
										<div
											className={cn(
												"w-5 h-5 rounded-full border transition-all flex items-center justify-center",
												isSelected
													? "bg-indigo-500 border-indigo-500 shadow-sm"
													: "bg-white/5 border-white/10",
											)}
										>
											{isSelected && (
												<Check className="w-3.5 h-3.5 text-white" />
											)}
										</div>
									</div>
									<p className="text-white/50 text-xs leading-relaxed line-clamp-2">
										{agent.agentDescription ||
											"This agent is specialized and ready to assist with your workspace tasks."}
									</p>
								</div>
							</div>
						</div>
					);
				})}
			</div>
		) : (
			<div className="flex flex-col items-center justify-center h-full space-y-6 opacity-50 py-20">
				<div className="p-6 bg-white/5 rounded-3xl ring-1 ring-white/10">
					<Info className="w-12 h-12 text-white/20" />
				</div>
				<div className="text-center">
					<p className="text-white font-semibold text-lg">
						No Agents Available
					</p>
					<p className="text-white/40 text-sm mt-1">
						Try refreshing or creating a new agent.
					</p>
				</div>
			</div>
		);

	if (!isOpen) return null;

	return (
		<div className="fixed top-0 left-0 h-screen z-[60] transition-all duration-500 ease-in-out md:w-1/2 w-full">
			<div className="absolute inset-0 bg-gradient-to-l from-indigo-600/20 via-blue-600/20 to-transparent blur-sm opacity-50 -z-10"></div>
			<div className="relative h-full bg-gradient-to-br from-slate-900/95 via-gray-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
				{/* HEADER */}
				<div className="flex items-center justify-between p-7 border-b border-white/5 flex-shrink-0 bg-white/[0.02]">
					<div className="flex items-center space-x-4">
						<div className="bg-gradient-to-br from-indigo-500 to-purple-600 p-3 rounded-2xl shadow-lg shadow-indigo-500/20 ring-1 ring-white/10">
							<Bot className="w-6 h-6 text-white" />
						</div>
						<div>
							<h3 className="text-xl font-bold text-white tracking-tight">
								Associate Agents
							</h3>
							<p className="text-white/40 text-sm font-medium">
								{localSelectedGuids.size} selection
								{localSelectedGuids.size > 1 ? "s" : ""} active
							</p>
						</div>
					</div>
					<button
						onClick={onClose}
						className="p-2.5 rounded-xl bg-white/5 hover:bg-red-500/10 border border-white/10 hover:border-red-500/20 transition-all duration-300 text-white/50 hover:text-red-400 group"
					>
						<ArrowRight className="w-5 h-5 group-hover:rotate-90 transition-transform" />
					</button>
				</div>

				{/* SEARCH BAR */}
				<div className="px-7 py-4 border-b border-white/5 bg-white/[0.01]">
					<div className="relative group">
						<div className="absolute inset-y-0 left-4 flex items-center pointer-events-none">
							<BotMessageSquare className="w-5 h-5 text-white/30 group-focus-within:text-indigo-400 transition-colors" />
						</div>
						<input
							type="text"
							value={searchQuery}
							onChange={(e) => setSearchQuery(e.target.value)}
							placeholder="Search by agent name or ID..."
							className="w-full bg-white/5 border border-white/10 rounded-xl py-3 pl-12 pr-4 text-white placeholder:text-white/20 focus:outline-none focus:border-indigo-500/50 focus:ring-1 focus:ring-indigo-500/50 transition-all"
						/>
						{searchQuery && (
							<button
								onClick={() => setSearchQuery("")}
								className="absolute inset-y-0 right-4 flex items-center text-white/20 hover:text-white/50 transition-colors"
							>
								<X className="w-4 h-4" />
							</button>
						)}
					</div>
				</div>

				{/* CONTENT */}
				<div className="flex-1 overflow-y-auto p-7 space-y-4 scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
					{IsLoading ? (
						<div className="space-y-4">
							{[1, 2, 3, 4].map((i) => (
								<div
									key={i}
									className="bg-white/5 border border-white/10 rounded-2xl p-5 animate-pulse"
								>
									<div className="flex items-start space-x-4">
										<div className="bg-white/10 w-10 h-10 rounded-xl" />
										<div className="flex-1 space-y-3">
											<div className="h-4 bg-white/10 rounded-full w-1/2" />
											<div className="h-3 bg-white/10 rounded-full w-3/4" />
										</div>
									</div>
								</div>
							))}
						</div>
					) : (
						<>{renderFilteredAgentsList()}</>
					)}
				</div>

				{/* FOOTER */}
				<div className="p-7 border-t border-white/5 bg-black/40 backdrop-blur-xl">
					<button
						onClick={handleDone}
						className="w-full h-14 bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-600 text-white font-bold rounded-2xl shadow-lg shadow-purple-500/20 hover:scale-[1.02] active:scale-[0.98] transition-all duration-300 border border-white/10"
					>
						Done Selecting
					</button>
				</div>
			</div>
		</div>
	);
}
