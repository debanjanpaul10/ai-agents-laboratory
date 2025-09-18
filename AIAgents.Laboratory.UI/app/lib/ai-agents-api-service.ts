import { GetAsync } from "@/helpers/http-utility";

export const GetAgentsApiAsync = async (accessToken: string) =>
	await GetAsync("agents/getallagents", accessToken);
