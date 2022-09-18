using Chat_Server.Messages;
using System.Net.Sockets;

namespace Chat_Server;

internal sealed class ServerClient
{
    private readonly NetworkStream _stream;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;

    internal ServerClient(TcpClient tcpClient)
    {
        _stream = tcpClient.GetStream();
        _reader = new(_stream);
        _writer = new(_stream);
    }

    internal void ListenUser()
    {
        try
        {
            while (Server.Running)
            {
                string message = _reader.ReadLine();

                Console.WriteLine("New message");

                MessageHandlerServer.HandleMessage(message, this);
            }
        }
        catch { throw; }
    }

    internal void SendMessageToUser(string text)
    {
        try
        {
            _writer.WriteLine(text);
            _writer.Flush();
        }
        catch { throw; }
    }

    internal void Disconnect()
    {
        try
        {
            _stream.Close();
            _stream.Dispose();
        }
        catch { throw; }
    }
}