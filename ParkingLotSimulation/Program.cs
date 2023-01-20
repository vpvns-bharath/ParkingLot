using ParkingLotSimulation.Services;
using System;
using ParkingLotSimulation.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ParkingLotSimulation
{
    public class Program
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IService, ParkingLotServiceV2>();
            services.AddSingleton<ParkingLotSimulator>();
        }
        public static void Main(string[] args) 
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                ParkingLotSimulator ?parkingLotSimulator = serviceProvider.GetService<ParkingLotSimulator>();
                //ParkingLotServiceV2 parkingLotServiceV2 = serviceProvider.GetService<ParkingLotServiceV2>();
/*                ParkingLotSimulator parkingLotSimulator2 = new ParkingLotSimulator(serviceProvider.GetService<ParkingLotServiceV2>());*/                
                parkingLotSimulator?.Simulate();
            }
            
            
        }
    }
}