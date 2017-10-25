namespace AeccApp.Core.Models
{
    public class RequestModel
    {
        public RequestTypeModel RequestType { get; set; }
        public string RequestDate { get; set; }
        public string RequestTime { get; set; }
        public string RequestComments { get; set; }
        public AddressModel RequestAddress { get; set; }


        public RequestModel()
        {

        }
        public RequestModel(RequestTypeModel RequestType, string RequestDate, string RequestTime, string RequestComments, AddressModel RequestAddress)
        {
            this.RequestType = RequestType;
            this.RequestDate = RequestDate;
            this.RequestTime = RequestTime;
            this.RequestComments = RequestComments;
            this.RequestAddress = RequestAddress;
        }


    }
}
