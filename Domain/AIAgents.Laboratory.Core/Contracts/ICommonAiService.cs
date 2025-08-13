// *********************************************************************************
//	<copyright file="ICommonAiService.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Common AI Service interface.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Core.Contracts;

/// <summary>
/// Common AI Service interface.
/// </summary>
public interface ICommonAiService
{
	/// <summary>
	/// Gets the current model identifier.
	/// </summary>
	/// <returns>The current model identifier.</returns>
	string GetCurrentModelId();
}