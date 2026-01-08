import { AiVisionImagesDataDTO } from "./ai-vision-images-data-dto";
import { CreateAgentDTO } from "./create-agent-dto";

export class AgentDataDTO extends CreateAgentDTO {
	agentId: string = "";
	createdBy: string = "";
	dateCreated: Date = new Date();
	aiVisionImagesData: AiVisionImagesDataDTO[] = [];
}
