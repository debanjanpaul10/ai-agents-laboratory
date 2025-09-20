import { CreateAgentDTO } from "./create-agent-dto";

export class AgentDataDTO extends CreateAgentDTO {
	agentId: string = "";
	createdBy: string = "";
}
