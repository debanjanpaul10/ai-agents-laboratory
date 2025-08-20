// *********************************************************************************
//	<copyright file="UserRequestDomain.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The user request domain.</summary>
// *********************************************************************************
namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The user request domain.
/// </summary>
public class UserRequestDomain
{
	/// <summary>
	/// Gets or sets the user query.
	/// </summary>
	/// <value>
	/// The user query.
	/// </value>
	public string UserQuery { get; set; } = string.Empty;
}
