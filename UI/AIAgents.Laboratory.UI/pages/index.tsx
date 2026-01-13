import AuthenticatedApp from "@components/common/providers/authenticated-app";
import CreateAgentComponent from "@components/create-agent";
import FeedbackComponent from "@components/feedback";
import DashboardComponent from "@pages/dashboard";
import MarketplaceComponent from "@pages/marketplace";

export default function Home() {
	return (
		<AuthenticatedApp>
			<DashboardComponent />
			<CreateAgentComponent />
			<FeedbackComponent />
			<MarketplaceComponent />
		</AuthenticatedApp>
	);
}
