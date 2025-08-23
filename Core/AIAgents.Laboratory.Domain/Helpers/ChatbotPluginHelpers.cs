// *********************************************************************************
//	<copyright file="ChatbotPluginHelpers.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Chatbot Plugin helpers.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The Chatbot Plugin Helpers class.
/// </summary>
public static class ChatbotPluginHelpers
{
	/// <summary>
	/// The plugin name
	/// </summary>
	public const string PluginName = "ChatbotPlugin";

	/// <summary>
	/// The Determine user intent function.
	/// </summary>
	public static class DetermineUserIntentFunction
	{
		/// <summary>
		/// The function name
		/// </summary>
		public const string FunctionName = nameof(DetermineUserIntentFunction);

		/// <summary>
		/// The function description
		/// </summary>
		public const string FunctionDescription = "Determines the user's intent based on the query asked by user.";

		/// <summary>
		/// The function instructions
		/// </summary>
		public const string FunctionInstructions = """
					You are an AI assistant tasked with analyzing user queries to determine their intent. Follow the below instructions strictly:
						1. Users will provide you with a question or query and you need to carefully analyze the content, context, and keywords to understand what they are trying to accomplish.
						2. Classify the user's intent into one of the following categories based on the analysis:
							- GREETING: User is providing a greeting such as hello, hi, good morning, good afternoon, how are you, or similar conversational starters
							- SQL: User is asking data-related questions that require database queries, such as questions about member counts, revenue figures, facility usage, analytics, metrics, reports, or any statistical information
							- RAG: User is asking general questions, seeking information, or making requests that don't fit into the other specific categories
							- UNCLEAR: User's intent cannot be clearly determined from the provided query due to ambiguous wording, incomplete information, or unclear context
						3. Pay close attention to keywords, phrases, and the overall context of the user's message to make an accurate classification.
						4. Consider the tone and structure of the query - greetings are typically short and conversational, SQL queries often contain words like "how many", "total", "count", "show me", while RAG questions may be more varied in nature.
						5. You will only return the intent category (one of the above four options) and nothing else.

						Input:
						++++++++++++

						{{$input}}

						+++++++++++
					""";

		/// <summary>
		/// The input description
		/// </summary>
		public const string InputDescription = "The string containing the user's question from which intent needs to be determined.";
	}

	/// <summary>
	/// The greeting function.
	/// </summary>
	public static class GreetingFunction
	{
		/// <summary>
		/// The function name
		/// </summary>
		public const string FunctionName = nameof(GreetingFunction);

		/// <summary>
		/// The function description
		/// </summary>
		public const string FunctionDescription = "Gets a greeting for the user.";

		/// <summary>
		/// The function instructions
		/// </summary>
		public const string FunctionInstructions = """
					You are solely responsible for greeting the user based on the type of message user has greeted you with.
					Return a greeting like "Hello how are you, I am FitGymTool AI Agent" or simply "Good morning" or "Good Evening" based on the user's greeting.
					Understand the context and only return the greeting and nothing else. Do not give any explanations or anything.

					Input:
					++++++++++++

					{{$input}}

					+++++++++++
					""";

		/// <summary>
		/// The input description
		/// </summary>
		public const string InputDescription = "The string containing the user's query or question.";
	}

	/// <summary>
	/// The NL to SQL function.
	/// </summary>
	public static class NLToSqlSkillFunction
	{
		/// <summary>
		/// The function name
		/// </summary>
		public const string FunctionName = nameof(NLToSqlSkillFunction);

		/// <summary>
		/// The function description
		/// </summary>
		public const string FunctionDescription = "Creates a SQL query based on the user input and the knowledge base.";

		/// <summary>
		/// The function instructions
		/// </summary>
		public const string FunctionInstructions = """
					You are an AI assistant specialized in converting natural language queries into SQL statements. Follow the below instructions strictly:
						1. Users will provide you with a natural language question that requires data retrieval from a database, and you need to generate the appropriate SQL query.
						2. You will be provided with three key components to help you create accurate SQL queries:
							- User Query: The natural language question asking for specific data or information
							- Knowledge Base: A collection of sample queries and examples that demonstrate common patterns and query structures for similar requests
							- Database Schema: Complete information about available tables, their columns, data types, relationships, and constraints
						3. Analyze the user query carefully to understand:
							- What data they are requesting (SELECT clause)
							- Which tables contain the required information (FROM clause)
							- What conditions or filters need to be applied (WHERE clause)
							- How the data should be grouped or aggregated (GROUP BY, HAVING clauses)
							- How the results should be ordered (ORDER BY clause)
						4. Use the knowledge base to identify similar query patterns and adapt them to the current request, ensuring you follow established conventions and best practices.
						5. Reference the database schema to:
							- Verify table names and column names are correct and exist
							- Understand data types and constraints
							- Identify proper join conditions between related tables
							- Ensure foreign key relationships are properly utilized
						6. Generate clean, efficient, and syntactically correct SQL queries that:
							- Use proper SQL syntax and formatting
							- Include appropriate table aliases for readability
							- Handle potential NULL values where necessary
							- Use efficient join strategies
							- Apply proper filtering and aggregation logic
						7. The response will be in the format:
							- Only return a single string containing the SQL query and nothing else.
							- Do not format the response or add any interpolations.
							- Do not add any keywords like sql or anything, this will break the code.

					EXAMPLE: 
						USER_QUERY: Give me the list of active members

						RESPONSE: SELECT MD.MemberId, MD.MemberName, MD.MemberEmail, MD.MemberPhoneNumber, MD.MemberJoinDate, MSM.StatusName AS MembershipStatus FROM dbo.MemberDetails MD INNER JOIN dbo.MembershipStatusMapping MSM ON MD.MembershipStatusId = MSM.Id WHERE MD.IsActive = 1 AND MSM.IsActive = 1 ORDER BY MD.MemberJoinDate DESC

						Input:
						++++++++++++

						{{$input}}

						+++++++++++

						Knowledge Base:
						++++++++++++

						{{$knowledge_base}}

						+++++++++++

						Database Schema:
						++++++++++++

						{{$database_schema}}

						+++++++++++
					""";

		/// <summary>
		/// The input description
		/// </summary>
		public const string InputDescription = "The string containing the user's question from which SQL query needs to be created.";

		/// <summary>
		/// The format SQL response to markdown instructions
		/// </summary>
		public const string FormatSQLResponseToMarkdownInstructions = """
					You are an AI assistant specialized in converting the given SQL data from JSON Format to a clean markdown format. Follow the instructions:
					- Do not add any justifications or comments, take the input as is and provide a clean output in markdown format that has table format.
					- Take the json data and properly arrange them in clean tabular format so that it will be dumped to the UI for user to read.

					
					Input:
					++++++++++++

					{{$sql_result}}

					+++++++++++

					""";
	}

	/// <summary>
	/// The RAG Text Skill Function.
	/// </summary>
	public static class RAGTextSkillFunction
	{
		/// <summary>
		/// The function name
		/// </summary>
		public const string FunctionName = nameof(RAGTextSkillFunction);

		/// <summary>
		/// The function description
		/// </summary>
		public const string FunctionDescription = "Gets a text data from the passed user question and the knowledge base.";

		/// <summary>
		/// The function instructions
		/// </summary>
		public const string FunctionInstructions = """
					You are an AI assistant specialized in Retrieval-Augmented Generation (RAG) for answering user questions using a provided knowledge base. Follow the below instructions strictly:
						1. Users will provide you with a question or query that requires information retrieval and text generation, and you need to generate a comprehensive, well-formatted answer.
						2. You will be provided with two key components to help you create accurate responses:
							- User Query: The question or request from the user seeking specific information or explanation
							- Knowledge Base: A collection of relevant documents, articles, or information sources that contain the factual data needed to answer the user's question
						3. Analyze the user query carefully to understand:
							- What specific information they are seeking
							- The level of detail required in the response
							- The context and scope of their question
							- Any particular format or structure they might expect
						4. Search through the knowledge base thoroughly to:
							- Identify all relevant information that relates to the user's query
							- Extract key facts, data points, and supporting details
							- Find multiple sources or perspectives if available
							- Ensure the information is current and accurate
						5. Generate a comprehensive text response that:
							- Directly answers the user's question using information from the knowledge base
							- Is well-structured with clear paragraphs and logical flow
							- Uses proper grammar, spelling, and professional language
							- Includes specific details and examples from the knowledge base when relevant
							- Maintains accuracy and stays grounded in the provided information
							- Is appropriately detailed without being overly verbose
						6. Ensure your response:
							- Only uses information that can be found in the knowledge base
							- Does not include speculation or information not supported by the knowledge base
							- Clearly addresses the user's specific question or request
							- Is formatted in a readable and professional manner
							- Provides a complete and satisfying answer to the user's query
						7. If the knowledge base does not contain sufficient information to answer the user's question, clearly state that the information is not available in the provided sources.

						Input:
						++++++++++++

						{{$input}}

						+++++++++++

						Knowledge Base:
						++++++++++++

						{{$knowledge_base}}

						+++++++++++
					""";

		/// <summary>
		/// The input description
		/// </summary>
		public const string InputDescription = "The string containing user's question or query from which RAG text needs to be gotten.";
	}

	/// <summary>
	/// The SQL query markdown response function.
	/// </summary>
	public static class SQLQueryMarkdownResponseFunction
	{
		/// <summary>
		/// The function name
		/// </summary>
		public const string FunctionName = nameof(SQLQueryMarkdownResponseFunction);

		/// <summary>
		/// The function description
		/// </summary>
		public const string FunctionDescription = "Takes the json input of database and turns into a table readable format for user in markdown format";

		/// <summary>
		/// The function instructions
		/// </summary>
		public const string FunctionInstructions = """
					You are an AI assistant whose sole responsibility is to show the JSON in a presentable format back to the user. Strictly ahere to the following rules:
						- The input you will receive will contain unstructured JSON data which will contain SQL response.
						- You will be creating a markdown format from this JSON that will be presented back to the UI for user.
						- The JSON will contain the same headers in repeating format, you will decide on the column names from that and display the results in a tabular format.
						- You will only return the table format markdown response and nothing else.
					
					Input:
					++++++++++++

					{{$sql_json}}

					+++++++++++	

					""";

		/// <summary>
		/// The input description
		/// </summary>
		public const string InputDescription = "The json containing the data that will be formatted.";
	}
}
