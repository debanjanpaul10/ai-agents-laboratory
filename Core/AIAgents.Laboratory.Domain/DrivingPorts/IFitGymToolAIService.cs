// *********************************************************************************
//	<copyright file="IFitGymToolAIService.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The FitGym Tool AI Service Interface.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities.FitGymTool;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// The FitGym Tool AI Service Interface.
/// </summary>
public interface IFitGymToolAIService
{
	/// <summary>
	/// Gets the bug severity asynchronous.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>The bug severity response.</returns>
	Task<BugSeverityResponse> GetBugSeverityAsync(BugSeverityInput bugSeverityInput);
}
