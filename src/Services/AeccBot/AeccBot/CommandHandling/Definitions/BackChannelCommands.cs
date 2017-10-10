namespace AeccBot.MessageRouting
{
    public class BackChannelCommands
    {
        public const string BackChannelKeyword = "backchannel";

        public const string CommandAddAggregation = "addAggregation";
        public const string CommandDeleteAggregation = "deleteAggregation";
        public const string CommandCheckAggregation = "checkaggregation";
        public const string CommandListAggregations = "listAggregations";

        public const string CommandAcceptRequest = "acceptRequest";
        public const string CommandRejectRequest = "rejectRequest";
        public const string CommandEndEngagement = "endEngagement";
        public const string CommandSyncEngagement = "syncEngagement";

        public const string CommandEngagementCounterpart = "engagemnetCounterpart";

        public const string CommandInitiateEngagement = "initiateEngagement";
        

        public const string MessageRouter = "messageRouter";

        public static string GetMessageRouter(MessageRouterResultType type)
        {
            return $"{BackChannelKeyword} {MessageRouter} {type}";
        }
    }
}