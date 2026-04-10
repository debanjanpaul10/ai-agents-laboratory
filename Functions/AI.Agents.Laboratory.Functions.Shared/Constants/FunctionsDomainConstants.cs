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
    }
}
