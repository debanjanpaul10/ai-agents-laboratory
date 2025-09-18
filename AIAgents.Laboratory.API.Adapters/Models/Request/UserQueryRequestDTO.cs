// *********************************************************************************
//	<copyright file="UserQueryRequestDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The User Query Request DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The User Query Request DTO.
/// </summary>
public class UserQueryRequestDTO
{
	/// <summary>
	/// Gets or sets the user query.
	/// </summary>
	/// <value>
	/// The user query.
	/// </value>
	public string UserQuery { get; set; } = string.Empty;
}
