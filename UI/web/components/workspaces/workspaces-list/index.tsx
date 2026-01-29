import { useMemo, useState } from "react";
import { Button, Input } from "@heroui/react";
import {
	Bot,
	Calendar,
	ChevronDown,
	Filter,
	Laptop,
	Notebook,
	Pencil,
	Search,
	SortAsc,
	User,
	X,
} from "lucide-react";

import { WorkspacesListComponentProps } from "@shared/types";
import { WorkspacesConstants } from "@helpers/constants";

export default function WorkspacesListComponent({
	workspacesList,
	handleWorkspaceClick,
	onClose,
	isDisabled,
	showCloseButton = true,
	actionButton,
	onEditWorkspace,
}: Readonly<WorkspacesListComponentProps>) {
	const [searchTerm, setSearchTerm] = useState<string>("");
	const [selectedCreator, setSelectedCreator] = useState<string>("all");
	const [sortBy, setSortBy] = useState<"name" | "date">("date");
	const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");

	const uniqueCreators = useMemo(() => {
		const creators = [
			...new Set(
				workspacesList.map(
					(workspace) => workspace.createdBy || "Unknown",
				),
			),
		];

		return creators.sort((a, b) => a.localeCompare(b));
	}, [workspacesList]);

	const filteredAndSortedWorkspaces = useMemo(() => {
		let filtered = workspacesList.filter((workspace) => {
			// Search filter
			const searchLower = searchTerm.toLocaleLowerCase();
			const matchesSearch =
				!searchTerm ||
				(workspace.agentWorkspaceName || "")
					.toLocaleLowerCase()
					.includes(searchLower) ||
				(workspace.createdBy || "")
					.toLocaleLowerCase()
					.includes(searchLower);
			const matchesCreator =
				selectedCreator === "all" ||
				(workspace.createdBy || "Unknown") === selectedCreator;

			return matchesCreator && matchesSearch;
		});

		// Sort workspaces
		filtered.sort((a, b) => {
			let comparison = 0;

			switch (sortBy) {
				case "name":
					comparison = (a.agentWorkspaceName || "").localeCompare(
						b.agentWorkspaceName || "",
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
	}, [workspacesList, searchTerm, sortBy, sortOrder]);

	const clearFilters = () => {
		setSearchTerm("");
		setSelectedCreator("all");
		setSortBy("date");
		setSortOrder("desc");
	};

	return (
		<div className="h-full flex flex-col bg-gradient-to-b from-gray-900 via-slate-900 to-black">
			{/* HEADER FIXED */}
			<div className="flex items-center justify-between md:p-6 p-4 flex-shrink-0 border-b border-white/5 bg-white/[0.02] backdrop-blur-sm">
				<div className="flex items-center space-x-4">
					<div className="bg-white/5 border border-white/10 p-2.5 rounded-xl shadow-inner">
						<Laptop className="w-5 h-5 text-blue-400" />
					</div>
					<div>
						<h2 className="text-xl font-semibold text-white tracking-tight">
							{WorkspacesConstants.Headers.Header}
						</h2>
						<p className="text-white/40 text-sm font-medium">
							{WorkspacesConstants.Headers.SubText}
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
							input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 p-3",
							inputWrapper:
								"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-blue-500/50",
						}}
					/>
				</div>

				{/* Filters Row */}
				<div className="flex items-center space-x-3 overflow-x-auto pb-2 scrollbar-hide">
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
								setSortBy(newSortBy as "name" | "date");
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

			{/* WORKSPACES List Content - Scrollable Grid */}
			<div className="flex-1 overflow-y-auto p-6 min-h-0 scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
				{filteredAndSortedWorkspaces.length > 0 ? (
					<div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
						{filteredAndSortedWorkspaces.map((workspace, index) => (
							<div
								key={workspace.agentWorkspaceGuid || index}
								className="relative group h-full"
							>
								{/* Card Glow */}
								<div className="absolute inset-0 bg-gradient-to-r from-blue-500/10 to-purple-500/10 rounded-2xl blur-sm opacity-0 group-hover:opacity-100 transition duration-300 -z-10"></div>

								{/* Card */}
								<div
									className="relative bg-white/5 backdrop-blur-sm rounded-2xl p-6 border border-white/10 hover:border-white/20 transition-all duration-300 cursor-pointer h-full flex flex-col hover:-translate-y-1 hover:shadow-xl hover:shadow-blue-500/10"
									onClick={() =>
										!isDisabled &&
										handleWorkspaceClick(
											workspace.agentWorkspaceGuid,
										)
									}
								>
									{/* Icon and Type */}
									<div className="flex items-start justify-between mb-4">
										<div className="bg-gradient-to-br from-orange-500 to-pink-500 p-3 rounded-xl shadow-lg shadow-blue-500/20">
											<Notebook className="w-6 h-6 text-white" />
										</div>
										<div className="relative">
											<div className="flex items-center space-x-1 bg-blue-500/10 px-2 py-1 rounded-full border border-blue-500/20 group-hover:opacity-0 transition-opacity duration-300">
												<span className="text-blue-400 text-[10px] font-medium uppercase tracking-wider">
													Workspace
												</span>
											</div>
											<div className="absolute top-0 right-0 flex space-x-1 opacity-0 group-hover:opacity-100 transition-opacity duration-300 transform translate-y-[-2px]">
												{onEditWorkspace && (
													<button
														onClick={(e) => {
															e.preventDefault();
															e.stopPropagation();
															onEditWorkspace(
																workspace,
															);
														}}
														className="p-1.5 bg-white/10 hover:bg-blue-500/20 text-white/70 hover:text-white rounded-lg backdrop-blur-md transition-all border border-white/5 hover:border-blue-500/30"
														title="Edit Workspace"
													>
														<Pencil className="w-3.5 h-3.5" />
													</button>
												)}
											</div>
										</div>
									</div>

									{/* Content */}
									<div className="flex-1">
										<h3
											className="text-white font-bold text-xl mb-2 line-clamp-1"
											title={workspace.agentWorkspaceName}
										>
											{workspace.agentWorkspaceName ||
												`Workspace ${index + 1}`}
										</h3>
									</div>

									{/* Footer Info */}
									<div className="space-y-3 pt-4 border-t border-white/5">
										<div className="flex items-center justify-between text-xs">
											<div className="flex items-center space-x-2 text-white/50">
												<Bot className="w-3.5 h-3.5" />
												<span
													className="truncate max-w-[100px]"
													title={`${workspace.activeAgentsListInWorkspace.length.toString()} agents associated`}
												>
													{workspace.activeAgentsListInWorkspace.length.toString()}{" "}
													agents
												</span>
											</div>
											<div className="flex items-center space-x-2 text-white/50">
												<User className="w-3.5 h-3.5" />
												<span
													className="truncate max-w-[80px]"
													title={workspace.createdBy}
												>
													{workspace.createdBy ||
														"Unknown"}
												</span>
											</div>
										</div>

										<div className="flex items-center text-white/30 text-[10px] bg-white/5 py-1.5 px-2 rounded-lg justify-center">
											<Calendar className="w-3 h-3 mr-1.5" />
											<span>
												Added{" "}
												{new Date(
													workspace.dateCreated,
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
							{workspacesList.length === 0 ? (
								<Laptop className="w-16 h-16 text-white/20" />
							) : (
								<Search className="w-16 h-16 text-white/20" />
							)}
						</div>
						<h3 className="text-white font-bold text-2xl mb-3">
							{workspacesList.length === 0
								? "No Workspaces Found"
								: "No Matching Workspaces"}
						</h3>
						<p className="text-white/50 text-base max-w-md mx-auto leading-relaxed">
							{workspacesList.length === 0
								? "The workspaces are currently empty. New workspaces will be available soon."
								: "No workspaces match your current search and filter criteria. Try adjusting your filters or search term."}
						</p>
						{workspacesList.length > 0 && (
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
		</div>
	);
}
