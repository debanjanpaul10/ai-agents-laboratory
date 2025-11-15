import { Inter } from "next/font/google";
import { MsalProvider } from "@azure/msal-react";
import { PublicClientApplication } from "@azure/msal-browser";
import { Provider } from "react-redux";
import type { AppProps } from "next/app";
import Head from "next/head";
import { ToastProvider } from "@heroui/toast";

import "../styles/globals.css";
import { msalConfig } from "@auth/authConfig";
import { ClientThemeProvider } from "@components/common/providers/client-theme-provider";
import { AuthProvider } from "@auth/AuthProvider";
import { metadata } from "@helpers/constants";
import { store } from "@store/index";

const inter = Inter({ subsets: ["latin"] });

// Create MSAL instance
const msalInstance = new PublicClientApplication(msalConfig);

// ToasterBridge was removed. Use the addToast API from @heroui/toast in non-React code.

export default function App({ Component, pageProps }: AppProps) {
	return (
		<>
			<Head>
				<title>{metadata.title}</title>
				<link rel="icon" href="/images/icon.png" />
			</Head>
			<div className={inter.className}>
				<ClientThemeProvider
					attribute="class"
					defaultTheme="dark"
					enableSystem={false}
					disableTransitionOnChange
				>
					<MsalProvider instance={msalInstance}>
						<Provider store={store}>
							<AuthProvider>
								<ToastProvider />
								<Component {...pageProps} />
							</AuthProvider>
						</Provider>
					</MsalProvider>
				</ClientThemeProvider>
			</div>
		</>
	);
}
