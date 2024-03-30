using NetMQ;
using NetMQ.Sockets;
using Network;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {


            SentMessage(args[0], args[1]);
        }


        public static void SentMessage(string From, string ip)
        {
            RequestSocket client = new RequestSocket(">tcp://" + ip + ":12345");

            bool isRecived = false;
            while (true)
            {
                string messageText;
                do
                {
                    Console.Clear();
                    if (isRecived)
                    {
                        Console.WriteLine("Cообщение доставлено!");
                        isRecived = false;
                    }
                    Console.WriteLine("Введите сообщение.");
                    messageText = Console.ReadLine();

                }
                while (string.IsNullOrEmpty(messageText));
                if (messageText == "Exit")
                    break;
                Message message = new Message() { Text = messageText, NicknameFrom = From, NicknameTo = "Server", DateTime = DateTime.Now };
                string json = message.SerializeMessageToJson();

                client.SendFrame(json);
                if(client.ReceiveFrameString() == "ok")
                    isRecived = true;
            }

            client.Close();
        }
    }
}