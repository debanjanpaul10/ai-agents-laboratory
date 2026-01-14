import React, { useEffect, useState, useMemo } from "react";
import { Input, Button } from "@heroui/react";
import {
	Bot,
	X,
	Settings,
	User,
	Calendar,
	BotMessageSquare,
	Search,
	Filter,
	SortAsc,
	ChevronDown,
} from "lucide-react";

import { AgentDataDTO } from "@models/agent-data-dto";
import { ManageAgentConstants } from "@helpers/constants";
import { AgentsListComponentProps } from "@shared/types";

export default function AgentsListComponent({
	agentsDataList,
	handleAgentClick,
	onClose,
	isDisabled,
	showCloseButton = true,
	actionButton,
}: AgentsListComponentProps) {
	const [currentAgentsData, setCurrentAgentsData] = useState<AgentDataDTO[]>(
		[]
	);
	const [searchTerm, setSearchTerm] = useState("");
	const [selectedApplication, setSelectedApplication] =
		useState<string>("all");
	const [selectedCreator, setSelectedCreator] = useState<string>("all");
	const [sortBy, setSortBy] = useState<"name" | "date" | "application">(
		"date"
	);
	const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");

	useEffect(() => {
		if (
			agentsDataList !== null &&
			agentsDataList.length > 0 &&
			agentsDataList !== currentAgentsData
		) {
			setCurrentAgentsData(agentsDataList);
		}
	}, [agentsDataList]);

	// Get unique applications and creators for filter dropdowns
	const uniqueApplications = useMemo(() => {
		const apps = [
			...new Set(
				agentsDataList.map(
					(agent) => agent.applicationName || "Unknown"
				)
			),
		];
		return apps.sort();
	}, [agentsDataList]);

	// Filter and sort agents
	const filteredAndSortedAgents = useMemo(() => {
		let filtered = agentsDataList.filter((agent) => {
			// Search filter
			const searchLower = searchTerm.toLowerCase();
			const matchesSearch =
				!searchTerm ||
				(agent.agentName || "").toLowerCase().includes(searchLower) ||
				(agent.applicationName || "")
					.toLowerCase()
					.includes(searchLower) ||
				(agent.createdBy || "").toLowerCase().includes(searchLower);

			// Application filter
			const matchesApplication =
				selectedApplication === "all" ||
				(agent.applicationName || "Unknown") === selectedApplication;

			// Creator filter
			const matchesCreator =
				selectedCreator === "all" ||
				(agent.createdBy || "Unknown") === selectedCreator;

			return matchesSearch && matchesApplication && matchesCreator;
		});

		// Sort agents
		filtered.sort((a, b) => {
			let comparison = 0;

			switch (sortBy) {
				case "name":
					comparison = (a.agentName || "").localeCompare(
						b.agentName || ""
					);
					break;
				case "application":
					comparison = (a.applicationName || "").localeCompare(
						b.applicationName || ""
					);
					break;
				case "date":
				default:
					comparison =
						new Date(a.dateCreated).getTime() -
						new Date(b.dateCreated).getTime();
					break;
			}

			return sortOrder === "asc" ? comparison : -comparison;
		});

		return filtered;
	}, [
		agentsDataList,
		searchTerm,
		selectedApplication,
		selectedCreator,
		sortBy,
		sortOrder,
	]);

	const clearFilters = () => {
		setSearchTerm("");
		setSelectedApplication("all");
		setSelectedCreator("all");
		setSortBy("date");
		setSortOrder("desc");
	};

	return (
		<div className="h-full flex flex-col bg-gradient-to-b from-gray-900 via-slate-900 to-black">
			{/* Header - Fixed */}
			<div className="flex items-center justify-between md:p-6 p-4 flex-shrink-0 border-b border-white/5 bg-white/[0.02] backdrop-blur-sm">
				<div className="flex items-center space-x-4">
					<div className="bg-white/5 border border-white/10 p-2.5 rounded-xl shadow-inner">
						<Bot className="w-5 h-5 text-blue-400" />
					</div>
					<div>
						<h2 className="text-xl font-semibold text-white tracking-tight">
							{ManageAgentConstants.Headers.Header}
						</h2>
						<p className="text-white/40 text-sm font-medium">
							{ManageAgentConstants.Headers.SubHeader}
						</p>
					</div>
				</div>
				<div className="flex items-center space-x-2">
					{actionButton}
					{showCloseButton && (
						<button
							onClick={onClose}
							disabled={isDisabled}
							className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400 disabled:opacity-50 disabled:cursor-not-allowed"
						>
							<X className="w-4 h-4" />
						</button>
					)}
				</div>
			</div>

			{/* Search and Filter Section */}
			<div className="p-4 space-y-4 flex-shrink-0">
				{/* Search Bar */}
				<div className="relative">
					<Input
						value={searchTerm}
						onChange={(e) => setSearchTerm(e.target.value)}
						placeholder="Search agents by name, application, or creator..."
						className="w-full"
						startContent={
							<Search className="w-4 h-4 text-white/40" />
						}
						classNames={{
							input: "bg-white/5 border-white/10 text-white placeholder:text-white/40",
							inputWrapper:
								"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-blue-500/50",
						}}
					/>
				</div>

				{/* Filters Row */}
				<div className="flex items-center space-x-3 overflow-x-auto">
					{/* Application Filter */}
					<div className="relative min-w-fit">
						<select
							value={selectedApplication}
							onChange={(e) =>
								setSelectedApplication(e.target.value)
							}
							className="bg-white/5 border border-white/10 hover:border-white/20 text-white/80 rounded-lg pl-8 pr-8 py-2 appearance-none cursor-pointer focus:outline-none focus:border-blue-500/50 min-w-[140px]"
						>
							<option
								value="all"
								className="bg-gray-900 text-white"
							>
								All Apps
							</option>
							{uniqueApplications.map((app) => (
								<option
									key={app}
									value={app}
									className="bg-gray-900 text-white"
								>
									{app}
								</option>
							))}
						</select>
						<Settings className="w-4 h-4 absolute left-2 top-1/2 transform -translate-y-1/2 text-white/60 pointer-events-none" />
						<ChevronDown className="w-4 h-4 absolute right-2 top-1/2 transform -translate-y-1/2 text-white/60 pointer-events-none" />
					</div>

					{/* Sort Options */}
					<div className="relative min-w-fit">
						<select
							value={`${sortBy}-${sortOrder}`}
							onChange={(e) => {
								const [newSortBy, newSortOrder] =
									e.target.value.split("-");
								setSortBy(
									newSortBy as "name" | "date" | "application"
								);
								setSortOrder(newSortOrder as "asc" | "desc");
							}}
							className="bg-white/5 border border-white/10 hover:border-white/20 text-white/80 rounded-lg pl-8 pr-8 py-2 appearance-none cursor-pointer focus:outline-none focus:border-blue-500/50 min-w-[140px]"
						>
							<option
								value="date-desc"
								className="bg-gray-900 text-white"
							>
								Newest First
							</option>
							<option
								value="date-asc"
								className="bg-gray-900 text-white"
							>
								Oldest First
							</option>
							<option
								value="name-asc"
								className="bg-gray-900 text-white"
							>
								Name (A-Z)
							</option>
							<option
								value="name-desc"
								className="bg-gray-900 text-white"
							>
								Name (Z-A)
							</option>
							<option
								value="application-asc"
								className="bg-gray-900 text-white"
							>
								App (A-Z)
							</option>
							<option
								value="application-desc"
								className="bg-gray-900 text-white"
							>
								App (Z-A)
							</option>
						</select>
						<SortAsc className="w-4 h-4 absolute left-2 top-1/2 transform -translate-y-1/2 text-white/60 pointer-events-none" />
						<ChevronDown className="w-4 h-4 absolute right-2 top-1/2 transform -translate-y-1/2 text-white/60 pointer-events-none" />
					</div>

					{/* Clear Filters Button */}
					{(searchTerm ||
						selectedApplication !== "all" ||
						selectedCreator !== "all" ||
						sortBy !== "date" ||
						sortOrder !== "desc") && (
						<Button
							onPress={clearFilters}
							variant="bordered"
							className="bg-red-500/10 border-red-500/20 hover:border-red-500/30 text-red-400 hover:text-red-300 min-w-fit"
						>
							<Filter className="w-4 h-4" />
							Clear
						</Button>
					)}
				</div>
			</div>

			{/* Agents List Content - Scrollable Grid */}
			<div className="flex-1 overflow-y-auto p-6 min-h-0">
				{filteredAndSortedAgents.length > 0 ? (
					<div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
						{filteredAndSortedAgents.map(
							(agent: AgentDataDTO, index: number) => (
								<div
									key={agent.agentId || index}
									className="relative group h-full"
								>
									{/* Agent Card Glow */}
									<div className="absolute inset-0 bg-gradient-to-r from-blue-500/10 to-purple-500/10 rounded-2xl blur-sm opacity-0 group-hover:opacity-100 transition duration-300 -z-10"></div>

									{/* Agent Card */}
									<div
										className="relative bg-white/5 backdrop-blur-sm rounded-2xl p-6 border border-white/10 hover:border-white/20 transition-all duration-300 cursor-pointer h-full flex flex-col hover:-translate-y-1 hover:shadow-xl hover:shadow-purple-500/10"
										onClick={() =>
											!isDisabled &&
											handleAgentClick(agent)
										}
									>
										{/* Agent Header */}
										<div className="flex items-start justify-between mb-4">
											<div className="bg-gradient-to-br from-blue-500 to-purple-600 p-3 rounded-xl shadow-lg shadow-purple-500/20">
												<BotMessageSquare className="w-6 h-6 text-white" />
											</div>
											<div className="flex items-center space-x-1 bg-green-500/10 px-2 py-1 rounded-full border border-green-500/20">
												<div className="w-1.5 h-1.5 bg-green-400 rounded-full animate-pulse"></div>
												<span className="text-green-400 text-[10px] font-medium uppercase tracking-wider">
													Active
												</span>
											</div>
										</div>

										{/* Content */}
										<div className="flex-1">
											<h3
												className="text-white font-bold text-xl mb-2 line-clamp-1"
												title={agent.agentName}
											>
												{agent.agentName ||
													`Agent ${index + 1}`}
											</h3>

											<p
												className="text-white/40 text-sm line-clamp-2 mb-4 h-10"
												title={
													agent.agentDescription ||
													"No description available for this agent."
												}
											>
												{agent.agentDescription ||
													"No description available for this agent."}
											</p>
										</div>

										{/* Footer Info */}
										<div className="space-y-3 pt-4 border-t border-white/5">
											<div className="flex items-center justify-between text-xs">
												<div className="flex items-center space-x-2 text-white/50">
													<Settings className="w-3.5 h-3.5" />
													<span
														className="truncate max-w-[80px]"
														title={
															agent.applicationName
														}
													>
														{agent.applicationName ||
															"Unknown App"}
													</span>
												</div>
												<div className="flex items-center space-x-2 text-white/50">
													<User className="w-3.5 h-3.5" />
													<span
														className="truncate max-w-[80px]"
														title={agent.createdBy}
													>
														{agent.createdBy ||
															"Unknown"}
													</span>
												</div>
											</div>

											<div className="flex items-center text-white/30 text-[10px] bg-white/5 py-1.5 px-2 rounded-lg justify-center">
												<Calendar className="w-3 h-3 mr-1.5" />
												<span>
													Created{" "}
													{new Date(
														agent.dateCreated
													).toLocaleDateString()}
												</span>
											</div>
										</div>
									</div>
								</div>
							)
						)}
					</div>
				) : (
					<div className="flex flex-col items-center justify-center py-20 text-center h-full">
						<div className="bg-white/5 backdrop-blur-sm rounded-full p-8 mb-6 animate-float">
							{agentsDataList.length === 0 ? (
								<Bot className="w-16 h-16 text-white/20" />
							) : (
								<Search className="w-16 h-16 text-white/20" />
							)}
						</div>
						<h3 className="text-white font-bold text-2xl mb-3">
							{agentsDataList.length === 0
								? "No Agents Found"
								: "No Matching Agents"}
						</h3>
						<p className="text-white/50 text-base max-w-md mx-auto leading-relaxed">
							{agentsDataList.length === 0
								? "You haven't created any AI agents yet. Click the 'Add New Agent' button to get started building your own assistant."
								: "No agents match your current search and filter criteria. Try adjusting your filters or search term to find what you're looking for."}
						</p>
						{agentsDataList.length > 0 && (
							<Button
								onPress={clearFilters}
								className="mt-8 bg-white/5 hover:bg-white/10 text-white border border-white/10"
								variant="bordered"
							>
								Clear Filters
							</Button>
						)}
					</div>
				)}
			</div>

			{/* Footer - Fixed */}
			<div className="p-6 flex-shrink-0">
				<div className="flex items-center justify-between">
					<div className="text-white/60 text-sm">
						{filteredAndSortedAgents.length !==
						agentsDataList.length ? (
							<>
								Showing {filteredAndSortedAgents.length} of{" "}
								{agentsDataList.length} agent
								{agentsDataList.length !== 1 ? "s" : ""}
							</>
						) : (
							<>
								{agentsDataList.length} agent
								{agentsDataList.length !== 1 ? "s" : ""} total
							</>
						)}
					</div>
					{(searchTerm ||
						selectedApplication !== "all" ||
						selectedCreator !== "all") && (
						<Button
							onPress={clearFilters}
							size="sm"
							variant="bordered"
							className="bg-white/5 border-white/10 hover:border-white/20 text-white/70 hover:text-white"
						>
							Reset Filters
						</Button>
					)}
				</div>
			</div>
		</div>
	);
}
