import { useCallback } from "react";
import { useAuth } from "../../app/auth/AuthProvider";
import { GetAsync, PostAsync } from "@/helpers/http-utility";
import { ResponseDTO } from "../../app/lib/types";

export function useAuthenticatedApi() {
	const { getAccessToken } = useAuth();

	const authenticatedGet = useCallback(
		async (apiUrl: string): Promise<ResponseDTO> => {
			const token = await getAccessToken();
			if (!token) {
				throw new Error("No access token available");
			}
			return GetAsync(apiUrl, token);
		},
		[getAccessToken]
	);

	const authenticatedPost = useCallback(
		async (apiUrl: string, data: any): Promise<ResponseDTO> => {
			const token = await getAccessToken();
			if (!token) {
				throw new Error("No access token available");
			}
			return PostAsync(apiUrl, data, token);
		},
		[getAccessToken]
	);

	return {
		authenticatedGet,
		authenticatedPost,
	};
}
