import { useEffect, useState } from "react";
import { Plus } from "lucide-react";
import { Button } from "@heroui/react";

import { useAuth } from "@auth/AuthProvider";
import { ShowErrorToaster } from "@shared/toaster";
import { useAppDispatch, useAppSelector } from "@store/index";
import {
	GetAllToolSkillsAsync,
	ToggleAddSkillDrawer,
} from "@store/tools-skills/actions";
import { GetAllConfigurations } from "@store/common/actions";
import { FullScreenLoading } from "@components/common/spinner";
import { MarketplaceConstants } from "@helpers/constants";
import MainLayout from "@components/common/main-layout";
import SkillsListComponent from "@components/marketplace/skills-list";
import CreateSkillComponent from "@components/marketplace/create-skill";
import EditSkillFlyoutComponent from "@components/marketplace/edit-skill";
import McpToolsListFlyoutComponent from "@components/marketplace/mcp-tools-list";
import { ToolSkillDTO } from "@models/tool-skill-dto";
import { ToggleMcpToolsDrawer } from "@store/tools-skills/actions";

export default function MarketplaceComponent() {
	const dispatch = useAppDispatch();
	const authContext = useAuth();

	const [selectedSkill, setSelectedSkill] = useState<ToolSkillDTO | null>(
		null
	);
	const [editFormData, setEditFormData] = useState<ToolSkillDTO>({
		associatedAgents: {},
		createdBy: "",
		dateCreated: new Date(),
		dateModified: new Date(),
		modifiedBy: "",
		toolSkillDisplayName: "",
		toolSkillGuid: "",
		toolSkillMcpServerUrl: "",
		toolSkillTechnicalName: "",
	});
	const [isEditDrawerOpen, setIsEditDrawerOpen] = useState<boolean>(false);
	const [isMcpSkillsDrawerOpen, setIsMcpSkillsDrawerOpen] =
		useState<boolean>(false);

	const IsSkillsMarketPlaceLoading = useAppSelector(
		(state) => state.ToolSkillsReducer.isToolSkillsLoading
	);

	const ConfigurationsStoreData = useAppSelector(
		(state) => state.CommonReducer.configurations
	);

	const ToolSkillsListStoreData = useAppSelector<ToolSkillDTO[]>(
		(state) => state.ToolSkillsReducer.allToolSkills
	);

	const IsAddSkillDrawerOpenStoreData = useAppSelector(
		(state) => state.ToolSkillsReducer.isAddSkillDrawerOpen
	);

	const IsMcpToolsFlyoutOpen = useAppSelector(
		(state) => state.ToolSkillsReducer.isMcpToolsDrawerOpen
	);

	useEffect(() => {
		if (authContext.isAuthenticated && !authContext.isLoading) {
			GetAllToolSkillsData();
			if (
				!ConfigurationsStoreData ||
				Object.keys(ConfigurationsStoreData).length === 0
			) {
				GetAllConfigurationsData();
			}
		}
	}, [authContext.isAuthenticated, authContext.isLoading]);

	const handleSkillClick = (toolSkill: ToolSkillDTO) => {
		setSelectedSkill(toolSkill);
		setEditFormData({
			associatedAgents: toolSkill.associatedAgents || {},
			createdBy: toolSkill.createdBy || "",
			dateCreated: toolSkill.dateCreated || new Date(),
			dateModified: toolSkill.dateModified || new Date(),
			modifiedBy: toolSkill.modifiedBy || "",
			toolSkillDisplayName: toolSkill.toolSkillDisplayName || "",
			toolSkillGuid: toolSkill.toolSkillGuid || "",
			toolSkillMcpServerUrl: toolSkill.toolSkillMcpServerUrl || "",
			toolSkillTechnicalName: toolSkill.toolSkillTechnicalName || "",
		});

		setIsEditDrawerOpen(true);
	};

	async function fetchToken() {
		try {
			if (authContext.isAuthenticated && !authContext.isLoading)
				return await authContext.getAccessToken();
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		}
	}

	async function GetAllToolSkillsData() {
		const token = await fetchToken();
		token && dispatch(GetAllToolSkillsAsync(token));
	}

	async function GetAllConfigurationsData() {
		const token = await fetchToken();
		token && dispatch(GetAllConfigurations(token));
	}

	const handleUnAuthorizedUser = () => {
		return (
			<FullScreenLoading
				isLoading={true}
				message={
					MarketplaceConstants.LoadingConstants.LoginRedirectLoader
				}
			/>
		);
	};

	const handleCreateNewSkill = () => {
		dispatch(ToggleAddSkillDrawer(true));
	};

	const renderAuthorizedMarketplace = () => {
		return (
			<MainLayout contentClassName="p-0" isFullWidth={true}>
				<div className="w-full h-screen overflow-hidden bg-gradient-to-br from-gray-900 via-slate-900 to-black">
					<SkillsListComponent
						toolSkillsList={ToolSkillsListStoreData}
						handleSkillClick={handleSkillClick}
						onClose={() => {}}
						isDisabled={isAnyDrawerOpen}
						showCloseButton={false}
						actionButton={
							<Button
								onPress={handleCreateNewSkill}
								className="bg-white/5 border border-white/10 hover:border-blue-500/50 hover:bg-blue-500/10 text-white font-medium px-6 rounded-xl transition-all duration-300 group shadow-lg"
							>
								<Plus className="w-4 h-4 mr-2 text-blue-400 group-hover:text-blue-300 group-hover:scale-110 transition-all" />
								<span>Add New Skill</span>
							</Button>
						}
					/>
					<CreateSkillComponent />
					<McpToolsListFlyoutComponent
						isOpen={IsMcpToolsFlyoutOpen}
						onClose={() => dispatch(ToggleMcpToolsDrawer(false))}
					/>
				</div>

				{/* Backdrop Overlay */}
				{isAnyDrawerOpen && (
					<div
						className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-all duration-300"
						onClick={() => {}}
					/>
				)}

				{/* Edit Skill Drawer */}
				{isEditDrawerOpen && (
					<div className="fixed top-0 right-0 h-screen z-50 transition-all duration-500 ease-in-out md:w-1/3 w-full">
						<div className="absolute inset-0 bg-gradient-to-r from-emerald-600/20 via-teal-600/20 to-cyan-600/20 blur-sm opacity-50 -z-10"></div>
						<div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-l border-white/10 shadow-2xl">
							<EditSkillFlyoutComponent
								editFormData={editFormData}
								selectedSkill={selectedSkill}
								setEditFormData={setEditFormData}
								setSelectedSkill={setSelectedSkill}
								isEditDrawerOpen={isEditDrawerOpen}
								onEditClose={() => setIsEditDrawerOpen(false)}
								isDisabled={false}
							/>
						</div>
					</div>
				)}
			</MainLayout>
		);
	};

	const isAnyDrawerOpen =
		isEditDrawerOpen ||
		isMcpSkillsDrawerOpen ||
		IsAddSkillDrawerOpenStoreData;

	return !authContext.isAuthenticated ? (
		handleUnAuthorizedUser()
	) : IsSkillsMarketPlaceLoading ? (
		<FullScreenLoading
			isLoading={IsSkillsMarketPlaceLoading}
			message={MarketplaceConstants.LoadingConstants.MainLoader}
		/>
	) : (
		renderAuthorizedMarketplace()
	);
}
