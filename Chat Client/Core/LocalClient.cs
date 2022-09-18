using ChatLibrary.User;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace Chat_Client.Core;

internal sealed class LocalClient
{
    private bool _isRunning;

    private TcpClient _tcpClient;

    private NetworkStream _networkStream;
    private StreamReader _reader;
    private StreamWriter _writer;

    public PublicUserData Data { get; set; }

    internal void ConnectToServer(string serverIP, int serverPort)
    {
        try
        {
            _tcpClient = new(new IPEndPoint(IPAddress.Any, 30000 + new Random().Next(1, 10001)));

            _tcpClient.Connect(new(IPAddress.Parse(serverIP), serverPort));

            _networkStream = _tcpClient.GetStream();
            _reader = new(_networkStream);
            _writer = new(_networkStream);

            _isRunning = true;

            Thread listenThread = new(o => ListenToServer((StreamReader)o));
            listenThread.SetApartmentState(ApartmentState.STA);

            listenThread.Start(_reader);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    internal void SendMessageToServer(string messageJSON)
    {
        if (!_isRunning || messageJSON is null) return;

        _writer.WriteLine(messageJSON);
        _writer.Flush();
    }

    private void ListenToServer(StreamReader reader)
    {
        while (_isRunning)
        {
            var message = reader.ReadLine();

            MessageHandler.HandleMessage(message);
        }
    }
}