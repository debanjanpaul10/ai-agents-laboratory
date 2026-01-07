import { ChangeEvent, useState } from "react";
import { Button } from "@heroui/react";
import {
	ArrowRight,
	Download,
	FileText,
	ScanEye,
	Trash2,
	Upload,
} from "lucide-react";

import { VisionImagesFlyoutProps } from "@shared/types";
import { AiVisionImagesFlyoutPropsConstants } from "@helpers/constants";
import { DownloadExistingDocument } from "@shared/utils";

export default function VisionImagesFlyoutComponent({
	isOpen,
	onClose,
	onImagesChange,
	selectedImages,
	existingImages = [],
	onExistingImagesChange,
	removedImages = [],
}: VisionImagesFlyoutProps) {
	const [dragActive, setDragActive] = useState(false);

	const handleImageUpload = (e: ChangeEvent<HTMLInputElement>) => {
		const imageInput = e.target as HTMLInputElement;
		const newImages = Array.from(imageInput.files || []);

		// Validate the image sizes
		const validImages = newImages.filter((img) => {
			if (img.size > 10 * 1024 * 1024) {
				alert(
					`Image "${img.name}" exceeds 10MB limit and will be skipped.`
				);
				return false;
			}
			return true;
		});

		// Add to existing images, avoiding duplicates
		const updatedImages = [...selectedImages];
		validImages.forEach((img) => {
			if (
				!updatedImages.some(
					(existing) =>
						existing.name === img.name && existing.size === img.size
				)
			) {
				updatedImages.push(img);
			}
		});

		onImagesChange(updatedImages);
		imageInput.value = "";
	};

	const handleDrag = (e: React.DragEvent) => {
		e.preventDefault();
		e.stopPropagation();
		if (e.type === "dragenter" || e.type === "dragover")
			setDragActive(true);
		else if (e.type === "dragleave") setDragActive(false);
	};

	const handleDrop = (e: React.DragEvent) => {
		e.preventDefault();
		e.stopPropagation();
		setDragActive(false);

		const droppedImages = Array.from(e.dataTransfer.files);

		// Validate file sizes
		const validImages = droppedImages.filter((image) => {
			if (image.size > 10 * 1024 * 1024) {
				alert(
					`File "${image.name}" exceeds 10MB limit and will be skipped.`
				);
				return false;
			}
			return true;
		});

		const updatedImages = [...selectedImages];
		validImages.forEach((image) => {
			if (
				!updatedImages.some(
					(existing) =>
						existing.name === image.name &&
						existing.size === image.size
				)
			)
				updatedImages.push(image);
		});

		onImagesChange(updatedImages);
	};

	const removeImage = (index: number) => {
		const updatedImages = selectedImages.filter((_, i) => i !== index);
		onImagesChange(updatedImages);
	};

	const removeExistingImage = (imageName: string) => {
		const updatedRemovedImages = [...removedImages, imageName];
		onExistingImagesChange?.(updatedRemovedImages);
	};

	const clearAllImages = () => {
		onImagesChange([]);
		const allRemovedImages = existingImages.map((img) => img.name);
		onExistingImagesChange?.(allRemovedImages);
	};

	const formatFileSize = (bytes: number) => {
		if (bytes === 0) return "0 Bytes";
		const k = 1024;
		const sizes = ["Bytes", "KB", "MB", "GB"];
		const i = Math.floor(Math.log(bytes) / Math.log(k));
		return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];
	};

	if (!isOpen) return null;

	return (
		<>
			<div className="relative h-full bg-gradient-to-br from-slate-900/95 via-gray-900/95 to-slate-800/95 backdrop-blur-xl border border-white/10 rounded-xl shadow-2xl flex flex-col">
				{/* HEADER */}
				<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
					<div className="flex items-center space-x-3">
						<div className="bg-gradient-to-r from-purple-500 to-blue-600 p-2 rounded-xl">
							<ScanEye className="w-5 h-5 text-white" />
						</div>
						<div>
							<h2 className="text-xl font-bold bg-gradient-to-r from-white via-purple-100 to-blue-100 bg-clip-text text-transparent">
								{
									AiVisionImagesFlyoutPropsConstants.Headers
										.Heading
								}
							</h2>
							<p className="text-white/60 text-sm">
								{
									AiVisionImagesFlyoutPropsConstants.Headers
										.SubHeading
								}
							</p>
						</div>
					</div>
					<Button
						onPress={onClose}
						title="Close window"
						className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
					>
						<ArrowRight className="w-4 h-4" />
					</Button>
				</div>

				{/* CONTENT */}
				<div className="flex-1 overflow-y-auto p-6 space-y-6 min-h-0">
					{/* IMAGE UPLOAD */}
					<div className="space-y-4">
						<div
							className={`relative border-2 border-dashed rounded-xl p-8 text-center transition-all duration-300 cursor-pointer ${
								dragActive
									? "border-red-500/50 bg-red-500/10"
									: "border-white/20 hover:border-red-500/30 hover:bg-red-500/5"
							}`}
							onDragEnter={handleDrag}
							onDragLeave={handleDrag}
							onDragOver={handleDrag}
							onDrop={handleDrop}
						>
							<div className="flex flex-col items-center space-y-4">
								<div className="bg-gradient-to-r from-red-500/20 to-pink-500/20 p-4 rounded-full">
									<Upload className="w-8 h-8 text-red-400" />
								</div>
								<div>
									<h3 className="text-white font-medium mb-2">
										{
											AiVisionImagesFlyoutPropsConstants
												.Hints.DropImages
										}
									</h3>
									<p className="text-white/60 text-sm">
										{
											AiVisionImagesFlyoutPropsConstants
												.Hints.SupportedTypes
										}
									</p>
								</div>
								<input
									onChange={handleImageUpload}
									type="file"
									accept=".jpg,.png,.jpeg,.svg"
									multiple
									className="absolute inset-0 w-full h-full opacity-0 cursor-pointer z-10"
								/>
							</div>
						</div>
					</div>

					{/* EXISTING IMAGES */}
					{existingImages.length > 0 && (
						<div className="space-y-4">
							<h3 className="text-white/80 font-medium flex items-center space-x-2">
								<FileText className="w-4 h-4 text-blue-400" />
								<span>
									Existing Images (
									{
										existingImages.filter(
											(doc) =>
												!removedImages.includes(
													doc.name
												)
										).length
									}
									)
								</span>
							</h3>
							<div className="space-y-2 max-h-32 overflow-y-auto">
								{existingImages
									.filter(
										(doc) =>
											!removedImages.includes(doc.name)
									)
									.map((doc, index) => (
										<div
											key={`existing-${doc.name}-${index}`}
											className="flex items-center justify-between p-3 bg-blue-500/5 rounded-lg border border-blue-500/20 hover:border-blue-500/30 transition-colors"
										>
											<div className="flex items-center space-x-3 flex-1 min-w-0">
												<FileText className="w-4 h-4 text-blue-400 flex-shrink-0" />
												<div className="min-w-0 flex-1">
													<p className="text-white text-sm font-medium truncate">
														{doc.name}
													</p>
													<p className="text-white/60 text-xs">
														{formatFileSize(
															doc.size
														)}{" "}
														• Existing
													</p>
												</div>
											</div>
											<div className="flex items-center space-x-2 flex-shrink-0">
												<Button
													onPress={() =>
														DownloadExistingDocument(
															doc
														)
													}
													title="Download existing document"
													className="p-2 rounded-lg bg-blue-500/10 hover:bg-blue-500/20 border border-blue-500/20 hover:border-blue-500/30 transition-all duration-200 text-blue-400 hover:text-blue-300 flex-shrink-0"
												>
													<Download className="w-4 h-4" />
												</Button>
												<Button
													onPress={() =>
														removeExistingImage(
															doc.name
														)
													}
													title="Remove existing document"
													className="p-2 rounded-lg bg-red-500/10 hover:bg-red-500/20 border border-red-500/20 hover:border-red-500/30 transition-all duration-200 text-red-400 hover:text-red-300 flex-shrink-0"
												>
													<Trash2 className="w-4 h-4" />
												</Button>
											</div>
										</div>
									))}
							</div>
						</div>
					)}

					{/* Selected Files List */}
					{selectedImages.length > 0 && (
						<div className="space-y-4">
							<h3 className="text-white/80 font-medium flex items-center space-x-2">
								<FileText className="w-4 h-4 text-green-400" />
								<span>
									New Images ({selectedImages.length})
								</span>
							</h3>
							<div className="space-y-2 max-h-32 overflow-y-auto">
								{selectedImages.map((file, index) => (
									<div
										key={`${file.name}-${index}`}
										className="flex items-center justify-between p-3 bg-white/5 rounded-lg border border-white/10 hover:border-white/20 transition-colors"
									>
										<div className="flex items-center space-x-3 flex-1 min-w-0">
											<FileText className="w-4 h-4 text-green-400 flex-shrink-0" />
											<div className="min-w-0 flex-1">
												<p className="text-white text-sm font-medium truncate">
													{file.name}
												</p>
												<p className="text-white/60 text-xs">
													{formatFileSize(file.size)}{" "}
													• New
												</p>
											</div>
										</div>
										<Button
											onPress={() => removeImage(index)}
											title="Remove file"
											className="p-2 rounded-lg bg-red-500/10 hover:bg-red-500/20 border border-red-500/20 hover:border-red-500/30 transition-all duration-200 text-red-400 hover:text-red-300 flex-shrink-0"
										>
											<Trash2 className="w-4 h-4" />
										</Button>
									</div>
								))}
							</div>
						</div>
					)}

					{/* Info */}
					<div className="bg-blue-500/10 border border-blue-500/20 rounded-lg p-4">
						<p className="text-blue-300 text-sm">
							<strong>Note: </strong>
							{AiVisionImagesFlyoutPropsConstants.Hints.Info}
						</p>
					</div>
				</div>

				{/* FOOTER */}
				<div className="border-t border-white/10 p-6 flex-shrink-0">
					<div className="flex items-center justify-between">
						<p className="text-white/60 text-sm">
							{selectedImages.length +
								existingImages.filter(
									(doc) => !removedImages.includes(doc.name)
								).length}{" "}
							file
							{selectedImages.length +
								existingImages.filter(
									(doc) => !removedImages.includes(doc.name)
								).length !==
							1
								? "s"
								: ""}{" "}
							total
						</p>
						<div className="flex space-x-3">
							<Button
								onPress={clearAllImages}
								title={
									AiVisionImagesFlyoutPropsConstants.Buttons
										.Clear
								}
								className="px-4 py-2 bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 text-white/70 hover:text-red-400 transition-all duration-200"
								radius="full"
								disabled={
									selectedImages.length === 0 &&
									existingImages.filter(
										(doc) =>
											!removedImages.includes(doc.name)
									).length === 0
								}
							>
								{
									AiVisionImagesFlyoutPropsConstants.Buttons
										.Clear
								}
							</Button>
							<Button
								onPress={onClose}
								title={
									AiVisionImagesFlyoutPropsConstants.Buttons
										.Done
								}
								className="px-6 py-2 bg-gradient-to-r from-green-500 to-emerald-600 text-white font-semibold hover:from-green-600 hover:to-emerald-700 transition-all duration-300"
								radius="full"
							>
								{
									AiVisionImagesFlyoutPropsConstants.Buttons
										.Done
								}
							</Button>
						</div>
					</div>
				</div>
			</div>
		</>
	);
}
