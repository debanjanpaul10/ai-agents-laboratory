import {
	GET_ALL_MCP_SERVER_TOOLS,
	GET_ALL_TOOLS_SKILLS,
	GET_TOOL_SKILL_BY_ID,
	TOGGLE_ADD_SKILL_DRAWER,
	TOGGLE_CREATE_SKILLS_LOADER,
	TOGGLE_EDIT_SKILLS_LOADER,
	TOGGLE_TOOLS_SKILLS_LOADER,
	TOGGLE_MCP_TOOLS_LOADER,
	TOGGLE_MCP_TOOLS_DRAWER,
} from "@store/tools-skills/actionTypes";

const initialState: any = {
	isToolSkillsLoading: false,
	isCreateSkillLoading: false,
	isEditSkillLoading: false,
	allToolSkills: [],
	toolSkills: {},
	isAddSkillDrawerOpen: false,
	mcpServerTools: [],
	isMcpToolsLoading: false,
	isMcpToolsDrawerOpen: false,
};

export function ToolSkillsReducer(state = initialState, action: any) {
	switch (action.type) {
		case TOGGLE_TOOLS_SKILLS_LOADER: {
			return {
				...state,
				isToolSkillsLoading: action.payload,
			};
		}
		case TOGGLE_EDIT_SKILLS_LOADER: {
			return {
				...state,
				isEditSkillLoading: action.payload,
			};
		}
		case TOGGLE_CREATE_SKILLS_LOADER: {
			return {
				...state,
				isCreateSkillLoading: action.payload,
			};
		}
		case TOGGLE_ADD_SKILL_DRAWER: {
			return {
				...state,
				isAddSkillDrawerOpen: action.payload,
			};
		}
		case TOGGLE_MCP_TOOLS_LOADER: {
			return {
				...state,
				isMcpToolsLoading: action.payload,
			};
		}
		case TOGGLE_MCP_TOOLS_DRAWER: {
			return {
				...state,
				isMcpToolsDrawerOpen: action.payload,
			};
		}
		case GET_ALL_TOOLS_SKILLS: {
			return {
				...state,
				allToolSkills: action.payload,
			};
		}
		case GET_TOOL_SKILL_BY_ID: {
			return {
				...state,
				toolSkills: action.payload,
			};
		}

		case GET_ALL_MCP_SERVER_TOOLS: {
			return {
				...state,
				mcpServerTools: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
