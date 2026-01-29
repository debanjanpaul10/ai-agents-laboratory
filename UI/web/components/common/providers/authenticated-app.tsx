import { useIsAuthenticated } from "@azure/msal-react";

import LoginPage from "@pages/login";
import { AuthenticatedAppProps } from "@shared/types";
import { tokenService } from "@helpers/token-service";

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
