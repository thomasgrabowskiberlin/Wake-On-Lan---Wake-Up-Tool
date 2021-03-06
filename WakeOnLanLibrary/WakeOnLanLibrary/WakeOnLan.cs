using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WakeOnLanLibrary
{
    public class WakeOnLan
    {
        byte DiscardPortNo = 9;
        string BroadCastIP { get; set; }
        byte[] NIC = new byte[6];
        Socket sock;
        IPAddress serverAddr;
        // discard Port , and Broadcast send  x.x.x.255 
        IPEndPoint endPoint;

        public WakeOnLan(ref string refBCIP, ref byte[] refNIC)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            BroadCastIP = refBCIP;
            NIC = refNIC;
            if (refBCIP != null)
                serverAddr = IPAddress.Parse(BroadCastIP);
            if (refNIC != null)
            {
                endPoint = new IPEndPoint(serverAddr, DiscardPortNo);

            }
        }

        public WakeOnLan()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            BroadCastIP = "255.255.255.255";
            serverAddr = IPAddress.Parse(BroadCastIP);
            endPoint = new IPEndPoint(serverAddr, DiscardPortNo);
        }

        public void setNIC(byte[] vNIC)
        {
            if (vNIC != null)
                for (int i = 0; i < 6; i++)
                    NIC[i] = vNIC[i];

            endPoint.Address = serverAddr;
            endPoint.Port = DiscardPortNo;
        }

        public byte[] getNIC()
        {
            return NIC;
        }

        public void setBroadCastIP(string BCIP)
        {
            BroadCastIP = BCIP;
            if (BCIP != null)
                serverAddr = IPAddress.Parse(BroadCastIP);
        }
        public string getBroadCastIP()
        {
            return BroadCastIP;
        }

        public void SendMagicBytes()
        {
            try
            {
                byte[] sendbuffer2 = new byte[17 * 6];
                for (int i = 0; i < 6; i++)
                    sendbuffer2[i] = 0xFF;

                for (int i = 0; i < 16; i++)
                {
                    for (int ii = 0; ii < 6; ii++)
                        sendbuffer2[6 + (i * 6) + ii] = NIC[ii];
                }

                sock.SendTo(sendbuffer2, endPoint);
                MessageBox.Show("Frame Send", "Wake On Lan");
               
            }
            catch (Exception e)
            {
                // Do nothing
                Console.WriteLine("Error: ", e);
                MessageBox.Show("Error", e.ToString());
            }
        }

        public string GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string ipaddress = "127.0.0.1";
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipaddress = ip.ToString();
                }
            }
            return ipaddress;
        }

    }
}
