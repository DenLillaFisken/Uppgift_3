using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using SharedLibraries.Models;
using SharedLibraries.Services;


namespace ConsoleApp
{
    class Program
    {
        //connectionstring till iot-apparaten i Azure
        private static readonly string _conn = "HostName=EC-WEB20-AJ.azure-devices.net;DeviceId=consoleapp;SharedAccessKey=FQFt0u9cyO0SEDTU7+zDQHAogBVuOPeag2O49QiDaPw=";

        //Skapar en iot Device
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_conn, TransportType.Mqtt);
        static void Main(string[] args)
        {
            //get.Awaiter är istället för att ändra om Main-funtionen till async
            DeviceService.SendMessageAsync(deviceClient).GetAwaiter();

            DeviceService.RecieveMessageAsyn(deviceClient).GetAwaiter();

            Console.ReadKey();
        }
    }
}
