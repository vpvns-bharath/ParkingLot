using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotSimulation.Models
{
    public class Slot
    {
        public string SlotId { get; set; }
        public string VehicleType { get; set; }
        public bool HasVehicle { get; set; }

        public ParkingTicket ?Ticket { get; set; }

        public Slot(string slotId,string vehicleType,bool hasVehicle)
        {
            this.SlotId = slotId;
            this.VehicleType = vehicleType;
            this.HasVehicle = hasVehicle;
            this.Ticket = null;
        }


    }
}
