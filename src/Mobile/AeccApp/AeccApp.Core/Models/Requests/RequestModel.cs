using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Models.Requests
{
    public class RequestModel
    {

        public string RequestType { get; set; }
        public string RequestLocation { get; set; }
        public string RequestDate { get; set; }
        public string RequestTime { get; set; }
        public string RequestComments { get; set; }

        public AddressModel RequestAddress { get; set; }

        public RequestModel(string RequestType,string RequestLocation, string RequestDate, string RequestTime, string RequestComments, AddressModel RequestAddress)
        {
            this.RequestType = RequestType;
            this.RequestLocation = RequestLocation;
            this.RequestDate = RequestDate;
            this.RequestTime = RequestTime;
            this.RequestComments = RequestComments;
            this.RequestAddress = RequestAddress;
        }


    }
}
