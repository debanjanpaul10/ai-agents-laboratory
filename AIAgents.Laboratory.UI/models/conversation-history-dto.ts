export class ConversationHistoryDTO {
	id: string = "";
	conversationId: string = "";
	userName: string = "";
	chatHistory: ChatHistoryDTO[] = [];
	lastModifiedOn: Date = new Date();
}

export class ChatHistoryDTO {
	role: string = "";
	content: string = "";
}
