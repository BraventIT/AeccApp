using Aecc.Models;
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
                 },
                 new RequestType()
                 {
                     Source= RequestSourceEnum.Hospital
                     ,Name ="Acompañamiento a pacientes hospitalizados"
                 },
                new RequestType()
                 {
                     Source= RequestSourceEnum.Hospital
                     ,Name ="Acompañamiento a pacientes durante el tratamiento o en la realización de pruebas diagnósticas"
                 },
                new RequestType()
                 {
                     Source= RequestSourceEnum.Hospital
                     ,Name ="Suplencia puntual del familiar"
                 },
                new RequestType()
                 {
                     Source= RequestSourceEnum.Hospital
                     ,Name ="Apoyo en la gestión de citas y guía hospitalaria"
                 }
            };

            foreach (var r in requestTypes)
            {
                context.RequestTypes.Add(r);
            }

            context.SaveChanges();

            var hospitals = new Hospital[]
            {
new Hospital(){ Name = "Hospital Universitario, Txagorritxu", Province = "Álava/Araba", Street = "Cl. José Achotegui, s/n  01009 Vitoria-Gasteiz (Álava)" },
new Hospital(){ Name = "Hospital General Universitario Albacete", Province = "Albacete", Street = "Cl. Hermanos Falcó, 37  02006 Albacete" },
new Hospital(){ Name = "Hospital General, Hellín", Province = "Albacete", Street = "Cl. Juan Ramón Jiménez, s/n  02400 Hellín - Albacete" },
new Hospital(){ Name = "Hospital General U. de Elche", Province = "Alicante/Alacant", Street = "Pda. Horts i Molins s/n 03202 Elche Alicante" },
new Hospital(){ Name = "Hospital Vega Baja, Orihuela", Province = "Alicante/Alacant", Street = "Ctra.Orihuela-Almoradi, s/n" },
new Hospital(){ Name = "Hospital General Universitario de Alicante;Alicante/Alacant", Province = "C/ Maestro Alonso 109", Street = " 03010 ALICANTE" },
new Hospital(){ Name = "Hospital de la Marina Baixa, Villajoyosa", Province = "Alicante/Alacant", Street = "Av. Alcalde Jaume Botella Mayor 7, 03570-Villajoyosa" },
new Hospital(){ Name = "Hospital Universitario de San Juan de Alicante", Province = "Alicante/Alacant", Street = "Ctra. Alicante-Valencia, S/N   03550 Sant Joan D' Alacant Alicante" },
new Hospital(){ Name = "Hospital de Torrevieja", Province = "Alicante/Alacant", Street = "CV-95, Torrevieja" },
new Hospital(){ Name = "Hospital Marina Salud Denia", Province = "Alicante/Alacant", Street = "Partida Beniadlà, S/N 03700 Dénia" },
new Hospital(){ Name = "Hospital General Universitario Virgen de la Salud de Elda", Province = "Alicante/Alacant", Street = "Ctra. Sax S/N     03600 Elda Alicante" },
new Hospital(){ Name = "Hospital Comarcal La Inmaculada de Huércal-Overa", Province = "Almería", Street = "Avda. Dra Ana Parra, s/n" },
new Hospital(){ Name = "Hospital Torrecárdenas ", Province = "Almería", Street = "Paraje Torrecárdenas, S/N 04009 Almería" },
new Hospital(){ Name = "Hospital Cabueñes, Gijón", Province = "Asturias", Street = "Cabueñes S/N, 33394, Gijón" },
new Hospital(){ Name = "Hospital Provincial (Paliativos)", Province = "Ávila", Street = "CALLE JESUS DEL GRAN PODER, 39" },
new Hospital(){ Name = "Hospital Materno Infantil de Badajoz", Province = "Badajoz", Street = "Calle la Violeta, 4, 06010 Badajoz" },
new Hospital(){ Name = "Hospital Infanta Cristina", Province = "Badajoz", Street = "Avda. Elvas, s/n  06006 Badajoz" },
new Hospital(){ Name = "Hospital Comarcal Don Benito-Villanueva", Province = "Badajoz", Street = "CTRA. DON BENITO VILLANUEVA, S/N" },
new Hospital(){ Name = "Hospital Zafra", Province = "Badajoz", Street = "ANTIGUA CARRETERA N-432 DE BADAJOZ GRANADA S/N" },
new Hospital(){ Name = "Hospital Perpetuo Socorro", Province = "Badajoz", Street = "AV. DAMIAN TELLEZ LAFUENTE, S/N" },
new Hospital(){ Name = "Hospital Son Llàtzer, Palma de Mallorca", Province = "Baleares/Balears, Illes", Street = "Ctra. de Manacor, 07198 Palma" },
new Hospital(){ Name = "Hospital Universitario Son Espases", Province = "Baleares/Balears, Illes", Street = "Ctra. Valldemosa ,79 ---07120" },
new Hospital(){ Name = "Hospital de Igualada", Province = "Barcelona", Street = "Pº Mossen Jacint Verdager, 128  08700 Igualada - Barcelona" },
new Hospital(){ Name = "Fundació Hospital de l’Esperit Sant", Province = "Barcelona", Street = "Avenida Mossèn Pons i Rabadà, s/n, 08923 Santa Coloma de Gramenet, Barcelona" },
new Hospital(){ Name = "Centro Sociosanitario Mollet del Valles", Province = "Barcelona", Street = "Carrer de Sant Llorenç, 41, 08100 Mollet del Vallès, Barcelona" },
new Hospital(){ Name = "Nou Hospital Evangelic", Province = "Barcelona", Street = "Camèlies, 15-17, 08024 Barcelona" },
new Hospital(){ Name = "Hestia Palau", Province = "Barcelona", Street = "Carrer de Sant Antoni Maria Claret, 135, 08025 Barcelona" },
new Hospital(){ Name = "Hestia Duran i Reynals", Province = "Barcelona", Street = "Avinguda Granvia, 199-203, 08908 L'Hospitalet de Llobregat, Barcelona" },
new Hospital(){ Name = "Centro Sociosanitario Mutuam Gúell", Province = "Barcelona", Street = "C/ Mare de Deú de la Salut, 49" },
new Hospital(){ Name = "CSS Frederica Montseny", Province = "Barcelona", Street = "C/ D'ESTIDA, 6-8. 6, 08840 Viladecans" },
new Hospital(){ Name = "Centro Sociosanitario El Carme", Province = "Barcelona", Street = "Camí de Sant Jeroni de la Murtra 60, Badalona" },
new Hospital(){ Name = "Hospital General de l´Hospitalet", Province = "Barcelona", Street = "Avda. Josep Molins, 29-41 08906 - L´Hospitalet - Barcelona" },
new Hospital(){ Name = "Hospital de Sant Pau", Province = "Barcelona", Street = "Cl. Sant Antoni Maria Claret, 167   08025 Barcelona" },
new Hospital(){ Name = "Centro Sociosanitario Isabel Roig", Province = "Barcelona", Street = "C Fernando Pessoa 51 (Sant Andreu)" },
new Hospital(){ Name = "HOSPITAL UNIVERSITARIO DE BURGOS", Province = "BURGOS", Street = "AVDA. ISALAS BALEARES Nº3 09006 BURGOS" },
new Hospital(){ Name = "San Pedro de Alcántara", Province = "Cáceres", Street = "Avda. Pablo Naranjo, s/n 10003 Cáceres" },
new Hospital(){ Name = "Hospital Universitario de Puerto Real", Province = "Cádiz", Street = "Crta. Nacional IV KM 665 - 11510 Puerto Real Cádiz" },
new Hospital(){ Name = "Hospital Punta Europa, Algeciras", Province = "Cádiz", Street = "Ctra. Getares s/n  - 11207 Algeciras" },
new Hospital(){ Name = "Hospital Universitario Puerta del Mar", Province = "Cádiz", Street = "Avda. Ana de Viya, 21" },
new Hospital(){ Name = "Hospital Jerez de la Frontera", Province = "Cádiz", Street = "Carr. Madrid-Cádiz, 0, 11407.- Jerez de la Frontera (Cádiz)" },
new Hospital(){ Name = "Hospital Universitario de La Plana", Province = "Castellón/Castelló", Street = "Crta. Vila-real Burriana km 0,5. 12540 Vila-real Castellón" },
new Hospital(){ Name = "Hospital Insalud- Cuidados Paliativos", Province = "Ceuta", Street = "" },
new Hospital(){ Name = "Hospital Tomelloso", Province = "Ciudad Real", Street = "Vereda de Socuéllamos, s/n, 13700 Tomelloso, Cdad. Real" },
new Hospital(){ Name = "Complejo Hosp. La Mancha Centro Álcazar de S.Juan", Province = "Ciudad Real", Street = "Av. Constitución, 3, 13600 Alcázar de San Juan, Cdad. Real" },
new Hospital(){ Name = "Hospital Provincial Reina Sofía", Province = "Córdoba", Street = "Avda. Menéndez Pidal, s/n 14004 Córdoba" },
new Hospital(){ Name = "Hospital General y Materno Infantil de Reina Sofia", Province = "Córdoba", Street = "Avda. Menéndez Pidal, s/n 14004 Córdoba" },
new Hospital(){ Name = "Sociosanitario Bernat Jaume (Figueres)", Province = "Gerona/Girona", Street = "Carrer de Joaquim Cusí i Fortunet, 17600 Figueres" },
new Hospital(){ Name = "Santa Caterina", Province = "Gerona/Girona", Street = "C/ Dr. Castany s/n 17190 Salt Girona" },
new Hospital(){ Name = "Instituto Catalán de Oncología", Province = "Gerona/Girona", Street = "Avda de Francia s/n 17007 Girona" },
new Hospital(){ Name = "Hospital Universitario Josep Trueta", Province = "Gerona/Girona", Street = "Avda. de Francia, s/n 17007 Girona" },
new Hospital(){ Name = "Hospital Comarcal de Blanes", Province = "Gerona/Girona", Street = "Accés Cala Sant Frances-5  Blanes" },
new Hospital(){ Name = "Sociosanitario LLoret de Mar", Province = "Gerona/Girona", Street = "C/ Castell, 42  17310 Lloret de Mar - Girona" },
new Hospital(){ Name = "Hospital Palamós", Province = "Gerona/Girona", Street = "Carrer Hospital, 36, 17230 Palamós, Girona" },
new Hospital(){ Name = "Hospital Virgen de las Nieves. Materno Infantil", Province = "Granada", Street = "Av. Fuerzas Armadas, 2 18014 Granada" },
new Hospital(){ Name = "Hospital Comarcal de Baza", Province = "Granada", Street = "Carretera Murcia KM.175 18800 Baza - Granada" },
new Hospital(){ Name = "Hospital Clínico Universitario San Cecilio", Province = "Granada", Street = "Avda. Doctor Olóriz, 16 - 18012 Granada" },
new Hospital(){ Name = "Hospital Virgen de las Nieves - Ruiz de Alda", Province = "Granada", Street = "Av. Fuerzas Armadas, 2 18014 Granada" },
new Hospital(){ Name = "Hospital Comarcal Santa Ana de Motril", Province = "Granada", Street = "Avenida Enrique Martín Cuevas, s/n. 18600. Motril" },
new Hospital(){ Name = "Hospital Virgen de las Nieves. Oncoginecología", Province = "Granada", Street = "Av. Fuerzas Armadas 2, 18014 Granada" },
new Hospital(){ Name = "Hospital General Universitario", Province = "Guadalajara", Street = "Calle Donante de Sangre, s/n, 19002 Guadalajara" },
new Hospital(){ Name = "Onkologikoa", Province = "Guipúzcoa/Gipuzkoa", Street = "Ps Begiristain, 121  20014 Donostia - San Sebastián" },
new Hospital(){ Name = "Fundación Matía - UCP Hospitalización", Province = "Guipúzcoa/Gipuzkoa", Street = "Cº de los Pinos, 35 20018  Donostia - San Sebastián" },
new Hospital(){ Name = "Hospital Universitario Donostia", Province = "Guipúzcoa/Gipuzkoa", Street = "Ps  Doctor Begiristain s/n  20014 Donostia - San Sebastián" },
new Hospital(){ Name = "Hospital Riotinto", Province = "Huelva", Street = "Av. de la Esquila, 5, 21660 Minas de Riotinto, Huelva" },
new Hospital(){ Name = "Hospital Juan Ramón Jiménez", Province = "Huelva", Street = "Ronda Exterior Norte, s/n 21005 Huelva" },
new Hospital(){ Name = "Hospital Provincial ", Province = "Huesca", Street = "Pº Lucas Mallada, 22 22006 Huesca" },
new Hospital(){ Name = "Hospital de Barbastro", Province = "Huesca", Street = "Ctra. de Tarragona-San Sebastián, s/n    (N-240) 22300" },
new Hospital(){ Name = "Hospital Universitario Doctor Sagaz", Province = "Jaén", Street = "Monte el Neveral, 23071 Jaén" },
new Hospital(){ Name = "Hospital Universitario Médico-Quirúrgico", Province = "Jaén", Street = "Av. del Ejército Español, 10, 23007 Jaén" },
new Hospital(){ Name = "Hospital Abente y Lago", Province = "La Coruña/A Coruña", Street = " Paseo Parrote, s/n, 15006 A Coruña" },
new Hospital(){ Name = "Centro Oncológico de Galicia", Province = "La Coruña/A Coruña", Street = "Av/ Montserrat s/n" },
new Hospital(){ Name = "Hospital El Bierzo", Province = "León", Street = "Médicos sin fronteras, 7. 24400 Fuentes Nuevas, Ponferrada (León)" },
new Hospital(){ Name = "Sanitas La Zarzuela / La Moraleja", Province = "Madrid", Street = "C/ Pléyades 25 " },
new Hospital(){ Name = "HM Monteprincipe adultos", Province = "Madrid", Street = "Avda. Montepríncipe, 25 28660 Boadilla del Monte - Madrid" },
new Hospital(){ Name = "Hospital de Torrelodones", Province = "Madrid", Street = "Av. Castillo de Olivares, s/n 28250" },
new Hospital(){ Name = "Hospital Principe de Asturias, Alcalá de Henares", Province = "Madrid", Street = "Crta. Alcalá-Meco s/n - 28805 Alcalá de Henares - Madrid" },
new Hospital(){ Name = "Hospital Clínico San Carlos", Province = "Madrid", Street = "Cl. Profesor Martin Lagos, s/n 28040 Madrid" },
new Hospital(){ Name = "Hospital La Princesa", Province = "Madrid", Street = "Cl. Diego de León, 62  28006 Madrid" },
new Hospital(){ Name = "Hospital Universitario del Tajo", Province = "Madrid", Street = "Aranjuez" },
new Hospital(){ Name = "H.M Puerta del Sur", Province = "Madrid", Street = "Ave. carlos V 70. Móstoles" },
new Hospital(){ Name = "", Province = "Madrid", Street = "" },
new Hospital(){ Name = "Hospital Puerta de Hierro", Province = "Madrid", Street = "Manuel de Falla, 1. 28222 - Majadahonda " },
new Hospital(){ Name = "Hospital Rey Juan Carlos", Province = "Madrid", Street = "CALLE GLADIOLO S/N 28933 MÓSTOLES" },
new Hospital(){ Name = "HM Sanchinarro", Province = "Madrid", Street = "C/ OÑA, 10" },
new Hospital(){ Name = "Hospital Virgen de la Torre (Unidad de Cuidados Paliativos)", Province = "Madrid", Street = "C/ Puerto de Lumbreras, 5 28031 - Madrid " },
new Hospital(){ Name = "Hospital Universitario Infanta Leonor", Province = "Madrid", Street = "C/ Gran Vía del Este, 80 28031 - Madrid" },
new Hospital(){ Name = "Hospital Ramón y Cajal", Province = "Madrid", Street = "Ctra. Colmenar Viejo, km. 9,100, 28034 Madrid" },
new Hospital(){ Name = "Hospital Universitario Fuenlabrada", Province = "Madrid", Street = "C/ Camino del Molino, 2 " },
new Hospital(){ Name = "Hospital del Henares", Province = "Madrid", Street = "Avda. de Marie Curie, s/n " },
new Hospital(){ Name = "Hospital Universitario La Paz ", Province = "Madrid", Street = "Pº Castellana, 261 - 28046 Madrid" },
new Hospital(){ Name = "Hospital Central de la Defensa Gómez Ulla", Province = "Madrid", Street = "Glorieta del Ejército, 1" },
new Hospital(){ Name = "Hospital Universitario Infanta Cristina, Parla", Province = "Madrid", Street = "Avda. 9 de junio, 2" },
new Hospital(){ Name = "Fundación Jiménez Díaz", Province = "Madrid", Street = "Avda. Reyes Católicos, 2 " },
new Hospital(){ Name = "Hospital del Sureste Arganda", Province = "Madrid", Street = "Ronda del Sur, 10, 28500 Arganda del Rey, Madrid" },
new Hospital(){ Name = "Hospital Universitario Infanta Elena, Valdemoro", Province = "Madrid", Street = "Avda. Reyes Católicos, 21. 28040 Valdemoro" },
new Hospital(){ Name = "", Province = "Madrid", Street = "" },
new Hospital(){ Name = "Hospital General de Villalba", Province = "Madrid", Street = "M-608, Km. 41, 28400 Collado Villalba, Madrid" },
new Hospital(){ Name = "Hospital Universitario Fundación Alcorcón", Province = "Madrid", Street = "C/Budapest, 1. Alcorcón 28922. Madrid. " },
new Hospital(){ Name = "Hospital Universitario de Getafe", Province = "Madrid", Street = "Carretera de Toledo, km12,500  " },
new Hospital(){ Name = "Hospital Universitario Infanta Sofía", Province = "Madrid", Street = "Paseo de Europa, 34, 28703 San Sebastián de los Reyes, Madrid" },
new Hospital(){ Name = "Hospital de Torrejón", Province = "Madrid", Street = "C/ Mateo Inurria s/n Torrejón de Ardoz" },
new Hospital(){ Name = "Madrid Monteprincipe Oncohematologia Pediátrica", Province = "Madrid", Street = "Av. de Montepríncipe, 25, 28660 Boadilla del Monte, Madrid" },
new Hospital(){ Name = "Hospital 12 de Octubre (Niños)", Province = "Madrid", Street = "Crta. Andalucia km. 5,4  28041 Madrid" },
new Hospital(){ Name = "Hospital 12 de Octubre (Adultos)", Province = "Madrid", Street = "Crta. Andalucia km. 5,4  28041 Madrid" },
new Hospital(){ Name = "Hospital General Universitario Gregorio Marañón ", Province = "Madrid", Street = "C/Dr. Esquerdo, 46  28007 Madrid" },
new Hospital(){ Name = "Hospital Universitario Quirónsalud Madrid", Province = "Madrid", Street = "C/ Diego de Velázquez, 1. 28223 Pozuelo de Alarcón, Madrid. España" },
new Hospital(){ Name = "Hospital Carlos Haya", Province = "Málaga", Street = "Av. Carlos Haya, s/n 29010 Málaga" },
new Hospital(){ Name = "Materno Infantil de Málaga", Province = "Málaga", Street = "Av. Arroyo de los Angeles, s/n 29011 Málaga" },
new Hospital(){ Name = "Materno Infantil de Málaga", Province = "Málaga", Street = "Arroyo de los Ángeles s/n" },
new Hospital(){ Name = "Hospital Costa del Sol", Province = "Málaga", Street = "Autovía A-7 km 187 29603 Marbella" },
new Hospital(){ Name = "Hospital Universitario Santa Lucía (Cartagena)", Province = "Murcia", Street = "C/ Mezquita - Paraje Los Arcos,302020. Santa Lucia" },
new Hospital(){ Name = "Hospital Universitario Virgen de La Arrixaca", Province = "Murcia", Street = "Ctra. Madrid-Cartagena, s/n. (El Palmar)  Murcia" },
new Hospital(){ Name = "Complejo Hospitalario de Navarra", Province = "Navarra", Street = "Calle Irunlarrea 3, 31008 Pamplona" },
new Hospital(){ Name = "clínica Universidad de Navarra", Province = "Navarra", Street = "Avenida Pio XII, 36" },
new Hospital(){ Name = "Hospital Comarcal del Barco de Valdeorras", Province = "Orense/Ourense", Street = "C/Rua Conde Fenosa, 52, 32300 O Barco" },
new Hospital(){ Name = "Hospital Rio Carrión", Province = "Palencia", Street = "Avda. Donantes de Sangre s/n - Crta. Villamuriel de Cerrato 34006 Palencia" },
new Hospital(){ Name = "Hospital Universitario de Gran Canaria Dr. Negrín", Province = "Palmas, Las", Street = "Barranco de La Ballena s/n" },
new Hospital(){ Name = "Complejo Hospitalario Universitario Insular-Materno Infantil", Province = "Palmas, Las", Street = "Avda. Maritima" },
new Hospital(){ Name = "Hospital Provincial", Province = "Pontevedra", Street = "Rua Loureiro Crespo, 2  36001 Pontevedra" },
new Hospital(){ Name = "Hospital do Meixoeiro", Province = "Pontevedra", Street = "Meixoeiro, s/n, 36200 Vigo, Pontevedra" },
new Hospital(){ Name = "Hospital Nicolás Peña, Vigo", Province = "Pontevedra", Street = "Avda. Camelias, 109  36211 Vigo - Pontevedra" },
new Hospital(){ Name = "Hospital Universitario Nuestra Señora de la Candelaria", Province = "Santa Cruz de Tenerife", Street = "Carretera del Rosario, s/n 38010 Santa Cruz de TenerifeTenerife" },
new Hospital(){ Name = "Hospital Universitario de Canarias", Province = "Santa Cruz de Tenerife", Street = "Carretera General La Cuesta – Taco, s/n. 38320 San Cristóbal de La Laguna. Tenerife" },
new Hospital(){ Name = "Hospital General de La Palma", Province = "Santa Cruz de Tenerife", Street = "Buenavista de arriba, s/n. 38713 Breña Alta" },
new Hospital(){ Name = "Hospital Universitario Virgen del Rocío", Province = "Sevilla", Street = "Avda. Manuel Siurot, s/n 41013 Sevilla" },
new Hospital(){ Name = "Hospital de La Merced (Osuna)", Province = "Sevilla", Street = "Avda. Constitución,2  - 41640 Osuna - Sevilla" },
new Hospital(){ Name = "Hospital Universitario del Valme", Province = "Sevilla", Street = "Crta. de Cádiz km. 548,9 - 41014 Sevilla" },
new Hospital(){ Name = "Hospital Universitario Virgen Macarena", Province = "Sevilla", Street = "Avda. Dr. Fedriani, 3  47017 Sevilla" },
new Hospital(){ Name = "Hospital El Tomillar", Province = "Sevilla", Street = "Crta. Alcalá-Dos Hermanas km.6 - 41700 Dos Hermanas - Sevilla" },
new Hospital(){ Name = "Complejo Asistencial Santa Bárbara", Province = "Soria", Street = "Pº de Santa Bárbara, s/n, 42005 Soria" },
new Hospital(){ Name = "Hospital San José", Province = "Teruel", Street = "Avda. Zaragoza, 16 -  44001 Teruel" },
new Hospital(){ Name = "Hospital Comarcal de Alcañiz", Province = "Teruel", Street = "Cl. Joaquín Repollés, s/n -  44600 - Alcañiz" },
new Hospital(){ Name = "Hospital Virgen del Prado, Talavera de la Reina", Province = "Toledo", Street = "Hospital Virgen del Prado Talavera de la Reina - Ctra.Nal. V - Km.114 - 45600 Talavera de la Reina" },
new Hospital(){ Name = "Hospital Clinico Universitario", Province = "Valencia/València", Street = "Avda.Blasco Ibañez, 17 46010 Valencia" },
new Hospital(){ Name = "Hospital La Fe", Province = "Valencia/València", Street = "Hospital Universitario La Fe Avda de Fernando Abril Martorell, n.106 46026 " },
new Hospital(){ Name = "Hospital Universitario Rio Hortega", Province = "Valladolid", Street = "C/Dulzaineros sn." },
new Hospital(){ Name = "Hospital de Galdakao-Usansolo", Province = "Vizcaya/Bizkaia", Street = "Labeaga Auzoa S/N, 48960 Galdakao " },
new Hospital(){ Name = "Clínica Victoria Eugenia ", Province = "Vizcaya/Bizkaia", Street = "Cl. Alameda Urquijo, 65" },
new Hospital(){ Name = "Hospital de Cruces ", Province = "Vizcaya/Bizkaia", Street = "Plaza de Cruces s/n. 48903 Cruces/Barakaldo - Vizcaya" },
new Hospital(){ Name = "Hospital Santa Marina", Province = "Vizcaya/Bizkaia", Street = "Cl. Errepidea, 41 - 48004 Bilbao" },
new Hospital(){ Name = "Hospital de Gorliz", Province = "Vizcaya/Bizkaia", Street = "Plaza Astondo Ibiltoki, 2 - 48630 Gorliz - Vizcaya" },
new Hospital(){ Name = "Hosp. Prov. Rodríguez Chamorro", Province = "Zamora", Street = "Cl. Hernán Cortés, 40 - 49021-Zamora" },
new Hospital(){ Name = "Hospital Virgen de la Concha", Province = "Zamora", Street = "Avda. Requejo, 35 - 49022 Zamora" },
new Hospital(){ Name = "Hosp. Comarcal de Benavente", Province = "Zamora", Street = "Pasaje San Nicolás, 4 - 49600 Benavente" },
new Hospital(){ Name = "Hospital Clínico Universitario Lozano Blesa", Province = "Zaragoza", Street = "Avenida San Juan Bosco 15" },
new Hospital(){ Name = "Miguel Servet", Province = "Zaragoza", Street = "Isabel La C atólina, 2  " },
new Hospital(){ Name = "Hospital Royo Villanova", Province = "Zaragoza", Street = "Avd/ San Gregorio s/n 50015 Zaragoza" },
new Hospital(){ Name = "Hospital Ernest Lluch, Calatayud", Province = "Zaragoza", Street = "A-2, s/n, 50299 Calatayud, Zaragoza" },
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
                      RequestSource= RequestSourceEnum.Hospital
                }
            };

            foreach (var c in coordinators)
            {
                context.Coordinators.Add(c);
            }

            context.SaveChanges();

            var hospitalAssignments = new HospitalAssignment[]
            {
                new HospitalAssignment()
                { CoordinatorID=1, HospitalID=1 }
            };

            foreach (var e in hospitalAssignments)
            {
                context.HospitalAssignments.Add(e);
            }

            context.SaveChanges();
        }
    }
}
