import React, { createContext, useContext, useEffect, useState } from "react";
import { useMsal } from "@azure/msal-react";
import { InteractionStatus } from "@azure/msal-browser";
import { useRouter } from "next/router";

import { FullScreenLoading } from "@components/common/spinner";
import { AuthContextType, AuthProviderProps, User } from "@shared/types";
import { DashboardConstants, RouteConstants } from "@helpers/constants";
import { tokenService } from "@helpers/token-service";

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
	const router = useRouter();

	const isAuthenticated = accounts.length > 0 && !!tokenService.getToken();

	useEffect(() => {
		const initAuth = async () => {
			if (inProgress === InteractionStatus.None) {
				if (accounts.length > 0) {
					const account = accounts[0];
					setUser({
						id: account.localAccountId,
						name: account.name || "",
						email: account.username,
					});

					// Sync token before ending loading state
					try {
						const token = await getAccessToken();
						if (token) {
							tokenService.setToken(token);
						} else {
							tokenService.clearToken();
						}
					} catch (error) {
						console.error("Failed to sync token during init:", error);
						tokenService.clearToken();
					}
				} else {
					tokenService.clearToken();
					setUser(null);
				}
				setIsLoading(false);
			}
		};
		initAuth();
	}, [accounts, inProgress]);

	useEffect(() => {
		if (
			!isLoading &&
			!isAuthenticated &&
			router.pathname !== RouteConstants.Home
		) {
			router.push(RouteConstants.Home);
		}
	}, [isLoading, isAuthenticated, router.pathname]);

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
			tokenService.clearToken();
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

	if (isLoading || (!isAuthenticated && router.pathname !== "/")) {
		return (
			<FullScreenLoading
				isLoading={true}
				message={
					DashboardConstants.LoadingConstants.CheckingAuthentication
				}
			/>
		);
	}

	return React.createElement(AuthContext.Provider, { value }, children);
}

export function useAuth(): AuthContextType {
	const context = useContext(AuthContext);
	return context;
}
