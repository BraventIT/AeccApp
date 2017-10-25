using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HomeRequestService: IHomeRequestService
    {
        public async Task<IEnumerable<RequestTypeModel>> GetRequestTypes()
        {
            await Task.Delay(2000);
            return new RequestTypeModel[]
            {
                 new RequestTypeModel()
                 { Source= RequestSourceEnum.Domicilio
                 , Name= "Acompañamiento a pacientes en el domicilio"
                 },
                 new RequestTypeModel()
                 {
                     Source= RequestSourceEnum.Domicilio
                     ,Name ="Acompañamiento y apoyo en la realización de gestiones en el domicilio"
                 }
            };
        }

        public async Task<IEnumerable<CoordinatorModel>> GetCoordinators(string province)
        {
            await Task.Delay(2000);
            if (province.StartsWith("Barcelona", StringComparison.CurrentCultureIgnoreCase))
            {
                return new CoordinatorModel[]
                {
                    new CoordinatorModel()
                    {
                        Name= "Jordi Abad"
                        , Email="jordi.abad@test.org"
                        , Province="Barcelona"
                    },
                    new CoordinatorModel()
                    {
                        Name="Ana Carretero"
                        , Email="anamaria.carretero@test.org"
                        , Province="Barcelona"
                    },
                };
            }
            else if (province.StartsWith("Madrid", StringComparison.CurrentCultureIgnoreCase))
            {
                return new CoordinatorModel[]
               {
                    new CoordinatorModel()
                    {
                        Name="Eva Sanchez",
                        Email= "eva.sanchez@test.org"
                    }
               };
            }
            else if (province.StartsWith("Gerona", StringComparison.CurrentCultureIgnoreCase))
            {
                return new CoordinatorModel[]
               {
                    new CoordinatorModel()
                    {
                        Name="Alberto Pulez",
                        Email= "alberto.pulez@test.org"
                    }
               };
            }
            else if (province.StartsWith("La Coruña", StringComparison.CurrentCultureIgnoreCase))
            {
                return new CoordinatorModel[]
               {
                    new CoordinatorModel()
                    {
                        Name="Victor Pulez",
                        Email= "Victor.pulez@test.org"
                    }
               };
            }
            else
                return new List<CoordinatorModel>();
        }
    }
}
