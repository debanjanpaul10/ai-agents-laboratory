import { environment } from "@environments/environment.base";
import { tokenService } from "@helpers/token-service";
import { NotificationsResponseDTO } from "@models/response/notifications-response-dto.model";
import { ShowErrorToaster } from "@shared/toaster";

type NotificationsStreamOptions = {
	onConnected?: () => void;
	onNotification: (notification: NotificationsResponseDTO) => void;
	onError?: (error: unknown) => void;
	signal: AbortSignal;
};

const STREAM_NOTIFICATIONS_URL = `${environment.apiBaseUrl}notifications/streamnotifications`;

export async function startNotificationsStream({
	onConnected,
	onNotification,
	onError,
	signal,
}: NotificationsStreamOptions) {
	const token = tokenService.getToken();
	if (!token) throw new Error("Missing access token");

	try {
		const response = await fetch(STREAM_NOTIFICATIONS_URL, {
			method: "GET",
			headers: {
				Authorization: `Bearer ${token}`,
				Accept: "text/event-stream",
			},
			signal,
		});

		if (!response.ok || !response.body) {
			throw new Error(`Notifications stream failed (${response.status})`);
		}

		const reader = response.body.getReader();
		const decoder = new TextDecoder("utf-8");
		let buffer = "";

		try {
			while (!signal.aborted) {
				const { value, done } = await reader.read();
				if (done) break;

				buffer += decoder.decode(value, { stream: true });
				buffer = await processStreamBuffer(
					buffer,
					onConnected,
					onNotification,
					onError,
				);
			}
		} finally {
			try {
				reader.releaseLock();
			} catch (error: any) {
				if (error.message) ShowErrorToaster(error.message);
			}
		}
	} catch (e: any) {
		if (e.name === "AbortError" || signal.aborted) {
			// Expected abort, exit cleanly
			return;
		}
		onError?.(e);
		throw e;
	}
}

async function processStreamBuffer(
	buffer: string,
	onConnected: (() => void) | undefined,
	onNotification: (notification: NotificationsResponseDTO) => void,
	onError: ((error: unknown) => void) | undefined,
): Promise<string> {
	let sepIndex: number;
	while ((sepIndex = buffer.indexOf("\n\n")) >= 0) {
		const rawBlock = buffer.slice(0, sepIndex).trimEnd();
		buffer = buffer.slice(sepIndex + 2);

		if (!rawBlock) continue;

		const { event, data } = parseSseBlock(rawBlock);
		handleSseEvent(event, data, onConnected, onNotification, onError);
	}

	return buffer;
}

function parseSseBlock(block: string): {
	event?: string;
	data?: string;
	id?: string;
} {
	const lines = block.split(/\r?\n/);
	let event: string | undefined;
	let data = "";
	let id: string | undefined;

	for (const line of lines) {
		if (!line || line.startsWith(":")) continue;

		if (line.startsWith("event:"))
			event = line.slice("event:".length).trim();
		else if (line.startsWith("data:"))
			data += line.slice("data:".length).trim();
		else if (line.startsWith("id:")) id = line.slice("id:".length).trim();
	}

	return { event, data: data || undefined, id };
}

function handleSseEvent(
	event: string | undefined,
	data: string | undefined,
	onConnected: (() => void) | undefined,
	onNotification: (notification: NotificationsResponseDTO) => void,
	onError: ((error: unknown) => void) | undefined,
): void {
	if (event === "connected") {
		onConnected?.();
		return;
	}

	if (event === "notification" && data) {
		try {
			onNotification(JSON.parse(data));
		} catch (e) {
			onError?.(e);
		}
	}
}
