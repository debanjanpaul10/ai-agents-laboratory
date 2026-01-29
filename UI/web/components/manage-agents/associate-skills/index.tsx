import { useEffect, useState } from "react";
import { Check, Search, Terminal, X, Info, Loader2 } from "lucide-react";
import { Button, Input } from "@heroui/react";

import { GetAllToolSkillsApiAsync } from "@shared/api-service";
import { ToolSkillDTO } from "@models/response/tool-skill-dto";
import { AssociateSkillsFlyoutProps } from "@shared/types";
import { AssociateSkillsFlyoutPropsConstants } from "@helpers/constants";

export default function AssociateSkillsFlyoutComponent({
	isOpen,
	onClose,
	onSkillsChange,
	selectedSkillGuids,
}: Readonly<AssociateSkillsFlyoutProps>) {
	const [skills, setSkills] = useState<ToolSkillDTO[]>([]);
	const [isLoading, setIsLoading] = useState<boolean>(false);
	const [searchQuery, setSearchQuery] = useState<string>("");
	const [localSelectedGuids, setLocalSelectedGuids] = useState<string[]>([]);

	useEffect(() => {
		if (isOpen) {
			fetchSkills();
			setLocalSelectedGuids(selectedSkillGuids);
		}
	}, [isOpen, selectedSkillGuids]);

	const fetchSkills = async () => {
		setIsLoading(true);
		try {
			const response = await GetAllToolSkillsApiAsync();
			if (response.isSuccess) {
				setSkills(response.responseData as ToolSkillDTO[]);
			}
		} catch (error) {
			console.error("Failed to fetch skills:", error);
		} finally {
			setIsLoading(false);
		}
	};

	const toggleSkill = (guid: string) => {
		setLocalSelectedGuids((prev) =>
			prev.includes(guid)
				? prev.filter((id) => id !== guid)
				: [...prev, guid],
		);
	};

	const handleDone = () => {
		onSkillsChange(localSelectedGuids);
		onClose();
	};

	const filteredSkills = skills.filter(
		(skill) =>
			skill.toolSkillDisplayName
				.toLowerCase()
				.includes(searchQuery.toLowerCase()) ||
			skill.toolSkillTechnicalName
				.toLowerCase()
				.includes(searchQuery.toLowerCase()),
	);

	const renderAssociatedSkillsList = () => {
		return filteredSkills.length > 0 ? (
			filteredSkills.map((skill) => (
				<div
					key={skill.toolSkillGuid}
					onClick={() => toggleSkill(skill.toolSkillGuid)}
					className={`relative group cursor-pointer transition-all duration-300 p-4 rounded-xl border ${
						localSelectedGuids.includes(skill.toolSkillGuid)
							? "bg-cyan-500/10 border-cyan-500/40"
							: "bg-white/5 border-white/10 hover:border-white/20"
					}`}
					onKeyDown={() => toggleSkill(skill.toolSkillGuid)}
				>
					<div className="flex items-center justify-between">
						<div className="flex items-center space-x-4">
							<div
								className={`p-2 rounded-lg transition-colors duration-300 ${
									localSelectedGuids.includes(
										skill.toolSkillGuid,
									)
										? "bg-cyan-500/20"
										: "bg-white/10"
								}`}
							>
								<Terminal
									className={`w-5 h-5 ${
										localSelectedGuids.includes(
											skill.toolSkillGuid,
										)
											? "text-cyan-400"
											: "text-white/60"
									}`}
								/>
							</div>
							<div>
								<h4 className="text-white font-semibold">
									{skill.toolSkillDisplayName}
								</h4>
								<p className="text-white/40 text-xs font-mono">
									{skill.toolSkillTechnicalName}
								</p>
							</div>
						</div>
						<div
							className={`w-6 h-6 rounded-full border-2 flex items-center justify-center transition-all duration-300 ${
								localSelectedGuids.includes(skill.toolSkillGuid)
									? "bg-cyan-500 border-cyan-500 scale-110 shadow-lg shadow-cyan-500/20"
									: "bg-white/5 border-white/10"
							}`}
						>
							{localSelectedGuids.includes(
								skill.toolSkillGuid,
							) && <Check className="w-4 h-4 text-white" />}
						</div>
					</div>
				</div>
			))
		) : (
			<div className="h-full flex flex-col items-center justify-center space-y-3 opacity-40 py-12">
				<Search className="w-12 h-12" />
				<p>{AssociateSkillsFlyoutPropsConstants.Hints.NoSkills}</p>
			</div>
		);
	};

	if (!isOpen) return null;

	return (
		<div className="flex flex-col h-full overflow-hidden">
			{/* Header */}
			<div className="flex items-center justify-between p-6 border-b border-white/10 flex-shrink-0 bg-white/[0.02] backdrop-blur-md">
				<div className="flex items-center space-x-3">
					<div className="bg-gradient-to-br from-cyan-500 to-blue-600 p-2.5 rounded-xl shadow-lg shadow-blue-500/20">
						<Terminal className="w-5 h-5 text-white" />
					</div>
					<div>
						<h2 className="text-xl font-bold bg-gradient-to-r from-white via-cyan-100 to-blue-100 bg-clip-text text-transparent">
							{
								AssociateSkillsFlyoutPropsConstants.Headers
									.Heading
							}
						</h2>
						<p className="text-white/40 text-sm">
							{
								AssociateSkillsFlyoutPropsConstants.Headers
									.SubHeading
							}
						</p>
					</div>
				</div>
				<button
					onClick={onClose}
					className="p-2 rounded-lg bg-white/5 hover:bg-red-500/10 border border-white/10 hover:border-red-500/20 transition-all duration-300 text-white/50 hover:text-red-400"
				>
					<X className="w-5 h-5" />
				</button>
			</div>

			{/* Info Box */}
			<div className="px-6 pt-6 flex-shrink-0">
				<div className="bg-blue-500/5 border border-blue-500/20 rounded-xl p-4 flex items-start space-x-3">
					<Info className="w-5 h-5 text-blue-400 flex-shrink-0 mt-0.5" />
					<p className="text-white/60 text-xs leading-relaxed">
						{AssociateSkillsFlyoutPropsConstants.Hints.Info}
					</p>
				</div>
			</div>

			{/* Search */}
			<div className="p-6 pb-2 flex-shrink-0">
				<Input
					value={searchQuery}
					onChange={(e) => setSearchQuery(e.target.value)}
					placeholder={
						AssociateSkillsFlyoutPropsConstants.Hints.Search
					}
					variant="bordered"
					startContent={<Search className="w-4 h-4 text-white/40" />}
					classNames={{
						input: "text-white placeholder:text-white/20",
						inputWrapper:
							"bg-white/5 border-white/10 hover:border-cyan-500/30 focus-within:!border-cyan-500/50 transition-all rounded-xl h-12",
					}}
				/>
			</div>

			{/* Skills List */}
			<div className="flex-1 overflow-y-auto p-6 space-y-3 scrollbar-thin scrollbar-thumb-white/10">
				{isLoading ? (
					<div className="h-full flex flex-col items-center justify-center space-y-4">
						<Loader2 className="w-8 h-8 text-cyan-500 animate-spin" />
						<span className="text-white/40 animate-pulse">
							Loading skills...
						</span>
					</div>
				) : (
					<>{renderAssociatedSkillsList()}</>
				)}
			</div>

			{/* Footer */}
			<div className="p-6 pt-2 border-t border-white/10 bg-black/40 backdrop-blur-xl flex-shrink-0">
				<div className="flex items-center justify-between">
					<p></p>
					<div className="flex space-x-3">
						<Button
							onPress={handleDone}
							className="px-6 h-12 bg-gradient-to-r from-green-600 to-emerald-600 text-white font-bold rounded-xl shadow-lg hover:shadow-green-500/20 transition-all duration-300 group"
						>
							<span>
								{
									AssociateSkillsFlyoutPropsConstants.Buttons
										.Done
								}
							</span>
							<Check className="w-4 h-4 ml-2 group-hover:scale-110 transition-transform" />
						</Button>
						<Button
							onPress={() => setLocalSelectedGuids([])}
							variant="bordered"
							className="h-12 px-6 border-white/10 text-white/60 hover:text-white hover:border-red-500/30 transition-all rounded-xl"
						>
							{AssociateSkillsFlyoutPropsConstants.Buttons.Clear}
						</Button>
					</div>
				</div>
			</div>
		</div>
	);
}
