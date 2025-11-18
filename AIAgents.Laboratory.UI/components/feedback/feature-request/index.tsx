import { useState } from "react";
import { Button, Input } from "@heroui/react";
import { Lightbulb, FileText, Send, X, Info } from "lucide-react";

import { NewFeatureRequestDTO } from "@models/new-feature-request-dto";
import { ShowErrorToaster } from "@shared/toaster";
import { useAuth } from "@auth/AuthProvider";
import { useAppDispatch } from "@store/index";
import { SubmitFeatureRequestDataAsync } from "@store/common/actions";
import { NewFeatureRequestConstants } from "@helpers/constants";

export default function FeatureRequestComponent({
	onClose,
}: {
	onClose: () => void;
}) {
	const authContext = useAuth();
	const dispatch = useAppDispatch();

	const [isSubmitting, setIsSubmitting] = useState(false);
	const [formData, setFormData] = useState<NewFeatureRequestDTO>({
		title: "",
		description: "",
		createdBy: "",
	});

	const handleInputChange = (
		field: keyof NewFeatureRequestDTO,
		value: string
	) => {
		setFormData((prev) => ({ ...prev, [field]: value }));
	};

	const handleSubmit = async () => {
		if (!formData.title.trim() || !formData.description.trim()) {
			ShowErrorToaster("Please fill in all required fields");
			return;
		}

		setIsSubmitting(true);
		const featureRequest: NewFeatureRequestDTO = {
			...formData,
			createdBy:
				authContext.user?.email ||
				authContext.user?.name ||
				"Anonymous",
		};

		const accessToken = await authContext.getAccessToken();
		accessToken &&
			dispatch(
				SubmitFeatureRequestDataAsync(featureRequest, accessToken)
			);

		setIsSubmitting(false);
	};

	return (
		<div className="h-full flex flex-col">
			{/* Header */}
			<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
				<div className="flex items-center space-x-3">
					<div className="bg-gradient-to-r from-emerald-500 to-teal-600 p-2 rounded-xl">
						<Lightbulb className="w-5 h-5 text-white" />
					</div>
					<div>
						<h2 className="text-xl font-bold bg-gradient-to-r from-white via-emerald-100 to-teal-100 bg-clip-text text-transparent">
							{NewFeatureRequestConstants.Headers.Heading}
						</h2>
						<p className="text-white/50 text-sm">
							{NewFeatureRequestConstants.Headers.SubHeading}
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
				{/* Feature Title Field */}
				<div className="space-y-2">
					<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
						<Lightbulb className="w-4 h-4 text-emerald-400" />
						<span>Feature Title</span>
						<span className="text-red-400">*</span>
					</label>
					<div className="relative group">
						<div className="absolute -inset-0.5 bg-gradient-to-r from-emerald-500/20 to-teal-500/20 rounded-xl blur opacity-50 group-focus-within:opacity-75 transition duration-300"></div>
						<Input
							value={formData.title}
							onChange={(e) =>
								handleInputChange("title", e.target.value)
							}
							placeholder={
								NewFeatureRequestConstants.Placeholders
									.FeatureTitle
							}
							className="relative"
							radius="full"
							classNames={{
								input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
								inputWrapper:
									"bg-white/5 border-white/10 hover:border-white/20 focus-within:border-emerald-500/50 min-h-[48px]",
							}}
						/>
					</div>
				</div>

				{/* Feature Description Field */}
				<div className="space-y-2">
					<label className="text-white/80 text-sm font-medium flex items-center space-x-2">
						<FileText className="w-4 h-4 text-teal-400" />
						<span>Description</span>
						<span className="text-red-400">*</span>
					</label>
					<div className="relative">
						<textarea
							value={formData.description}
							onChange={(e) =>
								handleInputChange("description", e.target.value)
							}
							placeholder={
								NewFeatureRequestConstants.Placeholders
									.FeatureDescription
							}
							rows={12}
							className="w-full bg-white/5 border border-white/10 text-white placeholder:text-white/40 resize-none px-4 py-3 rounded-xl hover:border-white/20 focus:border-teal-500/50 focus:outline-none transition-colors duration-200"
						/>
					</div>
				</div>

				{/* SubText */}
				<div className="space-y-2">
					<p className="text-white/50 text-sm font-medium flex items-center space-x-2">
						<Info className="w-4 h-4 text-yellow-400" />
						<span>{NewFeatureRequestConstants.PIIMessage}</span>
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
							!formData.title.trim() ||
							!formData.description.trim()
						}
						className="flex items-center bg-gradient-to-r from-emerald-500 to-teal-600 text-white font-semibold hover:from-emerald-600 hover:to-teal-700 transition-all duration-300 px-6 py-3 min-h-[44px] disabled:opacity-50 disabled:cursor-not-allowed"
						radius="full"
					>
						{!isSubmitting && <Send className="w-4 h-4" />}
						<span className="ml-2">Submit Feature Request</span>
					</Button>
				</div>
			</div>
		</div>
	);
}
