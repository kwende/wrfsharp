using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Helpers.Process
{
    public class ProcessLock : IDisposable
    {
        Socket _socket = null; 

        private ProcessLock()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 666);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(localEndPoint);
            _socket.Listen(10); 
        }

        public static ProcessLock TryLock()
        {
            try
            {
                return new ProcessLock(); 
            }
            catch(System.Net.Sockets.SocketException)
            {
                return null; 
            }
        }

        ~ProcessLock()
        {
            InnerDispose(true); 
        }

        private void InnerDispose(bool fromGC)
        {
            if(!fromGC)
            {
                GC.SuppressFinalize(this); 
            }
            if(_socket != null)
            {
                _socket.Close(); 
            }
        }

        public void Dispose()
        {
            InnerDispose(false); 
        }
    }
}
