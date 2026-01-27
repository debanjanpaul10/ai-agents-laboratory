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
                1. You will be receiving the user message and agent metaprompt as inputs always.
                2. Based on these inputs you will be framing a response for the user for the user message.
                3. You will never give any explanations or side notes. You will only return the fixed output and nothing else. 
                4. If user message or metaprompt is not present, do not return any response. 

            Tools:
                1. You will have access to knowledge base data. If user mentions to extract data from knowledge base, you will extract data from the knowledgebase document as well.
                2. You will have access to MCP server tools as well if present. If user mentions to get the data from the MCP server, call the MCP server and based on the required tool, gather the data.
                3. You will have access to images uploaded by user and from that image some keywords are extracted. If user mentions to get the data from the image, then use those keywords when framing your response.
            
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
                {{toolName: exact_tool_name, toolArguments: list_of_tool_arguments}}
                
                2. If NO tool is needed, respond with:
                {{toolName: """", toolArguments: {{}}}}
                
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
             string.IsNullOrEmpty(toolResult)
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

    /// <summary>
    /// The System Orchestrator Function.
    /// </summary>
    public static class SystemOrchestratorFunction
    {
        /// <summary>
        /// The function name
        /// </summary>
        public const string FunctionName = nameof(SystemOrchestratorFunction);

        /// <summary>
        /// The max orchestrator loops
        /// </summary>
        public const int MAX_ORCHESTRATOR_LOOPS = 10;

        /// <summary>
        /// The orchestrator response type delegate
        /// </summary>
        public const string OrchestratorResponseTypeDelegate = "delegate";

        /// <summary>
        /// The orchestrator response type final response
        /// </summary>
        public const string OrchestratorResponseTypeFinalResponse = "response";

        /// <summary>
        /// The function instructions
        /// </summary>
        /// <param name="agentsList">The list of agents.</param>
        /// <returns>The function instruction string.</returns>
        public static string GetFunctionInstructions(string agentsList) =>
            $@"
                You are an intelligent Orchestrator Agent responsible for managing a group chat between a user and multiple AI agents.
                Your goal is to answer the user's request by coordinating with the available agents.

                ### Available Agents:
                    {agentsList}

                ### Instructions:
                    1. Analyze the user's request and the current conversation history.
                    2. Decide if you can answer the request directly (ONLY if it's a simple greeting or if the task is complete) or if you need to delegate to a specific agent.
                    3. If you need to delegate:
                       - Choose the most appropriate agent from the list.
                       - Provide clear instructions to that agent based on the user's request or previous agents' outputs.
                    4. If the task is complete or you have the final answer, provide the final response to the user.

                ### Response Format:
                    You must respond in strictly valid JSON format. Do not include any other text.
        
                **Option 1: Delegate to an Agent**
                {{
                    ""type"": ""delegate"",
                    ""agentName"": ""Exact Agent Name from List"",
                    ""instruction"": ""Specific instruction for the agent""
                }}

                **Option 2: Final Response**
                {{
                    ""type"": ""response"",
                    ""content"": ""The final answer to the user""
                }}";
    }
}
