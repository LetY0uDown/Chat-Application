using System.Net;
using System.Net.Sockets;

namespace Chat_Server;

internal static class Server
{
    private readonly static TcpListener _tcpListener;

    private static bool _isConfigured;    

    static Server()
    {
        _tcpListener = new(IPAddress.Parse("127.0.0.1"), 30000);

        _isConfigured = true;
    }

    public static bool Running { get; private set; }

    internal static void Start()
    {
        if (!_isConfigured) throw new Exception("Server is not configured. Make sure to configure it before trying to start it again");
        if (Running) return;

        try
        {
            _tcpListener.Start();

            Console.WriteLine("Server is online");

            Running = true;

            while (Running)
            {
                TcpClient tcpClient = _tcpListener.AcceptTcpClient();

                Console.WriteLine("Accepted new client");

                Thread thread = new(o =>
                {
                    var client = new ServerClient((TcpClient)o);
                    client.ListenUser();
                });

                thread.Start(tcpClient);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void Stop()
    {
        Running = false;
        _tcpListener.Stop();
    }
}