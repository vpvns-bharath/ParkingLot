using ParkingLotSimulation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingLotSimulation.Interfaces;

namespace ParkingLotSimulation
{
    public class ParkingLotSimulator
    {
        int twoWheelerSlots, fourWheelerSlots, heavyVehicleSlots;
        public IService parkingLotServicesv2 { get; set; }
        public ParkingLotSimulator(IService parkingLotServicev2)
        {
            this.parkingLotServicesv2 = parkingLotServicev2;
        }
        public void Simulate()
        {
            bool Select = true;
            Console.Write("Enter 2-wheeler Slots:");
            twoWheelerSlots = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter 4-wheeler Slots:");
            fourWheelerSlots = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Heavy-Vehicle Slots:");
            heavyVehicleSlots = Convert.ToInt32(Console.ReadLine());
            this.parkingLotServicesv2.Initialise(twoWheelerSlots,fourWheelerSlots,heavyVehicleSlots);
            /*ParkingLotServiceV1 parkingLotServicesv1 = new ParkingLotServiceV1(twoWheelerSlots,fourWheelerSlots, heavyVehicleSlots);*/

            while (Select)
            {
                Console.WriteLine("\n----Available Operations----");
                Console.WriteLine("1 -> Park Vehicle");
                Console.WriteLine("2 -> Check ParkingLot Current Occupancy");
                Console.WriteLine("3 -> UnPark Vehicle");
                Console.WriteLine("4 -> View Tickets Log ");
                Console.WriteLine("5 -> Exit\n");

                Console.Write("Choose the Option:");
                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        /*parkingLotServicesv1.Parking();*/
                        parkingLotServicesv2.Parking();
                        break;

                    case 2:
                        /*parkingLotServicesv1.DisplayOccupancy();*/
                        parkingLotServicesv2.DisplayOccupancy();
                        break;

                    case 3:
                        /*parkingLotServicesv1.UnParking();*/
                        parkingLotServicesv2.UnParking();
                        break;

                    case 4:
                        /*parkingLotServicesv1.ViewTicketsHistory();*/
                        parkingLotServicesv2.ViewTicketsHistory();
                        break;


                    case 5:
                        Console.WriteLine("---- Terminated ----");
                        Select = false;
                        break;

                }
            }
        }
    }
}
