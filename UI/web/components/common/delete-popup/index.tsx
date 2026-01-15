import React from "react";
import { Button } from "@heroui/react";
import { AlertTriangle, X, Trash2 } from "lucide-react";

import { DeletePopupProps } from "@shared/types";

export default function DeletePopupComponent({
	isOpen,
	onClose,
	onDelete,
	title,
	description,
	isLoading = false,
}: DeletePopupProps) {
	if (!isOpen) return null;

	return (
		<div className="fixed inset-0 z-[100] flex items-center justify-center p-4">
			{/* Backdrop */}
			<div
				className="absolute inset-0 bg-black/60 backdrop-blur-md transition-opacity duration-300"
				onClick={!isLoading ? onClose : undefined}
			/>

			{/* Modal Container */}
			<div className="relative w-full max-w-md transform transition-all duration-300 ease-out animate-in zoom-in-95 fade-in duration-300">
				{/* Background Glow */}
				<div className="absolute -inset-1 bg-gradient-to-r from-red-600/30 to-rose-600/30 rounded-[2.5rem] blur-2xl opacity-50 -z-10"></div>

				{/* Modal Content */}
				<div className="relative bg-gradient-to-br from-slate-900/90 via-gray-900/95 to-black border border-white/10 rounded-[2rem] overflow-hidden shadow-2xl">
					{/* Close Button */}
					<button
						onClick={onClose}
						disabled={isLoading}
						className="absolute top-6 right-6 p-2 rounded-xl bg-white/5 hover:bg-white/10 border border-white/10 transition-all duration-300 text-white/50 hover:text-white disabled:opacity-0"
					>
						<X className="w-4 h-4" />
					</button>

					<div className="p-8">
						{/* Icon Header */}
						<div className="flex flex-col items-center text-center space-y-6">
							<div className="relative">
								<div className="absolute inset-0 bg-red-500/20 blur-xl rounded-full animate-pulse"></div>
								<div className="relative bg-gradient-to-br from-red-500/20 to-rose-500/10 p-5 rounded-2xl border border-red-500/30 shadow-lg shadow-red-500/10">
									<AlertTriangle className="w-10 h-10 text-red-500" />
								</div>
							</div>

							{/* Title and Description */}
							<div className="space-y-3">
								<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-red-100 to-rose-100 bg-clip-text text-transparent tracking-tight">
									{title}
								</h2>
								<p className="text-white/40 text-sm leading-relaxed font-medium">
									{description}
								</p>
							</div>
						</div>

						{/* Action Buttons */}
						<div className="mt-10 flex flex-col space-y-3">
							<Button
								onPress={onDelete}
								isLoading={isLoading}
								className="h-14 bg-gradient-to-r from-red-600 via-rose-600 to-red-700 text-white font-bold text-lg hover:shadow-2xl hover:shadow-red-500/40 transition-all duration-300 rounded-2xl group border border-white/10"
							>
								{!isLoading && (
									<Trash2 className="w-5 h-5 mr-2 group-hover:scale-110 transition-transform" />
								)}
								<span>Confirm Delete</span>
							</Button>

							<Button
								onPress={onClose}
								className="h-14 bg-white/5 text-white/60 font-semibold hover:bg-white/10 hover:text-white transition-all duration-300 rounded-2xl border border-white/10"
								disabled={isLoading}
							>
								Cancel
							</Button>
						</div>
					</div>

					{/* Danger Bar */}
					<div className="h-1.5 w-full bg-gradient-to-r from-red-500 via-rose-500 to-red-500 overflow-hidden relative">
						{isLoading && (
							<div className="absolute inset-0 bg-white/20 animate-shimmer"></div>
						)}
					</div>
				</div>
			</div>
		</div>
	);
}
