using AeccBot.CommandHandling;
using AeccBot.MessageRouting;
using AeccBot.MessageRouting.DataStore;
using Aecc.Models;

namespace AeccBot
{
    public class AeccStrings
    {
        private const string REFERENCE_CHANNEL = "test";

        //COMMANDS
        public const string CommandKeyword = "aecc"; // Used if the channel does not support mentions
        public const string Command_AddAggregationChannel = "disponible";
        public const string Command_AcceptRequest = "acepta";
        public const string Command_RejectRequest = "ignora";
        public const string Command_EndEngagement = "termina";
        public const string Command_InitiateEngagement = "charla";
        public const string Command_DeleteAggregationChannel = "no disponible";

        // COMMAND RESPONSES
        public const string HandleCommand_NotFound = "No he entendio el comando \"{0}\".";

        public const string AddNoAgentsAvailable_Text = "Lo sentimos, pero ahora mismo no hay ningún voluntario disponible.";
        public const string AddNoAgentsAvailable_Text2 = "Por favor, mantente a la espera hasta que te notifiquemos y luego escribe un saludo para comenzar la conversación. ¡Gracias!";

        public const string AddAggregationChannel_Text = "En este canal es donde se añadirán solicitudes.";
        public const string AddAggregationChannel_NotifyText = "\"{0}\" ya está disponible.";
        public const string AddAggregationChannel_AlreadyExistsText = "Esta canal ya está recibiendo solicitudes.";

        public const string DeleteAggregationChannel_Text = "Ya no estás disponible para recibir solicitudes. Para volver a estar disponible, teclea: {0}.";
        public const string DeleteAggregationChannel_NotExistText = "No estás disponible. Si quieres estar disponible, teclea: {0}";
        public const string DeleteAggregationChannel_InEngagementText = "Estás en una conversación. Si quieres cancelarla, teclea: {0}";
        public const string DeleteAggregationChannel_InEngagementEventText = "Estás en una conversación. Si quieres cancelarla, pulsa el botón cancelar.";

        public const string EndEngagement_NotFoundText = "No he podido encontrar la conversación";
        public const string EndEngagement_ErrorText = "No se ha podido finalizar la conversación";

        public const string AcceptAndRejectRequest_PartyNotFound = "No puedo encontrar la petición pendiente del beneficiario \"{0}\"";
        public const string AcceptAndRejectRequest_UserNotFound = "Falta el nombre del beneficiario.";
        public const string AcceptAndRejectRequest_NoRequest = "No hay peticiones pendientes.";
        public const string AcceptAndRejectRequest_InOtherConversation = "Ya estás involucrado en una conversación con \"{0}\"";
        public const string AcceptAndRejectRequest_NoAggregation = "Antes de entablar una conversación tienes que estar disponible. Para ello, teclea: {0}";
        public const string AcceptAndRejectRequest_Error = "Ha ocurrido un error.";

        public const string SendText_AlreadyAggregation = "Tienes que esperar a que se te envíe una solicitud para hablar con un beneficiario.";

        // ENGAGEMENT RESPONSES
        public const string EngagementInitiated_ClientText = "Danos un momento para iniciar la conversación.";
        public const string EngagementInitiated_AggregationNotFoundClientText = "No podemos contactar con el voluntario. Puedes intentarlo con otros voluntarios.";

        public const string EngagementInitiated_AlreadyClientText = "Estamos intentando iniciar la conversación. Si quieres cancelarla, teclea: {0}";
        public const string EngagementInitiated_AlreadyClientEventText = "Estamos intentando iniciar la conversación. Si quieres cancelarla, pulsa el botón cancelar.";
        public const string EngagementInitiated_TimeoutClientText = "Lo sentimos, el voluntario que has seleccionado ahora no puede hablar. Prueba más adelante o elige a otro voluntario.";

        public const string RejectRequest_OwnerText = "Petición del beneficiario \"{0}\" rechazada.";
        public const string RejectRequest_ClientText = "Lo sentimos, el voluntario que has seleccionado ahora no puede hablar. Prueba más adelante o elige a otro voluntario.";
        public const string RejectRequest_AlreadyRejectClientText = "Conversación cancelada.";

        public const string EngagementAdded_OwnerEventText = "Estás conectado con el beneficiario \"{0}\". Cuando termines la conversación pulsa el botón cancelar.";
        public const string EngagementAdded_OwnerText = "Estás conectado con el beneficiario \"{0}\". Cuando termines la conversación teclea: {1}.";
        public const string EngagementAdded_ClientText = "ahora estás hablando con \"{0}\"";

        public const string EngagementRemoved_OwnerEventText = "Conversación con el usuario \"{0}\" terminada. Sigues estando disponible para comunicarte con otros beneficiarios.";
        public const string EngagementRemoved_OwnerText = "Conversación con el usuario \"{0}\" terminada. Sigues estando disponible para comunicarte con otros beneficiarios. Para dejar de estar disponible, teclea: {1}";
        public const string EngagementRemoved_ClientText = "La conversación con \"{0}\" ha terminado.";

        //PENDING ENGAGEMENT REQUEST CARD
        public const string RequestCard_Title = "Petición de asistencia";
        public const string RequestCard_Subtitle = "Usuario: {0}";
        public const string RequestCard_Text = "Utiliza los botones para aceptar o rechazar.";
        public const string RequestCard_AcceptTitle = "Aceptar";
        public const string RequestCard_RejectTitle = "Rechazar";


        public const double Timeout_PendingRequest_InSg = 30;
        public const double Timeout_Engagement_InSg = 60 * 15;
        public const double Timeout_AggregationChecked_InSg = 60;

        private static IRoutingDataManager _routingDataManager;

        static AeccStrings()
        {
            _routingDataManager = WebApiConfig.RoutingDataManager;
        }

        public static string GetCommandKeyword(Party pendingRequest, Party aggregationParty)
        {
            string botName = _routingDataManager.ResolveBotNameInConversation(aggregationParty);
            string commandKeyword = (string.IsNullOrEmpty(botName) || pendingRequest.ChannelId != REFERENCE_CHANNEL) ? Commands.CommandKeyword : $"@{botName}";
            return commandKeyword;
        }

        public static string GetCommandKeyword(Party party)
        {
            return GetCommandKeyword(party, party);
        }
    }
}