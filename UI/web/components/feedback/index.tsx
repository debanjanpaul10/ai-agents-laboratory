import { useEffect, useState } from "react";

import { FEEDBACK_TYPES } from "@shared/types";
import { useAppDispatch, useAppSelector } from "@store/index";
import BugReportComponent from "@components/feedback/bug-report";
import FeatureRequestComponent from "@components/feedback/feature-request";
import { ToggleFeedbackDrawer } from "@store/common/actions";
import { FullScreenLoading } from "@components/common/spinner";

/**
 * This component is responsible for rendering the feedback drawer, which can be used to submit bug reports or feature requests. 
 * It listens to the global state to determine when to open or close the drawer and what type of feedback is being submitted. 
 * The component also handles disabling background scrolling when the drawer is open and shows a loading state when feedback is being sent.
 * @component `FeedbackComponent`
 * @returns A React component that renders the feedback drawer based on the global state.
 */
export default function FeedbackComponent() {
    const dispatch = useAppDispatch();

    const [feedbackDrawerData, setFeedbackDrawerData] = useState({
        drawerType: FEEDBACK_TYPES.BUGREPORT,
        isDrawerOpen: false,
    });

    const IsFeedbackDrawerOpenStoreData = useAppSelector(
        (state) => state.CommonReducer.isFeedbackDrawerOpen,
    );
    const IsFeedbackDrawerLoadingStoreData = useAppSelector(
        (state) => state.CommonReducer.isFeedbackDrawerLoading,
    );

    useEffect(() => {
        if (
            IsFeedbackDrawerOpenStoreData !== undefined &&
            IsFeedbackDrawerOpenStoreData !== feedbackDrawerData
        ) {
            setFeedbackDrawerData(IsFeedbackDrawerOpenStoreData);
        }
    }, [IsFeedbackDrawerOpenStoreData]);

    // Disable background scrolling when drawer is open
    useEffect(() => {
        if (feedbackDrawerData.isDrawerOpen) {
            document.body.style.overflow = "hidden";
        } else {
            document.body.style.overflow = "unset";
        }

        // Cleanup on unmount
        return () => {
            document.body.style.overflow = "unset";
        };
    }, [feedbackDrawerData.isDrawerOpen]);

    const onClose = () => {
        dispatch(ToggleFeedbackDrawer(false, feedbackDrawerData.drawerType));
    };

    const renderDrawerContent = () => {
        switch (feedbackDrawerData.drawerType) {
            case FEEDBACK_TYPES.BUGREPORT:
                return <BugReportComponent onClose={onClose} />;

            case FEEDBACK_TYPES.NEWFEATURE:
                return <FeatureRequestComponent onClose={onClose} />;

            default:
                return;
        }
    };

    return (
        feedbackDrawerData.isDrawerOpen && (
            <>
                {IsFeedbackDrawerLoadingStoreData ? (
                    <FullScreenLoading
                        isLoading={true}
                        message="Sending feedback"
                    />
                ) : (
                    <div className="fixed inset-0 bg-black/60 backdrop-blur-sm z-50 transition-opacity duration-300 max-w-full">
                        <div className="fixed left-0 top-0 md:w-1/2 h-screen z-[60] transition-all duration-500 ease-in-out">
                            <div className="absolute inset-0 bg-gradient-to-r from-cyan-600/20 via-blue-600/20 to-purple-600/20 blur-sm opacity-50 -z-10"></div>
                            <div className="relative h-full bg-gradient-to-br from-gray-900/95 via-slate-900/95 to-black/95 backdrop-blur-xl border-r border-white/10 shadow-2xl">
                                {renderDrawerContent()}
                            </div>
                        </div>
                    </div>
                )}
            </>
        )
    );
}
