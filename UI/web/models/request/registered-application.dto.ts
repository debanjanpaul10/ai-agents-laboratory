export class RegisteredApplicationDTO {
	id: number = 0;
	applicationName: string = "";
	description: string = "";
	applicationRegistrationGuid: string | null = "";
	isAzureRegistered: boolean = false;
}
