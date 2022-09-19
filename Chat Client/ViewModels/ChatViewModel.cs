using Chat_Client.Core;
using MVVM_Classes;

namespace Chat_Client.ViewModels;

internal sealed class ChatViewModel : ObservableObject
{
    public ChatViewModel(Chat chat)
    {
        Data = chat;

        InitCommands();
    }

    public Chat Data { get; init; }

    public Command SendMessageCommand { get; private set; }

    public Command DisconnectCommand { get; private set; }

    public string MessageText { get; set; }

    private void InitCommands()
    {
        SendMessageCommand = new(o =>
        {
            var message = MessageFactory.CreateChatMessage(App.CurrentClient.Data, MessageText, Data.ID);

            App.CurrentClient.SendMessageToServer(message);

            MessageText = string.Empty;

        }, b => !string.IsNullOrEmpty(MessageText));

        DisconnectCommand = new(o =>
        {
            App.CurrentClient.SendMessageToServer(MessageFactory.CreateChatDisconnectRequest(Data.ID));
        });
    }
}