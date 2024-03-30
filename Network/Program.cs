using NetMQ;
using NetMQ.Sockets;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace Network
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server("Hello");
        }

        public static void Server(string name)
        {
            ResponseSocket server = new ResponseSocket("@tcp://*:12345");
            
            Console.WriteLine("Сервер ждет сообщение от клиента");
            var t = new Thread(WaitAnyKey);
            t.Start();


            while (true) 
            {
                string messageText = server.ReceiveFrameString();
                
                if (messageText != null)
                {
                    Message message = Message.DeserializeFromJson(messageText);
                    message.Print();
                    server.SendFrame("ok");
                }
            }
        }
        private static void WaitAnyKey()
        {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                System.Environment.Exit(0);
        }
    }
}