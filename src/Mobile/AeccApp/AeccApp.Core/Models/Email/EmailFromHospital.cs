using Aecc.Models;

namespace AeccApp.Core.Models
{
    public class EmailFromHospital : EmailMessage
    {
        protected GlobalSetting GSetting { get { return GlobalSetting.Instance; } }

        public EmailFromHospital(RequestModel request, string[] receiverAddresses)
        {
            for (int i = 0; i < receiverAddresses.Length; i++)
            {
                if (i == 0)
                {
                    To += receiverAddresses[i];
                }
                else
                {
                    To += ";"+receiverAddresses[i] ;
                }
            }
           
            Subject = $"{request.RequestType.Name} - {request.RequestDate} - {request.RequestAddress.Province}";

            Body = $"El beneficiario {GSetting.User?.FirstName} {GSetting.User?.Surname}(Apodo:{GSetting.User?.Name}) ha enviado una peticion del tipo : \n{request.RequestType.Name} con fecha {request.RequestDate}. \n \n" +
                $"A continuación se detalla la dirección del hospital: \n{request.RequestAddress.DisplayAddress}" +
                $", Habitacion {request.RequestAddress.DisplayRoom} \n \n" +
                $"El beneficiario ha hecho los siguientes comentarios :\n" +
                $"{request.DisplayComments} \n \n \n Este mensaje ha sido enviado a todos los coordinadores de la provincia correspondiente a la petición.";
        }
    }
}
