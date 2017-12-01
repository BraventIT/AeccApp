using Aecc.Models;

namespace AeccApp.Core.Models
{
    public class EmailFromHospital : EmailMessage
    {
        protected GlobalSetting GSetting { get { return GlobalSetting.Instance; } }

        public EmailFromHospital(RequestModel request, string[] receiverAddresses)
        {
            To = string.Empty;
            foreach (var item in receiverAddresses)
            {
                To += item + ";";
            }
            Subject = $"{request.RequestType.Name} - {request.RequestDate} - {request.RequestAddress.Province}";

            Body = $"El beneficiario {GSetting.User?.Name} ha enviado una peticion del tipo :{request.RequestType.Name} con fecha {request.RequestDate}. \n" +
                $"A continuación se detalla la dirección del hospital: \n {request.RequestAddress.DisplayAddress} \n Habitacion {request.RequestAddress.HospitalRoom} , sala {request.RequestAddress.HospitalHall} \n El beneficiario ha hecho los siguientes comentarios :\n" +
                $"{request.RequestComments} \n \n \n \n Este mensaje ha sido enviado a todos los coordinadores de la provincia correspondiente a la petición.";
        }
    }
}
