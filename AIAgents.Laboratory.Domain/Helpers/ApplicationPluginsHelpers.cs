namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The application plugin helpers class.
/// </summary>
public static class ApplicationPluginsHelpers
{
	/// <summary>
	/// The plugin name
	/// </summary>
	public const string PluginName = "ApplicationPlugins";

	/// <summary>
	/// The Determine Bug Severity Function.
	/// </summary>
	public static class DetermineBugSeverityFunction
	{
		/// <summary>
		/// The function name
		/// </summary>
		public const string FunctionName = nameof(DetermineBugSeverityFunction);

		/// <summary>
		/// The function description
		/// </summary>
		public const string FunctionDescription = "Generates the bug severity based on the bug description provided by the user.";

		/// <summary>
		/// The function instructions
		/// </summary>
		public const string FunctionInstructions = """
				You are an AI assistant tasked to generate the bug severity based on the following parameters:
					1. Users will be providing you with the bug title and the bug description in a JSON object format and you will be thoroughly going through it.
					2. After perusing through the contents, you will be determining the bug severity out of High, Medium, Low and NA respectively.
					3. If the bug that user describes:
						- Is a blocker that is preventing user to do any action; then mark it as High
						- Is a issue that is not exactly blocking the user, but seems to be a problem in data or data mismatch or can be solved via a workaround; then mark it as Medium
						- Is a feature or request that would be a nice to have or good to have that might provide users with better clarity or could be simpler; then mark it as Low
						- For others, mark it as NA.
					4. Only return the bug severity out of -- High, Medium, Low, NA. Do not return anything else. No explanations.
					Input:
					++++++++++++

					{{$input}}

					+++++++++++
					
				""";

		/// <summary>
		/// The input description
		/// </summary>
		public const string InputDescription = "The object containing bug title and bug description provided by user.";
	}

	/// <summary>
	/// Generate genre tag for user's story plugin.
	/// </summary>
	public static class GenerateGenreTagForStoryFunction
	{
		/// <summary>
		/// The function name.
		/// </summary>
		public const string FunctionName = nameof(GenerateGenreTagForStoryFunction);

		/// <summary>
		/// The function description.
		/// </summary>
		public const string FunctionDescription = "Generates a genre tag for a story that tells what type of story it is.";

		/// <summary>
		/// The function instructions.
		/// </summary>
		public const string FunctionInstructions = """
			You are an AI assistant tasked to generate what is the genre of the story that user has described. Follow the below instruction strictly:
				1. Users will be giving you an experience or story and you will be reading the story thoroughly and generate a simple tag that tells what kind of story it is.
				2. The genre of story can be horror, comedy, romance, science-fiction, etc. You need to carefully go through the story and define the type.
				3. Pay close attention to the story and only after reading everything in the story, then only decide the genre.
				4. You will only return the genre of story and nothing else. It will be one word or maximum two words.

				Input:
				++++++++++++

				{{$input}}

				+++++++++++
			""";

		/// <summary>
		/// The input description.
		/// </summary>
		public const string InputDescription = "The user story text to generate the genre tag on";
	}

	/// <summary>
	/// Manage the content moderation.
	/// </summary>
	public static class ContentModerationFunction
	{
		/// <summary>
		/// The function name.
		/// </summary>
		public const string FunctionName = nameof(ContentModerationFunction);

		/// <summary>
		/// The function description.
		/// </summary>
		public const string FunctionDescription = "Handles the content moderation by checking the story for NSFW or 18+ content";

		/// <summary>
		/// The function description.
		/// </summary>
		public const string FunctionInstructions = """
			You are an AI assistant tasked to analyze the user's story content for NSFW or 18+ content. Follow the below instruction strictly:
				1. Users will be giving you an experience or story and you will be reading the story thoroughly and decide if it contains any mature language, sexual references, pornographic materials, etc.
				2. These stories can be read by minors or underaged users, so be very very careful and then mark the story as NSFW.
				3. If the story is NSFW or contains mature content or has 18+ details, then mark the story as NSFW (containing mature content), else return SAFE (Available to be read by users of all age groups)
				4. You will only return one word: either NSFW or SAFE, nothing else.

				Input:
				++++++++++++

				{{$input}}

				+++++++++++
			""";

		/// <summary>
		/// The input description.
		/// </summary>
		public const string InputDescription = "The user story text which is to be analyzed.";
	}

	/// <summary>
	/// Rewrite user story plugin.
	/// </summary>
	public static class RewriteUserStoryFunction
	{
		/// <summary>
		/// The function name.
		/// </summary>
		public const string FunctionName = nameof(RewriteUserStoryFunction);

		/// <summary>
		/// The function description.
		/// </summary>
		public const string FunctionDescription = "Rewrites a user story to be clearer, more concise, and actionable.";

		/// <summary>
		/// The function instructions.
		/// </summary>
		public const string FunctionInstructions = """
			You are an assistant tasked with modifying what user has written based on the following parameters:
				1. Users will be giving you an experience or story and you will be modifying and cleaning the story.
				2. Ensure that the modified response sounds natural and align's with the user's original intent and story.
				3. Pay attention to the user's story and intent and based on that only modify the content.
				4. If the story contains gramatical errors, fix them subtly.
				5. You will only return the fixed output and nothing else.

				Input:
				++++++++++++

				{{$input}}

				+++++++++++
			""";

		/// <summary>
		/// The input description.
		/// </summary>
		public const string InputDescription = "The user story text to rewrite";
	}

	/// <summary>
	/// The get chat message response function.
	/// </summary>
	public static class GetChatMessageResponseFunction
	{
		/// <summary>
		/// The function name
		/// </summary>
		public const string FunctionName = nameof(GetChatMessageResponseFunction);

		/// <summary>
		/// The function description
		/// </summary>
		public const string FunctionDescription = "Gets the response for user's query from the chat message.";

		/// <summary>
		/// The function instructions
		/// </summary>
		public const string FunctionInstructions = """
			You are an assistant for the user whose responsibility is to reply to user queries. 
				1. You will be receiving the user message and agent metaprompt as inputs.
				2. Based on these inputs you will be framing a response for the user for the user message.
				3. You will never give any explanations or side notes. You will only return the fixed output and nothing else.

			Input:
			++++++++++++

			{{$input}}

			+++++++++++
			""";

		/// <summary>
		/// The input description
		/// </summary>
		public const string InputDescription = "The chat request message that contains AgentName, UserMessage and AgentMetaprompt.";
	}
}
