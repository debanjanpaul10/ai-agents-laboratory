export class NotificationsResponseDTO {
	id: string = "";
	title: string = "";
	message: string = "";
	recipientUserName: string = "";
	notificationType: string = "";
	createdBy: string = "";
	isGlobal: boolean = false;
	isActive: boolean = false;
	dateCreated: Date = new Date();
}
