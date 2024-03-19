using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UdemyUDPBroadcastSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(10, 50);
            // khởi tạo 1 socket với kiểu dữ liệu truyền gói tin Dgram, giao thức udp
            Socket sockBroadcast = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //tại đây thì cho phép socket gửi các gói tin broadcast
            sockBroadcast.EnableBroadcast = true;
            //ipendpoint là điểm cuối của gói tin, trong trường hợp này điểm cuối sẽ là địa chỉ broadcast
            IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 23000);
            // tạo mảng dữ liệu muốn truyền qua mạng
            byte[] broadcastBuffer = new byte[] { 0x0D, 0x0A };
            string strUserInput = string.Empty;

            IPEndPoint ipepSender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint epSender = (EndPoint)ipepSender;
            int nCountReceived = 0;
            string textReceived = string.Empty;
            try
            {
                sockBroadcast.Bind(new IPEndPoint(IPAddress.Any, 0));
                while (true)
                {
                    Console.WriteLine("Please input a string to send broadcast, type <EXIT> to close.");
                    strUserInput = Console.ReadLine();
                    if (strUserInput == "<EXIT>") break;

                    broadcastBuffer = Encoding.ASCII.GetBytes(strUserInput);

                    //các gói tin trong socket sẽ gửi gói tin broadcastBuffer với địa chỉ ip là broadcastEP
                    sockBroadcast.SendTo(broadcastBuffer, broadcastEP);

                    if (strUserInput.Equals("<ECHO>"))
                    {
                        //hàm nhận được bao nhiêu dữ liệu thì sẽ đếm và show ra bảng console
                        nCountReceived = sockBroadcast.ReceiveFrom(broadcastBuffer, ref epSender);
                        Console.WriteLine("Number of bytes data: " + nCountReceived);
                        //sau khi nhận dữ liệu vào buffer thì sẽ getstring để chuyển từ byte sang string và xuất ra console
                        textReceived = Encoding.ASCII.GetString(broadcastBuffer, 0, nCountReceived);
                        Console.WriteLine("Text Received: " + textReceived);
                        //xuất thông tin sender
                        Console.WriteLine("Send from: " + epSender.ToString());

                    }
                }
                // khi gọi hàm shutdown thif đòng nghĩa sẽ đóng socket lại việc nhận(receive) và gửi (send). Ở đây chọn both(cả hai)
                sockBroadcast.Shutdown(SocketShutdown.Both);
                // đóng kết nối socket và giải phóng tài nguyên
                sockBroadcast.Close();
            }
            catch (Exception excp)
            {

                Console.WriteLine(excp.ToString());
            }
        }
    }
}
