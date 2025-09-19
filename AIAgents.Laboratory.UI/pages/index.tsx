import AuthenticatedApp from "@/components/common/authenticated-app";
import CreateAgentPage from "@/components/create-agent";
import DashboardComponent from "@pages/dashboard";

export default function Home() {
	return (
		<AuthenticatedApp>
			<DashboardComponent />
			<CreateAgentPage />
		</AuthenticatedApp>
	);
}
