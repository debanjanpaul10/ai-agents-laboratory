import AuthenticatedApp from "@components/common/authenticated-app";
import CreateAgentComponent from "@components/create-agent";
import ManageAgentsComponent from "@components/manage-agents";
import DashboardComponent from "@pages/dashboard";

export default function Home() {
	return (
		<AuthenticatedApp>
			<DashboardComponent />
			<CreateAgentComponent />
			<ManageAgentsComponent />
		</AuthenticatedApp>
	);
}
