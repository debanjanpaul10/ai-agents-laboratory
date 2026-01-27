import { environment as devEnvironment } from "@environments/environment.development";
import { environment as prodEnvironment } from "@environments/environment.prod";

export const getEnvironment = () => {
	// Check if we're in development mode
	const isDevelopment = process.env.NODE_ENV === "development";
	return isDevelopment ? devEnvironment : prodEnvironment;
};

// Export a singleton instance of the environment
export const environment = getEnvironment();
