using System;

namespace ConsoleApp2
{
    class Program
    {
        private static readonly string _conn = "HostName=EC-WEB20-AJ.azure-devices.net;DeviceId=consoleapp;SharedAccessKey=FQFt0u9cyO0SEDTU7+zDQHAogBVuOPeag2O49QiDaPw=";
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_conn, TransportType.Mqtt);
        
        static void Main(string[] args)
        {
            DeviceService.SendMessageAsync(deviceClient).GetAwaiter();
            DeviceService.RecieveMessageAsync(deviceClient).GetAwaiter();

            Console.ReadKey();
        }
    }
}
