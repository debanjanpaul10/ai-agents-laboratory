import { ApplicationConstants } from "@helpers/constants";

export const tokenService = {
	getToken: (): string | null => {
		if (typeof window !== "undefined") {
			return localStorage.getItem(
				ApplicationConstants.StorageKeys.AccessToken,
			);
		}
		return null;
	},

	setToken: (token: string): void => {
		if (typeof window !== "undefined") {
			localStorage.setItem(
				ApplicationConstants.StorageKeys.AccessToken,
				token,
			);
		}
	},

	clearToken: (): void => {
		if (typeof window !== "undefined") {
			localStorage.removeItem(
				ApplicationConstants.StorageKeys.AccessToken,
			);
		}
	},
};
