import { useMsal } from "@azure/msal-react";
import { Spotlight } from "lucide-react";

export default function WelcomeCardComponent() {
	const { accounts } = useMsal();

	return (
		accounts[0] && (
			<div className="mb-8 relative group/welcome">
				<div className="absolute -inset-0.5 bg-gradient-to-r from-blue-500/30 to-purple-500/30 rounded-2xl blur opacity-50 group-hover/welcome:opacity-75 transition duration-500"></div>
				<div className="relative bg-white/5 backdrop-blur-sm rounded-2xl p-4 md:p-6 border border-white/10 hover:border-white/20 transition-all duration-300">
					<div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
						<div className="flex items-center space-x-4">
							<div className="w-10 h-10 md:w-12 md:h-12 bg-gradient-to-r from-blue-500 to-purple-600 rounded-full flex items-center justify-center">
								<span className="text-white font-bold text-base md:text-lg">
									{(accounts[0].name || accounts[0].username)
										.charAt(0)
										.toUpperCase()}
								</span>
							</div>
							<div>
								<h2 className="text-base md:text-xl font-semibold text-white mb-1">
									Welcome,{" "}
									{accounts[0].name || accounts[0].username}!
								</h2>
								<p className="text-white/70 flex items-center space-x-2 text-xs md:text-base">
									<Spotlight />
									<span>{accounts[0].username}</span>
								</p>
							</div>
						</div>
					</div>
				</div>
			</div>
		)
	);
}
