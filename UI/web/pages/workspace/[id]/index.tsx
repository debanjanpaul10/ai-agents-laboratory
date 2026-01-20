import { useRouter } from "next/router";
import { Laptop, MoveLeft } from "lucide-react";
import { Button, Tooltip } from "@heroui/react";

import MainLayout from "@components/common/main-layout";
import { WorkspacesConstants } from "@helpers/constants";

export default function WorkspaceComponent() {
	const router = useRouter();

	const moveBack = () => {
		router.push("/workspaces");
	};

	return (
		<MainLayout>
			<div className="flex flex-col items-center justify-center h-[calc(100vh-4rem)] bg-white/5 rounded-3xl border border-white/10 p-8 text-center backdrop-blur-sm">
				<div className="bg-gradient-to-r from-blue-500 to-purple-600 p-6 rounded-full mb-6">
					<Laptop className="h-40 w-40" />
				</div>
				<h1 className="text-3xl font-bold bg-gradient-to-r from-white via-blue-100 to-purple-100 bg-clip-text text-transparent mb-4">
					{WorkspacesConstants.ComingSoonConstants.Header}
				</h1>
				<p className="text-white/60 max-w-md mx-auto text-lg">
					{WorkspacesConstants.ComingSoonConstants.SubHeading}
				</p>

				<div className="bg-gradient-to-r from-red-500 to-pink-600 p-6 mb-6 mt-5">
					<Tooltip
						content="Move back"
						showArrow={true}
						placement="bottom-start"
						offset={15}
					>
						<Button onPress={moveBack} isIconOnly>
							<MoveLeft className="h-10 w-10" />
						</Button>
					</Tooltip>
				</div>
			</div>
		</MainLayout>
	);
}
