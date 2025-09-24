export class DirectChatRequestDTO {
	userMessage: string = "";

	constructor(UserMessage: string = "") {
		this.userMessage = UserMessage;
	}
}
