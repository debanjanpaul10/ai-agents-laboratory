import axios from "axios";

import { ResponseDTO } from "@shared/types";
import { environment } from "@environments/environment.base";
import { tokenService } from "@helpers/token-service";

const BASE_API_URL: string = environment.apiBaseUrl;

const apiClient = axios.create({
	baseURL: BASE_API_URL,
});

apiClient.interceptors.request.use(
	(config) => {
		const token = tokenService.getToken();
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	},
	(error) => {
		return Promise.reject(error);
	},
);

apiClient.interceptors.response.use(
	(response) => {
		return response;
	},
	(error) => {
		if (error.response?.status === 401) {
			tokenService.clearToken();
			if (globalThis.window !== undefined) {
				globalThis.location.href = "/";
			}
		}
		return Promise.reject(error);
	},
);

export async function GetAsync(apiUrl: string): Promise<ResponseDTO> {
	try {
		const response = await apiClient.get(apiUrl);

		if (response?.data) return response?.data;
		else
			return {
				isSuccess: false,
				responseData: "",
			};
	} catch (error: any) {
		console.error(error);
		throw error(error.response ? error.response.data : error.message);
	}
}

export async function PostAsync(
	apiUrl: string,
	data: any,
): Promise<ResponseDTO> {
	try {
		const response = await apiClient.post(apiUrl, data);

		if (response?.data) return response.data;
		else
			return {
				isSuccess: false,
				responseData: "",
			};
	} catch (error: any) {
		console.error(error);
		throw error(error.response ? error.response.data : error.message);
	}
}

export async function DeleteAsync(apiUrl: string): Promise<ResponseDTO> {
	try {
		const response = await apiClient.delete(apiUrl);

		if (response?.data) return response?.data;
		else
			return {
				isSuccess: false,
				responseData: "",
			};
	} catch (error: any) {
		console.error(error);
		throw error(error.response ? error.response.data : error.message);
	}
}

export async function PutAsync(
	apiUrl: string,
	data: any,
): Promise<ResponseDTO> {
	try {
		const response = await apiClient.put(apiUrl, data);

		if (response?.data) return response?.data;
		else
			return {
				isSuccess: false,
				responseData: "",
			};
	} catch (error: any) {
		console.error(error);
		throw error(error.response ? error.response.data : error.message);
	}
}
