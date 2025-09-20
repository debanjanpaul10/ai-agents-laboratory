// *********************************************************************************
//	<copyright file="BaseResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The base response dto.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The base response dto.
/// </summary>
public class BaseResponseDTO
{
	/// <summary>
	/// The total tokens consumed.
	/// </summary>
	public int TotalTokensConsumed { get; set; }

	/// <summary>
	/// The candidates token count.
	/// </summary>
	public int CandidatesTokenCount { get; set; }

	/// <summary>
	/// The prompt token count.
	/// </summary>
	public int PromptTokenCount { get; set; }

	/// <summary>
	/// The AI model used for this request.
	/// </summary>
	public string ModelUsed { get; set; } = string.Empty;
}
