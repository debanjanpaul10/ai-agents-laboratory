import { useState } from "react";
import { Button, Input } from "@heroui/react";
import {
	AppWindow,
	FileText,
	KeyRound,
	Save,
	Settings,
	Trash,
	X,
} from "lucide-react";

import { useAppDispatch, useAppSelector } from "@store/index";
import {
	DeleteRegisteredApplicationByIdAsync,
	UpdateExistingRegisteredApplicationAsync,
} from "@store/tools-skills/actions";
import { RegisteredApplicationDTO } from "@models/request/registered-application.dto";
import DeletePopupComponent from "@components/common/delete-popup";
import { RegisterApplicationsConstants } from "@helpers/constants";

interface EditRegisteredApplicationProps {
	editFormData: RegisteredApplicationDTO;
	selectedApplication: RegisteredApplicationDTO | null;
	setEditFormData: React.Dispatch<
		React.SetStateAction<RegisteredApplicationDTO>
	>;
	setSelectedApplication: React.Dispatch<
		React.SetStateAction<RegisteredApplicationDTO | null>
	>;
	isEditDrawerOpen: boolean;
	onEditClose: () => void;
	isDisabled: boolean;
}

export default function EditRegisteredApplicationComponent({
	editFormData,
	selectedApplication,
	setEditFormData,
	setSelectedApplication,
	isEditDrawerOpen,
	onEditClose,
	isDisabled,
}: Readonly<EditRegisteredApplicationProps>) {
	const dispatch = useAppDispatch();
	const [isDeletePopupOpen, setIsDeletePopupOpen] = useState(false);

	const IsLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isRegisteredApplicationsLoading,
	);

	const handleInput = (
		field: keyof RegisteredApplicationDTO,
		value: string | boolean,
	) => {
		setEditFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleClose = () => {
		setSelectedApplication(null);
		onEditClose();
	};

	const handleSave = () => {
		dispatch(UpdateExistingRegisteredApplicationAsync(editFormData) as any);
	};

	const handleDelete = () => {
		if (!selectedApplication?.id) return;
		dispatch(
			DeleteRegisteredApplicationByIdAsync(selectedApplication.id) as any,
		);
		setIsDeletePopupOpen(false);
		handleClose();
	};

	if (!isEditDrawerOpen || !selectedApplication) return null;

	return (
		<>
			<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
				{/* Header */}
				<div className="flex items-center justify-between p-8 border-b border-white/5 flex-shrink-0 bg-white/[0.02] backdrop-blur-md">
					<div className="flex items-center space-x-4">
						<div className="bg-gradient-to-br from-emerald-500 via-teal-500 to-cyan-500 p-3 rounded-2xl shadow-lg shadow-emerald-500/20 ring-1 ring-white/20">
							<Settings className="w-6 h-6 text-white" />
						</div>
						<div>
							<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-emerald-100 to-teal-100 bg-clip-text text-transparent tracking-tight">
								Edit Application
							</h2>
							<p className="text-white/40 text-sm font-medium">
								Modify{" "}
								{editFormData.applicationName || "application"}{" "}
								details.
							</p>
						</div>
					</div>
					<button
						onClick={handleClose}
						disabled={isDisabled}
						className="p-2.5 rounded-xl bg-white/5 hover:bg-red-500/10 border border-white/10 hover:border-red-500/20 transition-all duration-300 text-white/50 hover:text-red-400 group"
					>
						<X className="w-5 h-5 group-hover:rotate-90 transition-transform duration-300" />
					</button>
				</div>

				{/* Form */}
				<div className="flex-1 overflow-y-auto p-8 space-y-8 min-h-0 scrollbar-thin scrollbar-thumb-white/10">
					<div className="space-y-6">
						<div className="flex items-center space-x-2 pb-2 border-b border-white/5">
							<AppWindow className="w-4 h-4 text-emerald-400" />
							<span className="text-white/60 text-xs font-bold uppercase tracking-wider">
								Application Details
							</span>
						</div>

						<div className="space-y-6">
							<div className="space-y-2">
								<label
									className="text-white/80 font-semibold text-sm ml-1"
									htmlFor="applicationName"
								>
									Application Name
								</label>
								<Input
									placeholder="Enter application name..."
									variant="bordered"
									size="lg"
									value={editFormData.applicationName}
									onValueChange={(v) =>
										handleInput("applicationName", v)
									}
									startContent={
										<AppWindow className="w-5 h-5 text-emerald-400 mr-3" />
									}
									classNames={{
										input: "text-white placeholder:text-white/20 p-3",
										inputWrapper:
											"bg-white/5 border-white/10 hover:border-emerald-500/30 focus-within:!border-emerald-500/50 transition-all min-h-[56px] rounded-2xl",
									}}
								/>
							</div>

							<div className="space-y-2">
								<label
									className="text-white/80 font-semibold text-sm ml-1"
									htmlFor="description"
								>
									Description
								</label>
								<Input
									placeholder="Describe what this application does..."
									variant="bordered"
									size="lg"
									value={editFormData.description}
									onValueChange={(v) =>
										handleInput("description", v)
									}
									startContent={
										<FileText className="w-5 h-5 text-teal-400 mr-3" />
									}
									classNames={{
										input: "text-white placeholder:text-white/20 p-3",
										inputWrapper:
											"bg-white/5 border-white/10 hover:border-teal-500/30 focus-within:!border-teal-500/50 transition-all min-h-[56px] rounded-2xl",
									}}
								/>
							</div>

							<div className="space-y-2">
								<label
									className="text-white/80 font-semibold text-sm ml-1"
									htmlFor="applicationRegistrationGuid"
								>
									Azure Registration GUID
								</label>
								<Input
									placeholder="Enter Azure App Registration GUID..."
									variant="bordered"
									size="lg"
									value={
										editFormData.applicationRegistrationGuid ??
										""
									}
									onValueChange={(v) =>
										handleInput(
											"applicationRegistrationGuid",
											v,
										)
									}
									startContent={
										<KeyRound className="w-5 h-5 text-cyan-400 mr-3" />
									}
									classNames={{
										input: "text-white font-mono placeholder:text-white/20 p-3",
										inputWrapper:
											"bg-white/5 border-white/10 hover:border-cyan-500/30 focus-within:!border-cyan-500/50 transition-all min-h-[56px] rounded-2xl",
									}}
								/>
							</div>

							<div className="flex items-center justify-between p-4 bg-white/5 rounded-2xl border border-white/10 hover:border-white/20 transition-all duration-300">
								<div className="flex items-center space-x-3">
									<div className="bg-emerald-500/10 p-2 rounded-lg border border-emerald-500/20">
										<AppWindow className="w-4 h-4 text-emerald-400" />
									</div>
									<div>
										<p className="text-white/80 font-semibold text-sm">
											Azure Registered
										</p>
										<p className="text-white/40 text-xs">
											This application is registered in
											Azure AD
										</p>
									</div>
								</div>
								<button
									onClick={() =>
										handleInput(
											"isAzureRegistered",
											!editFormData.isAzureRegistered,
										)
									}
									className={`relative w-12 h-6 rounded-full transition-all duration-300 focus:outline-none ${editFormData.isAzureRegistered ? "bg-emerald-500" : "bg-white/10"}`}
								>
									<span
										className={`absolute top-0.5 left-0.5 w-5 h-5 bg-white rounded-full shadow transition-transform duration-300 ${editFormData.isAzureRegistered ? "translate-x-6" : "translate-x-0"}`}
									/>
								</button>
							</div>
						</div>
					</div>

					{/* Info panel */}
					<div className="bg-white/5 backdrop-blur-sm rounded-2xl p-6 border border-white/10">
						<h3 className="text-white/80 font-bold mb-4 flex items-center space-x-2">
							<Settings className="w-4 h-4 text-emerald-400" />
							<span className="text-xs uppercase tracking-wider">
								Application Info
							</span>
						</h3>
						<div className="space-y-3 text-sm">
							<div className="flex justify-between items-center">
								<span className="text-white/40">
									Application ID:
								</span>
								<span className="text-white/80 font-mono text-xs bg-white/5 px-2 py-1 rounded">
									{selectedApplication.id}
								</span>
							</div>
							<div className="flex justify-between items-center">
								<span className="text-white/40">
									Azure Registered:
								</span>
								<span
									className={`text-xs font-medium px-2 py-1 rounded-full ${selectedApplication.isAzureRegistered ? "bg-emerald-500/10 text-emerald-400" : "bg-white/5 text-white/40"}`}
								>
									{selectedApplication.isAzureRegistered
										? "Yes"
										: "No"}
								</span>
							</div>
						</div>
					</div>
				</div>

				{/* Footer */}
				<div className="border-t border-white/5 p-8 flex-shrink-0 bg-black/40 backdrop-blur-xl">
					<div className="flex items-center space-x-4">
						<Button
							onPress={handleSave}
							disabled={
								isDisabled ||
								!editFormData.applicationName.trim()
							}
							isLoading={IsLoading}
							className="px-6 h-14 bg-gradient-to-r from-emerald-600 via-teal-600 to-cyan-600 text-white font-bold text-lg hover:shadow-2xl hover:shadow-emerald-500/30 transition-all duration-300 rounded-2xl group border border-white/10 disabled:opacity-50"
						>
							{!IsLoading && (
								<Save className="w-5 h-5 mr-2 group-hover:scale-110 transition-transform" />
							)}
							<span>Save Changes</span>
						</Button>
						<Button
							onPress={() => setIsDeletePopupOpen(true)}
							disabled={isDisabled}
							className="h-14 px-6 bg-red-500/10 text-red-400 font-semibold hover:bg-red-500/20 transition-all duration-300 rounded-2xl border border-red-500/20 hover:text-red-300 flex items-center space-x-2"
						>
							<Trash className="w-5 h-5" />
							<span>Delete</span>
						</Button>
					</div>
				</div>
			</div>

			<DeletePopupComponent
				isOpen={isDeletePopupOpen}
				onClose={() => setIsDeletePopupOpen(false)}
				onAction={handleDelete}
				title={RegisterApplicationsConstants.DeletePopupDialog.Title}
				description={`Are you sure you want to delete "${editFormData.applicationName}"? ${RegisterApplicationsConstants.DeletePopupDialog.Description}`}
				isLoading={IsLoading}
			/>
		</>
	);
}
