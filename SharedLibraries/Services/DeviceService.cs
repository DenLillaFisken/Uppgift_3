﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharedLibraries.Models;

namespace SharedLibraries.Services
{
    public static class DeviceService
    {
        private static readonly Random rnd = new Random();

        //IoT device
        public static async Task SendMessageAsync(DeviceClient deviceClient)
        {
            while (true)
            {
                var data = new TemperatureModel
                {
                    Temperature = rnd.Next(20, 30),
                    Humidity = rnd.Next(30, 40)
                };

                var json = JsonConvert.SerializeObject(data);

                //det vi vill skicka kallas för payload
                //Message kommer från Microsoft.Azure.Devices.Client;
                //formatera meddlandet till UTF8. Omvandla till bytes så att datorn förstår.
                var payload = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(json));

                await deviceClient.SendEventAsync(payload);
                Console.WriteLine($"Message sent: {json}");

                await Task.Delay(60 * 1000);
            }
        }

        public static async Task RecieveMessageAsync(DeviceClient deviceClient)
        {
            while (true)
            {
                var payload = await deviceClient.ReceiveAsync();

                if (payload == null)
                {
                    continue;
                }
                Console.WriteLine($"Message recieved {Encoding.UTF8.GetString(payload.GetBytes())}");

                //tar bort meddelandet från hubben
                await deviceClient.CompleteAsync(payload);

            }
        }

        //är en Service(utför någonting, tex mobiltelefon) client = simulerar Iot Hub
        public static async Task SendMessageToDeviceAsync(ServiceClient serviceClient, string targetDevice, string message)
        {
            var payload = new Microsoft.Azure.Devices.Message(Encoding.UTF8.GetBytes(message));

            await serviceClient.SendAsync(targetDevice, payload);
        }
    }
}
