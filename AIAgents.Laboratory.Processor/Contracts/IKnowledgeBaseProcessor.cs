namespace AIAgents.Laboratory.Processor.Contracts;

/// <summary>
/// The interface for processing knowledge base documents and retrieving relevant knowledge.
/// </summary>
public interface IKnowledgeBaseProcessor
{
	/// <summary>
	/// Processes the knowledge base document asynchronous.
	/// </summary>
	/// <param name="content">The content.</param>
	/// <param name="agentId">The agent identifier.</param>
	/// <returns>A task to wait on.</returns>
	Task ProcessKnowledgeBaseDocumentAsync(string content, string agentId);

	/// <summary>
	/// Gets the relevant knowledge asynchronous.
	/// </summary>
	/// <param name="query">The query.</param>
	/// <param name="agentId">The agent identifier.</param>
	/// <returns>The knowledge base data.</returns>
	Task<string> GetRelevantKnowledgeAsync(string query, string agentId);
}
