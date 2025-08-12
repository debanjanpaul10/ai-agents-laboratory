// *********************************************************************************
//	<copyright file="TagResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The user story tag response DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Shared.Models.IBBS
{
	/// <summary>
	/// The user story tag response DTO.
	/// </summary>
	public class TagResponseDTO
	{
		/// <summary>
		/// The user story genre.
		/// </summary>
		public string UserStoryTag { get; set; } = string.Empty;

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
	}
}
