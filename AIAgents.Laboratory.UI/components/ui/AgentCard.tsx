import { AgentDataDTO } from "@lib/types";
import Link from "next/link";

export default function AgentCard({ agent }: { agent: AgentDataDTO }) {
	return (
		<div className="border p-4 rounded shadow hover:shadow-lg transition">
			<h2 className="text-lg font-semibold">{agent.agentName}</h2>
			<p className="text-sm text-gray-600 mb-2">
				{agent.agentMetaPrompt.slice(0, 100)}...
			</p>
			<Link href={`/agents/${agent.agentId}`}>
				<button className="text-blue-600 hover:underline">View</button>
			</Link>
		</div>
	);
}
