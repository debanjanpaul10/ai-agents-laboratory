import AuthenticatedApp from "@/components/common/AuthenticatedApp";
import DashboardComponent from "./pages/dashboard/page";

export default function Home() {
	return (
		<AuthenticatedApp>
			<DashboardComponent />
		</AuthenticatedApp>
	);
}
