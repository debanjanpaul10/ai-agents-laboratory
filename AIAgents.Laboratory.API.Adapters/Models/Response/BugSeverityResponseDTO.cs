// *********************************************************************************
//	<copyright file="BugSeverityResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Bug Severity Response DTO Class.</summary>
// *********************************************************************************
namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The Bug Severity Response DTO.
/// </summary>
/// <seealso cref="BaseResponseDTO" />
public class BugSeverityResponseDTO : BaseResponseDTO
{
	/// <summary>
	/// Gets or sets the bug severity.
	/// </summary>
	/// <value>
	/// The bug severity.
	/// </value>
	public string BugSeverity { get; set; } = string.Empty;
}
