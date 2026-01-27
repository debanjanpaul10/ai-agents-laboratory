import { useEffect, useState } from "react";
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
import { FullScreenLoading } from "@components/common/spinner";

const inter = Inter({ subsets: ["latin"] });

// Create MSAL instance
const msalInstance = new PublicClientApplication(msalConfig);

export default function App({ Component, pageProps }: AppProps) {
	const [isMsalInitialized, setIsMsalInitialized] = useState(false);

	useEffect(() => {
		msalInstance.initialize().then(() => {
			setIsMsalInitialized(true);
		});
	}, []);

	if (!isMsalInitialized) {
		return (
			<FullScreenLoading
				isLoading={true}
				message="Initializing application..."
			/>
		);
	}

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
								<ToastProvider
									placement="top-center"
									toastProps={{
										classNames: { base: "z-[100]" },
									}}
								/>
								<Component {...pageProps} />
							</AuthProvider>
						</Provider>
					</MsalProvider>
				</ClientThemeProvider>
			</div>
		</>
	);
}
