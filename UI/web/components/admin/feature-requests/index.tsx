import { useEffect, useState } from "react";

import { GetAllSubmittedFeatureRequestsAsync } from "@store/app-admin/actions";
import { useAppDispatch, useAppSelector } from "@store/index";
import { NewFeatureRequestDTO } from "@models/request/new-feature-request-dto";

export default function FeatureRequestsAdminComponent() {
	const dispatch = useAppDispatch();

	const [isLoading, setIsLoading] = useState<boolean>(false);

	const IsFeatureRequestLoadingStoreData = useAppSelector<boolean>(
		(state) => state.ApplicationAdminReducer.isFeatureReaquestLoading,
	);
	const FeatureRequestsData = useAppSelector<NewFeatureRequestDTO[]>(
		(state) => state.ApplicationAdminReducer.featureRequestList,
	);

	useEffect(() => {
		dispatch(GetAllSubmittedFeatureRequestsAsync());
	}, [dispatch]);

	useEffect(() => {
		if (IsFeatureRequestLoadingStoreData !== isLoading)
			setIsLoading(IsFeatureRequestLoadingStoreData);
	}, [IsFeatureRequestLoadingStoreData, isLoading]);

	if (isLoading) {
		return (
			<div className="flex items-center justify-center py-12">
				<div className="text-center">
					<div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-400 mx-auto mb-4"></div>
					<p className="text-gray-400">Loading feature requests...</p>
				</div>
			</div>
		);
	}

	return (
		<div className="space-y-4">
			<div className="flex items-center justify-between">
				<h2 className="text-xl font-semibold text-white">
					Feature Requests
				</h2>
				<div className="text-sm text-gray-400">
					{FeatureRequestsData?.length || 0} requests found
				</div>
			</div>

			{FeatureRequestsData && FeatureRequestsData.length > 0 ? (
				<div className="space-y-3">
					{FeatureRequestsData.map((request: any, index: number) => (
						<div
							key={request.id}
							className="bg-white/5 border border-white/10 rounded-lg p-4 hover:bg-white/10 transition-colors"
						>
							<div className="flex items-start justify-between">
								<div className="flex-1">
									<h3 className="text-white font-medium mb-2">
										{request.title ||
											`Feature Request #${request.id}`}
									</h3>
									<p className="text-gray-300 text-sm mb-2">
										{request.description ||
											"No description provided"}
									</p>
									<div className="flex items-center space-x-4 text-xs text-gray-400">
										{request.dateCreated && (
											<span>
												{new Date(
													request.dateCreated ||
														request.dateModified,
												).toLocaleDateString()}
											</span>
										)}
										{request.createdBy && (
											<span className="px-2 py-1 bg-blue-500/20 text-blue-300 rounded">
												{request.createdBy}
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
						No feature requests found
					</div>
					<p className="text-sm text-gray-500">
						Feature requests will appear here when submitted
					</p>
				</div>
			)}
		</div>
	);
}
