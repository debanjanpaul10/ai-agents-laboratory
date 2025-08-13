// *********************************************************************************
//	<copyright file="RewriteResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Rewrite response data DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Shared.Models.IBBS
{
    /// <summary>
    /// The Rewrite response data DTO.
    /// </summary>
    public class RewriteResponseDTO
    {
        /// <summary>
        /// The rewrittent story.
        /// </summary>
        public string RewrittenStory { get; set; } = string.Empty;

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
}
