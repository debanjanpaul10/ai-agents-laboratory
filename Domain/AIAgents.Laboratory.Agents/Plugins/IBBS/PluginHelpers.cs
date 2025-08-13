// *********************************************************************************
//	<copyright file="PluginHelpers.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Plugin helpers.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Agents.Plugins.IBBS
{
	/// <summary>
	/// Plugin helpers.
	/// </summary>
	public static class PluginHelpers
	{
		/// <summary>
		/// The rewrite text plugin.
		/// </summary>
		public static class RewriteTextPlugin
		{
			/// <summary>
			/// The plugin name
			/// </summary>
			public const string PluginName = nameof(RewriteTextPlugin);

			/// <summary>
			/// Rewrite user story plugin.
			/// </summary>
			public static class RewriteUserStoryPlugin
			{
				/// <summary>
				/// The function name.
				/// </summary>
				public const string FunctionName = nameof(RewriteUserStoryPlugin);

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
		}

		/// <summary>
		/// The content related plugins.
		/// </summary>
		public static class ContentPlugins
		{
			/// <summary>
			/// The plugin name
			/// </summary>
			public const string PluginName = nameof(ContentPlugins);

			/// <summary>
			/// Generate genre tag for user's story plugin.
			/// </summary>
			public static class GenerateGenreTagForStoryPlugin
			{
				/// <summary>
				/// The function name.
				/// </summary>
				public const string FunctionName = nameof(GenerateGenreTagForStoryPlugin);

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
			public static class ContentModerationPlugin
			{
				/// <summary>
				/// The function name.
				/// </summary>
				public const string FunctionName = nameof(ContentModerationPlugin);

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
		}



	}

}

