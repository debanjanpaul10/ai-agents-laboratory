import React, { useMemo, useState } from "react";
import { Input, Button } from "@heroui/react";
import {
	StoreIcon,
	X,
	User,
	Calendar,
	Search,
	Filter,
	SortAsc,
	ChevronDown,
	Cpu,
	Link,
} from "lucide-react";

import { MarketplaceConstants } from "@helpers/constants";
import { SkillsListComponentProps } from "@shared/types";

export default function SkillsListComponent({
	toolSkillsList,
	handleSkillClick,
	onClose,
	isDisabled,
	showCloseButton = true,
	actionButton,
}: SkillsListComponentProps) {
	const [searchTerm, setSearchTerm] = useState("");
	const [selectedAgent, setSelectedAgent] = useState<string>("all");
	const [selectedCreator, setSelectedCreator] = useState<string>("all");
	const [sortBy, setSortBy] = useState<"name" | "date" | "technical">("date");
	const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");

	// Get unique agents and creators for filter dropdowns
	const uniqueAgents = useMemo(() => {
		const agents = [
			...new Set(
				toolSkillsList.flatMap((skill) =>
					Object.keys(skill.associatedAgents || {})
				)
			),
		];
		return agents.sort();
	}, [toolSkillsList]);

	const uniqueCreators = useMemo(() => {
		const creators = [
			...new Set(
				toolSkillsList.map((skill) => skill.createdBy || "Unknown")
			),
		];
		return creators.sort();
	}, [toolSkillsList]);

	// Filter and sort skills
	const filteredAndSortedSkills = useMemo(() => {
		let filtered = toolSkillsList.filter((skill) => {
			// Search filter
			const searchLower = searchTerm.toLowerCase();
			const matchesSearch =
				!searchTerm ||
				(skill.toolSkillDisplayName || "")
					.toLowerCase()
					.includes(searchLower) ||
				(skill.toolSkillTechnicalName || "")
					.toLowerCase()
					.includes(searchLower) ||
				(skill.createdBy || "").toLowerCase().includes(searchLower);

			// Agent filter
			const matchesAgent =
				selectedAgent === "all" ||
				Object.keys(skill.associatedAgents || {}).includes(
					selectedAgent
				);

			// Creator filter
			const matchesCreator =
				selectedCreator === "all" ||
				(skill.createdBy || "Unknown") === selectedCreator;

			return matchesSearch && matchesAgent && matchesCreator;
		});

		// Sort skills
		filtered.sort((a, b) => {
			let comparison = 0;

			switch (sortBy) {
				case "name":
					comparison = (a.toolSkillDisplayName || "").localeCompare(
						b.toolSkillDisplayName || ""
					);
					break;
				case "technical":
					comparison = (a.toolSkillTechnicalName || "").localeCompare(
						b.toolSkillTechnicalName || ""
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
		toolSkillsList,
		searchTerm,
		selectedAgent,
		selectedCreator,
		sortBy,
		sortOrder,
	]);

	const clearFilters = () => {
		setSearchTerm("");
		setSelectedAgent("all");
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
						<StoreIcon className="w-5 h-5 text-blue-400" />
					</div>
					<div>
						<h2 className="text-xl font-semibold text-white tracking-tight">
							{MarketplaceConstants.Headers.Header}
						</h2>
						<p className="text-white/40 text-sm font-medium">
							{MarketplaceConstants.Headers.SubText}
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
						placeholder="Search skills by name, technical name, or creator..."
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
				<div className="flex items-center space-x-3 overflow-x-auto pb-2 scrollbar-hide">
					{/* Agent Filter */}
					<div className="relative min-w-fit">
						<select
							value={selectedAgent}
							onChange={(e) => setSelectedAgent(e.target.value)}
							className="bg-white/5 border border-white/10 hover:border-white/20 text-white/80 rounded-lg pl-8 pr-8 py-2 appearance-none cursor-pointer focus:outline-none focus:border-blue-500/50 min-w-[140px]"
						>
							<option
								value="all"
								className="bg-gray-900 text-white"
							>
								All Agents
							</option>
							{uniqueAgents.map((agent) => (
								<option
									key={agent}
									value={agent}
									className="bg-gray-900 text-white"
								>
									{agent}
								</option>
							))}
						</select>
						<Cpu className="w-4 h-4 absolute left-2 top-1/2 transform -translate-y-1/2 text-white/60 pointer-events-none" />
						<ChevronDown className="w-4 h-4 absolute right-2 top-1/2 transform -translate-y-1/2 text-white/60 pointer-events-none" />
					</div>

					{/* Creator Filter */}
					<div className="relative min-w-fit">
						<select
							value={selectedCreator}
							onChange={(e) => setSelectedCreator(e.target.value)}
							className="bg-white/5 border border-white/10 hover:border-white/20 text-white/80 rounded-lg pl-8 pr-8 py-2 appearance-none cursor-pointer focus:outline-none focus:border-blue-500/50 min-w-[140px]"
						>
							<option
								value="all"
								className="bg-gray-900 text-white"
							>
								All Creators
							</option>
							{uniqueCreators.map((creator) => (
								<option
									key={creator}
									value={creator}
									className="bg-gray-900 text-white"
								>
									{creator}
								</option>
							))}
						</select>
						<User className="w-4 h-4 absolute left-2 top-1/2 transform -translate-y-1/2 text-white/60 pointer-events-none" />
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
									newSortBy as "name" | "date" | "technical"
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
						</select>
						<SortAsc className="w-4 h-4 absolute left-2 top-1/2 transform -translate-y-1/2 text-white/60 pointer-events-none" />
						<ChevronDown className="w-4 h-4 absolute right-2 top-1/2 transform -translate-y-1/2 text-white/60 pointer-events-none" />
					</div>

					{/* Clear Filters Button */}
					{(searchTerm ||
						selectedAgent !== "all" ||
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

			{/* Skills List Content - Scrollable Grid */}
			<div className="flex-1 overflow-y-auto p-6 min-h-0 scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
				{filteredAndSortedSkills.length > 0 ? (
					<div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
						{filteredAndSortedSkills.map((skill, index) => (
							<div
								key={skill.toolSkillGuid || index}
								className="relative group h-full"
							>
								{/* Card Glow */}
								<div className="absolute inset-0 bg-gradient-to-r from-blue-500/10 to-purple-500/10 rounded-2xl blur-sm opacity-0 group-hover:opacity-100 transition duration-300 -z-10"></div>

								{/* Card */}
								<div
									className="relative bg-white/5 backdrop-blur-sm rounded-2xl p-6 border border-white/10 hover:border-white/20 transition-all duration-300 cursor-pointer h-full flex flex-col hover:-translate-y-1 hover:shadow-xl hover:shadow-blue-500/10"
									onClick={() =>
										!isDisabled && handleSkillClick(skill)
									}
								>
									{/* Icon and Type */}
									<div className="flex items-start justify-between mb-4">
										<div className="bg-gradient-to-br from-blue-500 to-cyan-500 p-3 rounded-xl shadow-lg shadow-blue-500/20">
											<Cpu className="w-6 h-6 text-white" />
										</div>
										<div className="flex items-center space-x-1 bg-blue-500/10 px-2 py-1 rounded-full border border-blue-500/20">
											<span className="text-blue-400 text-[10px] font-medium uppercase tracking-wider">
												Skill
											</span>
										</div>
									</div>

									{/* Content */}
									<div className="flex-1">
										<h3
											className="text-white font-bold text-xl mb-2 line-clamp-1"
											title={skill.toolSkillDisplayName}
										>
											{skill.toolSkillDisplayName ||
												`Skill ${index + 1}`}
										</h3>

										<p
											className="text-white/40 text-sm font-mono mb-4 truncate bg-black/20 p-2 rounded border border-white/5"
											title={skill.toolSkillTechnicalName}
										>
											{skill.toolSkillTechnicalName}
										</p>
									</div>

									{/* Footer Info */}
									<div className="space-y-3 pt-4 border-t border-white/5">
										<div className="flex items-center justify-between text-xs">
											<div className="flex items-center space-x-2 text-white/50">
												<Link className="w-3.5 h-3.5" />
												<span
													className="truncate max-w-[100px]"
													title={
														skill.toolSkillMcpServerUrl
													}
												>
													{skill.toolSkillMcpServerUrl
														? "MCP Available"
														: "Direct Skill"}
												</span>
											</div>
											<div className="flex items-center space-x-2 text-white/50">
												<User className="w-3.5 h-3.5" />
												<span
													className="truncate max-w-[80px]"
													title={skill.createdBy}
												>
													{skill.createdBy ||
														"Unknown"}
												</span>
											</div>
										</div>

										<div className="flex items-center text-white/30 text-[10px] bg-white/5 py-1.5 px-2 rounded-lg justify-center">
											<Calendar className="w-3 h-3 mr-1.5" />
											<span>
												Added{" "}
												{new Date(
													skill.dateCreated
												).toLocaleDateString()}
											</span>
										</div>
									</div>
								</div>
							</div>
						))}
					</div>
				) : (
					<div className="flex flex-col items-center justify-center py-20 text-center h-full">
						<div className="bg-white/5 backdrop-blur-sm rounded-full p-8 mb-6">
							{toolSkillsList.length === 0 ? (
								<StoreIcon className="w-16 h-16 text-white/20" />
							) : (
								<Search className="w-16 h-16 text-white/20" />
							)}
						</div>
						<h3 className="text-white font-bold text-2xl mb-3">
							{toolSkillsList.length === 0
								? "No Skills Found"
								: "No Matching Skills"}
						</h3>
						<p className="text-white/50 text-base max-w-md mx-auto leading-relaxed">
							{toolSkillsList.length === 0
								? "The marketplace is currently empty. New skills and capabilities will be available soon."
								: "No skills match your current search and filter criteria. Try adjusting your filters or search term."}
						</p>
						{toolSkillsList.length > 0 && (
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
			<div className="p-6 flex-shrink-0 border-t border-white/5 bg-black/20 backdrop-blur-md">
				<div className="flex items-center justify-between">
					<div className="text-white/60 text-sm">
						{filteredAndSortedSkills.length !==
						toolSkillsList.length ? (
							<>
								Showing {filteredAndSortedSkills.length} of{" "}
								{toolSkillsList.length} skill
								{toolSkillsList.length !== 1 ? "s" : ""}
							</>
						) : (
							<>
								{toolSkillsList.length} skill
								{toolSkillsList.length !== 1 ? "s" : ""} total
							</>
						)}
					</div>
					{(searchTerm ||
						selectedAgent !== "all" ||
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
