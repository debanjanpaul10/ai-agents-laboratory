"use client";

import { Inter } from "next/font/google";
import { MsalProvider } from "@azure/msal-react";
import { PublicClientApplication } from "@azure/msal-browser";
import { Provider } from "react-redux";

import "./styles/globals.css";
import { msalConfig } from "./auth/authConfig";
import { ClientThemeProvider } from "./components/common/client-theme-provider";
import { AuthProvider } from "./auth/AuthProvider";
import { metadata } from "./helpers/constants";
import { store } from "./store";

const inter = Inter({ subsets: ["latin"] });

// Create MSAL instance
const msalInstance = new PublicClientApplication(msalConfig);

export default function RootLayout({
	children,
}: {
	children: React.ReactNode;
}) {
	return (
		<html lang="en" suppressHydrationWarning>
			<head>
				<title>{metadata.title}</title>
				<link rel="icon" href="/images/icon.png" />
			</head>
			<body className={inter.className} suppressHydrationWarning>
				<ClientThemeProvider
					attribute="class"
					defaultTheme="dark"
					enableSystem={false}
					disableTransitionOnChange
				>
					<MsalProvider instance={msalInstance}>
						<Provider store={store}>
							<AuthProvider>{children}</AuthProvider>
						</Provider>
					</MsalProvider>
				</ClientThemeProvider>
			</body>
		</html>
	);
}
