using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakeFW_Server_UI
{
    public static class WakeTarget
    {
        private static uint[] _defaultPorts = { 7, 9 };
        public static IPAddress GetBroadcastAddress(IPAddress ip , IPAddress mask)
        {
            byte[] broadcastIPBytes = new byte[4];
            byte[] hostBytes = ip.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();
            for (int i = 0; i < 4; i++)
            {
                broadcastIPBytes[i] = (byte)(hostBytes[i] | (byte)~maskBytes[i]);
            }
            return new IPAddress(broadcastIPBytes);
        }

        public static void WakeDevice(IPAddress ip, byte[] payload ,  List<uint>? ports , uint repetitions = 5)
        {
            if(ports == null)
            {
                ports = new List<uint>() {7,9};
            }

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            foreach (var port in ports)
            {

                // Declare destination host endpoint
                IPEndPoint EP = new(ip, (int)port);

                for (int i = 0; i < repetitions; i++)
                {
                    // Send data to destination host
                    s.SendTo(payload , EP);
                }

                Console.WriteLine();
                Console.WriteLine($"Magic packets ({repetitions}) sent to: {EP.Address.ToString}:{port}");
            }
            s.Dispose();
        }

        public static byte[] GeneratePayload(PhysicalAddress Mac)
        {
            byte[] GeneratedPayload = new byte[320];
            byte[] Magic = { byte.Parse("255"), byte.Parse("255"), byte.Parse("255"),
                byte.Parse("255"), byte.Parse("255"), byte.Parse("255") };
            byte[] MacBytes = Mac.GetAddressBytes();

            // Concatenate payload byte arrays
            System.Buffer.BlockCopy(Magic, 0, GeneratedPayload, 0, 6);
            for (int i = 1; i <= 16; i++)
            {
                System.Buffer.BlockCopy(MacBytes, 0, GeneratedPayload, 6 * i, 6);
            }

            return GeneratedPayload;
        }
    }
}
