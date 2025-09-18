import { Configuration, PopupRequest } from "@azure/msal-browser";
import { environment } from "../../environments/environment";

// MSAL configuration
export const msalConfig: Configuration = {
	auth: {
		clientId: environment.msalConfig.auth.clientId,
		authority: environment.msalConfig.auth.authority,
	},
	cache: {
		cacheLocation: "sessionStorage",
		storeAuthStateInCookie: false,
	},
};

// Add scopes here for ID token to be used at Microsoft identity platform endpoints.
export const loginRequest: PopupRequest = {
	scopes: environment.apiConfig.scopes,
};

// Add the endpoints here for Microsoft Graph API services you'd like to use.
export const graphConfig = {
	graphMeEndpoint: environment.apiConfig.uri,
};
