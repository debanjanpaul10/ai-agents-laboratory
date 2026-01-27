import { useState } from "react";
import { Button, Input, Select, SelectItem } from "@heroui/react";
import { Bug, FileText, AlertTriangle, Send, X, Info, Bot } from "lucide-react";

import { AddBugReportDTO } from "@models/request/add-bug-report-dto";
import { useAuth } from "@auth/AuthProvider";
import { BugReportConstants, SeverityOptions } from "@helpers/constants";
import { useAppDispatch } from "@store/index";
import { ShowErrorToaster } from "@shared/toaster";
import { AddBugReportDataAsync } from "@store/common/actions";
import { CommonToasterConstants } from "@helpers/toaster-constants";

export default function BugReportComponent({
	onClose,
}: {
	onClose: () => void;
}) {
	const authContext = useAuth();
	const dispatch = useAppDispatch();

	const [isSubmitting, setIsSubmitting] = useState(false);
	const [formData, setFormData] = useState<AddBugReportDTO>({
		bugTitle: "",
		bugDescription: "",
		bugSeverity: 0,
		createdBy: "",
		agentDetails: "",
	});

	const handleInputChange = (
		field: keyof AddBugReportDTO,
		value: string | number
	) => {
		setFormData((prev) => ({ ...prev, [field]: value }));
	};

	async function handleSubmit() {
		if (
			!formData.bugTitle.trim() ||
			!formData.bugDescription.trim() ||
			formData.bugSeverity === 0
		) {
			ShowErrorToaster(CommonToasterConstants.REQURED_FIELDS_MISSING);
			return;
		}

		setIsSubmitting(true);
		const bugReport: AddBugReportDTO = {
			...formData,
			createdBy:
				authContext.user?.email ||
				authContext.user?.name ||
				"Anonymous",
			agentDetails: formData.agentDetails || window.location.href,
		};

		dispatch(AddBugReportDataAsync(bugReport));

		setIsSubmitting(false);
	}

	return (
		<div className="h-full flex flex-col">
			{/* Header */}
			<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
				<div className="flex items-center space-x-3">
					<div className="bg-gradient-to-r from-red-400 to-red-500 p-2 rounded-xl">
						<Bug className="w-5 h-5 text-white" />
					</div>
					<div>
						<h2 className="text-xl font-bold bg-gradient-to-r from-white via-red-100 to-orange-100 bg-clip-text text-transparent">
							{BugReportConstants.Headers.Heading}
						</h2>
						<p className="text-white/50 text-sm">
							{BugReportConstants.Headers.SubHeading}
						</p>
					</div>
				</div>
				<Button
					onPress={onClose}
					isIconOnly
					className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400 min-w-[40px] h-[40px]"
					title="Close"
				>
					<X className="w-5 h-5" />
				</Button>
			</div>

			{/* Form content */}
			<div className="flex-1 overflow-y-auto p-6 space-y-6 min-h-0">
				{/* Bug Title Field */}
				<div className="space-y-2">
					<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
						<FileText className="w-4 h-4 text-red-400" />
						<span>Bug Title</span>
						<span className="text-red-400">*</span>
					</label>
					<div className="relative group">
						<div className="absolute -inset-0.5 bg-gradient-to-r from-red-500/20 to-orange-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
						<Input
							value={formData.bugTitle}
							onChange={(e) =>
								handleInputChange("bugTitle", e.target.value)
							}
							placeholder={
								BugReportConstants.Placeholders.BugTitle
							}
							className="relative"
							radius="full"
							classNames={{
								input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
								inputWrapper:
									"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-red-500/50 min-h-[48px]",
							}}
						/>
					</div>
				</div>

				{/* Bug Severity Field */}
				<div className="space-y-2">
					<label
						className="text-white/80 text-sm font-medium flex items-center space-x-2"
						htmlFor="severity-id"
					>
						<AlertTriangle className="w-4 h-4 text-orange-400" />
						<span>Severity</span>
						<span className="text-red-400">*</span>
					</label>
					<div className="relative group">
						<div className="absolute -inset-0.5 bg-gradient-to-r from-orange-500/20 to-yellow-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
						<Select
							aria-label="Severity"
							id="severity-id"
							selectedKeys={
								formData.bugSeverity > 0
									? [formData.bugSeverity.toString()]
									: []
							}
							onSelectionChange={(keys) => {
								const value = Array.from(keys)[0];
								handleInputChange(
									"bugSeverity",
									value ? parseInt(value as string) : 0
								);
							}}
							placeholder={
								BugReportConstants.Placeholders.BugSeverity
							}
							className="relative"
							radius="full"
							disallowEmptySelection={false}
							classNames={{
								trigger:
									"bg-white/5 border border-white/10 hover:border-white/20 data-[focus=true]:border-orange-500/50 min-h-[48px] text-white",
								value: "text-white group-data-[has-value=true]:text-white",
								popoverContent:
									"bg-gray-900/95 backdrop-blur-xl border border-white/10",
								listbox: "bg-gray-900/95",
								selectorIcon: "hidden",
							}}
						>
							{SeverityOptions.map((option) => (
								<SelectItem
									key={option.value}
									className="text-white hover:bg-white/10 data-[hover=true]:bg-white/10 py-3 px-4 my-1"
									textValue={option.label}
								>
									<span className={option.color}>
										{option.label}
									</span>
								</SelectItem>
							))}
						</Select>
					</div>
				</div>

				{/* Bug Description Field */}
				<div className="space-y-2">
					<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
						<FileText className="w-4 h-4 text-blue-400" />
						<span>Description</span>
						<span className="text-red-400">*</span>
					</label>
					<div className="relative">
						<textarea
							value={formData.bugDescription}
							onChange={(e) =>
								handleInputChange(
									"bugDescription",
									e.target.value
								)
							}
							placeholder={
								BugReportConstants.Placeholders.BugDescription
							}
							rows={8}
							className="w-full bg-white/5 border border-white/10 text-white placeholder:text-white/40 resize-none px-4 py-3 rounded-xl hover:border-white/20 focus:border-blue-500/50 focus:outline-none transition-colors duration-200"
						/>
					</div>
				</div>

				{/* Agent Details Field */}
				<div className="space-y-2">
					<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
						<Bot className="w-4 h-4 text-purple-400" />
						<span>Agent Name or Agent ID (Optional)</span>
					</label>
					<div className="relative group">
						<div className="absolute -inset-0.5 bg-gradient-to-r from-purple-500/20 to-blue-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
						<Input
							value={formData.agentDetails}
							onChange={(e) =>
								handleInputChange(
									"agentDetails",
									e.target.value
								)
							}
							placeholder={
								BugReportConstants.Placeholders.AgentName
							}
							className="relative"
							radius="full"
							classNames={{
								input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
								inputWrapper:
									"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-purple-500/50 min-h-[48px]",
							}}
						/>
					</div>
				</div>

				{/* SubText */}
				<div className="space-y-2">
					<p className="text-white/50 text-sm font-medium flex items-center space-x-2">
						<Info className="w-4 h-4 text-yellow-400" />
						<span>{BugReportConstants.PIIMessage}</span>
					</p>
				</div>
			</div>

			{/* Footer with submit button */}
			<div className="border-t border-white/10 p-6 flex-shrink-0">
				<div className="flex items-center justify-start space-x-3">
					<Button
						onPress={handleSubmit}
						isLoading={isSubmitting}
						isDisabled={
							isSubmitting ||
							!formData.bugTitle.trim() ||
							!formData.bugDescription.trim() ||
							formData.bugSeverity === 0
						}
						className="flex items-center bg-gradient-to-r from-red-500 to-orange-600 text-white font-semibold hover:from-red-600 hover:to-orange-700 transition-all duration-300 px-6 py-3 min-h-[44px] disabled:opacity-50 disabled:cursor-not-allowed"
						radius="full"
					>
						{!isSubmitting && <Send className="w-4 h-4" />}
						<span className="ml-2">Submit Bug Report</span>
					</Button>
				</div>
			</div>
		</div>
	);
}
