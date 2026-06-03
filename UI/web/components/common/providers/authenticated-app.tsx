import { useIsAuthenticated } from "@azure/msal-react";

import LoginPage from "@pages/login";
import { AuthenticatedAppProps } from "@shared/types";
import { tokenService } from "@helpers/token-service";

/**
 * This component checks if the user is authenticated using the useIsAuthenticated hook from the MSAL library.
 * If the user is not authenticated or if there is no token, it renders the LoginPage component.
 * If the user is authenticated and there is a token, it renders the children components passed to it.
 * @component `AuthenticatedApp`
 * @param param0 - An object containing the children components to be rendered if the user is authenticated.
 * @param param0.children - The child components that will be rendered if the user is authenticated.
 * @returns A React component that conditionally renders either the LoginPage or the authenticated children components based on the user's authentication status.
 */
export default function AuthenticatedApp({
	children,
}: Readonly<AuthenticatedAppProps>) {
	const isAuthenticated = useIsAuthenticated();
	const token = tokenService.getToken();

	if (!isAuthenticated || !token) {
		return <LoginPage />;
	}

	return <>{children}</>;
}
