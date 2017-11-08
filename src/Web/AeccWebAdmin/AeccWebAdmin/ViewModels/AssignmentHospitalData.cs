using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccApi.WebAdmin.ViewModels
{
    public class AssignmentHospitalData
    {
        public int HospitalID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}
