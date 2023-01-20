using ParkingLotSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotSimulation.Services
{
    public class ParkingLotServiceV1
    {
        List<ParkingTicket> ParkingLot { get; set; }

        List<ParkingTicket> TicketsLog { get; set; }


        public enum VehicleType
        {
            TwoWheeler,
            FourWheeler,
            HeavyVehicle
        }

        //two -> 0 to m-1 four -> m+n to m+n-1 heavy m+n -> m+n+o-1 

        int twoWheelerSlots,fourWheelerSlots,heavyVehicleSlots;

        System.Timers.Timer timer = new System.Timers.Timer();

        public ParkingLotServiceV1(int twoWheelerSlots,int fourWheelerSlots,int heavyVehicleSlots) 
        {
            this.twoWheelerSlots= twoWheelerSlots;
            this.fourWheelerSlots= fourWheelerSlots;
            this.heavyVehicleSlots= heavyVehicleSlots;
            ParkingLot = new List<ParkingTicket>(new ParkingTicket[twoWheelerSlots + fourWheelerSlots + heavyVehicleSlots]);
            TicketsLog = new List<ParkingTicket>();
            timer.Interval= 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(AutomateUnParking);
            timer.Start();
            AutomateUnParking(timer, null);
        }

        public int SlotHelper(int start,int end)
        {
            int slot;
            for (slot = start; slot <= end; slot++)
            {
                if (ParkingLot[slot] == null)
                {
                    break;
                }
            }

            if (slot == end + 1)
                return -1;
            return slot;

        }
        public int ProvideSlot(VehicleType vType)
        {
            int slot = 0;
            switch(vType)
            {
                case VehicleType.TwoWheeler:
                    slot = SlotHelper(0, twoWheelerSlots - 1);
                    break;
                case VehicleType.FourWheeler:
                    slot = SlotHelper(twoWheelerSlots,twoWheelerSlots+fourWheelerSlots - 1);
                    break;
                case VehicleType.HeavyVehicle:
                    slot = SlotHelper(twoWheelerSlots+fourWheelerSlots, twoWheelerSlots + fourWheelerSlots + heavyVehicleSlots - 1) ;
                    break;
            }    
            return slot;
        } 
        public void ParkingHelper(VehicleType vType)
        {
            Console.Write("Enter your vehicle number: ");
            string?vehicleNumber = Console.ReadLine();
            if(vehicleNumber == null)
            {
                Console.WriteLine("Invalid vehicle number\n");
            }
            else
            {
                int slot = ProvideSlot(vType);
                string ticketId = "TKT" + vType+slot.ToString();
                string inTime = DateTime.Now.ToString();
                string outTime = DateTime.Now.AddMinutes(1).ToString();
                if(slot== -1)
                {
                    Console.WriteLine("Parking is Full!\n");
                }
                else
                {
                    var ticket = new ParkingTicket(ticketId, vehicleNumber, slot.ToString(), inTime, outTime);
                    ParkingLot[slot] = ticket;
                    Console.WriteLine(" Vehicle Parked Successfully!!\n");
                    Console.WriteLine("------PARKING TICKET-----\n");
                    Console.Write($"Ticket-ID :{ParkingLot[slot].TicketId}\nVehicleNumber :{ParkingLot[slot].VehicleNumber}\n" +
                        $"ParkingSlot :{ParkingLot[slot].Slot}\nIn-Time :{ParkingLot[slot].InTime}\nOut-Time :{ParkingLot[slot].OutTime}\n"+"\n");
                    TicketsLog.Add(ticket);
                }
            }
        }
        public void Parking()
        {
            Console.WriteLine("Select Vehicle Type:");
            Console.Write("1 -> 2-WheelerParking\n2 -> 4-WheelerParking\n3 -> HeavyWheelerParking\n");
            int vehicleChoice = Convert.ToInt32(Console.ReadLine());
            switch(vehicleChoice)
            {
                case 1:
                    ParkingHelper(VehicleType.TwoWheeler); 
                    break;

                case 2:
                    ParkingHelper(VehicleType.FourWheeler);
                    break;

                case 3:
                    ParkingHelper(VehicleType.HeavyVehicle);
                    break;
            }
        }

        public int OccupancyHelper(int start,int end)
        {
            int filled = 0;
            for(int i=start; i<=end; i++) 
            {
                if (ParkingLot[i]!=null)
                {
                    filled++;
                }
            }
            return filled;
        }
        public void DisplayOccupancy()
        {
            Console.WriteLine("----- ParkingLot Occupancy -----");
            Console.WriteLine($"2-WheelerSlots:  Total Slots: {twoWheelerSlots} Available : {twoWheelerSlots - OccupancyHelper(0,twoWheelerSlots-1)}");
            Console.WriteLine($"4-WheelerSlots:  Total Slots: {fourWheelerSlots} Available: {fourWheelerSlots - OccupancyHelper(twoWheelerSlots, twoWheelerSlots+fourWheelerSlots - 1)}");
            Console.WriteLine($"Heavy-VehicleSlots: Total Slots: {heavyVehicleSlots} Available: {fourWheelerSlots - OccupancyHelper(twoWheelerSlots + fourWheelerSlots, twoWheelerSlots + fourWheelerSlots + heavyVehicleSlots - 1)}"+"\n");
        }

        public void UnParking() 
        {
            Console.Write("Enter ticket property: ");
            string?property = Console.ReadLine();
            if( property == "" || property == null)
            {
                Console.WriteLine("Invalid ticket!\n");
            }
            else
            {
                int i;
                for(i=0;i<ParkingLot.Count;i++)
                {
                    if (ParkingLot[i] == null) 
                    {
                        continue;    
                    }
                    else
                    {
                        if (ParkingLot[i].TicketId.Equals(property) || ParkingLot[i].VehicleNumber.Equals(property))
                            break;
                    }
                }

                if (i == ParkingLot.Count)
                {
                    Console.WriteLine("Vehicle Not Found!\n");
                }
                else
                {
                    ParkingLot[i] = null;
                    Console.WriteLine("Vehicle unparked!\n");
                }
            }

        }

        public void AutomateUnParking(object sender, System.Timers.ElapsedEventArgs e)
        {
            string time = DateTime.Now.ToString();
            int i;
            for(i=0;i<ParkingLot.Count;i++)
            {

                if (ParkingLot[i]!=null)
                {
                    if (time.CompareTo(ParkingLot[i].OutTime) > 0)
                    {
                        break;
                    }
                }
                
            }

            if (i != ParkingLot.Count)
            {
                Console.WriteLine($"\nVehicle Unparked....!! from slot {i} vehicle number : {ParkingLot[i].VehicleNumber}");
                ParkingLot[i] = null;
               
            }
        }

        public void ViewTicketsHistory()
        {
            if(TicketsLog.Count==0)
            {
                Console.WriteLine("No Tickets issued");
            }
            else
            {
                foreach(ParkingTicket ticket in TicketsLog) 
                {
                    Console.WriteLine($"Ticket-ID :{ticket.TicketId}\nVehicleNumber :{ticket.VehicleNumber}\n" +
                        $"ParkingSlot :{ticket.Slot}\nIn-Time :{ticket.InTime}\nOut-Time :{ticket.OutTime}\n" + "\n");
                }
            }
        }
  
        
    }
}
