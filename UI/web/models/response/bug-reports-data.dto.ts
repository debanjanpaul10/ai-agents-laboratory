import { BaseModelDto } from "@models/response/base-data-model.dto";

export class BugReportDataDto extends BaseModelDto {
	id: number = 0;
	title: string = "";
	description: string = "";
	bugSeverityId: number = 0;
	bugStatusId: number = 0;
	agentDetails: string = "";
	isActive: boolean = false;
}
