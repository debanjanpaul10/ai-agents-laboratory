import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]): string {
	return twMerge(clsx(inputs));
}

// Generate a unique ID for messages
export function GenerateMessageId(): string {
	return `msg_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
}

export function DownloadFile(downloadUrl: string, fileName: string) {
	// Fallback for CORS or other issues
	var link = document.createElement("a");
	link.href = downloadUrl;
	link.download = fileName;
	link.target = "_blank";
	document.body.appendChild(link);
	link.click();
	document.body.removeChild(link);
}
