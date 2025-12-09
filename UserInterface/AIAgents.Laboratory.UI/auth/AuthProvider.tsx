import React, { createContext, useContext, useEffect, useState } from "react";
import { useMsal } from "@azure/msal-react";
import { InteractionStatus } from "@azure/msal-browser";

import { AuthContextType, AuthProviderProps, User } from "../shared/types";

const defaultAuthContext: AuthContextType = {
	user: null,
	isAuthenticated: false,
	isLoading: true,
	login: async () => {
		throw new Error("AuthProvider not initialized");
	},
	logout: async () => {
		throw new Error("AuthProvider not initialized");
	},
	getAccessToken: async () => {
		throw new Error("AuthProvider not initialized");
	},
};

const AuthContext = createContext<AuthContextType>(defaultAuthContext);

export function AuthProvider({ children }: AuthProviderProps) {
	const [user, setUser] = useState<User | null>(null);
	const [isLoading, setIsLoading] = useState(true);
	const { instance, inProgress, accounts } = useMsal();

	const isAuthenticated = accounts.length > 0;

	useEffect(() => {
		if (inProgress === InteractionStatus.None) {
			if (accounts.length > 0) {
				const account = accounts[0];
				setUser({
					id: account.localAccountId,
					name: account.name || "",
					email: account.username,
				});
			}
			setIsLoading(false);
		}
	}, [accounts, inProgress]);

	const login = async (): Promise<void> => {
		try {
			await instance.loginPopup({
				scopes: ["openid", "profile", "email"],
			});
		} catch (error) {
			console.error("Login failed:", error);
		}
	};

	const logout = async (): Promise<void> => {
		try {
			await instance.logoutPopup();
			setUser(null);
		} catch (error) {
			console.error(error);
			throw error;
		}
	};

	const getAccessToken = async (): Promise<string | null> => {
		if (accounts.length === 0) return null;

		try {
			const response = await instance.acquireTokenSilent({
				scopes: ["openid", "profile", "email"],
				account: accounts[0],
			});
			return response.accessToken;
		} catch (error) {
			console.error(error);
			throw error;
		}
	};

	const value: AuthContextType = {
		user,
		isAuthenticated,
		isLoading,
		login,
		logout,
		getAccessToken,
	};

	return React.createElement(AuthContext.Provider, { value }, children);
}

export function useAuth(): AuthContextType {
	const context = useContext(AuthContext);
	return context;
}
