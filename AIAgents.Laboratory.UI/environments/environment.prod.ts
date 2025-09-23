export const environment = {
	production: true,
	apiBaseUrl:
		"https://app-webapi-ai-agents-lab.azurewebsites.net/aiagentsapi/",
	msalConfig: {
		auth: {
			clientId: "7df8eeab-dbed-4467-8675-05249f74170b",
			authority:
				"https://login.microsoftonline.com/499b9f66-f4dd-4c09-ab36-163bbc38a326",
		},
		scopes: ["AI.Read"],
	},
	apiConfig: {
		scopes: ["api://7791b1e0-dac1-4fe2-b957-038504708e8c/AI.Read"],
		uri: "https://graph.microsoft.com/v1.0/me",
		apiScope: ["api://7791b1e0-dac1-4fe2-b957-038504708e8c/AI.Read"],
	},
};
