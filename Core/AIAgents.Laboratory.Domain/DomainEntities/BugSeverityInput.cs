// *********************************************************************************
//	<copyright file="BugSeverityInput.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Bug Severity Input Domain Class.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Bug Severity Input Domain Class.
/// </summary>
public class BugSeverityInput
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
