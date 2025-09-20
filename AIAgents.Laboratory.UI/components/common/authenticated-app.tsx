"use client";

import { useIsAuthenticated } from "@azure/msal-react";

import LoginPage from "@pages/login";
import { AuthenticatedAppProps } from "@lib/types";

export default function AuthenticatedApp({ children }: AuthenticatedAppProps) {
	const isAuthenticated = useIsAuthenticated();

	if (!isAuthenticated) {
		return <LoginPage />;
	}

	return <>{children}</>;
}
