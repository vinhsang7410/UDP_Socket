using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UdemyUDPBroadcastReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //khởi tạo socket 
            Socket socketBroadcastReceiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //ipendpoint là địa chỉ ip bất kì địa chỉ nào tới
            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, 23000);
            //tạo mạng bufer truyền dữ liệu
            byte[] receivedBuffer = new byte[512];
            //dếm số dữ liệu nhận được
            int nCountReceived = 0;
            //tạo 1 string empty để nhận dữ liệu từ client gửi tới
            string textReceived = string.Empty;
            //tạo 1 ipendpoint để nhận thông tin của endpoint(ở đây là sender)
            IPEndPoint ipepSender = new IPEndPoint(IPAddress.Any, 0);
            //sau khi biết endpointsender là ai, thì sẽ ép kiểu thành 1 endpoint
            EndPoint epSender = (EndPoint)ipepSender;
            try
            {
                //hàm bind cho phép client lăng nghe từ địa chỉ của ipLocal(ở đây là any)
                socketBroadcastReceiver.Bind(ipLocal);
                while (true)
                {
                    //hàm nhận được bao nhiêu dữ liệu thì sẽ đếm và show ra bảng console
                    nCountReceived = socketBroadcastReceiver.ReceiveFrom(receivedBuffer, ref epSender);
                    Console.WriteLine("Number of bytes data: " + nCountReceived);
                    //sau khi nhận dữ liệu vào buffer thì sẽ getstring để chuyển từ byte sang string và xuất ra console
                    textReceived = Encoding.ASCII.GetString(receivedBuffer, 0, nCountReceived);
                    Console.WriteLine("Text Received: " + textReceived);
                    //xuất thông tin sender
                    Console.WriteLine("Send from: " + epSender.ToString());
                    //nếu nhận được dữ liệu có data là <ECHO> thì sẽ phản hồi lại 
                    if (textReceived.Equals("<ECHO>"))
                    {
                        socketBroadcastReceiver.SendTo(receivedBuffer, 0, nCountReceived, SocketFlags.None, epSender);
                    }

                    //khi nhận được dữ liệu thì sẽ làm sạch ô nhớ chuẩn bị cho các dữ liệu tiếp theo
                    Array.Clear(receivedBuffer, 0, receivedBuffer.Length);
                }
            }
            catch (Exception excp)
            {
                
                Console.WriteLine(excp.ToString());
            }
        }
    }
}
