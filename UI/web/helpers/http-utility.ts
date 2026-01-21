import axios from "axios";

import { ResponseDTO } from "@shared/types";
import { environment } from "@environments/environment.base";

const BASE_API_URL: string = environment.apiBaseUrl;

export async function GetAsync(
	apiUrl: string,
	accessToken: string,
): Promise<ResponseDTO> {
	try {
		const url = BASE_API_URL + apiUrl;
		const response = await axios.get(url, {
			headers: {
				Authorization: `Bearer ${accessToken}`,
			},
		});

		if (response?.data) return response?.data;
		else
			return {
				isSuccess: false,
				responseData: "",
			};
	} catch (error: any) {
		console.error(error);
		return Promise.reject(
			error.response ? error.response.data : error.message,
		);
	}
}

export async function PostAsync(
	apiUrl: string,
	data: any,
	accessToken: string,
): Promise<ResponseDTO> {
	try {
		const url: string = BASE_API_URL + apiUrl;
		const response = await axios.post(url, data, {
			headers: {
				Authorization: `Bearer ${accessToken}`,
			},
		});

		if (response?.data) return response.data;
		else
			return {
				isSuccess: false,
				responseData: "",
			};
	} catch (error: any) {
		console.error(error);
		return Promise.reject(
			error.response ? error.response.data : error.message,
		);
	}
}

export async function DeleteAsync(
	apiUrl: string,
	accessToken: string,
): Promise<ResponseDTO> {
	try {
		const url: string = BASE_API_URL + apiUrl;
		const response = await axios.delete(url, {
			headers: {
				Authorization: `Bearer ${accessToken}`,
			},
		});

		if (response?.data) return response?.data;
		else
			return {
				isSuccess: false,
				responseData: "",
			};
	} catch (error: any) {
		console.error(error);
		return Promise.reject(
			error.response ? error.response.data : error.message,
		);
	}
}

export async function PutAsync(
	apiUrl: string,
	data: any,
	accessToken: string,
): Promise<ResponseDTO> {
	try {
		const url: string = BASE_API_URL + apiUrl;
		const response = await axios.put(url, data, {
			headers: {
				Authorization: `Bearer ${accessToken}`,
			},
		});

		if (response?.data) return response?.data;
		else
			return {
				isSuccess: false,
				responseData: "",
			};
	} catch (error: any) {
		console.error(error);
		return Promise.reject(
			error.response ? error.response.data : error.message,
		);
	}
}
