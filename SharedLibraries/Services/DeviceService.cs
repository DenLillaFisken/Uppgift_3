using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharedLibraries.Models;

namespace SharedLibraries.Services
{
    //hela klassen är static, vilket innebär att allt i klassen också måste vara static
    public static class DeviceService
    {
        //Kan inte byta ut Random-typen. Fastlåst. ett Random-objekt
        private static readonly Random rnd = new Random();

        
        //funtion som sänger ett asyncront meddelande 
        //DeviceClient kommer från Microsoft.Azure.Devices.Client;

        //är en device client = IoT device
        public static async Task SendMessageAsync(DeviceClient deviceClient)
        {

            //evighetsloop
            while (true)
            {
                //Skapar ett nytt Temperatur-objekt
                var data = new TemperatureModel
                {
                    Temperature = rnd.Next(20, 30),
                    Humidity = rnd.Next(30, 40)
                };

                //formatera om objektet ovan till ett Json-objekt
                //alla kan läsa json, därför är det bra att konvertera till json-format
                var json = JsonConvert.SerializeObject(data);

                //det vi vill skicka kallas för payload
                //Message kommer från Microsoft.Azure.Devices.Client;
                //formatera meddlandet till UTF8. Omvandla till bytes så att datorn förstår. SKickas på nätet genom 1or och 0or
                var payload = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(json));

                //ALLTID när det står async med måste det vara en await!!
                await deviceClient.SendEventAsync(payload);

                Console.WriteLine($"Message sent: {json}");

                //vänta med svaret i x-antal sekunder
                await Task.Delay(5 * 1000);
            }

        }

        //är en device client = IoT device
        public static async Task RecieveMessageAsyn(DeviceClient deviceClient)
        {
            while (true)
            {
                var payload = await deviceClient.ReceiveAsync();

                if(payload == null)
                {
                    continue;
                }

                Console.WriteLine($"Message recieved {Encoding.UTF8.GetString(payload.GetBytes())}");

                //tar bort meddelandet från hubben
                await deviceClient.CompleteAsync(payload);

            }
        }

        //är en Service(utför någonting, tex mobiltelefon) client = simulerar Ioy Hub
        public static async Task SendMessageToDeviceAsync(ServiceClient serviceClient, string targetDevice, string message)
        {
            var payload = new Microsoft.Azure.Devices.Message(Encoding.UTF8.GetBytes(message));

            await serviceClient.SendAsync(targetDevice, payload);
        }
    }
}
