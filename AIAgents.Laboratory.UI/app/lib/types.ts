import { ReactNode } from "react";

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

export interface AgentDataDTO {
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

export interface AuthenticatedAppProps {
    children: React.ReactNode;
}
