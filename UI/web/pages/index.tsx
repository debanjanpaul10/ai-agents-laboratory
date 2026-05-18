import AuthenticatedApp from "@components/common/providers/authenticated-app";
import CreateAgentComponent from "@components/manage-agents/create-agent";
import FeedbackComponent from "@components/feedback";
import DashboardComponent from "@pages/dashboard";

/**
 * The main page of the application, which includes the dashboard, create agent component, and feedback component. 
 * This page is wrapped in the AuthenticatedApp component to ensure that only authenticated users can access it.
 * @component `Home`
 * @returns A React component that renders the main page of the application.
 */
export default function Home() {
    return (
        <AuthenticatedApp>
            <DashboardComponent />
            <CreateAgentComponent />
            <FeedbackComponent />
        </AuthenticatedApp>
    );
}
