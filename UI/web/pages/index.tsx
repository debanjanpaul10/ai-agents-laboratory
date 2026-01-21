import AuthenticatedApp from "@components/common/providers/authenticated-app";
import CreateAgentComponent from "@components/manage-agents/create-agent";
import FeedbackComponent from "@components/feedback";
import DashboardComponent from "@pages/dashboard";

export default function Home() {
	return (
		<AuthenticatedApp>
			<DashboardComponent />
			<CreateAgentComponent />
			<FeedbackComponent />
		</AuthenticatedApp>
	);
}
