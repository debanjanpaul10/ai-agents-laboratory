import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";
import { ShowErrorToaster } from "./toaster";

export function cn(...inputs: ClassValue[]): string {
	return twMerge(clsx(inputs));
}

// Generate a unique ID for messages
export function GenerateMessageId(): string {
	return `msg_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
}

export function DownloadExistingDocument(doc: any): void {
	try {
		// Create a blob from the file data
		const blob = new Blob([doc], { type: doc.contentType });

		// Create a temporary download link
		const url = URL.createObjectURL(blob);
		const a = document.createElement("a");
		a.href = url;
		a.download = doc.name; // sets the filename
		document.body.appendChild(a);
		a.click();

		// Cleanup
		document.body.removeChild(a);
		URL.revokeObjectURL(url);
	} catch (error: any) {
		console.error("Error downloading document:", error);
		if (error.message) ShowErrorToaster(error.message);
	}
}
