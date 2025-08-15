// *********************************************************************************
//	<copyright file="RewriteResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Rewrite response data DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Domain.DomainEntities.IBBS;

/// <summary>
/// The Rewrite response data DTO.
/// </summary>
public class RewriteResponse : BaseResponse
{
    /// <summary>
    /// The rewrittent story.
    /// </summary>
    public string RewrittenStory { get; set; } = string.Empty;

    
}
