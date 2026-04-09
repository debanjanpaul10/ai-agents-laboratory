export class NotificationsResponseDTO {
	id: number = 0;
	title: string = "";
	message: string = "";
	recipientUserName: string = "";
	notificationType: string = "";
	createdBy: string = "";
	isGlobal: boolean = false;
	isActive: boolean = false;
}
