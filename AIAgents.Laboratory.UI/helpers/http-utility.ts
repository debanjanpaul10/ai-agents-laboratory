import { ResponseDTO } from "@lib/types";
import { environment } from "@environments/environment";
import axios from "axios";

const BASE_API_URL: string = environment.apiBaseUrl;

export async function GetAsync(
	apiUrl: string,
	accessToken: string
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
			error.response ? error.response.data : error.message
		);
	}
}

export async function PostAsync(
	apiUrl: string,
	data: any,
	accessToken: string
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
			error.response ? error.response.data : error.message
		);
	}
}
