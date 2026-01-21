import { AiVisionImagesDataDTO } from "@models/response/ai-vision-images-data-dto";
import { CreateAgentDTO } from "@models/request/create-agent-dto";

export class AgentDataDTO extends CreateAgentDTO {
	agentId: string = "";
	createdBy: string = "";
	dateCreated: Date = new Date();
	dateModified: Date = new Date();
	aiVisionImagesData: AiVisionImagesDataDTO[] = [];
}
