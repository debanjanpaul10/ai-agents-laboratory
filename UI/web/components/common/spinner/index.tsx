import React from "react";
import Image from "next/image";

import AppLogo from "@public/images/icon.png";

interface LoadingSpinnerProps {
	size?: "sm" | "md" | "lg" | "xl";
	className?: string;
}

export function LoadingSpinner({
	size = "md",
	className = "",
}: LoadingSpinnerProps) {
	const sizeClasses = {
		sm: "w-4 h-4",
		md: "w-6 h-6",
		lg: "w-8 h-8",
		xl: "w-12 h-12",
	};

	return (
		<div className={`${sizeClasses[size]} ${className}`}>
			<div className="relative w-full h-full">
				{/* Outer ring */}
				<div className="absolute inset-0 rounded-full border-2 border-blue-200/20"></div>
				{/* Spinning ring */}
				<div className="absolute inset-0 rounded-full border-2 border-transparent border-t-blue-400 border-r-blue-400 animate-spin"></div>
				{/* Inner glow */}
				<div className="absolute inset-1 rounded-full bg-blue-400/10 animate-pulse"></div>
			</div>
		</div>
	);
}

interface FullScreenLoadingProps {
	isLoading: boolean;
	message?: string;
}

export function FullScreenLoading({
	isLoading,
	message = "Loading AI Agents Laboratory...",
}: FullScreenLoadingProps) {
	if (!isLoading) return null;

	return (
		<div className="fixed inset-0 z-50 flex items-center justify-center bg-gradient-to-br from-gray-900 via-slate-900 to-black">
			{/* Animated background elements */}
			<div className="absolute top-20 left-20 w-64 h-64 bg-gradient-to-br from-blue-500/10 to-purple-600/10 rounded-full blur-3xl animate-pulse"></div>
			<div className="absolute bottom-32 right-32 w-48 h-48 bg-gradient-to-br from-green-400/10 to-blue-500/10 rounded-full blur-2xl animate-pulse delay-1000"></div>
			<div className="absolute top-1/2 left-1/4 w-32 h-32 bg-gradient-to-br from-purple-400/10 to-pink-500/10 rounded-full blur-xl animate-pulse delay-2000"></div>

			{/* Loading content */}
			<div className="relative z-10 text-center">
				<div className="bg-white/5 backdrop-blur-xl rounded-3xl p-12 border border-white/10 shadow-2xl max-w-md mx-auto">
					<div className="flex flex-col items-center space-y-6">
						{/* Logo/Icon */}
						<div className="p-4 rounded-2xl">
							<Image
								src={AppLogo}
								alt="App Icon"
								height={100}
								width={100}
							/>
						</div>

						{/* Spinner */}
						<LoadingSpinner size="xl" />

						{/* Text */}
						<div className="text-center">
							<h2 className="text-2xl font-bold bg-gradient-to-r from-white via-blue-100 to-cyan-100 bg-clip-text text-transparent mb-2">
								{message}
							</h2>
						</div>

						{/* Progress dots */}
						<div className="flex space-x-2">
							<div className="w-2 h-2 bg-blue-400 rounded-full animate-pulse"></div>
							<div className="w-2 h-2 bg-blue-400 rounded-full animate-pulse delay-300"></div>
							<div className="w-2 h-2 bg-blue-400 rounded-full animate-pulse delay-700"></div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}
