import { Bot } from "lucide-react";

import { DashboardConstants } from "@helpers/constants";

export default function FooterComponent() {
	return (
		<footer className="mt-12">
			<div className="relative bg-white/5 backdrop-blur-sm rounded-xl border border-white/10">
				<div className="max-w-7xl mx-auto px-8 py-6">
					<div className="flex items-center justify-center space-x-2 text-white/60">
						<Bot className="w-4 h-4 text-blue-400" />
						<p className="text-sm">
							{DashboardConstants.FooterConstants.FooterText}
						</p>
					</div>
				</div>
			</div>
		</footer>
	);
}
