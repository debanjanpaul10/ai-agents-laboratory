import { ApplicationConstants } from "@helpers/constants";

export const tokenService = {
	getToken: (): string | null => {
		if (globalThis.window !== undefined) {
			return localStorage.getItem(
				ApplicationConstants.StorageKeys.AccessToken,
			);
		}
		return null;
	},

	setToken: (token: string): void => {
		if (globalThis.window !== undefined) {
			localStorage.setItem(
				ApplicationConstants.StorageKeys.AccessToken,
				token,
			);
		}
	},

	clearToken: (): void => {
		if (globalThis.window !== undefined) {
			localStorage.removeItem(
				ApplicationConstants.StorageKeys.AccessToken,
			);
		}
	},
};
