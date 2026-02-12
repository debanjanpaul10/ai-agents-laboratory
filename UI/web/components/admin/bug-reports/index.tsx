import { useEffect, useState } from "react";

import { useAppDispatch, useAppSelector } from "@store/index";
import { GetAllBugReportsDataAsync } from "@store/app-admin/actions";

export default function BugReportsAdminComponent() {
	const dispatch = useAppDispatch();

	const [isLoading, setIsLoading] = useState<boolean>(false);

	const IsBugReportLoadingStoreData = useAppSelector(
		(state) => state.ApplicationAdminReducer.isBugReportLoading,
	);

	const BugReportsData = useAppSelector(
		(state) => state.ApplicationAdminReducer.bugReportsData,
	);

	useEffect(() => {
		dispatch(GetAllBugReportsDataAsync());
	}, [dispatch]);

	useEffect(() => {
		if (IsBugReportLoadingStoreData !== isLoading)
			setIsLoading(IsBugReportLoadingStoreData);
	}, [IsBugReportLoadingStoreData, isLoading]);

	if (isLoading) {
		return (
			<div className="flex items-center justify-center py-12">
				<div className="text-center">
					<div className="animate-spin rounded-full h-8 w-8 border-b-2 border-red-400 mx-auto mb-4"></div>
					<p className="text-gray-400">Loading bug reports...</p>
				</div>
			</div>
		);
	}

	return (
		<div className="space-y-4">
			<div className="flex items-center justify-between">
				<h2 className="text-xl font-semibold text-white">
					Bug Reports
				</h2>
				<div className="text-sm text-gray-400">
					{BugReportsData?.length || 0} reports found
				</div>
			</div>

			{BugReportsData && BugReportsData.length > 0 ? (
				<div className="space-y-3">
					{BugReportsData.map((report: any, index: number) => (
						<div
							key={index}
							className="bg-white/5 border border-white/10 rounded-lg p-4 hover:bg-white/10 transition-colors"
						>
							<div className="flex items-start justify-between">
								<div className="flex-1">
									<h3 className="text-white font-medium mb-2">
										{report.title ||
											`Bug Report #${index + 1}`}
									</h3>
									<p className="text-gray-300 text-sm mb-2">
										{report.description ||
											report.message ||
											"No description provided"}
									</p>
									<div className="flex items-center space-x-4 text-xs text-gray-400">
										{report.createdAt && (
											<span>
												{new Date(
													report.createdAt,
												).toLocaleDateString()}
											</span>
										)}
										{report.status && (
											<span className="px-2 py-1 bg-red-500/20 text-red-300 rounded">
												{report.status}
											</span>
										)}
									</div>
								</div>
							</div>
						</div>
					))}
				</div>
			) : (
				<div className="text-center py-12">
					<div className="text-gray-400 mb-2">
						No bug reports found
					</div>
					<p className="text-sm text-gray-500">
						Bug reports will appear here when submitted
					</p>
				</div>
			)}
		</div>
	);
}
