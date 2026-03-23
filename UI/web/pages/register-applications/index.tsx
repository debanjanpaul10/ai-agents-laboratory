import { useEffect, useState } from "react";
import { Button } from "@heroui/react";
import { Plus } from "lucide-react";

import { useAuth } from "@auth/AuthProvider";
import MainLayout from "@components/common/main-layout";
import { FullScreenLoading } from "@components/common/spinner";
import { RegisterApplicationsConstants } from "@helpers/constants";
import { GetAllConfigurations } from "@store/common/actions";
import { useAppDispatch, useAppSelector } from "@store/index";
import {
    GetAllRegisteredApplicationsAsync,
    ToggleRegisterNewApplicationDrawer,
} from "@store/tools-skills/actions";
import ApplicationsListComponent from "@components/register-applications/applications-list";
import RegisterNewApplicationFlyoutComponent from "@components/register-applications/new-application";
import EditRegisteredApplicationComponent from "@components/register-applications/edit-application";
import { RegisteredApplicationDTO } from "@models/request/registered-application.dto";

export default function RegisterApplicationsComponent() {
    const dispatch = useAppDispatch();
    const authContext = useAuth();

    const [selectedApplication, setSelectedApplication] =
        useState<RegisteredApplicationDTO | null>(null);
    const [editFormData, setEditFormData] = useState<RegisteredApplicationDTO>({
        id: 0,
        applicationName: "",
        applicationRegistrationGuid: "",
        description: "",
        isAzureRegistered: false,
    });
    const [isEditDrawerOpen, setIsEditDrawerOpen] = useState<boolean>(false);

    const IsRegisteredApplicationsLoading = useAppSelector(
        (state) => state.ToolSkillsReducer.isRegisteredApplicationsLoading,
    );
    const IsNewApplicationDrawerOpen = useAppSelector(
        (state) => state.ToolSkillsReducer.isRegisterNewApplicationDrawerOpen,
    );
    const ConfigurationsStoreData = useAppSelector(
        (state) => state.CommonReducer.configurations,
    );
    const RegisteredApplicationsListStoreData = useAppSelector(
        (state) => state.ToolSkillsReducer.registeredApplications,
    );

    const isAnyDrawerOpen = isEditDrawerOpen || IsNewApplicationDrawerOpen;

    useEffect(() => {
        if (authContext.isAuthenticated && !authContext.isLoading) {
            dispatch(GetAllRegisteredApplicationsAsync());
            if (
                !ConfigurationsStoreData ||
                Object.keys(ConfigurationsStoreData).length === 0
            )
                dispatch(GetAllConfigurations());
        }
    }, [authContext.isAuthenticated, authContext.isLoading]);

    const handleEditApplication = (application: RegisteredApplicationDTO) => {
        setSelectedApplication(application);
        setEditFormData(application);
        setIsEditDrawerOpen(true);
    };

    const handleRegisterNewApplicationClick = () => {
        dispatch(ToggleRegisterNewApplicationDrawer(true));
    };

    const renderRegisterApplications = () =>
        IsRegisteredApplicationsLoading ? (
            <FullScreenLoading
                isLoading={IsRegisteredApplicationsLoading}
                message={
                    RegisterApplicationsConstants.LoadingConstants.MainLoader
                }
            />
        ) : (
            <MainLayout contentClassName="p-0" isFullWidth={true}>
                <div className="w-full h-screen overflow-hidden bg-gradient-to-br from-gray-900 via-slate-900 to-black">
                    <ApplicationsListComponent
                        applicationsList={RegisteredApplicationsListStoreData}
                        onEditApplication={handleEditApplication}
                        onClose={() => {}}
                        isDisabled={isAnyDrawerOpen}
                        showCloseButton={false}
                        actionButton={
                            <Button
                                onPress={handleRegisterNewApplicationClick}
                                className="bg-white/5 border border-white/10 hover:border-blue-500/50 hover:bg-blue-500/10 text-white font-medium px-6 rounded-xl transition-all duration-300 group shadow-lg"
                            >
                                <Plus className="w-4 h-4 mr-2 text-blue-400 group-hover:text-blue-300 group-hover:scale-110 transition-all" />
                                <span>
                                    {
                                        RegisterApplicationsConstants.Headers
                                            .NewApplicationRegisterButton
                                    }
                                </span>
                            </Button>
                        }
                    />
                </div>

                {/* Edit Drawer */}
                {isEditDrawerOpen && (
                    <>
                        <div className="fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-all duration-300" />
                        <div className="fixed right-0 top-0 md:w-1/2 h-screen z-50 transition-all duration-500 ease-in-out">
                            <EditRegisteredApplicationComponent
                                editFormData={editFormData}
                                selectedApplication={selectedApplication}
                                setEditFormData={setEditFormData}
                                setSelectedApplication={setSelectedApplication}
                                isEditDrawerOpen={isEditDrawerOpen}
                                onEditClose={() => setIsEditDrawerOpen(false)}
                                isDisabled={false}
                            />
                        </div>
                    </>
                )}

                {/* New Application Drawer */}
                <RegisterNewApplicationFlyoutComponent />
            </MainLayout>
        );

    return authContext.isAuthenticated ? (
        renderRegisterApplications()
    ) : (
        <FullScreenLoading
            isLoading={true}
            message={
                RegisterApplicationsConstants.LoadingConstants
                    .LoginRedirectLoader
            }
        />
    );
}
