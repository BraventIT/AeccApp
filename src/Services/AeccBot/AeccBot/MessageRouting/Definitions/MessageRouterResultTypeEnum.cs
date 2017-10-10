namespace AeccBot.MessageRouting
{
    public enum MessageRouterResultType
    {
        NoActionTaken, // The result handler should ignore results with this type
        OK, // The result handler should ignore results with this type
        NoAgentsAvailable,
        ConnectionRequested,
        ConnectionAlreadyRequested,
        ConnectionRejected,
        Connected,
        Disconnected,
        AddAggregation,
        DeleteAggregation,
        NoAggregationChannel,
        FailedToForwardMessage,
        Error, // Generic error including e.g. null arguments     
    }
}