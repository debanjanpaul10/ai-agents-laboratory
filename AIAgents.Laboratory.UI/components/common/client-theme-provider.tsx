import { useEffect, useState } from "react";
import { ThemeProvider } from "@components/common/theme-provider";

export function ClientThemeProvider({
	children,
	...props
}: React.ComponentProps<typeof ThemeProvider>) {
	const [mounted, setMounted] = useState(false);

	useEffect(() => {
		setMounted(true);
	}, []);

	// Always render the ThemeProvider but with suppressHydrationWarning
	// This prevents the flash but still allows theme functionality
	return (
		<div suppressHydrationWarning>
			{mounted ? (
				<ThemeProvider {...props}>{children}</ThemeProvider>
			) : (
				<div className="dark">{children}</div>
			)}
		</div>
	);
}
