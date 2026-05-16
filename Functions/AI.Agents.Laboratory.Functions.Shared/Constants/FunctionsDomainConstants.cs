namespace AI.Agents.Laboratory.Functions.Shared.Constants;

/// <summary>
/// Defines constant values used across the functions domain, such as reasons and descriptions for dead-lettering messages in Service Bus.
/// </summary>
public static class FunctionsDomainConstants
{
    /// <summary>
    /// Defines constant values related to dead-lettering messages in Service Bus, including reasons and descriptions for invalid payloads.
    /// </summary>
    public static class DeadLetterConstants
    {
        /// <summary>
        /// The reason for dead-lettering a message due to an invalid payload.
        /// </summary>
        public const string InvalidPayloadReason = "InvalidPayload";

        /// <summary>
        /// The description for dead-lettering a message due to an invalid payload, indicating that the message payload could not be deserialized into the expected format.
        /// </summary>
        public const string InvalidPayloadDescription = "The message payload could not be deserialized into the expected format.";

        /// <summary>
        /// The processing exception reason
        /// </summary>
        public const string ProcessingExceptionReason = "ProcessingException";

        /// <summary>
        /// The processing exception description
        /// </summary>
        public const string ProcessingExceptionDescription = "An exception occurred during the processing of the message.";

        /// <summary>
        /// The json deserialization exception reason
        /// </summary>
        public const string JsonDeserializationExceptionReason = "JsonDeserializationException";

        /// <summary>
        /// The json deserialization exception description
        /// </summary>
        public const string JsonDeserializationExceptionDescription = "An exception occurred during JSON deserialization of the message payload.";
    }
}
