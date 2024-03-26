using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPClient
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        // Tạo Server EndPoint
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

        // Tạo Client Socket
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        Console.WriteLine("Nhập toán hạng 1:");
        double operand1 = double.Parse(Console.ReadLine());
        Console.WriteLine("Nhập toán hạng 2:");
        double operand2 = double.Parse(Console.ReadLine());
        Console.WriteLine("Nhập phép tính (+, -, *, /):");
        char operation = char.Parse(Console.ReadLine());

        // Gửi dữ liệu tới Server
        string dataToSend = $"{operand1},{operand2},{operation}";
        byte[] buffer = Encoding.ASCII.GetBytes(dataToSend);
        clientSocket.SendTo(buffer, serverEndPoint);

        // Nhận kết quả từ Server
        buffer = new byte[1024];
        int receivedBytes = clientSocket.Receive(buffer);
        string resultMessage = Encoding.ASCII.GetString(buffer, 0, receivedBytes);
        Console.WriteLine("Kết quả từ server: " + resultMessage);

        // Đóng kết nối
        Console.ReadKey();
        clientSocket.Close();
    }
}
