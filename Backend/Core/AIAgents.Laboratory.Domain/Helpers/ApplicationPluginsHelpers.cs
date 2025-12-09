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
        public const string FunctionInstructions = @"""
			You are an assistant for the user whose responsibility is to reply to user queries. 
				1. You will be receiving the user message and agent metaprompt as inputs.
				2. Based on these inputs you will be framing a response for the user for the user message.
				3. You will never give any explanations or side notes. You will only return the fixed output and nothing else.  

            Tools:
                1. You will have access to knowledge base data. If user mentions to extract data from knowledge base, you will extract data from the knowledgebase document as well.
                2. You will have access to MCP server tools as well if present. If user mentions to get the data from the MCP server, call the MCP server and based on the required tool, gather the data.
            
            Make sure to never mention any MCP server or tools used when crafting the final response.

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

    /// <summary>
    /// The Determine Tool To Call Function.
    /// </summary>
    public static class DetermineToolToCallFunction
    {
        /// <summary>
        /// The function name
        /// </summary>
        public const string FunctionName = nameof(DetermineToolToCallFunction);

        /// <summary>
        /// The function description
        /// </summary>
        public const string FunctionDescription = "Decides which tool to call based on the given list of tools.";

        /// <summary>
        /// Gets the function instructions.
        /// </summary>
        /// <param name="toolDescriptions">The tool descriptions.</param>
        /// <param name="input">The input.</param>
        /// <returns>The Prompt response.</returns>
        public static string GetFunctionInstructions(string toolDescriptions, string input) =>
           $@"""
            # Role:
                - You are a tool selection assistant. Based on the user's query, determine if any of the available tools should be called.
            # Available Tools: 
                - {toolDescriptions}
            # User Query:
                - {input}
                
            # Instructions:
                1. If a tool is needed, respond with EXACTLY this JSON format:
                {{toolName: exact_tool_name, toolArguments: {{list_of_tool_arguments}}
                
                2. If NO tool is needed, respond with:
                {{toolName: """", toolArguments: {{}} }}
                
                3. For toolArguments, provide a valid JSON object with the parameters the tool needs. If the tool requires no parameters, use {{}}.

            # Response: 
                - The response will be a strict JSON format: 
                    {{
                        ""toolName"": ""name_of_the_tool_to_call_or_empty_if_none"",
                        ""toolArguments"": {{
                            // key-value pairs of tool arguments
                        }}  
                    }}.
            - Make the JSON valid and parsable.
                """;

        /// <summary>
        /// The input descriptions.
        /// </summary>
        public static class InputDescriptions
        {
            /// <summary>
            /// The user input
            /// </summary>
            public const string UserInput = "The user input received from the front end";

            /// <summary>
            /// The list of tools
            /// </summary>
            public const string ListOfTools = "The list of available tools available in the MCP server";
        }
    }

    /// <summary>
    /// The Generate Final Response With Tool Result Function.
    /// </summary>
    public static class GenerateFinalResponseWithToolResultFunction
    {
        /// <summary>
        /// The function name
        /// </summary>
        public const string FunctionName = nameof(GenerateFinalResponseWithToolResultFunction);

        /// <summary>
        /// The function description
        /// </summary>
        public const string FunctionDescription = "Generates the final response using the tool result.";

        /// <summary>
        /// Gets the function instructions.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="toolResult">The tool result.</param>
        /// <returns>The function instructions.</returns>
        public static string GetFunctionInstructions(string input, string? toolResult) =>
             string.IsNullOrEmpty(input)
                ? $@"User Query: {input} 
					Please provide a helpful response to the user's query."
                : $@"User Query: {input}
					Tool Result: {toolResult}
					Based on the tool result above, provide a clear and helpful response to the user's query. Format the information in a user-friendly way.";

        /// <summary>
        /// The input descriptions.
        /// </summary>
        public static class InputDescriptions
        {
            /// <summary>
            /// The user input
            /// </summary>
            public const string UserInput = "The user input received from the front end";

            /// <summary>
            /// The list of tools
            /// </summary>
            public const string ToolResult = "The response received from executing the tool.";
        }
    }
}
