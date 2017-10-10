namespace AeccBot.CommandHandling
{
    public class Commands
    {
        public const string CommandKeyword = AeccStrings.CommandKeyword; // Used if the channel does not support mentions

        public const string CommandAddAggregationChannel = AeccStrings.Command_AddAggregationChannel;
        public const string CommandDeleteAggregationChannel = AeccStrings.Command_DeleteAggregationChannel;

        public const string CommandAcceptRequest = AeccStrings.Command_AcceptRequest;
        public const string CommandRejectRequest = AeccStrings.Command_RejectRequest;

        public const string CommandEndEngagement = AeccStrings.Command_EndEngagement;
        public const string CommandInitiateEngagement = AeccStrings.Command_InitiateEngagement;
       
        public const string CommandDeleteAllRoutingData = "reset";

#if DEBUG
        // For debugging
        public const string CommandListAllParties = "list parties";
        public const string CommandListPendingRequests = "list requests";
        public const string CommandListEngagements = "list conversations";

        public const string CommandListLastMessageRouterResults = "list results";
#endif
    }
}