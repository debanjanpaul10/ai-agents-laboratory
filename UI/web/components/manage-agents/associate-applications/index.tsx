import { useEffect, useState } from "react";
import {
	AppWindow,
	ArrowRight,
	Check,
	Cloud,
	CloudOff,
	Info,
	KeyRound,
	Loader2,
	Search,
} from "lucide-react";
import { Button, Input } from "@heroui/react";

import { useAppSelector } from "@store/index";
import { RegisteredApplicationDTO } from "@models/request/registered-application.dto";
import { AssociateApplicationsFlyoutProps } from "@shared/types";
import { AssociateApplicationsFlyoutPropsConstants } from "@helpers/constants";

export default function AssociateApplicationFlyoutComponent({
	isOpen,
	onClose,
	onApplicationChange,
	selectedApplicationId,
}: Readonly<AssociateApplicationsFlyoutProps>) {
	const [searchQuery, setSearchQuery] = useState<string>("");
	const [localSelectedId, setLocalSelectedId] = useState<number>(0);

	const applications: RegisteredApplicationDTO[] = useAppSelector(
		(state) => state.ToolSkillsReducer.registeredApplications,
	);
	const isLoading: boolean = useAppSelector(
		(state) => state.ToolSkillsReducer.isRegisteredApplicationsLoading,
	);

	useEffect(() => {
		if (isOpen) {
			setLocalSelectedId(selectedApplicationId);
			setSearchQuery("");
		}
	}, [isOpen, selectedApplicationId]);

	const handleConfirm = () => {
		onApplicationChange(localSelectedId);
		onClose();
	};

	const filteredApplications = applications.filter(
		(app) =>
			app.applicationName
				.toLowerCase()
				.includes(searchQuery.toLowerCase()) ||
			(app.description || "")
				.toLowerCase()
				.includes(searchQuery.toLowerCase()),
	);

	const renderApplicationsList = () => {
		if (filteredApplications.length === 0) {
			return (
				<div className="h-full flex flex-col items-center justify-center space-y-3 opacity-40 py-12">
					<Search className="w-12 h-12" />
					<p>
						{
							AssociateApplicationsFlyoutPropsConstants.Hints
								.NoApplications
						}
					</p>
				</div>
			);
		}

		return filteredApplications.map((app) => {
			const isSelected = localSelectedId === app.id;
			return (
				<div
					key={app.id}
					onClick={() => setLocalSelectedId(isSelected ? 0 : app.id)}
					onKeyDown={(e) =>
						e.key === "Enter" &&
						setLocalSelectedId(isSelected ? 0 : app.id)
					}
					role="button"
					tabIndex={0}
					className={`relative cursor-pointer transition-all duration-300 p-4 rounded-xl border ${
						isSelected
							? "bg-blue-500/10 border-blue-500/40"
							: "bg-white/5 border-white/10 hover:border-white/20"
					}`}
				>
					<div className="flex items-start justify-between gap-3">
						<div className="flex items-start space-x-4 min-w-0">
							<div
								className={`p-2 rounded-lg flex-shrink-0 transition-colors duration-300 ${
									isSelected
										? "bg-blue-500/20"
										: "bg-white/10"
								}`}
							>
								<AppWindow
									className={`w-5 h-5 ${
										isSelected
											? "text-blue-400"
											: "text-white/60"
									}`}
								/>
							</div>
							<div className="min-w-0">
								<h4 className="text-white font-semibold truncate">
									{app.applicationName}
								</h4>
								{app.description && (
									<p className="text-white/40 text-xs mt-0.5 line-clamp-2">
										{app.description}
									</p>
								)}
								<div className="flex items-center flex-wrap gap-2 mt-2">
									{/* Azure badge */}
									<span
										className={`inline-flex items-center gap-1 text-[10px] font-medium px-2 py-0.5 rounded-full border ${
											app.isAzureRegistered
												? "bg-sky-500/10 border-sky-500/30 text-sky-400"
												: "bg-white/5 border-white/10 text-white/30"
										}`}
									>
										{app.isAzureRegistered ? (
											<Cloud className="w-3 h-3" />
										) : (
											<CloudOff className="w-3 h-3" />
										)}
										{app.isAzureRegistered
											? "Azure Registered"
											: "Not Azure Registered"}
									</span>

									{/* GUID badge */}
									{app.applicationRegistrationGuid && (
										<span className="inline-flex items-center gap-1 text-[10px] font-mono text-white/30 bg-white/5 border border-white/10 px-2 py-0.5 rounded-full">
											<KeyRound className="w-3 h-3 flex-shrink-0" />
											{app.applicationRegistrationGuid}
										</span>
									)}
								</div>
							</div>
						</div>
						<div
							className={`w-6 h-6 rounded-full border-2 flex-shrink-0 flex items-center justify-center transition-all duration-300 ${
								isSelected
									? "bg-blue-500 border-blue-500 scale-110 shadow-lg shadow-blue-500/20"
									: "bg-white/5 border-white/10"
							}`}
						>
							{isSelected && (
								<Check className="w-4 h-4 text-white" />
							)}
						</div>
					</div>
				</div>
			);
		});
	};

	return (
		isOpen && (
			<>
				<div className="flex flex-col h-full overflow-hidden">
					{/* Header */}
					<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0 bg-white/[0.02] backdrop-blur-md">
						<div className="flex items-center space-x-3">
							<div className="bg-gradient-to-br from-blue-500 to-indigo-600 p-2.5 rounded-xl shadow-lg shadow-blue-500/20">
								<AppWindow className="w-5 h-5 text-white" />
							</div>
							<div>
								<h2 className="text-xl font-bold bg-gradient-to-r from-white via-blue-100 to-indigo-100 bg-clip-text text-transparent">
									{
										AssociateApplicationsFlyoutPropsConstants
											.Headers.Heading
									}
								</h2>
								<p className="text-white/40 text-sm">
									{
										AssociateApplicationsFlyoutPropsConstants
											.Headers.SubHeading
									}
								</p>
							</div>
						</div>
						<button
							onClick={onClose}
							className="p-2 rounded-lg bg-white/5 hover:bg-red-500/10 border border-white/10 hover:border-red-500/20 transition-all duration-300 text-white/50 hover:text-red-400"
						>
							<ArrowRight className="w-5 h-5" />
						</button>
					</div>

					{/* Info Box */}
					<div className="px-6 pt-6 flex-shrink-0">
						<div className="bg-blue-500/5 border border-blue-500/20 rounded-xl p-4 flex items-start space-x-3">
							<Info className="w-5 h-5 text-blue-400 flex-shrink-0 mt-0.5" />
							<p className="text-white/60 text-xs leading-relaxed">
								{
									AssociateApplicationsFlyoutPropsConstants
										.Hints.Info
								}
							</p>
						</div>
					</div>

					{/* Search */}
					<div className="p-6 pb-2 flex-shrink-0">
						<Input
							value={searchQuery}
							onChange={(e) => setSearchQuery(e.target.value)}
							placeholder={
								AssociateApplicationsFlyoutPropsConstants.Hints
									.Search
							}
							variant="bordered"
							startContent={
								<Search className="w-4 h-4 text-white/40" />
							}
							classNames={{
								input: "text-white placeholder:text-white/20",
								inputWrapper:
									"bg-white/5 border-white/10 hover:border-blue-500/30 focus-within:!border-blue-500/50 transition-all rounded-xl h-12",
							}}
						/>
					</div>

					{/* Applications List */}
					<div className="flex-1 overflow-y-auto p-6 space-y-3 scrollbar-thin scrollbar-thumb-white/10">
						{isLoading ? (
							<div className="h-full flex flex-col items-center justify-center space-y-4">
								<Loader2 className="w-8 h-8 text-blue-500 animate-spin" />
								<span className="text-white/40 animate-pulse">
									Loading applications...
								</span>
							</div>
						) : (
							renderApplicationsList()
						)}
					</div>

					{/* Footer */}
					<div className="p-6 pt-2 border-t border-white/10 bg-black/40 backdrop-blur-xl flex-shrink-0">
						<div className="flex items-center justify-between">
							<p className="text-white/30 text-xs">
								{localSelectedId !== 0
									? `1 application selected`
									: "No application selected"}
							</p>
							<div className="flex items-center justify-start space-x-3 mt-4">
								<div className="relative">
									<Button
										onPress={handleConfirm}
										className="group relative bg-gradient-to-r from-blue-400 to-cyan-500 text-white font-semibold hover:from-blue-500 hover:to-cyan-600 shadow-lg hover:shadow-blue-500/50 transition-all duration-300 overflow-hidden whitespace-nowrap disabled:opacity-50"
										radius="full"
									>
										<div className="flex items-center">
											<Check className="w-4 h-4 group-hover:scale-110 transition-transform" />
											<span className="max-w-0 overflow-hidden group-hover:max-w-[100px] transition-all duration-300 ease-in-out ml-0 group-hover:ml-2">
												Confirm
											</span>
										</div>
									</Button>
								</div>
							</div>
						</div>
					</div>
				</div>
			</>
		)
	);
}
