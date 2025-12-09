import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
	return twMerge(clsx(inputs));
}

// Generate a unique ID for messages
export function generateMessageId() {
	return `msg_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
}
