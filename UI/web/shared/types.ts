import { ReactNode } from "react";
import { AgentDataDTO } from "@models/response/agent-data-dto";
import { ToolSkillDTO } from "@models/response/tool-skill-dto";
import { AgentsWorkspaceDTO } from "@models/response/agents-workspace-dto";
import { WorkspaceAgentsDataDTO } from "@models/response/workspace-agents-data.dto";

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
	onOpenAssociateSkills: () => void;
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

export interface WorkspacesListComponentProps {
	workspacesList: AgentsWorkspaceDTO[];
	handleWorkspaceClick: (workspaceId: string) => void;
	onClose: () => void;
	isDisabled: boolean;
	showCloseButton?: boolean;
	actionButton?: React.ReactNode;
	onEditWorkspace?: (workspace: AgentsWorkspaceDTO) => void;
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
	selectedSkillGuids: string[];
	onOpenAssociateSkills: () => void;
	onClearSkillGuids: () => void;
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
	agentGuid?: string;
	config: {
		headerConstants: any;
		icons: any;
		supportedTypes: string;
	};
}

export interface EditSkillFlyoutComponentProps {
	editFormData: ToolSkillDTO;
	selectedSkill: ToolSkillDTO | null;
	setEditFormData: React.Dispatch<React.SetStateAction<ToolSkillDTO>>;
	setSelectedSkill: React.Dispatch<React.SetStateAction<ToolSkillDTO | null>>;
	isEditDrawerOpen: boolean;
	onEditClose: () => void;
	isDisabled: boolean;
}

export interface EditWorkspaceFlyoutComponentProps {
	editFormData: AgentsWorkspaceDTO;
	selectedWorkspace: AgentsWorkspaceDTO | null;
	setEditFormData: React.Dispatch<React.SetStateAction<AgentsWorkspaceDTO>>;
	setSelectedWorkspace: React.Dispatch<
		React.SetStateAction<AgentsWorkspaceDTO | null>
	>;
	isEditDrawerOpen: boolean;
	onEditClose: () => void;
	isDisabled: boolean;
	onOpenAssociateAgents: (
		currentSelection: Set<string>,
		onComplete: (selected: Set<string>) => void,
	) => void;
}

export interface MainLayoutProps {
	children: ReactNode;
	title?: string;
}

export interface DeletePopupProps {
	isOpen: boolean;
	onClose: () => void;
	onDelete: () => void;
	title: string;
	description: string;
	isLoading?: boolean;
}

export interface PopupComponentProps {
	isOpen: boolean;
	onClose: () => void;
	onAction: () => void;
	title: string;
	description: string;
	isLoading?: boolean;
}

export interface AssociatedAgentsListPaneProps {
	workspaceDetailsData: AgentsWorkspaceDTO;
	selectedAgent: WorkspaceAgentsDataDTO | null;
	setSelectedAgent: (agent: WorkspaceAgentsDataDTO) => void;
	isGroupChatEnabled: boolean;
}

export interface AssociateSkillsFlyoutProps {
	isOpen: boolean;
	onClose: () => void;
	onSkillsChange: (skillGuids: string[]) => void;
	selectedSkillGuids: string[];
}

export interface FullScreenLoadingProps {
	isLoading: boolean;
	message?: string;
}

export interface LoadingSpinnerProps {
	size?: "sm" | "md" | "lg" | "xl";
	className?: string;
}
