// *********************************************************************************
//	<copyright file="ModerationContentResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The content moderation response DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Shared.Models.IBBS
{
	/// <summary>
	/// The moderation content response DTO.
	/// </summary>
	public class ModerationContentResponseDTO
	{
		/// <summary>
		/// The content rating for the user story.
		/// </summary>
		public string ContentRating { get; set; } = string.Empty;

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
