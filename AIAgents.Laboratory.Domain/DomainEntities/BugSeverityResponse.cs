// *********************************************************************************
//	<copyright file="BugSeverityResponse.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Bug Severity Response Domain Class.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Bug Severity Response.
/// </summary>
/// <seealso cref="BaseResponse" />
public class BugSeverityResponse : BaseResponse
{
	/// <summary>
	/// Gets or sets the bug severity.
	/// </summary>
	/// <value>
	/// The bug severity.
	/// </value>
	public string BugSeverity { get; set; } = string.Empty;
}
