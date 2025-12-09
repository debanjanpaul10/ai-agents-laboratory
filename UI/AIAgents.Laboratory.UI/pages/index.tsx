import AuthenticatedApp from "@components/common/providers/authenticated-app";
import CreateAgentComponent from "@components/create-agent";
import FeedbackComponent from "@components/feedback";
import ManageAgentsComponent from "@components/manage-agents";
import DashboardComponent from "@pages/dashboard";

export default function Home() {
	return (
		<AuthenticatedApp>
			<DashboardComponent />
			<CreateAgentComponent />
			<ManageAgentsComponent />
			<FeedbackComponent />
		</AuthenticatedApp>
	);
}
