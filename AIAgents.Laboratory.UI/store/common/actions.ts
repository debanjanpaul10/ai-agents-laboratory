import {
	TOGGLE_AGENT_TEST_DRAWER,
	TOGGLE_AGENTS_LIST_DRAWER,
	TOGGLE_EDIT_AGENT_DRAWER,
	TOGGLE_MAIN_SPINNER,
	TOGGLE_NEW_AGENT_DRAWER,
} from "./actionTypes";

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

export function ToggleAgentsListDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_AGENTS_LIST_DRAWER,
		payload: isOpen,
	};
}

export function ToggleEditAgentDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_EDIT_AGENT_DRAWER,
		payload: isOpen,
	};
}

export function ToggleAgentTestDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_AGENT_TEST_DRAWER,
		payload: isOpen,
	};
}
