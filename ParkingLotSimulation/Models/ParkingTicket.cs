using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotSimulation.Models
{
    public class ParkingTicket
    {
        public string TicketId { get; set; }
        public string VehicleNumber { get; set; }
        public string Slot { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public ParkingTicket(string TicketId,string VehicleNumber,string Slot,string InTime,string OutTime) 
        {
            this.TicketId = TicketId;
            this.VehicleNumber = VehicleNumber;
            this.Slot= Slot;
            this.InTime = InTime;
            this.OutTime = OutTime;
        }
    }
}
