import React, { useEffect } from "react";
import { Button, Input } from "@heroui/react";
import {
    Save,
    Play,
    Settings,
    User,
    Sparkles,
    Expand,
    ArrowRight,
} from "lucide-react";

import { ModifyAgentComponentProps } from "@/lib/types";
import { useAppDispatch, useAppSelector } from "@/store";
import { useAuth } from "@/auth/AuthProvider";
import { UpdateExistingAgentDataAsync } from "@/store/agents/actions";
import { FullScreenLoading } from "@/components/ui/loading-spinner";

export default function ModifyAgentComponent({
    editFormData,
    handleExpandPrompt,
    selectedAgent,
    setEditFormData,
    setSelectedAgent,
    isEditDrawerOpen,
    onTestAgent,
    onEditClose,
    isDisabled,
}: ModifyAgentComponentProps) {
    const dispatch = useAppDispatch();
    const auth = useAuth();

    const IsEditAgentDataLoading = useAppSelector(
        (state) => state.AgentsReducer.isEditAgentDataLoading
    );

    const handleInputChange = (field: string, value: string) => {
        setEditFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleEditSave = async () => {
        const accessToken = await auth.getAccessToken();
        accessToken &&
            dispatch(UpdateExistingAgentDataAsync(editFormData, accessToken));
    };

    const handleEditClose = () => {
        setSelectedAgent(null);
        onEditClose();
    };

    const renderEditAgentsData = () => {
        return (
            isEditDrawerOpen &&
            selectedAgent && (
                <div
                    className={`h-full flex flex-col ${
                        isDisabled ? "opacity-50 pointer-events-none" : ""
                    }`}
                >
                    {/* Header */}
                    <div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
                        <div className="flex items-center space-x-3">
                            <div className="bg-gradient-to-r from-green-500 to-blue-600 p-2 rounded-xl">
                                <Settings className="w-5 h-5 text-white" />
                            </div>
                            <div>
                                <h2 className="text-xl font-bold bg-gradient-to-r from-white via-green-100 to-blue-100 bg-clip-text text-transparent">
                                    Modify Agent Configuration
                                </h2>
                                <p className="text-white/60 text-sm">
                                    Edit {editFormData.agentName || "agent"}{" "}
                                    settings
                                </p>
                            </div>
                        </div>
                        <button
                            onClick={handleEditClose}
                            className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400"
                            disabled={isDisabled}
                        >
                            <ArrowRight className="w-4 h-4" />
                        </button>
                    </div>

                    {/* Form Content */}
                    <div className="flex-1 overflow-y-auto p-6 space-y-6 min-h-0">
                        {/* Agent Name Field */}
                        <div className="space-y-2">
                            <label className="text-white/80 text-sm font-medium flex items-center space-x-2">
                                <Sparkles className="w-4 h-4 text-green-400" />
                                <span>Agent Name</span>
                            </label>
                            <Input
                                value={editFormData.agentName}
                                onChange={(e) =>
                                    handleInputChange(
                                        "agentName",
                                        e.target.value
                                    )
                                }
                                placeholder="Enter agent name..."
                                disabled={isDisabled}
                                classNames={{
                                    input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
                                    inputWrapper:
                                        "bg-white/5 border-white/10 hover:border-white/20 focus-within:border-green-500/50 min-h-[48px]",
                                }}
                            />
                        </div>

                        {/* Application Name Field */}
                        <div className="space-y-2">
                            <label className="text-white/80 text-sm font-medium flex items-center space-x-2">
                                <Settings className="w-4 h-4 text-blue-400" />
                                <span>Application Name</span>
                            </label>
                            <Input
                                value={editFormData.applicationName}
                                onChange={(e) =>
                                    handleInputChange(
                                        "applicationName",
                                        e.target.value
                                    )
                                }
                                placeholder="Enter application name..."
                                disabled={isDisabled}
                                classNames={{
                                    input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
                                    inputWrapper:
                                        "bg-white/5 border-white/10 hover:border-white/20 focus-within:border-blue-500/50 min-h-[48px]",
                                }}
                            />
                        </div>

                        {/* Author Field */}
                        <div className="space-y-2">
                            <label className="text-white/80 text-sm font-medium flex items-center space-x-2">
                                <User className="w-4 h-4 text-purple-400" />
                                <span>Author</span>
                            </label>
                            <Input
                                value={editFormData.createdBy}
                                onChange={(e) =>
                                    handleInputChange(
                                        "createdBy",
                                        e.target.value
                                    )
                                }
                                placeholder="Enter author name..."
                                disabled={true} // Always disabled as per original design
                                classNames={{
                                    input: "bg-white/5 border-white/10 text-white placeholder:text-white/40 px-4 py-3",
                                    inputWrapper:
                                        "bg-white/5 border-white/10 hover:border-white/20 focus-within:border-purple-500/50 min-h-[48px]",
                                }}
                            />
                        </div>

                        {/* Meta Prompt Field */}
                        <div className="space-y-2">
                            <label className="text-white/80 text-sm font-medium">
                                Agent Meta Prompt
                            </label>
                            <div className="relative">
                                <textarea
                                    value={editFormData.agentMetaPrompt}
                                    onChange={(e) =>
                                        handleInputChange(
                                            "agentMetaPrompt",
                                            e.target.value
                                        )
                                    }
                                    placeholder="Define your agent's behavior, personality, and capabilities..."
                                    rows={6}
                                    disabled={isDisabled}
                                    className="w-full bg-white/5 border border-white/10 text-white placeholder:text-white/40 resize-none px-4 py-3 pr-12 rounded-xl hover:border-white/20 focus:border-orange-500/50 focus:outline-none transition-colors duration-200"
                                />
                                {/* Expand Button */}
                                <button
                                    onClick={handleExpandPrompt}
                                    disabled={isDisabled}
                                    className="absolute top-2 right-2 p-2 rounded-lg bg-gray-800/90 hover:bg-orange-500/80 border border-orange-500/50 hover:border-orange-400 transition-all duration-200 text-orange-400 hover:text-white z-20 shadow-lg hover:shadow-orange-500/25 backdrop-blur-sm disabled:opacity-50 disabled:cursor-not-allowed"
                                    title="Expand prompt editor"
                                >
                                    <Expand className="w-4 h-4" />
                                </button>
                            </div>
                            <p className="text-white/40 text-xs">
                                Describe how your agent should behave, what it
                                should know, and how it should respond to users.
                            </p>
                        </div>

                        {/* Agent Info Display */}
                        <div className="bg-white/5 backdrop-blur-sm rounded-xl p-4 border border-white/10">
                            <h3 className="text-white/80 font-medium mb-2 flex items-center space-x-2">
                                <Settings className="w-4 h-4 text-yellow-400" />
                                <span>Agent Information</span>
                            </h3>
                            <div className="space-y-1 text-sm">
                                <p className="text-white/60">
                                    <span className="text-white/80">
                                        Agent ID:
                                    </span>{" "}
                                    {selectedAgent.agentId || "N/A"}
                                </p>
                                <p className="text-white/60">
                                    <span className="text-white/80">
                                        Created:
                                    </span>{" "}
                                    {new Date().toLocaleDateString()}
                                </p>
                                <p className="text-white/60">
                                    <span className="text-white/80">
                                        Status:
                                    </span>{" "}
                                    <span className="text-green-400">
                                        Active
                                    </span>
                                </p>
                            </div>
                        </div>
                    </div>

                    {/* Footer */}
                    <div className="border-t border-white/10 p-6 flex-shrink-0">
                        <div className="flex items-center justify-start space-x-3">
                            <div className="relative group">
                                <div className="absolute -inset-0.5 bg-gradient-to-r from-green-500 to-blue-600 rounded-xl blur opacity-75 group-hover:opacity-100 transition duration-300"></div>
                                <Button
                                    onPress={handleEditSave}
                                    disabled={isDisabled}
                                    radius="full"
                                    className="relative bg-gradient-to-r from-green-500 to-blue-600 text-white font-semibold hover:from-green-600 hover:to-blue-700 transition-all duration-300 px-6 py-3 min-h-[44px] disabled:opacity-50"
                                >
                                    <Save className="w-4 h-4 mr-2" />
                                    Save Changes
                                </Button>
                            </div>
                            <div className="relative group">
                                <div className="absolute -inset-0.5 bg-gradient-to-r from-purple-500 to-indigo-600 rounded-xl blur opacity-75 group-hover:opacity-100 transition duration-300"></div>
                                <Button
                                    onPress={onTestAgent}
                                    disabled={isDisabled}
                                    radius="full"
                                    className="relative bg-gradient-to-r from-purple-500 to-indigo-600 text-white font-semibold hover:from-purple-600 hover:to-indigo-700 transition-all duration-300 px-6 py-3 min-h-[44px] disabled:opacity-50"
                                >
                                    <Play className="w-4 h-4 mr-2" />
                                    Test Agent
                                </Button>
                            </div>
                        </div>
                    </div>
                </div>
            )
        );
    };

    return IsEditAgentDataLoading ? (
        <FullScreenLoading isLoading={true} message="Saving agent data ..." />
    ) : (
        renderEditAgentsData()
    );
}
