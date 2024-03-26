using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPServer
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        // Tạo Server EndPoint
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

        // Tạo Server Socket
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Bind Server Socket với Server EndPoint
        serverSocket.Bind(serverEndPoint);

        Console.WriteLine("UDP Server đã khởi động...");

        byte[] buffer = new byte[1024];
        EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            // Nhận dữ liệu từ client
            int receivedBytes = serverSocket.ReceiveFrom(buffer, ref remote);
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, receivedBytes);

            // Phân tích dữ liệu nhận được
            string[] parts = dataReceived.Split(',');
            if (parts.Length != 3)
            {
                Console.WriteLine("Dữ liệu không hợp lệ từ client.");
                continue;
            }

            try
            {
                double operand1 = double.Parse(parts[0]);
                double operand2 = double.Parse(parts[1]);
                char operation = char.Parse(parts[2]);

                double result = 0;
                switch (operation)
                {
                    case '+':
                        result = operand1 + operand2;
                        break;
                    case '-':
                        result = operand1 - operand2;
                        break;
                    case '*':
                        result = operand1 * operand2;
                        break;
                    case '/':
                        if (operand2 != 0)
                            result = operand1 / operand2;
                        else
                            throw new DivideByZeroException();
                        break;
                    default:
                        Console.WriteLine("Phép tính không hợp lệ.");
                        continue;
                }

                // Gửi kết quả về client
                string resultMessage = $"Kết quả: {operand1} {operation} {operand2} = {result}";
                byte[] responseData = Encoding.ASCII.GetBytes(resultMessage);
                serverSocket.SendTo(responseData, remote);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xử lý phép tính: {ex.Message}");
            }
        }
    }
}
