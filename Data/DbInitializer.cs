using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShowRoom.Models;

namespace CarShowRoom.Data
{
    public class DbInitializer
    {
        public static void Initialize(ShowRoomContext context)
        {
            context.Database.EnsureCreated();
            if (context.Cars.Any())
            {
                return; // BD a fost creata anterior
            }
            var cars = new Car[]
            {
 new Car{Brand="Toyota",Model="Rush S",Price=Decimal.Parse("50000")},
 new Car{Brand="Bugatti",Model="Veyron",Price=Decimal.Parse("60000")},
 new Car{Brand="McLaren",Model="P1",Price=Decimal.Parse("90000")},
 new Car{Brand="Tesla",Model="S ",Price=Decimal.Parse("97000")},
 new Car{Brand="Dacia",Model="Logan",Price=Decimal.Parse("10000")},
 new Car{Brand="Volkswagen",Model="Golf",Price=Decimal.Parse("5000")},
            };
            foreach (Car b in cars)
            {
                context.Cars.Add(b);
            }
            context.SaveChanges();
            var clients = new Client[]
            {

 new Client{ClientID=1050,Name="Bill Gates",BirthDate=DateTime.Parse("1959-04-02")},
 new Client{ClientID=1045,Name="Robert MTK",BirthDate=DateTime.Parse("1970-03-07")},

            };
            foreach (Client c in clients)
            {
                context.Clients.Add(c);
            }
            context.SaveChanges();
            var commands = new Command[]
            {
 new Command{CarID=1,ClientID=1050,CommandDate=DateTime.Parse("02-25-2020")},
 new Command{CarID=3,ClientID=1045,CommandDate=DateTime.Parse("09-28-2020")},
 new Command{CarID=1,ClientID=1045,CommandDate=DateTime.Parse("10-28-2020")},
 new Command{CarID=2,ClientID=1050,CommandDate=DateTime.Parse("09-28-2020")},
 new Command{CarID=4,ClientID=1050,CommandDate=DateTime.Parse("09-28-2020")},
 new Command{CarID=6,ClientID=1050,CommandDate=DateTime.Parse("10-28-2020")},
 };
            foreach (Command e in commands)
            {
                context.Commands.Add(e);
            }
            context.SaveChanges();
            var mechanics = new Mechanic[]
            {

 new Mechanic{MechanicName="Ilie",Adress="Str. Opincarilor, nr. 20, Craiova"},
 new Mechanic{MechanicName="Vasile",Adress="Str. Libertății, nr. 777, Caracal"},
 new Mechanic{MechanicName="Aurel",Adress="Str. Tunetelor, nr.55, Tăuți"},
            };
            foreach (Mechanic p in mechanics)
            {
                context.Mechanics.Add(p);
            }
            context.SaveChanges();
            var assembledCars = new AssembledCar[]
            {
 new AssembledCar {
 CarID = cars.Single(c => c.Brand == "Toyota" ).ID,
 MechanicID = mechanics.Single(i => i.MechanicName ==
"Ilie").ID
 },
 new AssembledCar {
 CarID = cars.Single(c => c.Brand == "Bugatti" ).ID,
MechanicID = mechanics.Single(i => i.MechanicName ==
"Ilie").ID
 },
 new AssembledCar {
 CarID = cars.Single(c => c.Brand == "McLaren" ).ID,
 MechanicID = mechanics.Single(i => i.MechanicName ==
"Vasile").ID
 },
 new AssembledCar {
 CarID = cars.Single(c => c.Brand == "Tesla" ).ID,
MechanicID = mechanics.Single(i => i.MechanicName == "Aurel").ID
 },
 new AssembledCar {
 CarID = cars.Single(c => c.Brand == "Dacia" ).ID,
MechanicID = mechanics.Single(i => i.MechanicName == "Aurel").ID
 },
 new AssembledCar {
 CarID = cars.Single(c => c.Brand == "Volkswagen" ).ID,
 MechanicID = mechanics.Single(i => i.MechanicName == "Aurel").ID
 },
            };
            foreach (AssembledCar pb in assembledCars)
            {
                context.AssembledCars.Add(pb);
            }
            context.SaveChanges();
        }
    }
}