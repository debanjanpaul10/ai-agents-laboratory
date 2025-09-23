// *********************************************************************************
//	<copyright file="AgentStatus.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Agent Status Class.</summary>
// *********************************************************************************

using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Agent Status Class.
/// </summary>
public class AgentStatus
{
	/// <summary>
	/// Gets or sets a value indicating whether this instance is available.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance is available; otherwise, <c>false</c>.
	/// </value>
	public bool IsAvailable { get; set; }

	/// <summary>
	/// Gets or sets the updated at.
	/// </summary>
	/// <value>
	/// The updated at.
	/// </value>
	public DateTimeOffset UpdatedAt { get; set; }

	/// <summary>
	/// Gets or sets the source.
	/// </summary>
	/// <value>
	/// The source.
	/// </value>
	public string Source { get; set; } = AzureAppConfigurationConstants.AzureAppConfigurationConstant;
}
