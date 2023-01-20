using ParkingLotSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotSimulation.Interfaces
{
    public interface IService
    {

        List<Slot> ParkingSlots { get; set; }
        List<ParkingTicket> TicketHistory { get; set; }
        public void Initialise(int twoWheelerSlots, int fourWheelerSlots, int heavyVehicleSlots);

        void Parking();
        void DisplayOccupancy();
        void UnParking();
        void ViewTicketsHistory();
    }
}
