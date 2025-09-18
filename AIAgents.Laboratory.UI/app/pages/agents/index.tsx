import { useEffect, useState } from "react";
import Link from "next/link";

import AgentCard from "../../../app/ui/AgentCard";
import Header from "../../components/Header";
import ProtectedRoute from "../../components/ProtectedRoute";
import { AgentDataDTO } from "../../../app/lib/types";
import { useAppDispatch } from "@/store";
import { GetAllAgentsDataAsync } from "@/store/agents/actions";

export default function AgentsPage() {
	const dispatch = useAppDispatch();
	const [agents, setAgents] = useState<AgentDataDTO[]>([]);

	useEffect(() => {
		dispatch(GetAllAgentsDataAsync(""));
	}, []);

	return (
		<ProtectedRoute>
			<div className="min-h-screen bg-gray-50">
				<Header />
				<main className="max-w-7xl mx-auto py-8 px-4 sm:px-6 lg:px-8">
					<div className="flex justify-between items-center mb-6">
						<div>
							<h1 className="text-3xl font-bold text-gray-900">
								AI Agents
							</h1>
							<p className="mt-2 text-gray-600">
								Manage and monitor your AI agents
							</p>
						</div>
						<Link href="/agents/new">
							<button className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md transition-colors">
								+ Create New Agent
							</button>
						</Link>
					</div>

					{agents.length === 0 ? (
						<div className="text-center py-12">
							<div className="text-gray-400 text-6xl mb-4">
								🤖
							</div>
							<h3 className="text-lg font-medium text-gray-900 mb-2">
								No agents yet
							</h3>
							<p className="text-gray-600 mb-6">
								Get started by creating your first AI agent
							</p>
							<Link href="/agents/new">
								<button className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-md transition-colors">
									Create Your First Agent
								</button>
							</Link>
						</div>
					) : (
						<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
							{agents.map((agent: AgentDataDTO) => (
								<AgentCard key={agent.agentId} agent={agent} />
							))}
						</div>
					)}
				</main>
			</div>
		</ProtectedRoute>
	);
}
