import { ReactNode } from "react";
import { AgentDataDTO } from "@models/agent-data-dto";

export interface Environment {
	production: boolean;
	apiBaseUrl: string;
	msalConfig: {
		auth: {
			clientId: string;
			authority: string;
		};
		scopes: string[];
	};
	apiConfig: {
		scopes: string[];
		uri: string;
		apiScope: string[];
	};
}

export interface AgentData {
	agentName: string;
	agentMetaPrompt: string;
	applicationName: string;
	agentId: string;
}

export interface ResponseDTO {
	isSuccess: boolean;
	responseData: any;
}

export interface User {
	id: string;
	name: string;
	email: string;
}

export interface AuthProviderProps {
	children: ReactNode;
}

export interface AuthContextType {
	user: User | null;
	isAuthenticated: boolean;
	isLoading: boolean;
	login: () => Promise<void>;
	logout: () => Promise<void>;
	getAccessToken: () => Promise<string | null>;
}

export interface ModifyAgentComponentProps {
	editFormData: AgentDataDTO;
	selectedAgent: AgentDataDTO | null;
	setEditFormData: React.Dispatch<React.SetStateAction<AgentDataDTO>>;
	setSelectedAgent: React.Dispatch<React.SetStateAction<AgentDataDTO | null>>;
	isEditDrawerOpen: boolean;
	onTestAgent: () => void;
	onEditClose: () => void;
	isDisabled: boolean;
}

export interface AuthenticatedAppProps {
	children: React.ReactNode;
}

export interface AgentsListComponentProps {
	agentsDataList: AgentDataDTO[];
	handleAgentClick: (agent: AgentDataDTO) => void;
	onClose: () => void;
	isDisabled: boolean;
}

export interface ChatMessage {
	id: string;
	type: "user" | "bot";
	content: string;
	timestamp: Date;
}

export interface TestAgentComponentProps {
	selectedAgent: AgentDataDTO | null;
	editFormData: AgentDataDTO;
	onClose: () => void;
}
