import type { AppProps } from "next/app";
import { PublicClientApplication } from "@azure/msal-browser";
import { MsalProvider } from "@azure/msal-react";
import { Provider } from "react-redux";

import { environment } from "../../environments/environment";
import { AuthProvider } from "../../app/auth/AuthProvider";
import { store } from "@/store";
import "@/styles/globals.css";

const msalInstance = new PublicClientApplication(environment.msalConfig);

export default function App({ Component, pageProps }: AppProps) {
	return (
		<MsalProvider instance={msalInstance}>
			<Provider store={store}>
				<AuthProvider>
					<Component {...pageProps} />
				</AuthProvider>
			</Provider>
		</MsalProvider>
	);
}
