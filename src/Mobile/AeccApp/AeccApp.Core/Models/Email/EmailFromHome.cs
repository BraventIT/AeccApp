namespace AeccApp.Core.Models.Email
{
    public class EmailFromHome : EmailBase
    {
        protected GlobalSetting GSetting { get { return GlobalSetting.Instance; } }

        public EmailFromHome(RequestModel request , string  []receiverAddresses )
        {
            To = string.Empty;
            SentRequest = request;
            foreach (var item in receiverAddresses)
            {
                To += item + ";";
            }
            Subject = $"{SentRequest.RequestType.Name} - {SentRequest.RequestDate} - {SentRequest.RequestAddress.Province}";

            Body = $"El beneficiario {GSetting.User?.Name} ha enviado una peticion del tipo :{SentRequest.RequestType.Name} con fecha {SentRequest.RequestDate}. \n" +
                $"A continuación se detalla la dirección: \n {SentRequest.RequestAddress.DisplayAddress} \n El beneficiario ha hecho los siguientes comentarios :\n" +
                $"{SentRequest.RequestComments} \n \n \n \n Este mensaje ha sido enviado a todos los coordinadores de la provincia correspondiente a la petición.";
        }
    }
}
