import { Button } from "@heroui/react";
import { PackagePlus, X } from "lucide-react";

export default function FeatureRequestComponent({
	onClose,
}: {
	onClose: () => void;
}) {
	return (
		<div className="h-full flex flex-col">
			{/* HEADER */}
			<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0">
				<div className="flex items-center space-x-3">
					<div className="bg-gradient-to-r from-emerald-400 to-teal-500 p-2 rounded-xl">
						<PackagePlus className="w-5 h-5 text-white" />
					</div>
					<div>
						<h2 className="text-xl font-bold bg-gradient-to-r from-white via-red-100 to-orange-100 bg-clip-text text-transparent">
							Submit a feature request
						</h2>
						<p className="text-white/50 text-sm">
							Have an idea in mind? Share with us!
						</p>
					</div>
				</div>
				<Button
					onPress={onClose}
					isIconOnly
					className="p-2 rounded-lg bg-white/5 hover:bg-red-500/20 border border-white/10 hover:border-red-500/30 transition-all duration-200 text-white/70 hover:text-red-400 min-w-[40px] h-[40px]"
					title="Close"
				>
					<X className="w-5 h-5" />
				</Button>
			</div>
		</div>
	);
}
