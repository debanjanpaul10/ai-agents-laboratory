// *********************************************************************************
//	<copyright file="BugSeverityInputDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Bug Severity Input DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Adapters.Models.Request.FitGymTool;

/// <summary>
/// The Bug Severity Input DTO.
/// </summary>
public class BugSeverityInputDTO
{
	/// <summary>
	/// Gets or sets the bug title.
	/// </summary>
	/// <value>
	/// The bug title.
	/// </value>
	public string BugTitle { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the bug description.
	/// </summary>
	/// <value>
	/// The bug description.
	/// </value>
	public string BugDescription { get; set; } = string.Empty;
}
