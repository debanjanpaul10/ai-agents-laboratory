import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]): string {
	return twMerge(clsx(inputs));
}

// Generate a unique ID for messages
export function GenerateMessageId(): string {
	const cryptoRandomNumber = crypto.randomUUID();
	return `msg_${Date.now()}_${cryptoRandomNumber}`;
}

export function DownloadFile(downloadUrl: string, fileName: string) {
	const link = document.createElement("a");
	link.href = downloadUrl;
	link.download = fileName;
	link.target = "_blank";
	document.body.appendChild(link);
	link.click();
	link.remove();
}
