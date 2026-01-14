import { ReactNode } from "react";
import { AgentDataDTO } from "@models/agent-data-dto";
import { ToolSkillDTO } from "@models/tool-skill-dto";

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

export interface ReduxStoreType {
	type: string;
	payload: any;
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
	onOpenKnowledgeBase: () => void;
	selectedKnowledgeFiles: File[];
	removedExistingDocuments: string[];
	onOpenVisionImagesFlyout: () => void;
	selectedVisionImages: File[];
	removedExistingImages: string[];
}

export interface AuthenticatedAppProps {
	children: React.ReactNode;
}

export interface AgentsListComponentProps {
	agentsDataList: AgentDataDTO[];
	handleAgentClick: (agent: AgentDataDTO) => void;
	onClose: () => void;
	isDisabled: boolean;
	showCloseButton?: boolean;
	actionButton?: React.ReactNode;
}

export interface SkillsListComponentProps {
	toolSkillsList: ToolSkillDTO[];
	handleSkillClick: (skill: ToolSkillDTO) => void;
	onClose: () => void;
	isDisabled: boolean;
	showCloseButton?: boolean;
	actionButton?: React.ReactNode;
}

export interface ChatMessage {
	id: string;
	type: "user" | "bot";
	content: string;
}

export interface TestAgentComponentProps {
	selectedAgent: AgentDataDTO | null;
	editFormData: AgentDataDTO;
	onClose: () => void;
}

export enum FEEDBACK_TYPES {
	BUGREPORT,
	NEWFEATURE,
}

export interface MarkdownRendererProps {
	content: string;
}

export interface CreateAgentFlyoutProps {
	isOpen: boolean;
	onClose: () => void;
	selectedKnowledgeFiles: File[];
	onOpenKnowledgeBase: () => void;
	onClearKnowledgeFiles: () => void;
	selectedAiVisionImages: File[];
	onOpenAiVisionFlyout: () => void;
	onClearAiVisionImages: () => void;
}

// Union type for both new files and existing documents
export type KnowledgeBaseItem = File | null;

export interface FileUploadFlyoutProps {
	isOpen: boolean;
	onClose: () => void;
	onFilesChange: (files: File[]) => void;
	selectedFiles: File[];
	existingFiles?: File[] | any[];
	onExistingFilesChange?: (removedFileNames: string[]) => void;
	removedExistingFiles?: string[];
	config: {
		headerConstants: any;
		icons: any;
		supportedTypes: string;
	};
}

export interface MainLayoutProps {
	children: ReactNode;
	title?: string;
}
