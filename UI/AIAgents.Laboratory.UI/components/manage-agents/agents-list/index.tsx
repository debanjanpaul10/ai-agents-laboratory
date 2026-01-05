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
			<div className="flex items-center justify-between md:p-6 p-4 border-b border-white/10 flex-shrink-0">
				<div className="flex items-center space-x-3">
					<div className="bg-gradient-to-r from-purple-500 to-blue-600 p-2 rounded-xl">
						<Bot className="w-5 h-5 text-white" />
					</div>
					<div>
						<h2 className="text-xl font-bold bg-gradient-to-r from-white via-blue-100 to-purple-100 bg-clip-text text-transparent">
							{ManageAgentConstants.Headers.SubText}
						</h2>
						<p className="text-white/60 text-sm">
							Select an agent to modify
						</p>
					</div>
				</div>
				<div className="flex items-center space-x-2">
					<button
						onClick={onClose}
						disabled={isDisabled}
						className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400 disabled:opacity-50 disabled:cursor-not-allowed"
					>
						<X className="w-4 h-4" />
					</button>
				</div>
			</div>

			{/* Search and Filter Section */}
			<div className="border-b border-white/10 p-4 space-y-4 flex-shrink-0">
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

			{/* Agents List Content - Scrollable */}
			<div className="flex-1 overflow-y-auto p-6 space-y-4 min-h-0">
				{filteredAndSortedAgents.length > 0 ? (
					filteredAndSortedAgents.map(
						(agent: AgentDataDTO, index: number) => (
							<div
								key={agent.agentId || index}
								className="relative group"
							>
								{/* Agent Card Glow */}
								<div className="absolute inset-0 bg-gradient-to-r from-blue-500/10 to-purple-500/10 rounded-xl blur-sm opacity-0 group-hover:opacity-100 transition duration-300 -z-10"></div>

								{/* Agent Card */}
								<div
									className="relative bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10 hover:border-white/20 transition-all duration-300 cursor-pointer"
									onClick={() =>
										!isDisabled && handleAgentClick(agent)
									}
								>
									{/* Agent Header */}
									<div className="flex items-start justify-between mb-3">
										<div className="flex items-center space-x-3">
											<div className="bg-gradient-to-r from-blue-500 to-purple-600 p-2 rounded-lg">
												<BotMessageSquare className="w-4 h-4 text-white" />
											</div>
											<div>
												<h3 className="text-white font-semibold text-lg">
													{agent.agentName ||
														`Agent ${index + 1}`}
												</h3>
												<div className="flex items-center space-x-4 mt-1">
													<div className="flex items-center space-x-1 text-white/60 text-sm">
														<Settings className="w-3 h-3" />
														<span>
															{agent.applicationName ||
																"Unknown App"}
														</span>
													</div>
													<div className="flex items-center space-x-1 text-white/60 text-sm">
														<User className="w-3 h-3" />
														<span>
															{agent.createdBy ||
																"Unknown"}
														</span>
													</div>
												</div>
											</div>
										</div>
										<div className="flex items-center space-x-1">
											<div className="w-2 h-2 bg-green-400 rounded-full animate-pulse"></div>
											<span className="text-green-400 text-xs font-medium">
												Active
											</span>
										</div>
									</div>

									{/* Agent Actions */}
									<div className="flex items-center justify-between pt-3 border-t border-white/10">
										<div className="text-white/40 text-xs">
											<Calendar className="w-3 h-3 inline mr-1" />
											<span>{`${new Date(
												agent.dateCreated
											).toDateString()}`}</span>
										</div>
									</div>
								</div>
							</div>
						)
					)
				) : (
					<div className="flex flex-col items-center justify-center py-12 text-center">
						<div className="bg-white/5 backdrop-blur-sm rounded-full p-6 mb-4">
							{agentsDataList.length === 0 ? (
								<Bot className="w-12 h-12 text-white/40" />
							) : (
								<Search className="w-12 h-12 text-white/40" />
							)}
						</div>
						<h3 className="text-white/80 text-lg font-medium mb-2">
							{agentsDataList.length === 0
								? "No Agents Found"
								: "No Matching Agents"}
						</h3>
						<p className="text-white/60 text-sm max-w-sm">
							{agentsDataList.length === 0
								? "You haven't created any AI agents yet. Create your first agent to get started."
								: "No agents match your current search and filter criteria. Try adjusting your filters or search term."}
						</p>
						{agentsDataList.length > 0 && (
							<Button
								onPress={clearFilters}
								className="mt-4 bg-blue-500/20 hover:bg-blue-500/30 border border-blue-500/30 text-blue-300"
							>
								Clear Filters
							</Button>
						)}
					</div>
				)}
			</div>

			{/* Footer - Fixed */}
			<div className="border-t border-white/10 p-6 flex-shrink-0">
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
