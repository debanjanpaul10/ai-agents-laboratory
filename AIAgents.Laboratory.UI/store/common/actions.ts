import { TOGGLE_MAIN_SPINNER, TOGGLE_NEW_AGENT_DRAWER } from "./actionTypes";

export function ToggleNewAgentDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_NEW_AGENT_DRAWER,
		payload: isOpen,
	};
}

export function ToggleMainLoader(isLoading: boolean) {
	return {
		type: TOGGLE_MAIN_SPINNER,
		payload: isLoading,
	};
}
