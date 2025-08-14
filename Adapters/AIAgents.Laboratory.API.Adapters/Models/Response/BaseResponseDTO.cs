using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIAgents.Laboratory.API.Adapters.Models.Response;

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
