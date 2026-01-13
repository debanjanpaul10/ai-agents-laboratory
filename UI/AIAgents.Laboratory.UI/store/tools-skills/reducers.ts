import { ReduxStoreType } from "@shared/types";
import {
	GET_ALL_TOOLS_SKILLS,
	GET_TOOL_SKILL_BY_ID,
	TOGGLE_TOOLS_SKILLS_LOADER,
} from "@store/tools-skills/actionTypes";

const initialState: any = {
	isToolSkillsLoading: false,
	allToolSkills: [],
	toolSkills: {},
};

export function ToolSkillsReducer(
	state = initialState,
	action: ReduxStoreType
) {
	switch (action.type) {
		case TOGGLE_TOOLS_SKILLS_LOADER: {
			return {
				...state,
				isToolSkillsLoading: action.payload,
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

		default: {
			return {
				...state,
			};
		}
	}
}
