import { useEffect, useState } from "react";
import { Button, Input } from "@heroui/react";
import { AppWindow, KeyRound, RotateCcw, Sparkles, X } from "lucide-react";

import { useAppDispatch, useAppSelector } from "@store/index";
import {
	RegisterNewApplicationAsync,
	ToggleRegisterNewApplicationDrawer,
} from "@store/tools-skills/actions";
import { RegisteredApplicationDTO } from "@models/request/registered-application.dto";
import { RegisterApplicationsConstants } from "@helpers/constants";

const emptyForm = (): RegisteredApplicationDTO => ({
	id: 0,
	applicationName: "",
	description: "",
	applicationRegistrationGuid: null,
	isAzureRegistered: false,
});

export default function RegisterNewApplicationFlyoutComponent() {
	const dispatch = useAppDispatch();

	const [isDrawerOpen, setIsDrawerOpen] = useState(false);
	const [formData, setFormData] =
		useState<RegisteredApplicationDTO>(emptyForm());

	const IsDrawerOpen = useAppSelector(
		(state) => state.ToolSkillsReducer.isRegisterNewApplicationDrawerOpen,
	);
	const IsLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isRegisteredApplicationsLoading,
	);

	useEffect(() => {
		if (isDrawerOpen !== IsDrawerOpen) {
			setIsDrawerOpen(IsDrawerOpen);
			if (IsDrawerOpen) setFormData(emptyForm());
		}
	}, [IsDrawerOpen]);

	const onClose = () => dispatch(ToggleRegisterNewApplicationDrawer(false));

	const handleInput = (
		field: keyof RegisteredApplicationDTO,
		value: string | boolean,
	) => {
		setFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleSubmit = () => {
		dispatch(RegisterNewApplicationAsync(formData) as any);
		onClose();
	};

	const isFormValid = formData.applicationName.trim() !== "";

	return (
		isDrawerOpen && (
			<>
				<div className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300" />
				<div className="fixed right-0 top-0 md:w-1/2 h-screen z-50 transition-all duration-500 ease-in-out">
					<div className="absolute inset-0 bg-gradient-to-l from-blue-600/20 via-indigo-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10" />
					<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl flex flex-col">
						{/* Header */}
						<div className="flex items-center justify-between p-8 border-b border-white/5 flex-shrink-0 bg-white/[0.02] backdrop-blur-md">
							<div className="flex items-center space-x-4">
								<div className="bg-gradient-to-br from-blue-500 via-indigo-500 to-cyan-500 p-3 rounded-2xl shadow-lg shadow-blue-500/20 ring-1 ring-white/20">
									<AppWindow className="w-6 h-6 text-white" />
								</div>
								<div>
									<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-blue-100 to-indigo-100 bg-clip-text text-transparent tracking-tight">
										{
											RegisterApplicationsConstants
												.RegisterNewApp.Header
										}
									</h2>
									<p className="text-white/40 text-sm font-medium">
										{
											RegisterApplicationsConstants
												.RegisterNewApp.SubHeader
										}
									</p>
								</div>
							</div>
							<button
								onClick={onClose}
								className="p-2.5 rounded-xl bg-white/5 hover:bg-red-500/10 border border-white/10 hover:border-red-500/20 transition-all duration-300 text-white/50 hover:text-red-400 group"
							>
								<X className="w-5 h-5 group-hover:rotate-90 transition-transform duration-300" />
							</button>
						</div>

						{/* Form */}
						<div className="flex-1 overflow-y-auto p-8 space-y-8 min-h-0 scrollbar-thin scrollbar-thumb-white/10">
							<div className="space-y-6">
								<div className="flex items-center space-x-2 pb-2 border-b border-white/5">
									<Sparkles className="w-4 h-4 text-blue-400" />
									<span className="text-white/60 text-xs font-bold uppercase tracking-wider">
										Application Details
									</span>
								</div>

								<div className="space-y-6">
									<div className="space-y-2">
										<label className="text-white/80 font-semibold text-sm ml-1">
											Application Name{" "}
											<span className="text-red-400">
												*
											</span>
										</label>
										<Input
											placeholder="Enter application name..."
											variant="bordered"
											size="lg"
											value={formData.applicationName}
											onValueChange={(v) =>
												handleInput(
													"applicationName",
													v,
												)
											}
											startContent={
												<AppWindow className="w-5 h-5 text-blue-400 mr-3" />
											}
											classNames={{
												input: "text-white placeholder:text-white/20 p-3",
												inputWrapper:
													"mt-2 bg-white/5 border-white/10 hover:border-blue-500/30 focus-within:!border-blue-500/50 transition-all min-h-[56px] rounded-2xl",
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
										<textarea
											placeholder="Describe what this application does..."
											rows={4}
											value={formData.description}
											onChange={(e) =>
												handleInput(
													"description",
													e.target.value,
												)
											}
											className="mt-2 w-full bg-white/5 border border-white/10 text-white placeholder:text-white/20 resize-none px-4 py-3 rounded-2xl hover:border-orange-500/30 focus:border-orange-500/50 focus:outline-none transition-all duration-200"
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
											placeholder="Enter Azure App Registration GUID (optional)..."
											variant="bordered"
											size="lg"
											value={
												formData.applicationRegistrationGuid ??
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
													"mt-2 bg-white/5 border-white/10 hover:border-cyan-500/30 focus-within:!border-cyan-500/50 transition-all min-h-[56px] rounded-2xl",
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
													{
														RegisterApplicationsConstants
															.RegisterNewApp
															.AzureRegistration
															.Header
													}
												</p>
												<p className="text-white/40 text-xs">
													{
														RegisterApplicationsConstants
															.RegisterNewApp
															.AzureRegistration
															.SubHeader
													}
												</p>
											</div>
										</div>
										<button
											onClick={() =>
												handleInput(
													"isAzureRegistered",
													!formData.isAzureRegistered,
												)
											}
											className={`relative w-12 h-6 rounded-full transition-all duration-300 focus:outline-none ${formData.isAzureRegistered ? "bg-emerald-500" : "bg-white/10"}`}
										>
											<span
												className={`absolute top-0.5 left-0.5 w-5 h-5 bg-white rounded-full shadow transition-transform duration-300 ${formData.isAzureRegistered ? "translate-x-6" : "translate-x-0"}`}
											/>
										</button>
									</div>
								</div>
							</div>

							{/* Info box */}
							<div className="relative group">
								<div className="absolute inset-0 bg-blue-500/10 blur-xl opacity-50 group-hover:opacity-100 transition duration-500" />
								<div className="relative bg-gradient-to-br from-blue-500/10 to-indigo-500/5 border border-blue-500/20 rounded-2xl p-5 backdrop-blur-sm">
									<div className="flex items-start space-x-3">
										<div className="bg-blue-500/20 p-2 rounded-lg">
											<AppWindow className="w-4 h-4 text-blue-400" />
										</div>
										<p className="text-blue-100/70 text-sm leading-relaxed">
											<span className="text-blue-400 font-bold block mb-1">
												{
													RegisterApplicationsConstants
														.RegisterNewApp.InfoBox
														.Header
												}
											</span>
											{
												RegisterApplicationsConstants
													.RegisterNewApp.InfoBox.Text
											}
										</p>
									</div>
								</div>
							</div>
						</div>

						{/* Footer */}
						<div className="border-t border-white/5 p-8 flex-shrink-0 bg-black/40 backdrop-blur-xl">
							<div className="flex items-center space-x-4">
								<Button
									onPress={handleSubmit}
									isLoading={IsLoading}
									disabled={!isFormValid || IsLoading}
									className="px-6 h-14 bg-gradient-to-r from-blue-600 via-indigo-600 to-cyan-600 text-white font-bold text-lg hover:shadow-2xl hover:shadow-blue-500/30 transition-all duration-300 rounded-2xl group border border-white/10 disabled:opacity-50 disabled:cursor-not-allowed"
								>
									{!IsLoading && (
										<Sparkles className="w-5 h-5 mr-2 group-hover:scale-110 transition-transform" />
									)}
									<span>Register Application</span>
								</Button>
								<Button
									onPress={() => setFormData(emptyForm())}
									disabled={IsLoading}
									className="h-14 px-6 bg-blue-500/5 text-blue-400 font-semibold hover:bg-blue-500/10 transition-all duration-300 rounded-2xl border border-blue-500/20 hover:text-blue-300 flex items-center space-x-2"
								>
									<RotateCcw className="w-5 h-5" />
									<span>Reset</span>
								</Button>
							</div>
						</div>
					</div>
				</div>
			</>
		)
	);
}
