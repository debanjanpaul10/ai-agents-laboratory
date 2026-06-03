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
import { DashboardConstants, metadata } from "@helpers/constants";
import { store } from "@store/index";
import { FullScreenLoading } from "@components/common/spinner";

const inter = Inter({ subsets: ["latin"] });

// Create MSAL instance
const msalInstance = new PublicClientApplication(msalConfig);

/**
 * The main application component that wraps all pages. It initializes the MSAL instance for authentication, sets up the theme provider, and includes the global state provider. 
 * It also handles the loading state while MSAL is being initialized.
 * @component `App`
 * @param param0 - The component and page props passed by Next.js to render the appropriate page.
 * @param param0.Component - The active page component that should be rendered.
 * @param param0.pageProps - The initial props that were preloaded for the page by one of Next.js's data fetching methods.
 * @returns A React component that serves as the root of the application, providing authentication, theming, and global state management to all pages.
 */
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
                message={
                    DashboardConstants.LoadingConstants
                        .InitializingApplication
                }
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
