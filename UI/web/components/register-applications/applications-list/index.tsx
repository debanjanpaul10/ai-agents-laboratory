import { useMemo, useState } from "react";
import { Input, Button } from "@heroui/react";
import {
	AppWindow,
	Calendar,
	ChevronDown,
	Filter,
	KeyRound,
	Search,
	Settings,
	SortAsc,
	X,
} from "lucide-react";

import { RegisterApplicationsConstants } from "@helpers/constants";
import { RegisteredApplicationsListComponentProps } from "@shared/types";

export default function ApplicationsListComponent({
	applicationsList,
	onClose,
	isDisabled,
	showCloseButton = true,
	actionButton,
	onEditApplication,
}: Readonly<RegisteredApplicationsListComponentProps>) {
	const [searchTerm, setSearchTerm] = useState<string>("");
	const [sortBy, setSortBy] = useState<"name" | "date">("date");
	const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");

	const filteredAndSorted = useMemo(() => {
		let filtered = applicationsList.filter((app) => {
			const lower = searchTerm.toLowerCase();
			return (
				!searchTerm ||
				app.applicationName.toLowerCase().includes(lower) ||
				(app.description || "").toLowerCase().includes(lower)
			);
		});

		filtered.sort((a, b) => {
			const cmp =
				sortBy === "name"
					? a.applicationName.localeCompare(b.applicationName)
					: a.id - b.id;
			return sortOrder === "asc" ? cmp : -cmp;
		});

		return filtered;
	}, [applicationsList, searchTerm, sortBy, sortOrder]);

	const clearFilters = () => {
		setSearchTerm("");
		setSortBy("date");
		setSortOrder("desc");
	};

	const hasActiveFilters =
		searchTerm || sortBy !== "date" || sortOrder !== "desc";

	const pluralTerm = applicationsList.length > 1 ? "s" : "";

	return (
		<div className="h-full flex flex-col bg-gradient-to-b from-gray-900 via-slate-900 to-black">
			{/* Header */}
			<div className="flex items-center justify-between md:p-6 p-4 flex-shrink-0 border-b border-white/5 bg-white/[0.02] backdrop-blur-sm">
				<div className="flex items-center space-x-4">
					<div className="bg-white/5 border border-white/10 p-2.5 rounded-xl shadow-inner">
						<AppWindow className="w-5 h-5 text-blue-400" />
					</div>
					<div>
						<h2 className="text-xl font-semibold text-white tracking-tight">
							{RegisterApplicationsConstants.Headers.MainHeader}
						</h2>
						<p className="text-white/40 text-sm font-medium">
							Manage and configure your registered applications.
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

			{/* Search & Filters */}
			<div className="p-4 space-y-4 flex-shrink-0">
				<Input
					value={searchTerm}
					onChange={(e) => setSearchTerm(e.target.value)}
					placeholder="Search applications by name or description..."
					startContent={<Search className="w-4 h-4 text-white/40" />}
					classNames={{
						input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 p-3",
						inputWrapper:
							"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-blue-500/50",
					}}
				/>

				<div className="flex items-center space-x-3 overflow-x-auto pb-2 scrollbar-hide">
					<div className="relative min-w-fit">
						<select
							value={`${sortBy}-${sortOrder}`}
							onChange={(e) => {
								const [newSortBy, newSortOrder] =
									e.target.value.split("-");
								setSortBy(newSortBy as "name" | "date");
								setSortOrder(newSortOrder as "asc" | "desc");
							}}
							className="bg-white/5 border border-white/10 hover:border-white/20 text-white/80 rounded-lg pl-8 pr-8 py-2 appearance-none cursor-pointer focus:outline-none focus:border-blue-500/50 min-w-[160px]"
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
						<SortAsc className="w-4 h-4 absolute left-2 top-1/2 -translate-y-1/2 text-white/60 pointer-events-none" />
						<ChevronDown className="w-4 h-4 absolute right-2 top-1/2 -translate-y-1/2 text-white/60 pointer-events-none" />
					</div>

					{hasActiveFilters && (
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

			{/* Grid */}
			<div className="flex-1 overflow-y-auto p-6 min-h-0 scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
				{filteredAndSorted.length > 0 ? (
					<div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
						{filteredAndSorted.map((app, index) => (
							<div
								key={app.id || index}
								className="relative group h-full"
							>
								<div className="absolute inset-0 bg-gradient-to-r from-blue-500/10 to-indigo-500/10 rounded-2xl blur-sm opacity-0 group-hover:opacity-100 transition duration-300 -z-10" />
								<div
									className="relative bg-white/5 backdrop-blur-sm rounded-2xl p-6 border border-white/10 hover:border-white/20 transition-all duration-300 cursor-pointer h-full flex flex-col hover:-translate-y-1 hover:shadow-xl hover:shadow-blue-500/10"
									onClick={() =>
										!isDisabled && onEditApplication?.(app)
									}
								>
									<div className="flex items-start justify-between mb-4">
										<div className="bg-gradient-to-br from-blue-500 to-indigo-500 p-3 rounded-xl shadow-lg shadow-blue-500/20">
											<AppWindow className="w-6 h-6 text-white" />
										</div>
										<div
											className={`flex items-center space-x-1 px-2 py-1 rounded-full border text-[10px] font-medium uppercase tracking-wider ${app.isAzureRegistered ? "bg-emerald-500/10 border-emerald-500/20 text-emerald-400" : "bg-white/5 border-white/10 text-white/40"}`}
										>
											{app.isAzureRegistered
												? "Azure"
												: "Local"}
										</div>
									</div>

									<div className="flex-1">
										<h3
											className="text-white font-bold text-xl mb-2 line-clamp-1"
											title={app.applicationName}
										>
											{app.applicationName}
										</h3>
										<p className="text-white/40 text-sm mb-4 line-clamp-2 leading-relaxed">
											{app.description ||
												"No description provided."}
										</p>
									</div>

									<div className="space-y-3 pt-4 border-t border-white/5">
										{app.applicationRegistrationGuid && (
											<div className="flex items-center space-x-2 text-xs text-white/30 bg-black/20 p-2 rounded-lg">
												<KeyRound className="w-3.5 h-3.5 flex-shrink-0" />
												<span
													className="font-mono truncate"
													title={
														app.applicationRegistrationGuid
													}
												>
													{
														app.applicationRegistrationGuid
													}
												</span>
											</div>
										)}
										<div className="flex items-center justify-between text-xs text-white/30">
											<div className="flex items-center space-x-1.5">
												<Settings className="w-3.5 h-3.5" />
												<span>ID: {app.id}</span>
											</div>
											<div className="flex items-center space-x-1.5">
												<Calendar className="w-3.5 h-3.5" />
												<span>Click to edit</span>
											</div>
										</div>
									</div>
								</div>
							</div>
						))}
					</div>
				) : (
					<div className="flex flex-col items-center justify-center py-20 text-center h-full">
						<div className="bg-white/5 backdrop-blur-sm rounded-full p-8 mb-6">
							{applicationsList.length === 0 ? (
								<AppWindow className="w-16 h-16 text-white/20" />
							) : (
								<Search className="w-16 h-16 text-white/20" />
							)}
						</div>
						<h3 className="text-white font-bold text-2xl mb-3">
							{applicationsList.length === 0
								? "No Applications Registered"
								: "No Matching Applications"}
						</h3>
						<p className="text-white/50 text-base max-w-md mx-auto leading-relaxed">
							{applicationsList.length === 0
								? "Register your first application to get started. Applications allow agents to authenticate and interact with external services."
								: "No applications match your current search. Try adjusting your filters."}
						</p>
						{applicationsList.length > 0 && (
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

			{/* Footer */}
			<div className="p-6 flex-shrink-0 border-t border-white/5 bg-black/20 backdrop-blur-md">
				<div className="flex items-center justify-between">
					<span className="text-white/60 text-sm">
						{filteredAndSorted.length === applicationsList.length
							? `${applicationsList.length} application${pluralTerm} total`
							: `Showing ${filteredAndSorted.length} of ${applicationsList.length} applications`}
					</span>
					{hasActiveFilters && (
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
