import { AgentDataDTO } from "@models/response/agent-data-dto";

export class TopActiveAgentsDTO {
	activeAgentsCount: number = 0;
	topActiveAgents: AgentDataDTO[] = [];
}
