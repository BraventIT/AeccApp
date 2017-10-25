using AeccApi.Models;
using System.Linq;

namespace AeccApi.Data
{
    public class DbInitializer
    {
        public static void Initialize(AeccContext context)
        {
            context.Database.EnsureCreated();

            // Look for any requestType.
            if (context.RequestTypes.Any())
            {
                return;   // DB has been seeded
            }

            var requestTypes = new RequestType[]
            {
                 new RequestType()
                 { Source= RequestSourceEnum.Domicilio
                 , Name= "Acompañamiento a pacientes en el domicilio"
                 },
                 new RequestType()
                 {
                     Source= RequestSourceEnum.Domicilio
                     ,Name ="Acompañamiento y apoyo en la realización de gestiones en el domicilio"
                 }
            };

            foreach (var r in requestTypes)
            {
                context.RequestTypes.Add(r);
            }

            context.SaveChanges();

            var hospitals = new Hospital[]
            {
                new Hospital()
                {
                    City ="Zaragoza",
                    Name ="Miguel Servet",
                    Province ="Zaragoza",
                    Street ="Isabel La Católina, 2"
                },
                new Hospital()
                {
                    City="Zaragoza",
                    Name="Hospital Royo Villanova",
                    Province="Zaragoza",
                    Street="Avd/ San Gregorio s/n 50015 Zaragoza"
                }
            };

            foreach (var h in hospitals)
            {
                context.Hospitals.Add(h);
            }

            context.SaveChanges();

            var coordinators = new Coordinator[]
            {
                new Coordinator()
                {
                     Province="Zaragoza",
                      Name="Alberto Fraile",
                }
            };

            foreach (var c in coordinators)
            {
                context.Coordinators.Add(c);
            }

            context.SaveChanges();

            var employments = new Employ[]
            {
                new Employ()
                { CoordinatorID=1, HospitalID=1, RequestSource= RequestSourceEnum.Hospital }
            };

            foreach (var e in employments)
            {
                context.Employments.Add(e);
            }

            context.SaveChanges();
        }
    }
}
