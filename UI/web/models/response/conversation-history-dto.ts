import { ChatHistoryDTO } from "@models/response/chat-history-dto";

export class ConversationHistoryDTO {
	id: string = "";
	conversationId: string = "";
	userName: string = "";
	chatHistory: ChatHistoryDTO[] = [];
	lastModifiedOn: Date = new Date();
}
