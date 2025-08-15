// *********************************************************************************
//	<copyright file="TagResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The user story tag response DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Adapters.Models.Response.IBBS
{
    /// <summary>
    /// The user story tag response DTO.
    /// </summary>
    public class TagResponseDTO : BaseResponseDTO
    {
        /// <summary>
        /// The user story genre.
        /// </summary>
        public string UserStoryTag { get; set; } = string.Empty;
    }
}
