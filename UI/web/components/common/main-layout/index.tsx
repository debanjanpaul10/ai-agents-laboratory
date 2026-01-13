import Head from "next/head";

import SidebarComponent from "@components/common/sidebar";
import { metadata } from "@helpers/constants";
import { MainLayoutProps } from "@shared/types";

export default function MainLayout({
	children,
	title,
	contentClassName,
	isFullWidth = false,
}: MainLayoutProps & { contentClassName?: string; isFullWidth?: boolean }) {
	const pageTitle = title ? `${title} | ${metadata.title}` : metadata.title;

	return (
		<div className="min-h-screen bg-gradient-to-br from-gray-900 via-slate-900 to-black text-white">
			<Head>
				<title>{pageTitle}</title>
			</Head>
			<SidebarComponent />
			<div className={`ml-85 ${contentClassName || "p-8"}`}>
				<main
					className={`${
						!isFullWidth ? "max-w-7xl mx-auto" : "w-full"
					} h-full`}
				>
					{children}
				</main>
			</div>
		</div>
	);
}
