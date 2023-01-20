using ParkingLotSimulation.Interfaces;
using ParkingLotSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace ParkingLotSimulation.Services
{
    public class ParkingLotServiceV2:IService
    {
        public List<Slot> ParkingSlots { get; set; }

        public List<ParkingTicket> TicketHistory { get; set; }

        int twoWheelerSlots, fourWheelerSlots, heavyVehicleSlots;
        public enum VehicleType
        {
            TwoWheeler=1,
            FourWheeler,
            HeavyVehicle
        }

        public ParkingLotServiceV2()
        {
            ParkingSlots = new List<Slot>();
            TicketHistory = new List<ParkingTicket>();
          
        }

        public void Initialise(int twoWheelerSlots, int fourWheelerSlots, int heavyVehicleSlots)
        {
            this.twoWheelerSlots = twoWheelerSlots;
            this.fourWheelerSlots = fourWheelerSlots;
            this.heavyVehicleSlots = heavyVehicleSlots;
            AddSlots(this.twoWheelerSlots, VehicleType.TwoWheeler);
            AddSlots(this.fourWheelerSlots, VehicleType.FourWheeler);
            AddSlots(this.heavyVehicleSlots, VehicleType.HeavyVehicle);

        }

        public void AddSlots(int slots,VehicleType vtype)
        {
            for(int i=0; i<slots; i++) 
            {
                if(vtype.Equals(VehicleType.TwoWheeler))
                {
                    ParkingSlots.Add(new Slot($"SLTW{i}", vtype.ToString(), false));
                }
                else if(vtype.Equals(VehicleType.FourWheeler)) 
                {
                    ParkingSlots.Add(new Slot($"SLFW{i}", vtype.ToString(), false));
                }
                else
                {
                    ParkingSlots.Add(new Slot($"SLHV{i}", vtype.ToString(), false));
                }
            }
        }

        public void ParkingHelper(VehicleType vType)
        {
            if(!Enum.IsDefined(typeof(VehicleType),vType))
            {
                Console.WriteLine("Choose correct option!!\n");
            }
            else
            {
                Console.Write("Enter Your Vehicle Number: ");
                string? vehicleNumber = Console.ReadLine();
                if (vehicleNumber == null || !Regex.IsMatch(vehicleNumber, "^[A-Z]{2}\\s[0-9]{2}\\s[A-Z]{2}\\s[0-9]{4}$"))
                {
                    Console.WriteLine("Invalid Vehicle Number!\n");
                }
                else if (ParkingSlots.Where(slot => slot.HasVehicle == true && slot.Ticket?.VehicleNumber == vehicleNumber).ToList().Count > 0 ? true : false)
                {
                    Console.WriteLine("Vehicle Already Parked!\n");
                }
                else
                {
                    var slots = ParkingSlots.Where(slot => slot.VehicleType == vType.ToString() && slot.HasVehicle == false).ToList();
                    string? slotId = slots.Count == 0 ? null : slots.First().SlotId;
                    if (slotId == null)
                    {
                        Console.WriteLine("Parking is Full !!\n");
                    }
                    else
                    {
                        ParkingTicket ticket = new ParkingTicket("", vehicleNumber, slotId, "", "-");
                        ticket.InTime = DateTime.Now.ToString();
                        ticket.TicketId = $"TKT{vType}{ticket.InTime.Replace(" ", "")}";
                        Slot allocatedSlot = ParkingSlots.Where(slot => slot.SlotId == slotId).First();
                        allocatedSlot.Ticket = ticket;
                        allocatedSlot.HasVehicle = true;
                        Console.WriteLine("Vehicle Parked Successfully\n");
                        TicketHistory.Add(ticket);
                        Console.WriteLine("------PARKING TICKET-----\n");
                        Console.Write($"Ticket-ID :{allocatedSlot.Ticket.TicketId}\nVehicleNumber :{allocatedSlot.Ticket.VehicleNumber}\n" +
                            $"ParkingSlot :{allocatedSlot.Ticket.Slot}\nIn-Time :{allocatedSlot.Ticket.InTime}\nOut-Time :{allocatedSlot.Ticket.OutTime}\n" + "\n");
                        Console.WriteLine("-------------------------\n");
                    }
                }
            }
        }

        public void Parking()
        {
            Console.WriteLine("Select Vehicle Type:");
            Console.Write("1 -> 2-WheelerParking\n2 -> 4-WheelerParking\n3 -> HeavyWheelerParking\n");
            VehicleType vehicleChoice = (VehicleType)Enum.Parse(typeof(VehicleType),Console.ReadLine());
            ParkingHelper(vehicleChoice);
        }

        public int OccupancyHelper(VehicleType vType)
        {
           int filled = ParkingSlots.Where(slot=> slot.VehicleType== vType.ToString() && slot.HasVehicle==true).Count();
           return filled;
        }
        public void DisplayOccupancy()
        {
            Console.WriteLine("----- ParkingLot Occupancy -----");
            Console.WriteLine($"2-WheelerSlots:  Total Slots: {twoWheelerSlots}  Filled : {OccupancyHelper(VehicleType.TwoWheeler)}  Available : {twoWheelerSlots - OccupancyHelper(VehicleType.TwoWheeler)}");
            Console.WriteLine($"4-WheelerSlots:  Total Slots: {fourWheelerSlots}  Filled : {OccupancyHelper(VehicleType.FourWheeler)}  Available: {fourWheelerSlots - OccupancyHelper(VehicleType.FourWheeler)}");
            Console.WriteLine($"Heavy-VehicleSlots: Total Slots: {heavyVehicleSlots}  Filled : {OccupancyHelper(VehicleType.HeavyVehicle)}  Available: {fourWheelerSlots - OccupancyHelper(VehicleType.HeavyVehicle)}\n");

        }

        public void UnParking()
        {
            Console.Write("Enter Ticket Info: ");
            string ?info = Console.ReadLine();
            if (info == null) 
            {
                Console.WriteLine("Enter Valid Information!!\n");
            }
            else
            {
                bool isFound = false;
                foreach(var slot in ParkingSlots)
                {
                    if( slot.Ticket!=null &&(slot.Ticket.TicketId.Equals(info) || slot.Ticket.VehicleNumber.Equals(info) || slot.Ticket.Slot.Equals(info)))
                    {
                        UpdateLog(slot.Ticket.TicketId);
                        slot.HasVehicle = false;
                        slot.Ticket = null;
                        isFound= true;
                        break;
                    }
                    
                }
                if(isFound)
                {
                    Console.WriteLine("Vehicle unparked Successfully!\n");
                }
                else
                {
                    Console.WriteLine("Vehicle Not Found!\n");
                }
            }
        }

        public void UpdateLog(string ticketId)
        {
            
            ParkingTicket ticket = TicketHistory.Where(t => t.TicketId == ticketId).First();
            ticket.OutTime = DateTime.Now.ToString();
        }

        public void ViewTicketsHistory()
        {
            if (TicketHistory.Count == 0)
            {
                Console.WriteLine("No Tickets issued!\n");
            }
            else
            {
                foreach (ParkingTicket ticket in TicketHistory)
                {
                    Console.WriteLine("-------------------------------------------------------------------\n");
                    Console.WriteLine($"Ticket-ID :{ticket.TicketId}\nVehicleNumber :{ticket.VehicleNumber}\n" +
                        $"ParkingSlot :{ticket.Slot}\nIn-Time :{ticket.InTime}\nOut-Time :{ticket.OutTime}\n" + "\n");
                    Console.WriteLine("--------------------------------------------------------------------\n");
                }
            }
        }
    }
}
