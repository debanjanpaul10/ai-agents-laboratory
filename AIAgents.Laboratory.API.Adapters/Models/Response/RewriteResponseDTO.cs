// *********************************************************************************
//	<copyright file="RewriteResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Rewrite response data DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Adapters.Models.Response
{
    /// <summary>
    /// The Rewrite response data DTO.
    /// </summary>
    public class RewriteResponseDTO : BaseResponseDTO
    {
        /// <summary>
        /// The rewrittent story.
        /// </summary>
        public string RewrittenStory { get; set; } = string.Empty;
    }
}
